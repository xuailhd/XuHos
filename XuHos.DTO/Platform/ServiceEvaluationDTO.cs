using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class RequestServiceEvaluationDTO
    {
       
        /// <summary>
        /// 外部订单ID
        /// </summary>		
        public string OuterID { get; set; }

        /// <summary>
        /// 评价分值
        /// </summary>		
        public int Score { get; set; }

        /// <summary>
        /// 评价标签，多个标签以(;)分割
        /// </summary>		
        public string EvaluationTags { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>		
        public string Content { get; set; }
  
    }

    public class ResponseServiceEvaluationDTO: ImageBaseDTO
    {
        /// <summary>
        /// ServiceEvaluationID
        /// </summary>		
        public string ServiceEvaluationID { get; set; }

        /// <summary>
        /// 外部订单ID
        /// </summary>		
        public string OuterID { get; set; }

        /// <summary>
        /// 评价分值
        /// </summary>		
        public int Score { get; set; }

        /// <summary>
        /// 评价标签，多个标签以(;)分割
        /// </summary>		
        public string EvaluationTags { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>		
        public string Content { get; set; }

        /// <summary>
        /// 服务提供者ID
        /// </summary>		
        public string ProviderID { get; set; }

        /// <summary>
        /// 服务类型(0-挂号、1-图文咨询、2-语音问诊、3-视频问诊、4-处方支付、5-家庭医生、6-会员套餐、7-远程会诊、8-影像判读、100-其它)
        /// </summary>		
        public EnumProductType ServiceType { get; set; }

        /// <summary>
        /// UserID
        /// </summary>		
        public string UserID { get; set; }


        public DateTime CreateTime { get; set; }

        /// <summary>
        /// UserName
        /// </summary>		
        public string UserName { get; set; }

        string _UserPhotoUrl;
        public string UserPhotoUrl {

            get
            {
                if (string.IsNullOrWhiteSpace(_UserPhotoUrl))
                {
                    return PaddingUrlPrefix("images/doctor/unknow.png");
                }
                else
                {
                    return PaddingUrlPrefix(_UserPhotoUrl);
                }
            }
            set
            {
                _UserPhotoUrl = value;
            }
        }

    }

    public class ServiceEvaluationDTOCondition : RequestSearchCondition
    {
        public string ProviderID { get; set; }
        public string OuterID { get; set; }
        public int? Score { get; set; }
        public string EvaluationTag { get; set; }
    }
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
