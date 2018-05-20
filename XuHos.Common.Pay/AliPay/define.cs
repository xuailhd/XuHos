using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Pay.AliPay
{
    internal static class define
    {

        #region 变量定义
        //支付宝消息验证地址
        public static readonly string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

        //支付宝网关地址（新）
        public static readonly string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";

        // 签名方式
        public static string sign_type
        {
            get
            {
                return "MD5";
            }
        }
        // 字符编码格式 目前支持 gbk 或 utf-8
        public static string input_charset
        {
            get
            {
                return "utf-8";
            }
        }
        // 支付类型 ，无需修改
        public static readonly string payment_type = "1";
        // 即时到账调用的接口名，无需修改
        public static readonly string payService= "create_direct_pay_by_user";
        /// <summary>
        /// APP支付方式（APP接入）
        /// </summary>
        public static readonly string mobilePayService = "mobile.securitypay.pay";

        public static readonly string WapPayService = "alipay.wap.create.direct.pay.by.user";
        // 即时到账有密退款调用的接口名，无需修改
        public static readonly string refundService = "refund_fastpay_by_platform_nopwd";
        //防钓鱼时间戳  若要使用请调用类文件submit中的Query_timestamp函数
        public static readonly string anti_phishing_key = "";

        //客户端的IP地址 非局域网的外网IP地址，如：221.0.0.1
        public static readonly string exter_invoke_ip = "";
        #endregion
    }
}
