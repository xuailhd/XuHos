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
using XuHos.BLL.User.DTOs;
using XuHos.BLL.User.Implements;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.User.DTOs.Response;
using XuHos.Service.Infrastructure;

namespace XuHos.WebApi.Controllers
{
    public class WechatAppController : ApiBaseController
    {
        private readonly UserService userService;

        public WechatAppController()
        {
            userService = new UserService();
        }

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
           "mobile":"15711112222"  --有值得话代表已经绑定了手机号
          },"Total":0,"Status":0,"Msg":""}
       **/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [IgnoreAuthenticate]
        [HttpGet]
        public ApiResult AppAuth(string code)
        {
            var authToken = XuHos.Integration.WechatApp.Authorize.WechatAppAuth(code);

            if(authToken == null || string.IsNullOrEmpty(authToken.openid))
            {
                return false.ToApiResultForBoolean();
            }

            var logdto =  userService.Login(new RequestUserLoginDTO()
            {
                OpenID = authToken.openid,
                AppID = XuHos.Integration.WechatApp.Configuration.WechatAppID
            });

            if (logdto.Data != null)
            {
                var user = (ResponseUserTicketReturnDTO)logdto.Data;
                authToken.mobile = user.Mobile;
            }

            return authToken.ToApiResultForObject();
        }

        /**
       * @api {GET} http://39.108.180.207/WechatApp/BindMobile 101102/小程序绑定手机号
       * @apiGroup 101 WechatAPP_Authorize
       * @apiDescription 如果校验Code，没有返回手机号，则绑定手机号 
       * @apiPermission 已经校验Code
       * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
       * @apiHeader {String} usertoken 校验小程序Code 返回的 session_key
       * @apiParam {string} Mobile 要绑定的手机号，测试阶段暂时不用验证码
       * @apiParamExample {json} 请求样例：
       *{
               "Mobile": "XXXX", 
        *}
       * @apiSuccess (Response) {String} Msg 提示信息 
       * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
       * @apiSuccess (Response) {int} Total 总记录数
       * @apiSuccess (Response) {Array} Data 数据
       * @apiSuccessExample {json} 返回样例:
       *{"Data":{
          "UserID":"XXXXX",
          },"Total":0,"Status":0,"Msg":""}
       **/
        [HttpPost]
        public ApiResult BindMobile([FromBody]RequestBingdMobileDTO model)
        {
            if (model != null)
            {
                var user = CurrentOperatorUser;

                if (user == null)
                {
                    return EnumApiStatus.BizError.ToApiResultForApiStatus();
                }
                model.OpenID = user.OpenID;
                model.AppID = XuHos.Integration.WechatApp.Configuration.WechatAppID;
                model.UserToken = SecurityHelper.UserToken;
                return userService.BindMobile(model);
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }
    }

}
