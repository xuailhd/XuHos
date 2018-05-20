using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XuHos.Common.Log;
using XuHos.Common.Log.ApiTrack;
using System.Collections.ObjectModel;

namespace XuHos.Service.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class ApiOperateNotTrackAttribute : Attribute
    {
    }

    
    public class ApiOperateTrackAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 自定义参数
        /// </summary>
        public string msg { get; set; }
        public ApiOperateTrackAttribute()
        {

        }


        /// <summary>
        /// 初始化时填入类的说明
        /// </summary>
        /// <param name="message"></param>
        public ApiOperateTrackAttribute(string message)
        {
            msg = message;
        }


        private static readonly string key = "enterTime";
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (SkipLogging(actionContext))
                {
                    return base.OnActionExecutingAsync(actionContext, cancellationToken);

                }
                //记录进入请求的时间
                actionContext.Request.Properties[key] = DateTime.Now.ToBinary();
            }
            catch(Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
            }
        
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
        public string getRequestParam(string paramName)
        {
            var req = HttpContext.Current.Request;
            return req.Headers[paramName] ?? req.QueryString["x-" + paramName];
        }


        /// <summary>
        /// 在请求执行完后 记录请求的数据以及返回数据
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
           
            try
            {

                if (!SkipLogging(actionExecutedContext.ActionContext))
                {
                    object beginTime = null;

                    if (actionExecutedContext.Request.Properties.TryGetValue(key, out beginTime))
                    {
                        DateTime time = DateTime.FromBinary(Convert.ToInt64(beginTime));
                        HttpRequest request = HttpContext.Current.Request;

                        ApiTrackLog apiActionLog = new ApiTrackLog
                        {
                            RequestHeaders = new System.Collections.Generic.Dictionary<string, string>(),
                            Id = Guid.NewGuid(),
                            General = new WebApiTrackLogGeneral()
                            {
                                statusCode = actionExecutedContext.Response == null ? 500 : (int)actionExecutedContext.Response.StatusCode,
                                ///获取action名称
                                actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                                ///获取Controller 名称
                                controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,

                                navigator = request.UserAgent,
                                ///获取用户token
                                userId = SecurityHelper.IsLogin() ? SecurityHelper.LoginUser.UserID : "",
                                ///获取访问的ip
                                ip = request.UserHostAddress,
                                remoteHostName = request.UserHostName,
                                urlReferrer = request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "",
                                remoteBrowser = request.Browser.Browser + " - " + request.Browser.Version + " - " + request.Browser.Type,

                                comments = msg,
                                requestUri = request.Url.AbsoluteUri,
                                requestType = request.RequestType,
                                appId = BLL.Sys.Implements.ApiSecurityService.GetAppId(getRequestParam("apptoken")),
                            },
                            Statistics = new WebApiTrackLogStatistics()
                            {
                                ///获取action开始执行的时间
                                enterTime = time,
                                ///获取执行action的耗时
                                costTime = (DateTime.Now - time).TotalMilliseconds,
                            },
                            ///获取request提交的参数
                            RequestParamters = GetRequestValues(actionExecutedContext),
                            //获取response响应的结果
                            Response = GetResponseValues(actionExecutedContext),

                        };

                        if (HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null)
                        {
                            for (var i = 0; i < request.Headers.Keys.Count; i++)
                            {
                                var key = request.Headers.Keys[i];

                                apiActionLog.RequestHeaders.Add(key, request.Headers[key]);
                            }
                        }

                        XuHos.Common.LogHelper.WriteTrackLog("TrackKMEHospApiOperateLog", apiActionLog);
                    }
                }

            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
            }
         
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

        }

  

        /// <summary>
        /// 读取request 的提交内容
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetRequestValues(HttpActionExecutedContext actionExecutedContext)
        {

            Stream stream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result;
            stream.Position = 0;
            Encoding encoding = Encoding.UTF8;
            /*
                这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
                因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0;
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }

        /// <summary>
        /// 读取action返回的result
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetResponseValues(HttpActionExecutedContext actionExecutedContext)
        {
            if(actionExecutedContext.Response ==null || actionExecutedContext.Response.Content==null)
            {
                return string.Empty;
            }
            Stream stream = actionExecutedContext.Response.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            /*
            这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
            因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            var reader = new StreamReader(stream, encoding);
            string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0; 
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            stream.Position = 0;
            return result;
        }
        /// <summary>
        /// 判断类和方法头上的特性是否要进行Action拦截
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool SkipLogging(HttpActionContext actionContext)
        {
            var atts = GetActionOrControllerAttributes<ApiOperateNotTrackAttribute>(actionContext);

            if (atts == null || atts.Count <= 0)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 获取action或Controller的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private  static Collection<T> GetActionOrControllerAttributes<T>(HttpActionContext actionContext)
            where T : class
        {
            var actionIgnoreAuthcate = actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<T>();
            if (actionIgnoreAuthcate == null || actionIgnoreAuthcate.Count <= 0)
                actionIgnoreAuthcate = actionContext.ActionDescriptor.GetCustomAttributes<T>();
            return actionIgnoreAuthcate;
        }

    }



}
