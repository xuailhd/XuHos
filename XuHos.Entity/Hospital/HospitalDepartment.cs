using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 医院科室
    /// </summary>
    public partial class HospitalDepartment : AuditableEntity
    {
        public HospitalDepartment()
        {
        }

        /// <summary>
        /// 科室ID
        /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DepartmentID { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string HospitalID { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        //[MaxLength(2048)]
        public string Intro { get; set; }
        /// <summary>
        /// 科室分类代码
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string CAT_NO { get; set; }

        /// <summary>
        /// 一级科室分类代码
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string PARENT_CAT_NO { get; set; }
    }
}
