using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Common.DTOs.Response
{
    /// <summary>
    /// 登录后，缓存在服务器的用户信息
    /// </summary>
    public class UserLoginServerTicketDTO
    {
        #region 用户信息
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户级别
        /// </summary>
        public int UserLevel { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }

        /// <summary>
        /// 用户名称（中文）
        /// </summary>
        public string UserCNName { get; set; }
        /// <summary>
        /// 用户名称（英文）
        /// </summary>
        public string UserENName { get; set; }


        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 用户邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 成员编号
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }
        public EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }

        public string UserAccount { get; set; }

        public string UserToken { get; set; }

        public string PhotoUrl { get; set; }

        public int Identifier { get; set;}

        #endregion

        #region 机构信息
        public string OrgID { get; set; }

        public string OrgPath { get; set; }
        #endregion

        #region 医生关联信息
        /// <summary>
        /// 登陆成功后赋值， 缓存，是医生则有值
        /// </summary>
        public string DoctorID { get; set; }


        /// <summary>
        /// 医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生
        /// </summary>
        public int DoctorType { get; set; }
        /// <summary>
        /// 是否审方医生
        /// </summary>
        public bool RecipeDoctor { get; set; }

        /// <summary>
        /// 认证状态（0-未认证、1-已通过、2-未通过、3-认证中）
        /// </summary>
        public int CheckState { get; set; }

        /// <summary>
        /// 北京CA认证
        /// </summary>
        public string BJCA_ClientID { get; set; }

        /// <summary>
        /// 是否有远程会诊资质
        /// </summary>
        public bool ConsulAptitude { get; set; }
        #endregion
    }

}
