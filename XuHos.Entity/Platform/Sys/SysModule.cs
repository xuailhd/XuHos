using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    public class SysModule: AuditableEntity
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ModuleID { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string ModuleName { get; set; }

        /// <summary>
        /// 权限类型  0-菜单  1-功能
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int ModuleType { get; set; }
        /// <summary>
        /// 模块Url
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(256)]
        public string ModuleUrl { get; set; }

        /// <summary>
        /// 模块父ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ParentModuleID { get; set; }
        /// <summary>
        /// 顶级模块ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string TopModuleID { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column(TypeName = "int")]
        public int Sort { get; set; }

        /// <summary>
        ///  样式
        /// </summary>
        [Column(TypeName = "varchar")]
        public string CSSClass { get; set; }

        /// <summary>
        ///  样式
        /// </summary>
        [Column(TypeName = "varchar")]
        public string Target { get; set; }
    }
}
