using XuHos.BLL.Common.DTOs.Response;
using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace XuHos.Service.Infrastructure.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizationFilterAttribute : AuthorizeAttribute//, AuthorizationFilterAttribute
    {
        public ApiMessageResult OnAppAuthorization(            
            string appToken,
            string nonceStr,
            string sign,
            string userToken="",
            string encryptUserId = "",
            string appId = "")
        {
            //返回结果
            var result = new ApiMessageResult() { Status =  EnumApiStatus.BizOK };

            // 根据appToken判断是否忽略api接口验证
            if (!string.IsNullOrEmpty(appToken) && BLL.Sys.Implements.ApiSecurityService.IsIgnoreAuthByAppToken(appToken))
            {
                HttpContext.Current.Items["AccessAccount"] = BLL.Sys.Implements.ApiSecurityService.GetAppToken(appToken);
                return result;
            }

            // 根据appId判断是否忽略api接口验证
            if (!string.IsNullOrEmpty(appId) && BLL.Sys.Implements.ApiSecurityService.IsIgnoreAuthByAppId(appId))
            {
                if (HttpContext.Current.Items["AccessAccount"] != null)
                {
                    return result;
                }
                else
                {
                    var account = BLL.Sys.Implements.ApiSecurityService.GetAppAccessAccountByAppID(appId);
                    var AccessAccount=BLL.Sys.Implements.ApiSecurityService.CreateAppToken(Guid.NewGuid().ToString("N"), account);
                    HttpContext.Current.Items["AccessAccount"] = AccessAccount;
                    return result;
                }
            }

            var appkey = BLL.Sys.Implements.ApiSecurityService.GetAppKey(appToken);

            if (!BLL.Sys.Implements.ApiSecurityService.CheckAppToken(appToken))
            {
                result.Status =  EnumApiStatus.ApiParamAppTokenExpire;
                result.Msg = "apptoken过期或无效，请重新获取";                
            }
            else if (!BLL.Sys.Implements.ApiSecurityService.CheckNonceStr(nonceStr, appToken))
            {
                result.Status =  EnumApiStatus.ApiRepeatedAccess;
                result.Msg = "非法请求(重复请求)";
            }
            else if (!BLL.Sys.Implements.ApiSecurityService.CheckSign(appkey,sign, appToken, nonceStr, userToken, encryptUserId))
            {
                result.Status =  EnumApiStatus.ApiParamSignError;
                result.Msg = "非法请求(签名错误)";
            }
            else
            {
                //存入通过认证的接入用户信息
                HttpContext.Current.Items["AccessAccount"] = BLL.Sys.Implements.ApiSecurityService.GetAppToken(appToken);
            }

            return result;

        }

        public ApiMessageResult OnUserAuthorization(
            string appToken,
            string nonceStr,
            string sign,
            string userToken,
            string encryptUserId,
            string appId,
             UserAuthenticateAttribute userAuthenticateAttribute)
        {
            //返回结果
            var result = new ApiMessageResult() { Status = 0 };


            if (!string.IsNullOrEmpty(encryptUserId))
            {
                #region //BAT用户通过第三方登录没有usertoken时，通过这种方式验证用户(要使用这种方式，必须在SysAccessAccounts表分配一个UserKey)

                var userId = "";

                if (!string.IsNullOrEmpty(appToken))
                {
                    userId = BLL.Sys.Implements.ApiSecurityService.DecryptUserIdByAppToken(encryptUserId, appToken);
                }
                else if(!string.IsNullOrEmpty(appId))
                {
                    userId = BLL.Sys.Implements.ApiSecurityService.DecryptUserIdByAppId(encryptUserId,appId);
                }

                if (string.IsNullOrEmpty(userId))
                {
                    result = new ApiMessageResult() { Status =  EnumApiStatus.ApiUserNotLogin, Msg = "非法请求，验证userId失败" };
                }
                else
                {
                    //通过固定规则确定Token
                    userToken = $"uid-" + userId;

                    UserLoginServerTicketDTO serverTicket = null;

                    //首次调用，自动完成登录操作
                    if (!BLL.Sys.Implements.ApiSecurityService.CheckUserTicket(userToken))
                    {
                        //通过用户编号获取票据
                        serverTicket = new BLL.User.Implements.UserService().GetUserLoginServerTicket(userId);
                        //设置Token
                        serverTicket.UserToken = userToken;

                        BLL.Sys.Implements.ApiSecurityService.SetUserTicket(serverTicket);
                    }
                    else
                    {
                        //获取Token
                        serverTicket = ApiSecurityService.GetUserTicket(userToken);
                    }

                    //用户不存在
                    if (serverTicket != null)
                    {

                        //扩展 药店用户，权限等同 用户
                        if (userAuthenticateAttribute!=null
                            && userAuthenticateAttribute.IsValidUserType == true
                            && serverTicket.UserType != userAuthenticateAttribute.UserType
                            && !(serverTicket.UserType == EnumUserType.Drugstore 
                            && userAuthenticateAttribute.UserType == EnumUserType.User))
                        {
                            result = new ApiMessageResult() { Status =  EnumApiStatus.ApiUserUnauthorized, Msg = "用户无权限访问" };
                        }
                        else
                        {
                            //存入通过认证的登录用户信息
                            HttpContext.Current.Items["LoginUser"] = serverTicket;
                        }
                    }
                    else
                    {
                        result = new ApiMessageResult() { Status =  EnumApiStatus.ApiUserNotLogin, Msg = "用户不存在" };
                    }
                }

                #endregion
            }
            else
            {
                #region //通过API正常登录，有usertoken的验证方式
                //用户是否登录(根据userToken取用户信息)
                if (!BLL.Sys.Implements.ApiSecurityService.CheckUserTicket(userToken))
                {
                    result = new ApiMessageResult() { Status =  EnumApiStatus.ApiUserNotLogin, Msg = "用户未登录" };
                }
                else
                {
                    var loginUser = ApiSecurityService.GetUserTicket(userToken);

                    //扩展 药店用户，权限等同 用户
                    if (userAuthenticateAttribute!=null 
                        && userAuthenticateAttribute.IsValidUserType 
                        && loginUser.UserType != userAuthenticateAttribute.UserType 
                        && !(loginUser.UserType == EnumUserType.Drugstore && userAuthenticateAttribute.UserType == EnumUserType.User))
                    {
                        result = new ApiMessageResult() { Status =  EnumApiStatus.ApiUserUnauthorized, Msg = "用户无权限访问" };
                    }
                    else
                    {
                        //存入通过认证的登录用户信息
                        HttpContext.Current.Items["LoginUser"] = loginUser;
                    }
                }

                // 根据appToken判断是否忽略api接口验证
                if (!string.IsNullOrEmpty(appToken) && BLL.Sys.Implements.ApiSecurityService.IsIgnoreAuthByAppToken(appToken))
                {
                    result = new ApiMessageResult() { Status = 0 };
                    return result;
                }

                #endregion
            }

            return result;

        }

        string getRequestParam(string paramName)
        {
            var req = HttpContext.Current.Request;
            var str=req.Headers[paramName] ?? req.QueryString["x-" + paramName];
            return str;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
          
            var req = HttpContext.Current.Request;
            var appId = getRequestParam("appid");
            var userToken = getRequestParam("usertoken");
            var appToken = getRequestParam("apptoken");
            var nonceStr = getRequestParam("noncestr");
            var encryptUserId = getRequestParam("userid");
            var sign = getRequestParam("sign");//签名            
            var timestamp = getRequestParam("timestamp");//时间戳

            if (!IsIgnoreAuthenticate(actionContext))
            {
                var result = OnAppAuthorization(appToken, nonceStr, sign, userToken, encryptUserId, appId);

                if (result.Status != 0)
                {
                    actionContext.Response = new HttpResponseMessage() { Content = new StringContent(JsonHelper.ToJson(result, false, ""), Encoding.UTF8, "application/json") };
                    return;
                }
                else if(!string.IsNullOrEmpty(nonceStr) && !string.IsNullOrEmpty(appToken))
                {
                    BLL.Sys.Implements.ApiSecurityService.SetNonceStr(nonceStr, appToken);
                }
            }

            UserAuthenticateAttribute userAuthenticate = null;

            if (!IsIgnoreUserAuthenticate(actionContext,out userAuthenticate))
            {
                var result = OnUserAuthorization(appToken, nonceStr, sign, userToken, encryptUserId, appId, userAuthenticate);

                if (result.Status != 0)
                {
                    actionContext.Response = new HttpResponseMessage() { Content = new StringContent(JsonHelper.ToJson(result, false, ""), Encoding.UTF8, "application/json") };
                    return;
                }
                else
                {

                    BLL.Sys.Implements.ApiSecurityService.SetUserTokenExpire(userToken);
                }
               
            }
        }

        /// <summary>
        /// 是否忽略接入API认证
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        bool IsIgnoreAuthenticate(HttpActionContext actionContext)
        {
            var actionIgnoreAuthcate = GetActionOrControllerAttributes<IgnoreAuthenticateAttribute>(actionContext);
            if (actionIgnoreAuthcate == null || actionIgnoreAuthcate.Count <= 0)
                return false;

            return true;
        }

        /// <summary>
        /// 是否忽略用户登录认证
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        bool IsIgnoreUserAuthenticate(HttpActionContext actionContext,out UserAuthenticateAttribute userAuthenticate)
        {
            var userAuthenticates = GetActionOrControllerAttributes<UserAuthenticateAttribute>(actionContext);

            userAuthenticate = null;

            //控制器或者Action上标识了用户验证
            if (userAuthenticates != null && userAuthenticates.Count > 0)
            {
                userAuthenticate = userAuthenticates[0];

                //查找是否有特殊标识
                var actionIgnoreAuthcate = GetActionOrControllerAttributes<IgnoreUserAuthenticateAttribute>(actionContext);
                //没有找到特殊标识
                if (actionIgnoreAuthcate == null || actionIgnoreAuthcate.Count <= 0)
                {
                    //不需要忽略用户认证
                    return false;
                }
                else
                {
                    //忽略用户认证
                    return true;
                }
            }
            else
            {
                userAuthenticate = new UserAuthenticateAttribute();
                userAuthenticate.IsValidUserType = false;
                return true;
            }
        }


        /// <summary>
        /// 获取action或Controller的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        Collection<T> GetActionOrControllerAttributes<T>(HttpActionContext actionContext)
            where T : class
        {
            var actionIgnoreAuthcate = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<T>();
            if (actionIgnoreAuthcate == null || actionIgnoreAuthcate.Count <= 0)
                actionIgnoreAuthcate = actionContext.ActionDescriptor.GetCustomAttributes<T>();
            return actionIgnoreAuthcate;
        }
    }
}