using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common;

namespace XuHos.Integration.WechatApp
{
    public class Authorize
    {
        public static AuthTokeDTO WechatAppAuth(string code)
        {
            code = WebUtility.UrlEncode(code);
            var url = $"https://api.weixin.qq.com/sns/jscode2session?appid={Configuration.WechatAppID}&secret={Configuration.WechatAppSec}&js_code={code}&grant_type=authorization_code";
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
                        return JsonConvert.DeserializeObject<AuthTokeDTO>(sr.ReadToEnd());
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
