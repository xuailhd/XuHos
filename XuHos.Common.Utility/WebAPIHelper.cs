using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using XuHos.Common;
using XuHos.Common.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;

namespace XuHos.Common.Utility
{
    public static class WebAPIHelper
    {
        static WebAPIHelper()
        {
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
            System.Net.ServicePointManager.DefaultConnectionLimit = 100;
            System.Net.ServicePointManager.MaxServicePointIdleTime = 2000;
            System.Net.ServicePointManager.MaxServicePoints = 1000;
        }


        #region 私有
        private static bool RemoteCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static string GetSign(System.Collections.Generic.SortedList<string, string> reqParams)
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

            string sign = StringEncrypt.GetMD5(sb.ToString(), "UTF-8").ToUpper();
            return sign;
        }

        private static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <returns></returns>
        private static T JsonDeserialize<T>(string jsonString)
        {
            try
            {
                T entity = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
                return entity;
            }
            catch(Exception ex)
            {
                return default(T);
            }
        }


        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <returns></returns>
        private static string JsonSerialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }


        private static string BuildParams(Dictionary<string, string> dicPara)
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
            return sb.ToString();
        }
        #endregion

        #region POST请求
  

        /// <summary>
        /// 发送HTTP的POST请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="paramStr">参数</param>
        /// <returns></returns>
        public static  string HttpPost(
            string Url,
            byte[] buffer, 
            SortedList<string, string> head = null, 
            string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = contentType;

            if (head != null)
            {
                foreach (var key in head.Keys)
                {
                    request.Headers.Add(key, head[key]);
                }
            }

            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(buffer, 0, buffer.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 发送HTTP的POST请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="paramStr">参数</param>
        /// <returns></returns>
        public static T HttpPost<T>(
            string Url,
            byte[] buffer,
            SortedList<string, string> head = null,
            string contentType = "application/x-www-form-urlencoded")
        {
            var response = HttpPost(Url, buffer, head, contentType);
            return JsonDeserialize<T>(response);

        }

        /// <summary>
        /// 发送HTTP的POST请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="body">参数</param>
        /// <returns></returns>
        public static string HttpPost(
            string Url, 
            string body, 
            SortedList<string, string> head = null, 
            string contentType = "application/x-www-form-urlencoded")
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(body);
            return HttpPost(Url, buffer, head, contentType);
        }

        /// <summary>
        /// 请求HTTP，并把返回的JSON处理成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">HTTP地址</param>
        /// <param name="body">body参数</param>
        /// <param name="head">head参数</param>
        /// <returns></returns>
        public static T HttpPost<T>(string url, string body = null, SortedList<string, string> head = null, string contentType = "application/x-www-form-urlencoded")
        {
            if (string.IsNullOrEmpty(body))
            {
                body = "";
            }
            string requestString = HttpPost(url, body, head);
            return JsonDeserialize<T>(requestString);
        }

        /// <summary>
        /// 发送HTTP的POST请求
        /// </summary>
        /// <param name="url">HTTP地址</param>
        /// <param name="body">body参数</param>
        /// <param name="head">head参数</param>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> body, SortedList<string, string> head = null)
        {
            return HttpPost(url, BuildParams(body), head);
        }

        /// <summary>
        /// 请求HTTP，并把返回的JSON处理成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">HTTP地址</param>
        /// <param name="body">body参数</param>
        /// <param name="head">head参数</param>
        /// <returns></returns>
        public static T HttpPost<T>(string url, Dictionary<string, string> body, SortedList<string, string> head = null)
        {
            string requestString = HttpPost(url, body, head);
            return JsonDeserialize<T>(requestString);
        }

        /// <summary>
        /// 请求HTTP，并把返回的JSON处理成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">HTTP地址</param>
        /// <param name="body">body参数</param>
        /// <param name="head">head参数</param>
        /// <returns></returns>
        public static T HttpPost<T>(string url, object body, SortedList<string, string> head = null, string contentType = "application/json; charset=utf-8")
        {
            string requestString = HttpPost(url, JsonConvert.SerializeObject(body), head, contentType);
            return JsonDeserialize<T>(requestString);
        }  
        #endregion

        #region GET请求
        /// <summary>
        /// 发送HTTP的GET请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="paramStr">参数</param>
        /// <returns></returns>
        public static string HttpGet(string Url, string paramStr)
        {
            if (!string.IsNullOrEmpty(paramStr))
            {
                if (!Url.Contains("?"))
                {
                    Url = Url + "?";
                }
                else
                {
                    Url = Url + "&";
                }
                Url = Url + paramStr;
            }


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Timeout = 2000;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 发送HTTP的GET请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="dicPara">参数</param>
        /// <returns></returns>
        public static string HttpGet(string Url, Dictionary<string, string> dicPara)
        {
            return HttpGet(Url, BuildParams(dicPara));
        }


        /// <summary>
        /// 请求HTTP，并把返回的JSON处理成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Url"></param>
        /// <param name="dicPara"></param>
        /// <returns></returns>
        public static T HttpGet<T>(string Url, Dictionary<string, string> dicPara)
        {
            string requestString = HttpGet(Url, dicPara);
            return JsonDeserialize<T>(requestString);
        }


        #endregion


        #region Delete
        /// <summary>
        /// 发送HTTP的GET请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="paramStr">参数</param>
        /// <returns></returns>
        public static string HttpDelete(string Url, string paramStr)
        {
            if (!string.IsNullOrEmpty(paramStr))
            {
                if (!Url.Contains("?"))
                {
                    Url = Url + "?";
                }
                else
                {
                    Url = Url + "&";
                }
                Url = Url + paramStr;
            }


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "Delete";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 发送HTTP的GET请求
        /// </summary>
        /// <param name="Url">HTTP地址</param>
        /// <param name="dicPara">参数</param>
        /// <returns></returns>
        public static string HttpDelete(string Url, Dictionary<string, string> dicPara)
        {
            return HttpDelete(Url, BuildParams(dicPara));
        }


        /// <summary>
        /// 请求HTTP，并把返回的JSON处理成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Url"></param>
        /// <param name="dicPara"></param>
        /// <returns></returns>
        public static T HttpDelete<T>(string Url, Dictionary<string, string> dicPara)
        {
            string requestString = HttpDelete(Url, dicPara);
            return JsonDeserialize<T>(requestString);
        }

        #endregion

        #region 下载

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] DownloadData(string url)
        {
            try
            {
                var client = new WebClient();
                var byteData = client.DownloadData(url);
                return byteData;
            }
            catch
            {
                return null;
            }
        }

        #endregion


        #region 上传文件

        internal class HttpUploadParamBuilder
        {

            static Encoding encoding = Encoding.UTF8;

            /**/
            /// <summary>
            /// 拼接所有的二进制数组为一个数组
            /// </summary>
            /// <param name="byteArrays">数组</param>
            /// <returns></returns>
            /// <remarks>加上结束边界</remarks>
            public byte[] JoinBytes(ArrayList byteArrays)
            {
                int length = 0;
                int readLength = 0;

                // 加上结束边界
                string endBoundary = Boundary + "--\r\n"; //结束边界
                byte[] endBoundaryBytes = encoding.GetBytes(endBoundary);
                byteArrays.Add(endBoundaryBytes);

                foreach (byte[] b in byteArrays)
                {
                    length += b.Length;
                }
                byte[] bytes = new byte[length];

                // 遍历复制
                //
                foreach (byte[] b in byteArrays)
                {
                    b.CopyTo(bytes, readLength);
                    readLength += b.Length;
                }

                return bytes;
            }

            /**/
            /// <summary>
            /// 获取普通表单区域二进制数组
            /// </summary>
            /// <param name="fieldName">表单名</param>
            /// <param name="fieldValue">表单值</param>
            /// <returns></returns>
            /// <remarks>
            /// -----------------------------7d52ee27210a3c\r\nContent-Disposition: form-data; name=\"表单名\"\r\n\r\n表单值\r\n
            /// </remarks>
            public byte[] CreateFieldData(string fieldName, string fieldValue)
            {
                string textTemplate = Boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
                string text = String.Format(textTemplate, fieldName, fieldValue);
                byte[] bytes = encoding.GetBytes(text);
                return bytes;
            }

            /**/
            /// <summary>
            /// 获取文件上传表单区域二进制数组
            /// </summary>
            /// <param name="fieldName">表单名</param>
            /// <param name="filename">文件名</param>
            /// <param name="contentType">文件类型</param>
            /// <param name="contentLength">文件长度</param>
            /// <param name="stream">文件流</param>
            /// <returns>二进制数组</returns>
            public byte[] CreateFieldData(string fieldName, string filename, string contentType, byte[] fileBytes)
            {
                string end = "\r\n";
                string textTemplate = Boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

                // 头数据
                string data = String.Format(textTemplate, fieldName, filename, contentType);
                byte[] bytes = encoding.GetBytes(data);



                // 尾数据
                byte[] endBytes = encoding.GetBytes(end);

                // 合成后的数组
                byte[] fieldData = new byte[bytes.Length + fileBytes.Length + endBytes.Length];

                bytes.CopyTo(fieldData, 0); // 头数据
                fileBytes.CopyTo(fieldData, bytes.Length); // 文件的二进制数据
                endBytes.CopyTo(fieldData, bytes.Length + fileBytes.Length); // \r\n

                return fieldData;
            }

            string Boundary
            {
                get
                {
                    string[] bArray, ctArray;
                    string contentType = ContentType;
                    ctArray = contentType.Split(';');
                    if (ctArray[0].Trim().ToLower() == "multipart/form-data")
                    {
                        bArray = ctArray[1].Split('=');
                        return "--" + bArray[1];
                    }
                    return null;
                }
            }

            public string ContentType
            {
                get
                {
                    return "multipart/form-data; boundary=---------------------------7d5b915500cee";
                }
            }

        }

        /// <summary>
        /// 文件上传
        
        /// 日期：2017年8月11日
        /// </summary>
        /// <param name="Url">上传地址</param>
        /// <param name="files">文件</param>
        /// <param name="formBody">表单</param>
        /// <param name="head">头信息</param>
        /// <returns></returns>
        public static async Task<T> HttpUploadFileAsync<T>(
            string Url,
            Dictionary<string,byte[]> files, 
            Dictionary<string, string> formBody,
            SortedList<string, string> head = null)
        {
            return await System.Threading.Tasks.Task.Run(() => {

                HttpUploadParamBuilder cb = new HttpUploadParamBuilder();
                // 所有表单数据
                ArrayList bytesArray = new ArrayList();

                if (formBody != null)
                {
                    foreach (var name in formBody.Keys)
                    {
                        // 普通表单
                        bytesArray.Add(cb.CreateFieldData(name, formBody[name]));
                    }
                }

                if (files != null)
                {
                    var i = 1;
                    foreach (var fileName in files.Keys)
                    {
                        // 文件表单
                        bytesArray.Add(cb.CreateFieldData("file" + i, fileName, "application/octet-stream", files[fileName]));
                        i++;
                    }
                }

                // 合成所有表单并生成二进制数组
                byte[] bytes = cb.JoinBytes(bytesArray);

                return HttpPost<T>(Url, bytes, head, cb.ContentType);
            });
          
        }

        #endregion

    }
}