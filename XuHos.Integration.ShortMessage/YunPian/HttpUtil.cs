using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XuHos.Integration.ShortMessage.YunPian
{
    public class HttpUtil
    {
        public static Result HttpPost(string Url, Dictionary<string,string> parms)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var item in parms)
            {
                if (sb.Length != 0)
                    sb.Append("&");
                sb.Append(HttpUtility.UrlEncode(item.Key) + '='+ HttpUtility.UrlEncode(item.Value));
            }
            return HttpPost(Url, sb.ToString());
        }

        public static Result HttpPost(string Url, string parms)
        {
            byte[] dataArray = Encoding.UTF8.GetBytes(parms.ToString());
            // Console.Write(Encoding.UTF8.GetString(dataArray));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Timeout = 5000;
            request.ReadWriteTimeout = 5000;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = dataArray.Length;
            //request.CookieContainer = cookie;

            try
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(dataArray, 0, dataArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                String res = reader.ReadToEnd();
                reader.Close();
                //写入日志
                var ret = new Result((int)response.StatusCode, res);
                XuHos.Common.LogHelper.WriteTrackLog("TrackSMSApiOperatorLog",
                         requestUri: Url,
                         comments: "发送短信",
                         RequestParamters: parms,
                         requestEnterTime: DateTime.Now,
                         Response: ret.ToString());
                return ret;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine(e);
                    if (response == null)
                    {
                        return new Result(-1, e.ToString());
                    }
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        // Console.WriteLine(text);
                        var ret = new Result((int)httpResponse.StatusCode, text, e);

                        XuHos.Common.LogHelper.WriteTrackLog("TrackSMSApiOperatorLog",
                         requestUri: Url,
                         comments: "发送短信",
                         RequestParamters: parms,
                         requestEnterTime: DateTime.Now,
                         Response: ret.ToString());
                        return ret;
                    }
                }
            }
        }
        public static bool CheckReturnJsonStatus(string retrunJsonResult)
        {
            Dictionary<string, string> jsonMap = Common.JsonHelper.FromJson<Dictionary<string, string>>(retrunJsonResult);
            if (jsonMap.ContainsKey("status") && jsonMap["status"].Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
