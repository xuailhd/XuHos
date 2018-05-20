using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    public class SysEventConsume
    {
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ConsumeID { get; set; }


        /// <summary>
        /// 事件编号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string EventID { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(1000)]
        public string QueueName { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        [Required]
        public int RetryCount { get; set; }


        /// <summary>
        /// 是否已经完成
        /// </summary>
        [Required]
        public bool Finished { get; set; }
    }
}
