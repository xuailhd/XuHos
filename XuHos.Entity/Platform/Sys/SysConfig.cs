using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    public class SysConfig : AuditableEntity
    {
        public SysConfig()
        {
            this.Remark = "";
        }

        /// <summary>
        /// ÅäÖÃ½ÚKey
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ConfigKey { get; set; }

        /// <summary>
        /// ÅäÖÃ½ÚValue
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(512)]
        public string ConfigValue { get; set; }

        /// <summary>
        /// ±¸×¢
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        public string Remark { get; set; }

    }
}
