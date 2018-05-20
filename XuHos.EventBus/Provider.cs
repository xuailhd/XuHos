using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using XuHos.EventBus;
using RabbitMQ.Client;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Polly.Retry;
using System.Net.Sockets;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace XuHos.EventBus
{
    /// <summary>
    /// 消息队列
    
    /// 日期：2017年4月5日
    /// </summary>
    public class MQChannel :IDisposable
    {
        IConnection _conn = null;
        IModel _channel = null;
        string _exchange = "amq.topic";
        string _exchangeType = "topic";
        ushort _preFetch = 1;
        bool _transactionUnCommit = false;
        object _sync_root = new object();
        int _retryCount = 3;

        private bool IsConnected
        {
            get
            {
                return _conn != null && _conn.IsOpen && !disposedValue;
            }
        }

        private bool TryConnect()
        {
            XuHos.Common.LogHelper.WriteDebug("RabbitMQ Client is trying to connect");

            lock (_sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        XuHos.Common.LogHelper.WriteWarn(ex.ToString());
                    }
                );

                policy.Execute(() =>
                {
                    _conn = Configuration.DefaultFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _conn.ConnectionShutdown += (object sender, ShutdownEventArgs e) =>
                    {
                        if (e.ReplyCode == 200)
                        {
                            XuHos.Common.LogHelper.WriteDebug(e.ReplyText);
                        }
                        else
                        {
                            XuHos.Common.LogHelper.WriteError(new Exception(e.ReplyText));
                        }
                    };
                    _conn.CallbackException += (object sender, CallbackExceptionEventArgs e) =>
                    {
                        XuHos.Common.LogHelper.WriteError(e.Exception);
                    };
                    _conn.RecoverySucceeded += (object sender, EventArgs e) =>
                    {
                        Console.WriteLine("RecoverySucceeded");
                    };
                    _conn.ConnectionRecoveryError += (object sender, ConnectionRecoveryErrorEventArgs e) =>
                    {
                        Console.WriteLine("ConnectionRecoveryError");
                    };

                    XuHos.Common.LogHelper.WriteDebug($"RabbitMQ persistent connection acquired a connection {_conn.Endpoint.HostName} and is subscribed to failure events");
                    return true;
                }
                else
                {
                    XuHos.Common.LogHelper.WriteWarn("ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        private IModel GetModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            if (_channel == null)
            {
                lock(_sync_root)
                {
                    if (_channel == null)
                    {
                        _channel = _conn.CreateModel();
                    }
                }
            }

            if (_channel != null)
            {
                _channel.ModelShutdown += (object sender, ShutdownEventArgs e) =>
                {
                    Console.WriteLine($"ModelShutdown：DeliveryTag={e.ReplyCode},ReplyText={e.ReplyText}");
                };
            }

            return _channel;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _conn.CreateModel();
        }

        public MQChannel()
        {
        
        }

        #region 事务模式
        /// <summary>
        /// 开启事务性消息
        /// </summary>
        public void BeginTransaction()
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            _channel = GetModel();

            if (_channel != null)
            {
                _channel.TxSelect();
                _transactionUnCommit = true;
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            _channel = GetModel();

            if (_channel != null && _transactionUnCommit)
            {
                _channel.TxCommit();
                _transactionUnCommit = false;
            }

        }

        /// <summary>
        /// 回滚事务消息
        /// </summary>
        void Rollback()
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            _channel = GetModel();

            if (_channel != null)
            {
                _channel.TxRollback();
                _transactionUnCommit = false;
            }

        }

        #endregion

        /// <summary>
        /// 批量发送消息（异步确认模式）
        /// </summary>
        public void Publish<IEventData>(
            List<IEventData> Events,
            Action<List<string>> ackHandler = null,
            Action<List<string>> nackHandler = null,
            Action<List<string>> returnHandler = null,
            int EventDelaySeconds = 0,
            int TimeoutMilliseconds=500)
            where IEventData : IEvent
        {
            var dict = Events.ToDictionary(a => a.EventId, msg => new Dictionary<string, string>() {

                                { "Body",JsonConvert.SerializeObject(msg)},
                                { "EventTypeName",msg.GetType().FullName}
                        });


            Publish(dict, ackHandler, nackHandler, returnHandler, EventDelaySeconds, TimeoutMilliseconds);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void Publish(
            Dictionary<string, Dictionary<string, string>> Events,
            Action<List<string>> ackHandler = null,
            Action<List<string>> nackHandler = null,
            Action<List<string>> returnHandler = null,
            int EventDelaySeconds = 0,
            int TimeoutMilliseconds = 500)
        {
            try
            {
                if (!IsConnected)
                {
                    TryConnect();
                }

                var policy = RetryPolicy.Handle<BrokerUnreachableException>()
               .Or<SocketException>()
               .Or<System.IO.IOException>()
               .Or<RabbitMQ.Client.Exceptions.AlreadyClosedException>()
               .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
               {
                   XuHos.Common.LogHelper.WriteWarn($"publish events wait and Retry",ex);
               });

                using (var _channel = CreateModel())
                {
                    var DeliveryTags = new Dictionary<ulong, string>();
                    var ReturnTags = new Dictionary<string, string>();

                    //消息无法投递失被退回（如：队列找不到）
                    _channel.BasicReturn += (object sender, BasicReturnEventArgs e) =>
                    {
                        var EventIDs = new List<string>();

                        if (!string.IsNullOrEmpty(e.BasicProperties.MessageId))
                        {
                            EventIDs.Add(e.BasicProperties.MessageId);
                            ReturnTags.Add(e.BasicProperties.MessageId, e.RoutingKey);
                        }

                        if (returnHandler != null && EventIDs.Count > 0)
                        {
                            returnHandler(EventIDs);
                        }
                    };

                    //消息路由到队列并持久化后执行
                    _channel.BasicAcks += (object sender, BasicAckEventArgs e) =>
                    {
                        var EventIDs = new List<string>();

                        if (e.Multiple)
                        {
                            foreach (var EventID in DeliveryTags.Where(a => a.Key < e.DeliveryTag + 1).Select(a => a.Value))
                            {
                                if (!EventIDs.Contains(EventID) && !ReturnTags.ContainsKey(EventID))
                                {
                                    EventIDs.Add(EventID);
                                }
                            }
                        }
                        else
                        {
                            var EventID = DeliveryTags[e.DeliveryTag];

                            if (!EventIDs.Contains(EventID) && !ReturnTags.ContainsKey(EventID))
                            {
                                EventIDs.Add(DeliveryTags[e.DeliveryTag]);
                            }
                        }

                        if (ackHandler != null && EventIDs.Count>0)
                        {
                            ackHandler(EventIDs);
                        }

                    };

                    //消息投递失败
                    _channel.BasicNacks += (object sender, BasicNackEventArgs e) =>
                    {
                        var EventIDs = new List<string>();

                        //批量确认
                        if (e.Multiple)
                        {
                            foreach (var EventID in DeliveryTags.Where(a => a.Key < e.DeliveryTag + 1).Select(a => a.Value))
                            {
                                if (!EventIDs.Contains(EventID))
                                {
                                    EventIDs.Add(EventID);
                                }
                            }
                        }
                        else
                        {
                            var EventID = DeliveryTags[e.DeliveryTag];

                            if (!EventIDs.Contains(EventID))
                            {
                                EventIDs.Add(EventID);
                            }
                        }

                        if (nackHandler != null && EventIDs.Count > 0)
                        {
                            nackHandler(EventIDs);
                        }
                    };

                    policy.Execute(() =>
                    {   _channel.ConfirmSelect();
                    });

                    foreach (var msg in Events)
                    {
                        policy.Execute(() =>
                        {
                            var EventId = msg.Key;
                            var json = msg.Value["Body"];
                            var routeKey = msg.Value["EventTypeName"];

                            byte[] bytes = Encoding.UTF8.GetBytes(json);


                            //设置消息持久化
                            IBasicProperties properties = _channel.CreateBasicProperties();
                            properties.DeliveryMode = 2;
                            properties.MessageId = msg.Key;

                            if (!DeliveryTags.ContainsValue(EventId))
                            {
                                DeliveryTags.Add((ulong)DeliveryTags.Count + 1, EventId);
                            }

                            //需要发送延时消息
                            if (EventDelaySeconds > 0)
                            {
                                Dictionary<string, object> dic = new Dictionary<string, object>();
                                dic.Add("x-expires", EventDelaySeconds * 10000);//队列过期时间 
                                dic.Add("x-message-ttl", EventDelaySeconds * 1000);//当一个消息被推送在该队列的时候 可以存在的时间 单位为ms，应小于队列过期时间  
                                dic.Add("x-dead-letter-exchange", _exchange);//过期消息转向路由  
                                dic.Add("x-dead-letter-routing-key", routeKey);//过期消息转向路由相匹配routingkey  
                                routeKey = routeKey + "_DELAY_" + EventDelaySeconds;

                                //创建一个队列                         
                                _channel.QueueDeclare(
                                        queue: routeKey,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: dic);
                                
                                _channel.BasicPublish(
                                    exchange: "",
                                    mandatory: true,
                                    routingKey: routeKey,
                                    basicProperties: properties,
                                    body: bytes);

                            }
                            else
                            {
                                _channel.BasicPublish(
                                    exchange: _exchange,
                                    mandatory: true,
                                    routingKey: routeKey,
                                    basicProperties: properties,
                                    body: bytes);

                            }
                        });

                    }

                    policy.Execute(() =>
                    {
                      _channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(TimeoutMilliseconds));

                    });
                }
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>    
        [Obsolete("此模式性能较差建议修改成批量模式")]
        public bool Publish<IData>(
            IData msg,            
            int EventDelaySeconds=0,
            int TimeoutMillseconds= 500)
            where IData : XuHos.EventBus.IEvent
        {
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .Or<System.IO.IOException>()
            .Or<RabbitMQ.Client.Exceptions.AlreadyClosedException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                XuHos.Common.LogHelper.WriteWarn($"publish events wait and Retry", ex);
            });

            if (!IsConnected)
            {
                TryConnect();
            }

            try
            {
                _channel = GetModel();

                return policy.Execute(()=>
                {

                    string json = JsonConvert.SerializeObject(msg);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    var routeKey = msg.GetType().FullName;
                    //设置消息持久化
                    IBasicProperties properties = _channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;

                    //是否需要确认,事务模式和Confirm不能同事开启
                    if (!_transactionUnCommit)
                    {
                       _channel.ConfirmSelect();
                    }

                    //需要发送延时消息
                    if (EventDelaySeconds > 0)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("x-expires", EventDelaySeconds * 10000);//队列过期时间 
                        dic.Add("x-message-ttl", EventDelaySeconds * 1000);//当一个消息被推送在该队列的时候 可以存在的时间 单位为ms，应小于队列过期时间  
                        dic.Add("x-dead-letter-exchange", _exchange);//过期消息转向路由  
                        dic.Add("x-dead-letter-routing-key", routeKey);//过期消息转向路由相匹配routingkey  

                        routeKey = routeKey + "_DELAY_" + EventDelaySeconds;

                        //创建一个队列                         
                        _channel.QueueDeclare(
                            queue: routeKey,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: dic);
                   
                        _channel.BasicPublish(
                            exchange: "",
                            mandatory: true,
                            routingKey: routeKey,
                            basicProperties: properties,
                            body: bytes);                   
                    }
                    else
                    {
                        _channel.BasicPublish(_exchange, routeKey, true, properties, bytes);                                           
                    }

                    if (!_transactionUnCommit)
                    {
                        _channel.WaitForConfirmsOrDie(TimeSpan.FromMilliseconds(TimeoutMillseconds));
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                });

                       
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }
        }

        /// <summary>
        /// 订阅消息（同一类消息可以重复订阅）
        
        /// 日期：2017年4月3日
        /// </summary>
        /// <param name="msg"></param>
        public MQChannel Subscribe<IData>(IEventHandler<IData> EventAction)
               where IData : IEvent
        {

            if (!IsConnected)
            {
                TryConnect();
            }

            _channel = GetModel();

            var _queueName = EventAction.GetType().FullName;
            var _routeKey = typeof(IData).FullName;

            //direct fanout topic  
            _channel.ExchangeDeclare(_exchange, _exchangeType, true, false, null);
            //在MQ上定义一个持久化队列，如果名称相同不会重复创建
            _channel.QueueDeclare(_queueName, true, false, false, null);
            //绑定交换器和队列
            _channel.QueueBind(_queueName, _exchange, _routeKey);
            //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
            _channel.BasicQos(0, _preFetch, false);
            //在队列上定义一个消费者a
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                if (_channel.IsOpen)
                {
                    var EventID = ea.BasicProperties.MessageId;
                    byte[] bytes;
                    string str=string.Empty;
                    var msg = default(IData);

                    try
                    {

                        bytes = ea.Body;
                        str = Encoding.UTF8.GetString(bytes);
                        msg = JsonConvert.DeserializeObject<IData>(str);
                  
                        if (EventAction.Handle(msg))
                        {
                            if (ackHandler != null)
                            {
                                ackHandler(EventID,_queueName);
                            }

                            //回复确认`
                            _channel.BasicAck(ea.DeliveryTag, false);

                        }
                        else
                        {
                            XuHos.Common.LogHelper.WriteWarn($"MQ:处理消息 {ea.RoutingKey} 失败,EventID={EventID}、Body={str}");

                            var requeue = true;

                            if (nackHandler != null)
                            {
                                requeue = nackHandler(EventID,_queueName, null,msg);
                            }

                            //拒绝重新写入队列，处理
                            _channel.BasicReject(ea.DeliveryTag, requeue);

                        }
                    }
                    catch (Exception ex)
                    {
                        XuHos.Common.LogHelper.WriteWarn($"MQ:处理消息 {ea.RoutingKey} 失败,EventID={EventID}、Body={str}");

                        var requeue = true;

                        if (nackHandler != null)
                        {
                            requeue = nackHandler(EventID, _queueName, ex,msg);
                        }
                        _channel.BasicReject(ea.DeliveryTag, requeue);
                    }
                }
                else
                {
                    Console.WriteLine("MQ:Channel Is Closed");
                }
            };

            consumer.Unregistered += (ch, ea) =>
            {
                XuHos.Common.LogHelper.WriteWarn($"MQ:{_queueName} Consumer_Unregistered");
            };

            consumer.Registered += (ch, ea) =>
            {
                XuHos.Common.LogHelper.WriteInfo($"MQ:{_queueName} Consumer_Registered");
            };

            consumer.Shutdown += (ch, ea) =>
            {
                XuHos.Common.LogHelper.WriteWarn($"MQ:{_queueName} Consumer_Shutdown.{ea.ReplyText}");
            };

            consumer.ConsumerCancelled += (object sender, ConsumerEventArgs e) =>
            {
                XuHos.Common.LogHelper.WriteWarn($"MQ:{_queueName} ConsumerCancelled");
            };

            //消费队列，并设置应答模式为程序主动应答
            _channel.BasicConsume(_queueName, false, consumer);

            return this;
        }

        internal Action<string,string> ackHandler = null;
        internal Func<string,string,Exception,dynamic,bool> nackHandler = null;

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //如果已经开启了事务还未提交则回滚
                    if (_transactionUnCommit)
                    {
                        Rollback();
                    }

                    if (_channel != null)
                    {
                        _channel.Close();
                        _channel.Dispose();
                    }

                    if (_conn != null)
                    {
                        _conn.Close();
                        _conn.Dispose();
                    }

                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Channel() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

    public static class MQChannelEx
    {
        public static MQChannel IfAckThen(this MQChannel mq, Action<string,string> handler)
        {
            mq.ackHandler = handler;
            return mq;
        }

        public static MQChannel IfNackThen(this MQChannel mq, Func<string,string,Exception,dynamic,bool> handler)
        {
            mq.nackHandler = handler;
            return mq;
        }
    }
}
