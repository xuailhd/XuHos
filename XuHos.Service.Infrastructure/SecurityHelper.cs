using XuHos.Common.Cache;
using XuHos.DTO.Common;
using XuHos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using XuHos.Common.Enum;
using XuHos.BLL.Common.DTOs.Response;

namespace XuHos.Service.Infrastructure
{
    public class SecurityHelper
    {
        static string cookiePrefix = XuHos.Common.Config.ConfigHelper.GetAppSetting("Cookie.Prefix");

        static string UserToken
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var userTokenStr = HttpContext.Current.Request.Headers["usertoken"];

                    if (string.IsNullOrWhiteSpace(userTokenStr))
                    {
                        //获取Cookie
                        HttpCookie authCookie = HttpContext.Current.Request.Cookies[$"{cookiePrefix}usertoken"];

                        if (authCookie != null)
                        {
                            return authCookie.Value;
                        }
                        else
                        {
                            authCookie = HttpContext.Current.Request.Cookies[$"{cookiePrefix}userToken"];

                            if (authCookie != null)
                            {
                                return authCookie.Value;
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                    else
                    {
                        return userTokenStr;
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        static string AppToken
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var appTokenStr = HttpContext.Current.Request.Headers["apptoken"];

                    if (string.IsNullOrWhiteSpace(appTokenStr))
                    {
                        //获取Cookie
                        HttpCookie authCookie = HttpContext.Current.Request.Cookies[$"{cookiePrefix}apptoken"];

                        if (authCookie != null)
                        {
                            return authCookie.Value;
                        }
                        else
                        {
                            authCookie = HttpContext.Current.Request.Cookies[$"{cookiePrefix}apptoken"];

                            if (authCookie != null)
                            {
                                return authCookie.Value;
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                    else
                    {
                        return appTokenStr;
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 登录用户信息
        /// </summary>
        public static UserLoginServerTicketDTO LoginUser
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    #region webApi获取当前用户
                    var user = HttpContext.Current.Items["LoginUser"] as UserLoginServerTicketDTO;
                    if (user != null)
                    {
                        return user;
                    }
                    #endregion

                    if (UserToken != "")
                    {
                        user = XuHos.BLL.Sys.Implements.ApiSecurityService.GetUserTicket(UserToken);

                        if (user == null)
                        {
                            return new UserLoginServerTicketDTO() { UserID = "", UserType = EnumUserType.Default };
                        }
                        else
                        {
                            user.UserToken = UserToken;
                            return user;
                        }
                    }
                    else
                    {
                        return new UserLoginServerTicketDTO() { UserID = "", UserType = EnumUserType.Default };
                    }
                }
                else
                {
                    return new UserLoginServerTicketDTO() { UserID = "", UserType = EnumUserType.Default };
                }
            }
        }

        /// <summary>
        /// 检查是否已经登录        
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin(EnumUserType userType = EnumUserType.Default)
        {
            var user = LoginUser;

            if (user != null && 
                user.UserID != "" 
                && (user.UserType == userType || userType == EnumUserType.Default))  //没有设置用户角色，则通过
                return true;
            return false;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void SignOut()
        {
            //清除Cookie
            HttpContext.Current.Response.Cookies.Clear();
            HttpContext.Current.Request.Cookies.Clear();
            HttpContext.Current.Items["LoginUser"] = null;
            BLL.Sys.Implements.ApiSecurityService.RemoveUserToken(UserToken);
        }

        /// <summary>
        /// 登入，保存用户信息
        /// </summary>
        /// <param name="userTokenStr"></param>
        /// <param name="user"></param>
        public static void SignIn(UserLoginServerTicketDTO user)
        {
            BLL.Sys.Implements.ApiSecurityService.SetUserTicket(user);
        }


        /// <summary>
        /// 当前请求的apptoken信息
        /// </summary>
        /// <returns></returns>
        public static XuHos.BLL.Common.DTOs.Response.ResponseToken GetCurrentAppToken()
        {
            if (HttpContext.Current != null && HttpContext.Current.Items["AccessAccount"] != null)
                return HttpContext.Current.Items["AccessAccount"] as XuHos.BLL.Common.DTOs.Response.ResponseToken;

            return null;
        }

    }
}

