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
    public class TokenController : ApiController
    {
        /// <summary>
        /// 获取Token
        /// 前置条件：无
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        [IgnoreAuthenticate]
        [ApiOperateNotTrack]
        [IgnoreUserAuthenticate]
        [HttpGet]
        public ApiMessageResult get(string appId, string appSecret)
        {
            
            var result = new TokenResult();
            var account = new SysAccessAccountDTO();

            if (BLL.Sys.Implements.ApiSecurityService.CheckAppAccessAccount(appId, appSecret, out account) == false)
            {
                return new ApiMessageResult { Status = EnumApiStatus.BizError, Msg = "验证失败" };
            }

            result.Token = Guid.NewGuid().ToString("N");

            //保存token
            BLL.Sys.Implements.ApiSecurityService.SetAppToken(result.Token, account);

            return new ApiResult(result) { Data = result };
        }

    }

}
