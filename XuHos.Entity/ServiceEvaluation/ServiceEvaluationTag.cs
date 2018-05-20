using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public partial class ServiceEvaluationTag : AuditableEntity
    {
        /// <summary>
        /// ServiceEvaluationID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ServiceEvaluationTagID { get; set; }

        /// <summary>
        /// 标签分值
        /// </summary>	
        [Required]
        public int Score { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>	
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string TagName { get; set; }
    }
}
