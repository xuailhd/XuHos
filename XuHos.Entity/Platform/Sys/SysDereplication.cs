using System;
using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 系统去重表
    /// </summary>
    public class SysDereplication:AuditableEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string SysDereplicationID { get; set; }


        /// <summary>
        /// 去重复的表名称
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string TableName { get; set; }

        /// <summary>
        /// 外部编号
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OutID { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public EnumDereplicationType DereplicationType { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int SuccessCount { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int FailCount { get; set; }

    }
}
