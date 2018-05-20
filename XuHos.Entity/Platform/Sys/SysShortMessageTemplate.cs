using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    public class SysShortMessageTemplate : AuditableEntity
    {
        public SysShortMessageTemplate()
        {
          
        }


        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string TemplateID { get; set; }

        /// <summary>
        /// Ä£°åÄÚÈÝ
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(512)]
        public string TemplateContent { get; set; }

        /// <summary>
        /// ±¸×¢
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        public string Remark { get; set; }


    }
}
