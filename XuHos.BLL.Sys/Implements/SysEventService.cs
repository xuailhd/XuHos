using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Extensions;
using XuHos.DAL.EF;
using XuHos.Entity;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.DTO.Common;
using EntityFramework.Extensions;

namespace XuHos.BLL.Sys.Implements
{
    /// <summary>
    /// 服务类型相关业务处理
    /// </summary>
    public class SysEventService
    {
        /// <summary>
        /// 获取未写入队列的消息
        
        /// 日期：2017年8月30日
        /// </summary>
        public List<SysEvent> GetUnEnqueueEvent(int count = 500)
        {
            using (DBEntities db = new DBEntities())
            {
                //仅查询5秒中前的数据（允许数据出现5秒的不一致）
                var time = DateTime.Now.AddSeconds(5);
                return db.SysEvents.Where(a => !a.Enqueued &&
                a.CreateTime < time).Take(count).OrderByDescending(a => a.Priority).OrderBy(a => a.CreateTime).ToList();
            }
        }

        /// <summary>
        /// 更新队列状态
        
        /// 日期：2017年8月30日
        /// </summary>
        /// <param name="EventIds"></param>
        /// <returns></returns>
        public bool UpdateEnqueueState(List<string> EventIds)
        {
            using (DBEntities db = new DBEntities())
            {
                db.SysEvents.Where(a => !a.Enqueued && EventIds.Contains(a.EventID)).Update(a => new SysEvent()
                {
                    Enqueued = true
                });

                return true;
            }
        }

        /// <summary>
        /// 更新队列完成状态
        
        /// 日期：2017年8月31日
        /// </summary>
        /// <param name="EventIds"></param>
        public void UpdateFinishedState(string EventId, string QueueName)
        {
            using (DBEntities db = new DBEntities())
            {
                var eventModel = db.SysEventConsumes.Where(a => a.EventID == EventId && QueueName == a.QueueName).FirstOrDefault();

                if (eventModel == null)
                {
                    db.SysEventConsumes.Add(new SysEventConsume()
                    {
                        EventID = EventId,
                        Finished = true,
                        QueueName = QueueName,
                        RetryCount = 0
                    });
                }
                else
                {
                    eventModel.Finished = true;
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 更新队列完成状态
        
        /// 日期：2017年8月31日
        /// </summary>
        /// <param name="EventIds"></param>
        public bool TriggerEvent<T>(T eventObj)
            where T : XuHos.EventBus.IEvent
        {
            using (DBEntities db = new DBEntities())
            {
                db.TriggerEvent<T>(eventObj);
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 递增重试次数
        
        /// 日期：2017年8月31日
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        public bool IncrementRetryCount(string EventId, string QueueName)
        {
            using (DBEntities db = new DBEntities())
            {
                var eventModel = db.SysEventConsumes.Where(a => a.EventID == EventId && QueueName == a.QueueName).FirstOrDefault();

                if (eventModel != null)
                {
                    if (eventModel.Finished)
                    {
                        //已经完成，不许哟入队
                        return false;
                    }
                    else
                    {

                        eventModel.RetryCount++;
                        db.Update(eventModel);
                    }
                }
                else
                {
                    eventModel = new SysEventConsume()
                    {
                        EventID = EventId,
                        QueueName = QueueName,
                        Finished = false,
                        RetryCount = 1
                    };

                    db.SysEventConsumes.Add(eventModel);
                }

                db.SaveChanges();

                //超过阀值，不在队列消息不在入队（人工处理）
                if (eventModel.RetryCount > 3)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
