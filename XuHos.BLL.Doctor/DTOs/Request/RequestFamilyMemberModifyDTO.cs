using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Common;
using XuHos.Common.Enum;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestFamilyMemberModifyDTO
    {

        public string FamilyDoctorID { get; set; }

        public string CurrentOperatorDoctorID { get; set; }

        public string MemberID { get; set; }

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
        /// 地址
        /// </summary>
        public string Address { get; set; }

        public int Age
        {
            get; set;
        }

        /// <summary>
        /// 是否默认就诊人(0-非默认、1-默认，一个用户下只有一个默认就诊人)
        /// </summary>        
        public bool IsDefault { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
