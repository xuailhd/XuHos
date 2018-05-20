using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    public class RequestServiceEvaluationDTO
    {
        [Required(ErrorMessage = "外部订单ID不能为空")]
        /// <summary>
        /// 外部订单ID
        /// </summary>		
        public string OuterID { get; set; }

        [Required(ErrorMessage = "评价分值不能为空")]
        /// <summary>
        /// 评价分值
        /// </summary>		
        public int Score { get; set; }

        [Required(ErrorMessage = "评价标签不能为空")]
        /// <summary>
        /// 评价标签，多个标签以(;)分割
        /// </summary>		
        public string EvaluationTags { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>		
        public string Content { get; set; }

    }
}
