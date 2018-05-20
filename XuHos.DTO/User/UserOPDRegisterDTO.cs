using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;
namespace XuHos.DTO
{

    /// <summary>
    /// 预约信息
    /// </summary>
    public class UserOPDRegisterDTO
    {
        /// <summary>
        /// 预约登记ID
        /// </summary>
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }


        /// <summary>
        /// 医生分组编号
        /// </summary>       
        public string DoctorGroupID { get; set; }

        [Required]
        /// <summary>
        /// 排班ID
        /// </summary>
        public string ScheduleID { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime RegDate { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime OPDDate { get; set; }


        [Required]
        /// <summary>
        /// 预约类型（0-挂号、1-图文、2-语音、3-视频）
        /// </summary>
        public EnumDoctorServiceType OPDType { get; set; }     


        /// <summary>
        /// 预约金额
        /// </summary>
        public decimal Fee { get; set; }


        [Required]
        public string MemberID { get; set; }

        /// <summary>
        /// 成员姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }

        /// <summary>
        /// 婚姻情况(0-未婚、1-已婚、2-未知)
        /// </summary>
        public EnumUserMaritalStatus Marriage { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 分诊状态（0无，1待分诊，2分诊中，3已分诊）
        /// </summary>
        public EnumTriageStatus TriageStatus { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 证件类型（0-身份证）
        /// </summary>
        public EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public string OrgnazitionID { get; set; }


        public string OrgName { set; get; }

        public EnumOrgType? OrgType { set; get; }

        public string RecipeFileID { get; set; }

        public DTO.Platform.OrderDTO Order { get; set;}

        public DTO.UserMemberDTO Member { get; set; }

        public DTO.UserDTO User { get; set; }

        public DTO.DoctorDto Doctor { get; set; }

        public DTO.DoctorScheduleDto Schedule { get; set; }

        public DTO.ConversationRoomDTO Room { get; set; }

        public string ConsultContent { get; set; }

        public UserMedicalRecordDTO UserMedicalRecord { get; set; }

        public DoctorTriageDTO DoctorTriage { get; set; }

        /// <summary>
        /// 是否存病历资料
        /// </summary>
        public bool IsExistMedicalRecord { get; set; }

        /// <summary>
        /// 是否一键呼叫问诊
        /// </summary>
        public bool IsUseTaskPool { get; set; }

        /// <summary>
        /// 处方文件路径
        /// </summary>
        public string RecipeFileUrl { get; set; }


        /// <summary>
        /// 用户上传的附件
        /// </summary>
        public List<UserFileDTO> AttachFiles { get; set; }

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


        /// <summary>
        /// 就诊开始时间
        /// </summary>    
        public string OPDBeginTime { get; set; }

        /// <summary>
        /// 就诊结束时间
        /// </summary>
 
        public string OPDEndTime { get; set; } 

        /// <summary>
        /// 问诊疾病
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string ConsultDisease { get; set; }

        /// <summary>
        /// 是否是离线处方，离线处方的处方要通过审方平台才能获取到
        /// </summary>
        public bool? OfflineRecipe { get; set; }

        public List<XuHos.DTO.ResponseDoctorDiagnoseAndRecipeDTO.DoctorRecipeFileDTO> RecipeList { get; set; }

    }

}
