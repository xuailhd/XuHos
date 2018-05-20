using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace XuHos.Entity
{

    /// <summary>
    /// 用户病历
    /// </summary>
    public partial class UserMedicalRecord : AuditableEntity,IUserBaseEntity
    {
        public UserMedicalRecord()
        {
            this.CreateTime = DateTime.Now;
            this.Sympton = "";
            this.PastMedicalHistory = "";
            this.PresentHistoryIllness = "";
            this.AllergicHistory = "";

        }

        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserMedicalRecordID
        {
            get;
            set;
        }

        /// <summary>
        /// 预约登记ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string MemberID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DoctorID { get; set; }

        /// <summary>
        /// 症状
        /// </summary>
        //[Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string Sympton { get; set; }

        /// <summary>
        /// 既往病史
        /// </summary>
        //[Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string PastMedicalHistory { get; set; }

        /// <summary>
        /// 现病史
        /// </summary>
        //[Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string PresentHistoryIllness { get; set; }

        /// <summary>
        /// 初步诊断
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string PreliminaryDiagnosis { get; set; }

        /// <summary>
        /// 患者主诉
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string ConsultationSubject { get; set; }

        /// <summary>
        /// 病情描述
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 手术史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string PastOperatedHistory { get; set; }

        /// <summary>
        /// 家族史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string FamilyMedicalHistory { get; set; }

        /// <summary>
        /// 过敏史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string AllergicHistory { get; set; }

        /// <summary>
        /// 个人疾病史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string IndividualHistory { get; set; }

        /// <summary>
        /// 生活史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string LifeHistory { get; set; }

        /// <summary>
        /// 婚育史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string ObstetricalHistory { get; set; }

        /// <summary>
        /// 月经史
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string MenstrualHistory { get; set; }

        /// <summary>
        /// 医嘱
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string Advised { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OrgnazitionID { get; set; }

    }
}
