using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    public class SysEvent: IComparable<SysEvent>
    {
        public SysEvent()
        { }

        private object _EventObject;

        public T As<T>()
            where T : class
        {
            if (_EventObject == null)
            {
                return XuHos.Common.JsonHelper.FromJson<T>(EventObject);
            }
            else
            {
                return _EventObject as T;
            }
        }

        public SysEvent(object EventObject,string EventId)
        {
          
            var routeKey = EventObject.GetType().FullName;
            this.EventID = EventId;
            this.CreateTime = DateTime.Now;
            this.RouteKey = routeKey;
            this.Priority = 0;
            this.EventObject =XuHos.Common.JsonHelper.ToJson(EventObject);
            this.Enqueued = false;

            _EventObject = EventObject;
        }



        public int CompareTo(SysEvent obj)
        {
            return DateTime.Compare(this.CreateTime, obj.CreateTime);

        }

        /// <summary>
        /// 银行ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string EventID { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(1000)]
        public string RouteKey { get; set;}

        [Required]
        [Column(TypeName = "varchar")]        
        public string EventObject { get; set; }


        /// <summary>
        /// 优先级
        /// </summary>
        [Required]
        public int Priority { get; set; }



        /// <summary>
        /// 写入队列状态
        /// </summary>
        public bool Enqueued { get; set;}


        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }

        
    }
}
