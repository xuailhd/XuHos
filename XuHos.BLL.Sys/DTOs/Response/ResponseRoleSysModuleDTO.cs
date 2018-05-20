using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    /// <summary>
    /// 角色菜单功能权限
    /// </summary>
    public class ResponseRoleSysModuleDTO
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleID { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 权限类型  0-菜单  1-功能
        /// </summary>
        public int ModuleType { get; set; }
        /// <summary>
        /// 模块Url
        /// </summary>
        public string ModuleUrl { get; set; }

        /// <summary>
        /// 模块父ID
        /// </summary>
        public string ParentModuleID { get; set; }
        /// <summary>
        /// 顶级模块ID
        /// </summary>
        public string TopModuleID { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        ///  样式
        /// </summary>
        public string CSSClass { get; set; }

        /// <summary>
        ///  样式
        /// </summary>
        public string Target { get; set; }

        public string RoleID { get; set; }
        public int RoleType { get; set; }

    }
}
