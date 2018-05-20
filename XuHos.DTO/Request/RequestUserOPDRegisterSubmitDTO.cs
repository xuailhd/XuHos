using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;

namespace XuHos.DTO
{

    /// <summary>
    /// 预约提交
    /// </summary>
    public class RequestUserOPDRegisterSubmitDTO
    {
        public RequestUserOPDRegisterSubmitDTO()
        {

        }


        [Required]
        /// <summary>
        /// 预约类型（0-挂号、1-图文、2-语音、3-视频）
        /// </summary>
        public EnumDoctorServiceType OPDType { get; set; }

        /// <summary>
        /// 排班ID
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ScheduleID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MemberID { get; set; }

        /// <summary>
        /// 诊疗卡号
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MedicalCardID { get; set; }

        /// <summary>
        /// 诊疗卡医院编号
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string MedicalCardHospID { get; set; }

        /// <summary>
        /// 患者名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出生日期  1989-01-02
        /// </summary>
        public string Birth { get; set; }

        /// <summary>
        /// 用户性别 0-男 1-女
        /// </summary>
        public EnumUserGender Sex { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 用户婚姻状况 (0-未婚、1-已婚、2-未知) 
        /// </summary>
        public EnumUserMaritalStatus Marriage { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OrgnazitionID
        { get; set; }


        /// <summary>
        /// 咨询内容
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(400)]
        public string ConsultContent { get; set; }


        /// <summary>
        /// 咨询疾病
        /// </summary>
        [MaxLength(30)]
        public string ConsultDisease { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public virtual List<RequestUserFileDTO> Files { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        public EnumUserType UserType { get; set; }

        public int UserLevel { get; set; }

        /// <summary>
        /// 没有指定排版的时候需要指定医生编号
        /// </summary>
        [DisplayFormat(NullDisplayText = "")]
        public string DoctorID { get; set; }


        /// <summary>
        /// 医生分组编号
        /// </summary>
        [DisplayFormat(NullDisplayText = "")]
        public string DoctorGroupID { get; set; }

        /// <summary>
        /// 优惠或特权
        /// </summary>
        public EnumPayPrivilege Privilege { get; set; }

        /// <summary>
        /// 用户套餐ID，需要指定使用哪个套餐的情况下使用
        /// </summary>
        public string UserPackageID { get; set; }

        /// <summary>
        /// 过敏史
        /// </summary>
        public string AllergicHistory { get; set; }

        /// <summary>
        /// 是否过导诊(兼容旧版本不过导诊)
        /// </summary>
        public bool IsToGuidance { get; set; }
    }
}
