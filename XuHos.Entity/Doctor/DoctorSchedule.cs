using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace XuHos.Entity
{
    
    /// <summary>
    /// 医生排版
    /// </summary>
    public partial class DoctorSchedule : AuditableEntity
    {
        public DoctorSchedule()
        {
        }

        /// <summary>
        /// 排班ID
        /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ScheduleID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DoctorID { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime OPDate { get; set; }

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
        /// 号源数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 已预约号源
        /// </summary>
        public int AppointNumber { get; set; }

    }
}
