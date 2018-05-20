using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    public class UserMember : AuditableEntity
    {
        public UserMember()
        {
            IDType = Common.Enum.EnumUserCardType.IDCard;
            Birthday = "";
            MemberName = "";
            Mobile = "";
            Address = "";
            Email = "";
            PostCode = "";
            //UserID = "";
        }
        /// <summary>
        /// 成员ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string MemberID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 成员姓名
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(32)]
        public string MemberName { get; set; }

        /// <summary>
        /// 关系（0-自己、1-配偶、2-父亲、3-母亲、4-儿子、5女儿、6-其他）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public Common.Enum.EnumUserRelation Relation { get; set; }

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
        /// 出生日期
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(10), MinLength(10)]
        public string Birthday { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string Mobile { get; set; }

        /// <summary>
        /// 证件类型(0-身份证,1-居民户口本,2-护照,3-军官证,4-驾驶证,5-港澳通行证,6-台湾通行证,7-港澳台身份证,99-其它)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public XuHos.Common.Enum.EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        //[Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string IDNumber { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Nationality { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Province { get; set; }


        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ProvinceRegionID { get; set; }


        /// <summary>
        /// 市
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string City { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string CityRegionID { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string District { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string DistrictRegionID { get; set; }

        /// <summary>
        /// 村，街道
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Town { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string TownRegionID { get; set; }

        /// <summary>
        /// 村，居委
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Village { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string VillageRegionID { get; set; }

        /// <summary>
        /// 是否有过敏史
        /// </summary>
        public bool IsAllergic { get; set; }

        /// <summary>
        /// 过敏历史备注
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string AllergicRemark { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(6)]
        public string PostCode { get; set; }

        /// <summary>
        /// 是否默认就诊人(0-非默认、1-默认，一个用户下只有一个默认就诊人)
        /// </summary>
        [Required]
        [Column(TypeName = "bit")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string Remark { get; set; }

        //public virtual User User { get; set; }


        /// <summary>
        /// 民族
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        public string Ethnic { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Occupation { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string CompanyName { get; set; }

    }

}
