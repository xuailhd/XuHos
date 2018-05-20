using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using XuHos.DTO.Common;
using XuHos.DTO;

using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Common.Cache;
using XuHos.Common.Utility;
using XuHos.Extensions;
namespace XuHos.Service.Infrastructure.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {

        public bool WithoutDetailException { get; set; }

        public ApiExceptionFilterAttribute():this(false)
        {

        }

        public ApiExceptionFilterAttribute(bool WithoutDetailException)
        {
            this.WithoutDetailException = WithoutDetailException;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //统计错误次数
            CountError(actionExecutedContext.Request.RequestUri.AbsoluteUri);
            //记录日志
            LogHelper.WriteError(actionExecutedContext.Exception.GetDetailException());
      
            //不包括详细异常信息（生产环境下）
            if (WithoutDetailException)
            {
                var result = EnumApiStatus.BizError.ToApiResultForApiStatus(actionExecutedContext.Exception.GetDetailException().Message);

                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    Content = new StringContent(JsonHelper.ToJson(result, false, ""),
                    System.Text.Encoding.UTF8,
                    "application/json")
                };
            }
            else
            {

                var result = EnumApiStatus.BizError.ToApiResultForApiStatus(
                    actionExecutedContext.Exception.GetDetailException(),
                    actionExecutedContext.Exception.GetDetailException().Message);

                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    Content = new StringContent(JsonHelper.ToJson(result, false, ""),
                    System.Text.Encoding.UTF8,
                    "application/json")
                };
            }

            actionExecutedContext.Response.StatusCode = HttpStatusCode.OK;

            base.OnException(actionExecutedContext);
        }

        /// <summary>
        /// 接口调用错误统计，统计接口稳定性
        
        /// 日期：2016年12月20日
        /// </summary>
        /// <param name="name"></param>
        static void CountError(string name)
        {
            XuHos.Common.Cache.Manager.Instance.HashIncrement("Count.Error", "KMEHosp/" + name, 1);
        }

    }
}