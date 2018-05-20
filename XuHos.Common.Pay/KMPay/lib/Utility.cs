using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace XuHos.Common.Pay.KMPay
{
    /// <summary>
    /// Utility HTTP请求类
    /// author：Tang
    /// data：2016-7-1
    /// </summary>
    internal class Utility
    {
        public Utility()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 发送HTTP的POST请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="postDataStr">参数</param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr)
        {
            string retString = "";
            var requestBeginTime = DateTime.Now;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                //request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

               

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
            finally
            {
                TrackApiLog.WriteLog("KMPay", Url, "POST", postDataStr, requestBeginTime, retString);
            }
            return retString;
        }

        /// <summary>
        /// 发送HTTP的POST请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="dicPara">参数</param>
        /// <returns></returns>
        public static string HttpPost(string Url, Dictionary<string, string> dicPara)
        {
            var sb = new StringBuilder("");
            if (dicPara != null)
            {
                foreach (KeyValuePair<string, string> kv in dicPara)
                {
                    sb.Append(kv.Key + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
                }
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return HttpPost(Url, sb.ToString());

        }

        /// <summary>
        /// 发送HTTP的GET请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="postDataStr">参数</param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr)
        {
            var requestBeginTime = DateTime.Now;
            var retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
            finally
            {
                TrackApiLog.WriteLog("KMPay", Url, "GET", postDataStr, requestBeginTime, retString);
            }

            return retString;
        }

        /// <summary>
        /// 请求HTTP，并把返回的JSON处理成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Url"></param>
        /// <param name="dicPara"></param>
        /// <returns></returns>
        public static T HttpPostToEntity<T>(string Url, Dictionary<string, string> dicPara)
        {
            string requestString = HttpPost(Url, dicPara);
            return JsonDeserialize<T>(requestString);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            T entity = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
            return entity;
        }
    }

}