using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public partial class ExamItem : AuditableEntity
    {
        /// <summary>
        /// 检查项目ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ExamItemID { get; set; }
        /// <summary>
        /// 检查类型ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ExamItemTypeID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string MemberID { get; set; }
    }
}
