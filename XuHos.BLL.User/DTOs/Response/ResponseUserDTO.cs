using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
namespace XuHos.BLL.User.DTOs.Response
{

    public class ResponseUserDTO :ImageBaseDTO
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 自己关系的 memberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户中文名
        /// </summary>
        public string UserCNName { get; set; }

        /// <summary>
        /// 用户英文名
        /// </summary>
        public string UserENName { get; set; }

        /// <summary>
        /// 用户类型(0-管理员、1-患者、2-医生)
        /// </summary>
        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        public string IDNumber { get; set; }
        public EnumUserCardType IDType { get; set; }
        public EnumUserGender Gender { get; set; }
      

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }

         string _PhotoUrl { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PhotoUrl))
                {
                    if (UserType == EnumUserType.Doctor)
                    {
                        return PaddingUrlPrefix("images/doctor/default.jpg");
                    }
                    else
                    {
                        return PaddingUrlPrefix("images/doctor/unknow.png");
                    }

                }
                else
                {
                    return PaddingUrlPrefix(_PhotoUrl);
                }
            }
            set
            {
                _PhotoUrl = value;
            }
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 注销时间
        /// </summary>
        public DateTime CancelTime { get; set; }

        /// <summary>
        /// 用户状态(0-正常、1-冻结、2-销户)
        /// </summary>
        public EnumUserState UserState { get; set; }

        /// <summary>
        /// 用户级别(0-普通用户、10 以上 Vip等级)
        /// </summary>
        public int UserLevel { get; set; }


        /// <summary>
        /// 注册终端(0-Web、1-安卓、2-IOS)
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public int Identifier { get; set; }
        public string OrgID { get; set; }
        public string OrgPath { get; set; }

        public DateTime? ExpiredTime { get; set; }

        public bool IsExpired
        {
            get
            {
                return this.ExpiredTime.HasValue && this.ExpiredTime < DateTime.Now;
            }
        }
        /// <summary>
        /// 时代帐号或者创建者ID
        /// </summary>
        public string CreateUserID { get; set; }
    }
}
