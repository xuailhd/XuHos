using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public partial class ExamResultAttachment : AuditableEntity
    {
        /// <summary>
        /// 附件ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string AttachmentID { get; set; }

        /// <summary>
        /// 检查结果ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ExamResultID { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(512)]
        public string FilePath { get; set; }
    }
}
