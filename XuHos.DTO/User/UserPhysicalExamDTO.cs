using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace XuHos.DTO
{
    public class UserPhysicalExamDTO
    { 

        /// <summary>
        /// 项目编码
        /// </summary>
        [Required]
        public string ItemCode { get; set; }

        /// <summary>
        /// 项目中文名称
        /// </summary>
        [Required]
        public string ItemCNName { get; set; }

        /// <summary>
        /// 项目英文名称
        /// </summary>
        [Required]
        public string ItemENName { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 参考范围
        /// </summary>
        public string RefRange { get; set; }

        /// <summary>
        /// /单位
        /// </summary>
        [Required]
        public string Unit { get; set; }



    }
}
