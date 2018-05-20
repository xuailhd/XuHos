using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 电子病历
    /// </summary>
    public partial class UserMemberEMR : AuditableEntity
    {
        /// <summary>
        /// 患者电子病历ID
        /// </summary>
        [Key, MaxLength(32)]
        public string UserMemberEMRID { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Required, MaxLength(32)]
        public string MemberID { get; set; }

        /// <summary>
        /// 病历名称
        /// </summary>
        [Required, MaxLength(64)]
        public string EMRName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string HospitalName { get; set; }

        /// <summary>
        /// 病情说明
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string Remark { get; set; }
    }
}
