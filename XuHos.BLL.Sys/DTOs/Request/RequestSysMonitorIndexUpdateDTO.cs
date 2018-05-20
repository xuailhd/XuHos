using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestSysMonitorIndexUpdateDTO
    {

        /// <summary>
        /// 外部编号
        /// </summary>
        [Required]
        public string OutID { get; set; }

        /// <summary>
        /// 指标分类
        /// </summary>
        [Required]
        public string Category { get; set; }

        public Dictionary<string, string> Values { get; set; }
    }
}
