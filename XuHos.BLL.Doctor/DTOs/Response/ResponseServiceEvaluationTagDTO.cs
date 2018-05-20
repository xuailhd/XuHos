using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    public class ResponseServiceEvaluationTagDTO
    {

        /// <summary>
        /// ServiceEvaluationTagID
        /// </summary>		
        public string ServiceEvaluationTagID { get; set; }

        /// <summary>
        /// 标签分值
        /// </summary>		
        public int Score { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>		
        public string TagName { get; set; }
    }
}
