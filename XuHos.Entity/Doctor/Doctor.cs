using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 医生
    /// </summary>
    public partial class Doctor : AuditableEntity
    {
        public Doctor()
        {
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DoctorID { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string DoctorName { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public XuHos.Common.Enum.EnumUserGender Gender { get; set; }

        /// <summary>
        /// 婚姻情况(0-未婚、1-已婚、2-未知)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public XuHos.Common.Enum.EnumUserMaritalStatus Marriage { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(10)]
        public string Birthday { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public XuHos.Common.Enum.EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string IDNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(6)]
        public string PostCode { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string Intro { get; set; }

        /// <summary>
        /// 是否专家（0-否、1-是）
        /// </summary>
        [Required]
        [Column(TypeName = "bit")]
        public bool IsExpert { get; set; }

        /// <summary>
        /// 特长
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string Specialty { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(16)]
        public string areaCode { get; set; }

        /// <summary>
        /// 医院ID（没有关联的医院ID默认为0）
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string HospitalID { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string HospitalName { get; set; }

        /// <summary>
        /// 医院等级
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string Grade { get; set; }

        /// <summary>
        /// 科室ID（没有关联的科室ID默认为0）
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DepartmentID { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string Education { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string Title { get; set; }

        /// <summary>
        /// 社会机构及职务
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        public string Duties { get; set; }

        /// <summary>
        /// 认证状态（0-未认证、1-已通过、2-未通过、3-认证中、4-第三方认证中）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int CheckState { get; set; }

        /// <summary>
        /// 签名图片URL
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string SignatureURL { get; set; }

        /// <summary>
        /// 医网签签名图片(base64)
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(4000)]
        public string BJCASignature { get; set; }

        /// <summary>
        /// 医师执业证书编码 
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string CertificateNo { get; set; }

        /// <summary>
        /// BJCA医生唯一标识
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string OpenID { get; set; }

        /// <summary>
        /// 是否显示在前端
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生
        /// </summary>
        public int DoctorType { get; set; }

        /// <summary>
        /// 疾病标签（甲状腺癌，乳腺癌，胃癌，结肠癌等）
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        public string DiseaseLabel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int Sort { get; set; }

        /// <summary>
        /// 医生诊室名称
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string RoomName { get; set; }
    }
}