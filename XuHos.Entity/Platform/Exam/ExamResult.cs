using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public partial class ExamResult : AuditableEntity
    {
        public ExamResult()
        {
        }
        /// <summary>
        /// 检查结果ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ExamResultID { get; set; }

        /// <summary>
        /// 检查类型ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ExamItemTypeID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string MemberID { get; set; }

        /// <summary>
        /// 检查时间
        /// </summary>
        [Required]
        [Column(TypeName = "date")]
        public DateTime ExamTime { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        public string Result { get; set; }

        /// <summary>
        /// 检查状态（0-正常、1-高于正常范围、-1-低于正常范围）
        /// </summary>
        [Column(TypeName = "int")]
        public int Status { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        public string StatusMsg { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string HospitalID { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string HospitalName { get; set; }
    }
}
