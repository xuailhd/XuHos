using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace XuHos.Entity
{
    /// <summary>
    /// 医生价格服务
    /// </summary>
    public partial class DoctorService : AuditableEntity
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ServiceID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DoctorID { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public Common.Enum.EnumDoctorServiceType ServiceType { get; set; }

        /// <summary>
        /// 服务开关(0-关闭服务、1-开启服务)
        /// </summary>
        [Required]
        [Column(TypeName = "bit")]
        public bool ServiceSwitch { get; set; }

        /// <summary>
        /// 服务价格
        /// </summary>
        [Required]
        [Column(TypeName = "decimal")]
        public decimal ServicePrice { get; set; }

    }
}
