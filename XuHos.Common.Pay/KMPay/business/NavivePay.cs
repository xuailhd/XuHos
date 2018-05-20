using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using XuHos.Extensions;
namespace XuHos.Common.Pay.KMPay
{
    /// <summary>
    /// 支付宝原生支付
    /// </summary>
    public  class NavivePay
    {
      
        public XuHos.Common.Config.Sections.Pay.KMPay KMConfig
        { get; set; }
  
        public string notify_url
        {

            get
            {
                return string.Format("{0}/Cashier/KMPay/NotifyUrl/{1}", Configuration.Config.NotifyUrlPrefix, KMConfig.seller_id);
            }
        }

        //支付返回页面，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
        public string return_url
        {
            get
            {
                return string.Format("{0}/Cashier/KMPay/ReturnUrl/{1}", Configuration.Config.ReturnUrlPrefix, KMConfig.seller_id);

            }
        }

        public NavivePay(string appId)
        {
            KMConfig = Configuration.GetAppPayConfig<XuHos.Common.Config.Sections.Pay.KMPay>(appId);
        }

        /// <summary>
        /// 获取移动端支付参数
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="total_fee"></param>
        /// <returns></returns>
        public string GetMobilePayParams(string out_trade_no, string subject, string body, string total_fee,string returnUrl)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////

            float totalFee;
            if (float.TryParse(total_fee, out totalFee) == false)
            {
                totalFee = 0.01f;
            }
            totalFee = totalFee * 100;




            ////////////////////////////////////////////////////////////////////////////////////////////////
            string timestamp = DateTime.Now.ToTimeStamp().ToString(); //DateTime.Now.ToString("yyyyMMddHHmmss");//时间戳 (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds.ToString();
            string clientIp = string.Empty;
            ///获取本地的IP地址
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    clientIp = _IPAddress.ToString();
                }
            }

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", KMConfig.partner);//配制中读取
            sParaTemp.Add("inputCharset", define.input_charset);
            sParaTemp.Add("timestamp", timestamp);
            sParaTemp.Add("sellerEmail", KMConfig.seller_email);//配制中读取

            sParaTemp.Add("notifyUrl", notify_url);//配制中读取
            sParaTemp.Add("returnUrl", string.IsNullOrEmpty(returnUrl) ? return_url : returnUrl);//配制中读取
            sParaTemp.Add("outTradeNo", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("totalAmount", totalFee.ToString());
            sParaTemp.Add("body", body);
            //sParaTemp.Add("paymentType", KMConfig.payment_type);//配制中读取
            //sParaTemp.Add("showUrl", "");//商品展示地址，绝对路径，外网可访问
            //sParaTemp.Add("price", "1");
            //sParaTemp.Add("quantity", "1");
            //sParaTemp.Add("clientIp", clientIp);
            Dictionary<string, string> dict = KMSubmit.BuildRequestPara(sParaTemp, KMConfig);
            return LitJson.JsonMapper.ToJson(dict);
        }


        public string GetPayForm(string out_trade_no, string subject, string body, string total_fee)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////
        
            float totalFee;
            if (float.TryParse(total_fee, out totalFee) == false)
            {
                totalFee = 0.01f;
            }
            totalFee = totalFee * 100;


            ////////////////////////////////////////////////////////////////////////////////////////////////
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");//时间戳 (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds.ToString();
            string clientIp = string.Empty;
            ///获取本地的IP地址
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    clientIp = _IPAddress.ToString();
                }
            }

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", KMConfig.partner);//配制中读取
            sParaTemp.Add("inputCharset", define.input_charset);
            sParaTemp.Add("clientIp", clientIp);
            sParaTemp.Add("timestamp", timestamp);
            sParaTemp.Add("sellerEmail", KMConfig.seller_email);//配制中读取
            sParaTemp.Add("paymentType", define.payment_type);//配制中读取
            sParaTemp.Add("notifyUrl", notify_url);//配制中读取
            sParaTemp.Add("returnUrl", return_url);//配制中读取
            sParaTemp.Add("showUrl", "");//商品展示地址，绝对路径，外网可访问
            sParaTemp.Add("outTradeNo", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("totalAmount", totalFee.ToString());
            sParaTemp.Add("body", body);
            sParaTemp.Add("price", "1");
            sParaTemp.Add("quantity", "1");

            //建立请求
            return KMSubmit.BuildRequest(sParaTemp, "post", "确认",KMConfig); ;
        }
    }
}
