using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    public class UserWechatMap : AuditableEntity
    {
        [Key]
        [Required]
        [MaxLength(32)]
        [Column(TypeName = "varchar")]
        public string UserWechatMapID { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        [MaxLength(256)]
        public string OpenID { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(256)]
        public string AppID { get; set; }
    }
}