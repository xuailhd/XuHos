using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace XuHos.Entity
{
    /// <summary>
    /// 工作时间
    /// </summary>
    public partial class HospitalWorkingTime : AuditableEntity
    {  
        /// <summary>
       /// 工作时间ID
       /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string WorkingTimeID { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string HospitalID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(16)]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(16)]
        public string EndTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int Sort { get; set; }

    }
}
