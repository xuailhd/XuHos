using XuHos.Common.Enum;
using XuHos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    /// <summary>
    /// 视频问诊记录
    /// </summary>
    public class ResponseTaskDTO
    {
        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime RegDate { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime OPDDate { get; set; }

        public DTO.Platform.OrderDTO Order { get; set; }

        /// <summary>
        /// 病情描述
        /// </summary>
        public string ConsultContent { get; set; }

        public DTO.UserMemberDTO Member { get; set; }

        public DTO.ConversationRoomDTO Room { get; set; }

        public DTO.DoctorScheduleDto Schedule { get; set; }

        public List<ConversationMessageDTO> Messages { get; set; }

        public List<ResponseUserFileDTO> AttachFiles { get; set; }

        /// <summary>
        /// 是否已下诊断
        /// </summary>
        public bool? IsDiagnosed { set; get; }

        /// <summary>
        /// 已签名处方数量
        /// </summary>
        public int? RecipeSignedCount { set; get; }

        /// <summary>
        /// 分诊状态（0无，1待分诊，2分诊中，3已分诊）
        /// </summary>
        public EnumTriageStatus TriageStatus { get; set; }

        /// <summary>
        /// 是否可删除（订单状态为已取消或者已完成的才能删除）
        /// </summary>
        public bool Deletable
        {
            get
            {
                if (this.Order != null)
                {
                    return this.Order.OrderState == EnumOrderState.Canceled || this.Order.OrderState == EnumOrderState.Finish;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否可取消（未退款、待确认、待支付、已支付 且 物流状态不能是已送达和配送中的）
        /// </summary>
        public bool Cancelable
        {
            get
            {
                if (this.Order != null)
                {
                    return this.Order.RefundState == EnumRefundState.NoRefund && (this.Order.OrderState == EnumOrderState.Paid || this.Order.OrderState == EnumOrderState.NoPay || this.Order.OrderState == EnumOrderState.NoConfirm) && !(this.Order.LogisticState == EnumLogisticState.配送中 || this.Order.LogisticState == EnumLogisticState.已送达);
                }
                return false;
            }
        }
    }
}
