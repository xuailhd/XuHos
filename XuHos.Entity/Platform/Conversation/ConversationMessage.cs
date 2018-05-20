using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 会话内容
    /// </summary>
    public class ConversationMessage:AuditableEntity,IAuditableEntity
    {

        /// <summary>
        /// 会话内容ID
        /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ConversationMessageID { get; set; }

        /// <summary>
        /// 会话房间ID
        /// </summary>
        [Required]
        public int ConversationRoomID { get; set; }

        /// <summary>
        /// 业务ID(关联外部业务ID)
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ServiceID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        [Required]
        public string MessageType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string MessageContent { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime MessageTime { get; set; }

        /// <summary>
        /// 内容状态(0-未读、1-已读)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int MessageState { get; set; }

        /// <summary>
        /// 消息序号
        /// </summary>
        [ Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string MessageSeq { get; set; }

        /// <summary>
        /// 消息需要
        /// </summary>
        [Required]
        public int MessageIndex { get; set; }

    }
}
