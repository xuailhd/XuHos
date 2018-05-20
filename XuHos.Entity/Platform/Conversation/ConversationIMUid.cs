using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 实时通讯用户唯一标识
    /// </summary>
    public class ConversationIMUid : AuditableEntity
    {
        public ConversationIMUid()
        {
            this.Enable = false;
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required, Column(TypeName = "varchar")]
        [MaxLength(32)]   
        public string UserID { get; set; }

        /// <summary>
        /// 用户唯一标识（必须是Int类型）
        /// </summary>
        [Required]
        [Key]
        public int Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
