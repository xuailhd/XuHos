using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class RequestUserConsultsQueryDTO:RequestUserQueryDTO
    {
        public RequestUserConsultsQueryDTO()
        {
            IncludeRemoved = 1;
            PageSize = int.MaxValue;
            CurrentPage = 1;
        }   

        public string UserID { get; set; }

        public EnumConsultType? ConsultType
        { get; set; }

        public string ConsultTime { get; set; }

        public string MemberName { get; set; }

        public int SelectType { get; set; }

        /// <summary>
        /// 指定需要获取的数据
        /// </summary>
        public string ChannelIDs { get; set; }

        /// <summary>
        /// 排除已经移除
        /// </summary>
        public int IncludeRemoved { get; set; }
        public EnumOrderState? OrderState { get; set; }

        public EnumConsultState? ConsultState { get; set; }

        /// <summary>
        /// 是否已支付
        /// </summary>
        public bool IsPayed { get; set; }

        /// <summary>
        /// 问诊类型：0-图文咨询，1-报告解读
        /// </summary>
        public int? InquiryType { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public EnumRecordOrderType? OrderType { get; set; }
    }
}
