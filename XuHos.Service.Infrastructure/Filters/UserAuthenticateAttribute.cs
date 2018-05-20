using XuHos.DTO.Common;
using XuHos.DTO;

using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Common.Cache;
using XuHos.Common.Utility;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace XuHos.Service.Infrastructure.Filters
{

    /// <summary>
    /// 登录用户的权限认证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UserAuthenticateAttribute : Attribute
    {
        public UserAuthenticateAttribute()
        {
            UserType = EnumUserType.User;
            IsValidUserType = true;
        }

        /// <summary>
        /// 是否验证角色
        /// </summary>
        public bool IsValidUserType { get; set; }


        /// <summary>
        /// 用户类型，患者或医生
        /// </summary>
        public EnumUserType UserType { get; set; }
    }
}