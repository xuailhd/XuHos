using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Pay.AliPay
{
    /// <summary>
    /// 支付宝原生支付
    /// </summary>
    public  class NavivePay
    {

        public XuHos.Common.Config.Sections.Pay.AliPay AliPayConfig
        { get; set; }

        // 支付服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        public string payNotify_url
        {
            get
            {
                return string.Format("{0}/Cashier/AliPay/NotifyUrl", Configuration.Config.NotifyUrlPrefix);
            }
        }

        // 支付页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
        public string payReturn_url
        {
            get
            {
                return string.Format("{0}/Cashier/AliPay/ReturnUrl", Configuration.Config.ReturnUrlPrefix);
            }

        }

        public NavivePay(string appId)
        {
            AliPayConfig = Configuration.GetAppPayConfig<XuHos.Common.Config.Sections.Pay.AliPay>(appId);
        
        }

        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="total_fee"></param>
        /// <returns></returns>
        public string GetMobilePayParams(string out_trade_no, string subject, string body, string total_fee,string ReturnUrl)
        {
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", define.mobilePayService);
            sParaTemp.Add("partner", AliPayConfig.partner);
            sParaTemp.Add("seller_id", AliPayConfig.seller_id);
            sParaTemp.Add("_input_charset", define.input_charset.ToLower());
            sParaTemp.Add("payment_type", define.payment_type);
            sParaTemp.Add("notify_url", payNotify_url);
            sParaTemp.Add("return_url",string.IsNullOrEmpty(ReturnUrl) ?payReturn_url:ReturnUrl);
            sParaTemp.Add("anti_phishing_key", define.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", define.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);

            return AliPay.Submit.BuildRequestParaToString(sParaTemp, Encoding.UTF8, AliPayConfig);

        }

        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="total_fee"></param>
        /// <returns></returns>
        public string GetWapPayParams(string out_trade_no,string subject,string body,string total_fee,string ReturnUrl)
        {
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", define.WapPayService);
            sParaTemp.Add("partner", AliPayConfig.partner);
            sParaTemp.Add("seller_id",AliPayConfig.seller_id);
            sParaTemp.Add("_input_charset", define.input_charset.ToLower());
            sParaTemp.Add("payment_type", define.payment_type);
            sParaTemp.Add("notify_url", payNotify_url);
            sParaTemp.Add("return_url", string.IsNullOrEmpty(ReturnUrl) ? payReturn_url : ReturnUrl);
            sParaTemp.Add("anti_phishing_key", define.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", define.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);

            return AliPay.Submit.BuildRequestParaToString(sParaTemp, Encoding.UTF8,AliPayConfig);

        }
    }
}
