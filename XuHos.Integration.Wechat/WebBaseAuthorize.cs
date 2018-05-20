using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common;
using XuHos.Common.Config.Sections;
using Newtonsoft.Json;

namespace XuHos.Integration.Wechat
{
    namespace WebBaseAuthorize
    {
        public enum EnumScope
        {
            snsapi_base,

            snsapi_userinfo,
        }

        public class CodeResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonProperty("openid")]
            public string OpenId { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }

            [JsonProperty("errcode")]
            public string ErrCode { get; set; }

            [JsonProperty("errmsg")]
            public string ErrMsg { get; set; }
        }

        public class WebBaseAuthorizeClient
        {
            public IWechatConfig WechatConfig { get; set; }

            public WebBaseAuthorizeClient(IWechatConfig wechatConfig)
            {
                WechatConfig = wechatConfig;
            }

            public string GenerateRedirctUrl(string returnUrl, EnumScope scope = EnumScope.snsapi_base, string state = null)
            {
                returnUrl = WebUtility.UrlEncode(returnUrl);
                if (!string.IsNullOrWhiteSpace(state))
                {
                    state = WebUtility.UrlEncode(state);
                }
                return $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={WechatConfig.AppID}&redirect_uri={returnUrl}&response_type=code&scope={scope:G}&state={state}#wechat_redirect";
            }

            public string GetOpenId(string code)
            {
                return GetAccessToken(code)?.OpenId;
            }

            public CodeResponse GetAccessToken(string code)
            {
                code = WebUtility.UrlEncode(code);
                var url = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={WechatConfig.AppID}&secret={WechatConfig.AppSecret}&code={code}&grant_type=authorization_code";
                var request = WebRequest.CreateHttp(url);
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if ((int)response.StatusCode / 100 != 2)
                        {
                            return null;
                        }
                        using (var sr = new StreamReader(response.GetResponseStream()))
                        {
                            return JsonConvert.DeserializeObject<CodeResponse>(sr.ReadToEnd());
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.WriteError(e);
                }
                return null;
            }
        }
    }
}