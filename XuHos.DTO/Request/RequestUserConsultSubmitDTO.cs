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
    /// 提交图文咨询
    /// </summary>
    public class RequestUserConsultSubmitDTO
    {
        public RequestUserConsultSubmitDTO()
        {
            this.Privilege = EnumPayPrivilege.None;
            this.ConsultType = EnumConsultType.Pay;
        }
        /// <summary>
        /// 成员ID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 诊疗卡号
        /// </summary>
        public string MedicalCardID { get; set; }

        /// <summary>
        /// 诊疗卡医院编号
        /// </summary>
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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Mobile { get; set; }
        /// <summary>
        /// 用户婚姻状况 (0-未婚、1-已婚、2-未知) 
        /// </summary>
        public EnumUserMaritalStatus Marriage { get; set; }

        /// <summary>
        /// 疾病名称
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ConsultDisease { get; set; }

        /// <summary>
        /// 咨询内容
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ConsultContent { get; set; }

        /// <summary>
        /// 咨询类型（0-付费、1-免费、2-义诊、3-套餐、4-会员、5-家庭医生）
        /// </summary>
        public EnumConsultType ConsultType { get; set; }
        
        /// <summary>
        /// 咨询类型（0-付费、1-免费、2-义诊、3-套餐、4-会员）
        /// </summary>
        public bool Free { get; set; }


        /// <summary>
        /// 医生编号
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DoctorID
        { get; set; }

        /// <summary>
        /// 医生分组编号
        /// </summary>
        [DisplayFormat(NullDisplayText = "")]
        public string DoctorGroupID { get; set; }

        public virtual List<RequestUserFileDTO> Files { get; set; }

        public string UserID { get; set; }
        
        public string OrgnazitionID { get; set; }

        /// <summary>
        /// 优惠方式
        /// </summary>
        public EnumPayPrivilege Privilege { get; set; }

        /// <summary>
        /// 用户套餐ID，需要指定使用哪个套餐的情况下使用
        /// </summary>
        public string UserPackageID { get; set; }

        /// <summary>
        /// 问诊类型：0-图文咨询，1-报告解读
        /// </summary>
        public int InquiryType { get; set; } = 0;
        public int UserLevel { get; set; }

        /// <summary>
        /// 是否进导诊（兼容旧版本不过导诊）
        /// </summary>
        public bool IsToGuidance { get; set; }
    }

}
