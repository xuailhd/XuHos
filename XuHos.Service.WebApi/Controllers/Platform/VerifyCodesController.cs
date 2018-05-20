using XuHos.Common.Cache;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace XuHos.Service.WebApi.Common.Controllers
{
    public class VerifyCodesController : ApiController
    {
        [Infrastructure.Filters.IgnoreAuthenticate]
        [Infrastructure.Filters.IgnoreUserAuthenticate]
        [Infrastructure.Filters.ApiOperateNotTrack]
        [HttpGet]
        [Route("~/VerifyCodes")]
        // GET: VerifyCodes
        public async Task<HttpResponseMessage> Index(string apptoken)
        {

            return await Task.Run<HttpResponseMessage>(() =>
            {
                if (string.IsNullOrEmpty(apptoken))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                   
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                    //生成验证码
                    var v = new XuHos.Common.Utility.VerifyCode();
                    var code = v.CreateVerifyCode(4);
                    var VerifyCodeCacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.VerifyCode, apptoken);
                    code.ToCache(VerifyCodeCacheKey,TimeSpan.FromMinutes(10));
                    result.Content = new ByteArrayContent(v.GetVerifyCodeImage(code));
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    return result;
                }

            });

         

           
        }
    }

}