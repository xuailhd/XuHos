using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    /// <summary>
    /// 预约编号
    /// </summary>
    public partial class UserOPDRegister : AuditableEntity,IUserBaseEntity
    {
        public UserOPDRegister()
        {
            this.CreateTime = DateTime.Now;
            this.RegDate = DateTime.Now;       
            this.ConsultContent = "";
            this.DoctorID = "";
            this.DoctorGroupID = "";
            this.MemberID = "";
            this.MemberName = "";
            this.ScheduleID = "";
            this.IsUseTaskPool = false;
            this.Gender = Common.Enum.EnumUserGender.Other;
            this.Marriage = Common.Enum.EnumUserMaritalStatus.Other;
            this.IDNumber = "";
            this.IDType = Common.Enum.EnumUserCardType.IDCard;
            this.Mobile = "";
            this.Birthday = "";
        }
        /// <summary>
        /// 预约登记ID
        /// </summary>
        [Key,Required]
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
        /// 医生ID
        /// </summary>       
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        [Required]
        public string DoctorID { get; set; }

        /// <summary>
        /// 医生分组编号
        /// </summary>       
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        [Required]
        public string DoctorGroupID { get; set; }

        /// <summary>
        /// 排班ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ScheduleID { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        [Required]
        public DateTime RegDate { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        [Required]
        public DateTime OPDDate { get; set; }

        /// <summary>
        /// 就诊开始时间
        /// </summary>
        [Required]
        public string OPDBeginTime { get; set; }

        /// <summary>
        /// 就诊结束时间
        /// </summary>
        [Required]
        public string OPDEndTime { get; set; }

        /// <summary>
        /// 咨询内容
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(400)]
        public string ConsultContent { get; set; }

        /// <summary>
        /// 问诊疾病
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string ConsultDisease { get; set; }

        /// <summary>
        /// 预约类型（0-挂号、1-图文、2-语音、3-视频）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public Common.Enum.EnumDoctorServiceType OPDType { get; set; }

        /// <summary>
        /// 预约金额
        /// </summary>
        [Required]
        [Column(TypeName = "decimal")]
        public decimal Fee { get; set; }


        [Required]
        public string MemberID { get; set; }

        /// <summary>
        /// 成员姓名
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(32)]
        public string MemberName { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public Common.Enum.EnumUserGender Gender { get; set; }

        /// <summary>
        /// 婚姻情况(0-未婚、1-已婚、2-未知)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public Common.Enum.EnumUserMaritalStatus Marriage { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string IDNumber { get; set; }

        /// <summary>
        /// 证件类型（0-身份证）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public XuHos.Common.Enum.EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string Mobile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(10), MinLength(10)]
        public string Birthday { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        [Required]
        public string OrgnazitionID { get; set; }

        /// <summary>
        /// 使用任务池
        /// </summary>
        [Required]
        public bool IsUseTaskPool { get; set;}

        /// <summary>
        /// 预约标识（0：默认看诊，1：私人医生看诊）
        /// </summary>
        public int Flag { get; set; }

    }
}
