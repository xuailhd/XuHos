using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus
{
    /// <summary>
    /// 
    /// 发布与订阅处理逻辑
    /// 核心功能代码
    /// </summary>
    public class Configuration
    {
        internal static ConnectionFactory DefaultFactory;

        public static void RegisterConfig(string HostName = "127.0.0.1", int Port = 5672, string UserName = "guest", string Password = "guest", string VirtualHost = "/")
        {
            DefaultFactory = new ConnectionFactory();
            DefaultFactory.HostName = HostName;
            DefaultFactory.UserName = UserName;
            DefaultFactory.Password = Password;
            DefaultFactory.Port = Port;
            DefaultFactory.VirtualHost = VirtualHost;            
            DefaultFactory.Protocol = Protocols.AMQP_0_9_1;
            DefaultFactory.AutomaticRecoveryEnabled = true;                       
            DefaultFactory.RequestedConnectionTimeout = 1000;
            DefaultFactory.SocketReadTimeout = 100;
            DefaultFactory.SocketWriteTimeout = 100;
            DefaultFactory.TopologyRecoveryEnabled = true;
            DefaultFactory.HandshakeContinuationTimeout = TimeSpan.FromSeconds(5);
            DefaultFactory.ContinuationTimeout = TimeSpan.FromSeconds(5);

        }

    }
}
