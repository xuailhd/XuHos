using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Request
{
    public class RequestUserMemberDTO
    {
        public RequestUserMemberDTO()
        {
            this.IsDefault = null;
            this.IDNumber = null;
            this.Gender = EnumUserGender.Other;
            this.Marriage = EnumUserMaritalStatus.Married;
        }

        public string UserID { get; set; }

        public string MemberID { get; set; }
        /// <summary>
        /// 对接会员系统
        /// </summary>
        public string OrgID { get; set; }

        #region 必填

        [Required]
        /// <summary>
        /// 成员姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 关系（0-自己、1-配偶、2-父亲、3-母亲、4-儿子、5女儿、6-其他）
        /// </summary>
        [Required]
        public EnumUserRelation Relation { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        [Required]
        public EnumUserGender Gender { get; set; }

        /// <summary>
        /// 婚姻情况(0-未婚、1-已婚、2-未知)
        /// </summary>
        [Required]
        public EnumUserMaritalStatus Marriage { get; set; }
        #endregion

        #region 可选

        /// <summary>
        /// 出生日期（可选）
        /// </summary>
        [MaxLength(10)]
        public string Birthday { get; set; }

        /// <summary>
        /// 手机号码（可选）
        /// </summary>
        [MaxLength(11)]
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }  

        /// <summary>
        /// 证件号码（可选）
        /// </summary>
        public EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 证件号码（可选）
        /// </summary>
        [MaxLength(32)]
        public string IDNumber { get; set; }

        /// <summary>
        /// 国籍（可选）
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// 省（可选）
        /// </summary>
        public string Province { get; set; }

        public string ProvinceRegionID { get; set; }

        /// <summary>
        /// 市（可选）
        /// </summary>
        public string City { get; set; }

        public string CityRegionID { get; set; }

        /// <summary>
        /// 区（可选）
        /// </summary>
        public string District { get; set; }

        public string DistrictRegionID { get; set; }

        /// <summary>
        /// 村，街道
        /// </summary>
        public string Town { get; set; }

        public string TownRegionID { get; set; }


        /// <summary>
        /// 村，居委
        /// </summary>
        public string Village { get; set; }

        public string VillageRegionID { get; set; }

        /// <summary>
        /// 地址（可选）
        /// </summary>
        [MaxLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// 电子邮箱（可选）
        /// </summary>
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// 邮编（可选）
        /// </summary>
        [MaxLength(6)]
        public string PostCode { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
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
                    _Age = XuHos.Common.ToolHelper.GetAgeFromBirth(Birthday);
                    return _Age;
                }
            }
            set
            {
                _Age = value;
            }
        }

        /// <summary>
        /// 是否默认就诊人(0-非默认、1-默认，一个用户下只有一个默认就诊人)
        /// </summary>        
        public bool? IsDefault { get; set; }

        public string SMSVerifyCode { get; set; }
        #endregion
    }
}
