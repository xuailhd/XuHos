using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{

    public partial class Hospital : AuditableEntity
    {
        public Hospital()
        {
        }

        /// <summary>
        /// 医院ID
        /// </summary>
        [Key, Required]
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
        /// 简介
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(4000)]
        public string Intro { get; set; }

        /// <summary>
        /// 许可证
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string License { get; set; }

        /// <summary>
        /// Logo地址
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string LogoUrl { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(10)]
        public string AreaName { get; set; }

        /// <summary>
        /// 店长姓名
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(32)]
        public string DrugstoreManageName { get; set; }

        /// <summary>
        /// 店长/门店电话
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(16)]
        public string Mobile { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string Latitude { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(6)]
        public string PostCode { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string Telephone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// 机构类型
        /// </summary>
        [Column(TypeName = "int")]
        public EnumOrgType OrgType { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 层级路径
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(256)]
        public string Path { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ParentID { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 列表图片
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string ListImageUrl { get; set; }

        /// <summary>
        /// 是否在Web前端显示
        /// </summary>
        public bool IsShowInWeb { get; set; }

        /// <summary>
        /// 是否为独立机构 用户数据来源标识
        /// </summary>
        public bool IsStandalone { get; set; }

        /// <summary>
        /// 是否合作机构 (医疗馆)
        /// </summary>
        public bool IsCooperation { get; set; }

        /// <summary>
        /// 是否有自己渠道使用智慧药房药品
        /// </summary>
        public bool IsUseWisdom { get; set; }
        
        /// <summary>
        /// 案例路径
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string Mp4Url { get; set; }

        /// <summary>
        /// 案例预览图
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string Mp4PreviewUrl { get; set; }

        /// <summary>
        /// 面向智慧药房渠道ID（网络医院ID 或者 自己渠道 不使用智慧药房的药，为空）
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string ChannelID { get; set; }

        /// <summary>
        /// 首页主题样式
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string HomePageTheme { get; set; }

        /// <summary>
        /// 所属省，直辖市
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ProvinceID { get; set; }

        /// <summary>
        /// 所属市
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string CityID { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string AreaID { get; set; }

        /// <summary>
        /// 是否使用总店药品
        /// </summary>
        [Column(TypeName = "bit")]
        public bool IsUseParentOrgDrug { get; set; }
    }
}
