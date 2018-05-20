using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    /// <summary>
    /// 服务提供者获得的标签评价次数
    /// </summary>
    public class ResponseServiceProviderEvaluatedTagDTO
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string ServiceEvaluationTagID { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 评价次数
        /// </summary>
        public long EvaluatedCount { get; set; }
    }
}
