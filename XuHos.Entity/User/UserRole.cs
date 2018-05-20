using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public partial class UserRole : AuditableEntity
    {
    
        /// <summary>
        /// 角色编号
        /// </summary>‘
        [Key,MaxLength(32)]
        [Column(TypeName = "varchar")]
        public string RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required,MaxLength(50)]
        [Column(TypeName = "nvarchar")]
        public string RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public string Memo { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        [Column(TypeName = "int")]
        public EnumRoleType RoleType { get; set; }
    }
}
