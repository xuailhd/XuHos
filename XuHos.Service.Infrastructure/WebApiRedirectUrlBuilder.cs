using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.Infrastructure
{
     public class WebApiRedirectUrlBuilder
    {
     
        public override string ToString()
        {
            return Url.ToString();
        }

        string Url { get; set; }

        string GetSign(System.Collections.Generic.SortedList<string, string> reqParams)
        {
            var sb = new StringBuilder();
            foreach (string key in reqParams.Keys)
            {
                if (reqParams[key] != null && !string.IsNullOrWhiteSpace(reqParams[key].ToString()))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("&");
                    }
                    sb.Append(key + "=" + reqParams[key].ToString());
                }
            }

            string sign = XuHos.Common.StringEncrypt.GetMD5_32(sb.ToString()).ToUpper();
            return sign;
        }


        public WebApiRedirectUrlBuilder(string url, 
            System.Collections.Generic.SortedList<string, string> list)
        {
            var sign = GetSign(list); //生成签名;
            var keyPrefix = "x-";
            var sb = new StringBuilder("");
            if (list != null)
            {
                foreach (KeyValuePair<string, string> kv in list)
                {
                    sb.Append(keyPrefix + kv.Key + "=" + System.Web.HttpUtility.UrlEncode(kv.Value) + "&");
                }
            }

            //if (sb.Length > 0)
            //    sb.Remove(sb.Length - 1, 1);

            sb.Append("x-sign=").Append(sign);

            Url = $"{url}?{sb.ToString()}";

        }

    }
}
