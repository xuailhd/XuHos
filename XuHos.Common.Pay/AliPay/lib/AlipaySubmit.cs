using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Xml;

namespace XuHos.Common.Pay.AliPay
{
    /// <summary>
    /// 类名：Submit
    /// 功能：支付宝各接口请求提交类
    /// 详细：构造支付宝各接口表单HTML文本，获取远程HTTP数据
    /// 版本：3.3
    /// 修改日期：2011-07-05
    /// </summary>
    internal class Submit
    {
      


        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求给支付宝的参数数组</param>
        /// <returns>签名结果</returns>
        public static string BuildRequestMysign(Dictionary<string, string> sPara,string _key)
        {
           
            //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            string prestr = Core.CreateLinkString(sPara);

            //把最终的字符串签名，获得签名结果
            string mysign = "";
            switch (define.sign_type)
            {
                case "MD5":
                    mysign = AlipayMD5.Sign(prestr, _key, define.input_charset);
                    break;
                default:
                    mysign = "";
                    break;
            }

            return mysign;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        public static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp, XuHos.Common.Config.Sections.Pay.AliPay Config)
        {

            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //签名结果
            string mysign = "";

            //过滤签名参数数组
            sPara = Core.FilterPara(sParaTemp);

            //获得签名结果
            mysign = BuildRequestMysign(sPara,Config.key);

            //签名结果与签名方式加入请求提交参数组中
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", define.sign_type);

            return sPara;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code, XuHos.Common.Config.Sections.Pay.AliPay Config)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara = BuildRequestPara(sParaTemp,Config);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = Core.CreateLinkStringUrlencode(sPara, code);

            return strRequestData;
        }

        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue, XuHos.Common.Config.Sections.Pay.AliPay Config)
        {
            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp,Config);

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + define.GATEWAY_NEW + "_input_charset=" + define.input_charset + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }

  


        public  static string AjaxRequest(SortedDictionary<string, string> sParaTemp, XuHos.Common.Config.Sections.Pay.AliPay Config)
        {
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp,Config);


            System.Collections.Specialized.NameValueCollection values = new System.Collections.Specialized.NameValueCollection();
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                if (data.Length > 0)
                {
                    data.Append("&");
                }

                data.AppendFormat("{0}={1}",temp.Key,temp.Value);
                
                values.Add(temp.Key, temp.Value);
            }

            var requestUrl = define.GATEWAY_NEW + data.ToString();
            var requestBeginTime = DateTime.Now;
            var response = "";
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                response = client.DownloadString(requestUrl);               

         
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
            finally
            {
                
                TrackApiLog.WriteLog("AliPay",requestUrl, "GET", data.ToString(), requestBeginTime, response);
            }
            return response;

        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        private string Query_timestamp(XuHos.Common.Config.Sections.Pay.AliPay Config)
        {
            string url = define.GATEWAY_NEW + "service=query_timestamp&partner=" + Config.partner + "&_input_charset=" + define.input_charset;
            string encrypt_key = "";

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }


    }
}