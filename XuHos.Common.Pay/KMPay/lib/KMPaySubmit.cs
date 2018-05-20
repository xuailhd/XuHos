using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Security.Cryptography;

namespace XuHos.Common.Pay.KMPay
{
    /// <summary>
    /// 类名：Submit
    /// 功能：康美支付HTTP请求提交类
    /// 详细：构造康美支付接口表单HTML文本，获取远程HTTP数据
    /// author：Tang
    /// data：2016-7-1
    /// </summary>
    internal class KMSubmit
    {
        #region 字段

        #endregion

        /// <summary>
        /// 生成要请求的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        public static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp, XuHos.Common.Config.Sections.Pay.KMPay KMConfig)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //签名结果
            string mysign = "";

            //过滤签名参数数组
            sPara = KMCore.FilterPara(sParaTemp);

            //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            string prestr = KMCore.CreateLinkString(sPara);

            mysign = RSAEncrypt(define.private_key, prestr);

            //签名结果与签名方式加入请求提交参数组中
            sPara.Add("sign", mysign);
            sPara.Add("signtype", define.sign_type);

            return sPara;
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string privateKeyJAVA, string content)
        {
            string inputCharset = "utf-8";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] data;
            rsa.FromXmlString(RSAKeyConvert.RSAPrivateKeyJava2DotNet(privateKeyJAVA));
            // rsa.FromXmlString(publickey);
            Encoding encode = Encoding.GetEncoding(inputCharset);
            data = encode.GetBytes(content);
            var sha = SHA1.Create();
            var sigDate = rsa.SignData(data, typeof(SHA1));

            return Convert.ToBase64String(sigDate);

        }

        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue, XuHos.Common.Config.Sections.Pay.KMPay KMConfig)
        {
            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp,KMConfig);

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + define.pay_url + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }

        /// <summary>
        /// 参数生成签名，并加入参数集合
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        public static Dictionary<string, string> BuildSignPara(SortedDictionary<string, string> sParaTemp , XuHos.Common.Config.Sections.Pay.KMPay KMConfig)
        {
            return BuildRequestPara(sParaTemp,KMConfig);
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code, XuHos.Common.Config.Sections.Pay.KMPay KMConfig)
        {
          
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara = BuildRequestPara(sParaTemp,KMConfig);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = KMCore.CreateLinkStringUrlencode(sPara, code);

            return strRequestData;
        }


      
    }
}