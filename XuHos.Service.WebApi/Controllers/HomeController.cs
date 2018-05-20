using XuHos.BLL;
using XuHos.BLL.User.DTOs;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Entity;
using XuHos.Extensions;
using XuHos.Service.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 别删除， 做运行状态检测用
    /// </summary>
    [IgnoreAuthenticate]
    public class HomeController : ApiController
    {
        [HttpGet]
        [IgnoreAuthenticate]
        [ApiOperateNotTrack]
        public dynamic Index()
        {
            return Ok(new {
                status = "Healthy",
                culture=System.Globalization.CultureInfo.CurrentCulture.DisplayName,
                uiCulture = System.Globalization.CultureInfo.CurrentUICulture.DisplayName,
                currentTime= DateTime.Now
            });
        }

        [HttpGet]
        public ApiResult GetMs(string mobile, string type)
        {
            if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(type))
            {
                return new ApiResult() { Msg = "mobile 或者 type 不能为空" };
            }

            int itype = 0;

            try
            {
                itype = Convert.ToInt32(type);
            }
            catch
            {
                return new ApiResult() { Msg = "type 类型错误" };
            }

            var serviceMsgLog = new BLL.Sys.Implements.SysShortMessageService();
            UserShortMessageLog msgModel = serviceMsgLog.GetLastSMSLog(mobile, itype);
            if (msgModel == null)
            {
                return new ApiResult() { Msg = "没找到改手机号，type为" + itype + "的验证码" };
            }

            return new ApiResult() { Data = msgModel.MsgTitle };
        }

        [HttpGet]
        public ApiResult CheckSign()
        {
            var req = HttpContext.Current.Request;
            var userToken = getRequestParam("usertoken");
            var appToken = getRequestParam("apptoken");
            var nonceStr = getRequestParam("noncestr");
            var encryptUserId = getRequestParam("userid");
            var sign = getRequestParam("sign");//签名            
            var timestamp = getRequestParam("timestamp");//时间戳


            var appkey = BLL.Sys.Implements.ApiSecurityService.GetAppKey(appToken);

            if (string.IsNullOrEmpty(appkey))
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus("没有验证参数");

            }

            //返回结果
            var result = new ApiResult() { Status = EnumApiStatus.BizOK };

            if (!BLL.Sys.Implements.ApiSecurityService.CheckAppToken(appToken))
            {
                result.Status = EnumApiStatus.ApiParamAppTokenExpire;
                result.Msg = "apptoken过期或无效，请重新获取,appkey:" + appkey;

            }
            else if (!BLL.Sys.Implements.ApiSecurityService.CheckSign(appkey, sign, appToken, nonceStr, userToken, encryptUserId))
            {
                string msg = "服务器端：apptoken=" + appToken + "&noncestr=" + nonceStr + (!string.IsNullOrWhiteSpace(encryptUserId) ? ("&userid=" + encryptUserId) : "") +
                (!string.IsNullOrWhiteSpace(userToken) ? ("&usertoken=" + userToken) : "") + "&appkey=" + appkey + ";  服务端加密结果（Sign）：" +
                GetSign("apptoken = " + appToken + " & noncestr = " + nonceStr + (!string.IsNullOrWhiteSpace(encryptUserId) ? (" & userid = " + encryptUserId) : "") +
                (!string.IsNullOrWhiteSpace(userToken) ? ("&usertoken=" + userToken) : "") + "&appkey=" + appkey) + "; 客户端传过来的是：" + sign;

                result.Status = EnumApiStatus.ApiParamSignError;
                result.Msg = "非法请求(签名错误)";
            }
            else
            {
                return "ok".ToApiResultForObject();
            }

            return result;
        }

        static string GetSign(string str)
        {
            string sign = StringEncrypt.GetMD5(str, "UTF-8").ToUpper();
            return sign;
        }

        string getRequestParam(string paramName)
        {
            var req = HttpContext.Current.Request;
            return req.Headers[paramName] ?? req.QueryString["x-" + paramName];
        }
    }
}
