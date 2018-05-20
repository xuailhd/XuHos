using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 用户-角色关系
    /// </summary>
    public partial class UserRoleMap : AuditableEntity, IUserBaseEntity
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Key,MaxLength(32)]
        public string UserRoleMapID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required,MaxLength(32)]
        public string RoleID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required, MaxLength(32)]
        public string UserID { get; set; }
    }
}
