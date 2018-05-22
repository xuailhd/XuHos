using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Response
{
    public class ResponseUserMemberDTO
    {
        /// <summary>
        /// 成员ID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 对接会员系统
        /// </summary>
        public string OrgID { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

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

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 证件类型（0-身份证）
        /// </summary>
        public EnumUserCardType IDType { get; set; }

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
        /// 街道
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// 地址
        /// </summary>    
        public string Address { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>

        public string Email { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>

        public string PostCode { get; set; }

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
                    if (!string.IsNullOrEmpty(Birthday))
                    {
                        DateTime dt;

                        if (DateTime.TryParse(Birthday, out dt) == true)
                        {
                            _Age = DateTime.Now.Year - dt.Year - 1;
                            if (DateTime.Now.Month > dt.Month || (DateTime.Now.Month == dt.Month && DateTime.Now.Day >= dt.Day))
                            {
                                _Age += 1;
                            }
                            return _Age;
                        }
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            set
            {
                _Age = value;
            }
        }

        public bool IsDefault { get; set; }


        public string ProvinceRegionID { get; set; }

        public string CityRegionID { get; set; }

        public string DistrictRegionID { get; set; }

        public string TownRegionID { get; set; }

    }
}
