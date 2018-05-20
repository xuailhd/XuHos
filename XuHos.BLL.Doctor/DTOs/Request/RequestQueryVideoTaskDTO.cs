using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{

    public class RequestQueryTaskDTO : RequestSearchCondition
    {

        public RequestQueryTaskDTO()
        {
            BeginDate = string.Empty;
            EndDate = string.Empty;
            RoomState = new List<EnumRoomState>();
            OPDType = new List<EnumDoctorServiceType>();
            ResponseFilters = new List<string>();
        }       


        /// <summary>
        /// 开始时间(不要随便改类型)
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束时间(不要随便改类型)
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 预约类型（0-挂号、1-图文、2-语音、3-视频）
        /// </summary>
        public List<EnumDoctorServiceType> OPDType { get; set; }

        /// <summary>
        /// 房间状态 -1 其它，0 未就诊，1 候诊中，2 就诊中，3 已就诊，4 呼叫中，5 离开中，6 断开连接
        /// </summary>
        public List<EnumRoomState> RoomState { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public List<EnumCostType> OrderCostType { get; set; }

        public string DoctorID { get; set; }

        public List<string> ResponseFilters { get; set; }

        /// <summary>
        /// 返回诊断状态
        /// </summary>
        public bool? IncludeDiagnoseStatus { get; set; }

        /// <summary>
        /// 返回签名的处方数
        /// </summary>
        public bool? IncludeRecipeSignedCount { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public EnumRecordOrderType? OrderType { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        public string MemberID { get;  set; }
    }
}
