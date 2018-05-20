using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    public class SysDictType : AuditableEntity
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DictTypeID { get; set; }

        /// <summary>
        /// 类型中文名
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string DictTypeName { get; set; }

    }
}
