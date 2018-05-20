using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO
{
    /// <summary>
    /// 会话内容
    /// </summary>
    public class ConversationMessageDTO 
    {
        /// <summary>
        /// 会话内容ID
        /// </summary>
        [Required]
        public string ConversationMessageID { get; set; }

        /// <summary>
        /// 会话房间ID
        /// </summary>
        public int ConversationRoomID { get; set; }

        /// <summary>
        /// 业务ID(关联外部业务ID)
        /// </summary>
        [Required]
        public string ServiceID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
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
        public string MessageContent { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Required]
        public DateTime MessageTime { get; set; }

        /// <summary>
        /// 内容状态(0-未读、1-已读)
        /// </summary>
        [Required]
        public int MessageState { get; set; }

    }

    public class ConversationMessageReturnDTO
    {
        public List<string> MsgBody
        { get; set; }

        public string MsgSeq { get; set; }

        public DateTime MsgTime { get; set; }

        public string FromAccount { get; set; }

        public string ToGroupId { get; set; }        
    }
}
