using XuHos.Common.Enum;
using XuHos.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace XuHos.DTO
{

    /// <summary>
    /// 用户病历
    /// </summary>
    public partial class UserMedicalRecordDTO
    {
        public UserMedicalRecordDTO()
        {
            this.Sympton = "";
            this.PastMedicalHistory = "";
            this.PresentHistoryIllness = "";
            this.AllergicHistory = "";

        }

        public string UserMedicalRecordID
        {
            get;
            set;
        }

        /// <summary>
        /// 预约登记ID
        /// </summary>
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>    
        public string UserID { get; set; }



        public string MemberID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>     
        public string DoctorID { get; set; }


        /// <summary>
        /// 症状
        /// </summary>

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Sympton { get; set; }

        /// <summary>
        /// 既往病史
        /// </summary>

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PastMedicalHistory { get; set; }


        /// <summary>
        /// 现病史
        /// </summary>

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PresentHistoryIllness { get; set; }


        /// <summary>
        /// 初步诊断
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PreliminaryDiagnosis { get; set; }

        /// <summary>
        /// 患者主诉
        /// </summary>
        public string ConsultationSubject { get; set; }

        /// <summary>
        /// 病情描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 手术史
        /// </summary>
        public string PastOperatedHistory { get; set; }

        /// <summary>
        /// 家族史
        /// </summary>
        public string FamilyMedicalHistory { get; set; }

        /// <summary>
        /// 过敏史
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AllergicHistory { get; set; }

        /// <summary>
        /// 医嘱
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Advised { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>      
        public string OrgnazitionID { get; set; }

        /// <summary>
        /// 个人疾病史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string IndividualHistory { get; set; }

        /// <summary>
        /// 生活史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string LifeHistory { get; set; }

        /// <summary>
        /// 婚育史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string ObstetricalHistory { get; set; }

        /// <summary>
        /// 月经史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string MenstrualHistory { get; set; }

        /// <summary>
        /// 会员
        /// </summary>
        public virtual UserMemberDTO Member { get; set; }

        /// <summary>
        /// 医生
        /// </summary>
        public virtual DoctorDto Doctor { get; set; }

        /// <summary>
        /// 预约记录
        /// </summary>
        public virtual UserOPDRegisterDTO UserOPDRegister { get; set; }


    }
}
