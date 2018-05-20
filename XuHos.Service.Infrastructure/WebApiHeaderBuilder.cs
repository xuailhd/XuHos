using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.Infrastructure
{

    public class WebApiHeaderBuilder
    {
        /// <summary>
        /// 接收API的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class ApiResult<T>
        {

            /// <summary>
            /// 接口业务状态
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 消息状态说明
            /// </summary>
            public string Msg { get; set; }


            /// <summary>
            /// 接口是否调用陈工
            /// </summary>
            public bool Result
            { get; set; }



            public int Total { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public new T Data { get; set; }

        }

        class TokenResult
        {
            public string Token { get; set; }
        }


        string _UserToken = "";
        string _TokenUrl = "";
        string _AppKey = "";
        string _AppID = "";
        string _AppSecret = "";

        public Lazy<string> _AppToken = null;

        public string AppToken
        {
            get
            {

                return _AppToken.Value;
            }
        }

        public WebApiHeaderBuilder(string AppID, string AppKey, string AppSecret, string TokenUrl)
        {
            this._AppKey = AppKey;
            this._AppID = AppID;
            this._TokenUrl = TokenUrl;
            this._AppSecret = AppSecret;

            _AppToken = new Lazy<string>(() =>
            {
                return GetAppToken(_AppID, _AppSecret, _TokenUrl);
            });
        }

        public async Task<System.Collections.Generic.SortedList<string, string>> GetHeaderAsync()
        {
            return await Task.Run(() =>
            {

                var list = new System.Collections.Generic.SortedList<string, string>();
                list.Add("appkey", _AppKey);
                list.Add("apptoken", AppToken);
                list.Add("noncestr", Guid.NewGuid().ToString("N"));
                list.Add("usertoken", _UserToken);
                var sign = GetSign(list);
                list.Add("sign", sign);
                return list;
            });
        }

    
        #region 私有
        string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

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

        /// <summary>
        /// 从webapi获取app token
        /// </summary>
        /// <returns></returns
        string GetAppToken(string appId, string appSecret, string tokenUrl)
        {
            var url = tokenUrl.TrimEnd('/') + "/Token/get?appId=" + UrlEncode(appId) + "&appSecret=" + UrlEncode(appSecret);

            return XuHos.Common.Utility.WebAPIHelper.HttpGet<ApiResult<TokenResult>>(url, null).Data.Token;

        }
        #endregion
    }
}
