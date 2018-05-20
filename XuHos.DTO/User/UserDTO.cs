using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Extensions;
namespace XuHos.DTO
{
    public class UserBaseDTO : Common.ImageBaseDTO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

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

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }


        public string _PhotoUrl { get; set; }

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
                        return PaddingUrlPrefix("images/doctor/default.jpg");
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
    }
    public class UserDTO : UserBaseDTO
    {

        /// <summary>
        /// 自己关系的 memberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 星级
        /// </summary>
        public int Star { get; set; }

        /// <summary>
        /// 评价总数
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 好评数
        /// </summary>
        public int Good { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public int Fans { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 认证
        /// </summary>
        public int Checked { get; set; }

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
        /// 用户级别(0-普通用户、1-会员、2-黑名单)
        /// </summary>
        public int UserLevel { get; set; }

        /// <summary>
        /// 找回密码问题
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 注册终端(0-Web、1-安卓、2-IOS)
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastTime { get; set; }


        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public int identifier { get; set; }

    }
}
