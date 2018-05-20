using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XuHos.Common.Enum;
namespace XuHos.DTO
{
    public class UserMemberDTO : Common.ImageBaseDTO
    {
        /// <summary>
        /// 成员ID
        /// </summary>
        [MaxLength(32)]
        public string MemberID { get; set; }

        /// <summary>
        /// 对接会员系统
        /// </summary>
        public string OrgID { get; set; }

        //[Required]
        /// <summary>
        /// 用户ID
        /// </summary>
        [MaxLength(32)]
        public string UserID { get; set; }

        [Required]
        [MaxLength(20)]
        /// <summary>
        /// 成员姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 关系（0-自己、1-配偶、2-父亲、3-母亲、4-儿子、5女儿、6-其他）
        /// </summary>
        public EnumUserRelation Relation { get; set; }

        //[Required]
        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }

        //[Required]
        /// <summary>
        /// 婚姻情况(0-未婚、1-已婚、2-未知)
        /// </summary>
        public EnumUserMaritalStatus Marriage { get; set; }

        //[Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(10)]
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        [MaxLength(11)]
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 证件类型（0-身份证）
        /// </summary>
        public EnumUserCardType IDType { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(32)]
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Ethnic { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public string Occupation { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(6)]
        public string PostCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string GenderName { get; set; }

        int _Age;
        public int Age
        {
            get
            {
                if (_Age > 0)
                {
                    return _Age;
                }
                else
                {
                    return XuHos.Common.ToolHelper.GetAgeFromBirth(Birthday);
                }
            }
            set
            {
                _Age = value;
            }
        }

        public bool IsDefault { get; set; }

        public int Identifier { get; set; }

        string _PhotoUrl { get; set; }

        
        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoUrl
        {
            get
            {
                return PaddingUrlPrefix(_PhotoUrl);
            }
            set
            {
                _PhotoUrl = value;
            }
        }
    }


    public class DeleteOrgMemberDTO
    {
        public string MemberID { get; set; }
    }
}
