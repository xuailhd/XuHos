using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    /// <summary>
    /// 提供给前段url权限
    /// </summary>
    public class ResponseSysModuleForDocDTO
    {
        /// <summary>
        /// 模块Url
        /// </summary>
        public string href { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        ///  样式
        /// </summary>
        public string cssClass { get; set; }

        /// <summary>
        ///  样式
        /// </summary>
        public string target { get; set; }

        public string id { get; set;}

        public int ModuleType { get; set; }

        public int Sort { get; set; }
    }
}
