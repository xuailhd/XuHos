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
        public ApiMessageResult OnUserAuthorization(string nonceStr, string userToken,
             UserAuthenticateAttribute userAuthenticateAttribute)
        {
            //返回结果
            var result = new ApiMessageResult() { Status = 0 };

            if (!string.IsNullOrEmpty(userToken))
            {
                if (!BLL.Sys.Implements.ApiSecurityService.CheckNonceStr(nonceStr, userToken))
                {
                    result.Status = EnumApiStatus.ApiRepeatedAccess;
                    result.Msg = "非法请求(重复请求)";
                    return result;
                }
            }

            #region //通过API正常登录，有usertoken的验证方式
            //用户是否登录(根据userToken取用户信息)
            if (!BLL.Sys.Implements.ApiSecurityService.CheckUserTicket(userToken))
            {
                result = new ApiMessageResult() { Status = EnumApiStatus.ApiUserNotLogin, Msg = "用户未登录" };
            }
            else
            {
                var loginUser = ApiSecurityService.GetUserTicket(userToken);

                //扩展 药店用户，权限等同 用户
                if (userAuthenticateAttribute != null
                    && userAuthenticateAttribute.IsValidUserType
                    && loginUser.UserType != userAuthenticateAttribute.UserType)
                {
                    result = new ApiMessageResult() { Status = EnumApiStatus.ApiUserUnauthorized, Msg = "用户无权限访问" };
                }
                else
                {
                    //存入通过认证的登录用户信息
                    HttpContext.Current.Items["LoginUser"] = loginUser;
                }
            }
            #endregion
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
            var userToken = getRequestParam("usertoken");
            var nonceStr = getRequestParam("noncestr");

            UserAuthenticateAttribute userAuthenticate = null;

            if (!IsIgnoreUserAuthenticate(actionContext,out userAuthenticate))
            {
                var result = OnUserAuthorization(nonceStr, userToken, userAuthenticate);

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