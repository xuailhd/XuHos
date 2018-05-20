using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 录音文件
    /// </summary>
    public class ConversationRecording : AuditableEntity
    {
        /// <summary>
        /// 文件编号
        /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string FileID { get; set; }

        /// <summary>
        /// 房间号码
        /// </summary>
        [MaxLength(32)]
        public string ChannelID { get; set; }

        /// <summary>
        /// 会话文件URL
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(256)]
        public string FileURL { get; set; }
    }
}
