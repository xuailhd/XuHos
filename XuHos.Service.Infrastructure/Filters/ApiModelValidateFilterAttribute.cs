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

namespace XuHos.Service.Infrastructure.Filters
{
    /// <summary>
    /// API模型验证
    /// </summary>
    public class ApiModelValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                var dicError = new Dictionary<string, string>();
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        dicError[key] = "";
                        foreach (var p in state.Errors)
                        {
                            if (dicError[key] != "")
                                dicError[key] += ",";
                            dicError[key] += p.ErrorMessage + p.Exception;
                        }
                    }
                }

                var result = new ApiResult(dicError) { Status =   EnumApiStatus.ApiParamModelValidateError, Msg = "数据验证失败" };

                actionContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(JsonHelper.ToJson(result, false, ""),System.Text.Encoding.UTF8, "application/json")
                };
            }

            base.OnActionExecuting(actionContext);
        }
    }
}