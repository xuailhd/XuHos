using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;

namespace XuHos.Entity
{
    public  class UserLoginLog : AuditableEntity
    {
        
        /// <summary>
        /// 登录ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserLoginID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 登录账号（useraccount或mobile）
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string LoginAccount { get; set; }

        /// <summary>
        /// OrgID 登录用户所属机构ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OrgID { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumLoginType LoginType { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime LoginTime { get; set; }
    }
}
