using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 用户信息扩展表
    /// </summary>
    public class UserExtend : AuditableEntity, IUserBaseEntity
    {

        public UserExtend()
        {
            JRegisterID = "";
            LastTime = DateTime.Now;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 极光推送的设备ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        [Required]
        public string JRegisterID { get; set; }
    }
}
