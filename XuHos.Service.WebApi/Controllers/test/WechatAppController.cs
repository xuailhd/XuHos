using XuHos.DTO.Platform;
using XuHos.DTO.Common;
using XuHos.DTO;

using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Common.Cache;
using XuHos.Common.Utility;

using XuHos.Service.Infrastructure.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace XuHos.WebApi.Controllers
{
    public class WechatAppController : ApiController
    {
        /**
       * @api {GET} http://39.108.180.207/WechatApp/AppAuth 101101/校验小程序Code
       * @apiGroup 101 WechatAPP_Authorize
       * @apiDescription 校验小程序Code 
       * @apiPermission 所有人
       * @apiParam {string} code 临时登录凭证 
       * @apiParamExample {json} 请求样例：
       *     /WechatApp/AppAuth?code=xxxx
       * @apiSuccess (Response) {String} Msg 提示信息 
       * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
       * @apiSuccess (Response) {int} Total 总记录数
       * @apiSuccess (Response) {Array} Data 数据
       * @apiSuccessExample {json} 返回样例:
       *{"Data":{
          "openid":"XXXXX",
          "session_key":"XXX",
          "unionid":"XXX",
          },"Total":0,"Status":0,"Msg":""}
       **/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [HttpGet]
        public ApiResult AppAuth(string code)
        {
            var authToken = XuHos.Integration.WechatApp.Authorize.WechatAppAuth(code);

            if(authToken == null || string.IsNullOrEmpty(authToken.openid))
            {
                return false.ToApiResultForBoolean();
            }

            return authToken.ToApiResultForObject();
        }
    }

}
