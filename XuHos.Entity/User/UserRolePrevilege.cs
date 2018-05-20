using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace XuHos.Entity
{
    /// <summary>
    /// 用户角色权限
    /// </summary>
    public partial class UserRolePrevilege : AuditableEntity
    {
        /// <summary>
        /// 权限编号
        /// </summary>
        [Key,MaxLength(32)]
        public string PrevilegeID { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        [Required,MaxLength(32)]
        public string RoleID { get; set; }

        /// <summary>
        /// 允许的操作
        /// </summary>
        [Required]
        public string ModuleID { get; set; }

    }
}
