using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Pay.KMPay
{
    internal static class define
    {

        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        //商户的私钥文件路径
        public static string private_key
        {
            get
            {

                return getPrivateKeyStr(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\key\\kmpay\\private_key.pem"));
            }
        }

        //公钥文件路径
        public static string kmpay_public_key
        {
            get
            {
                return getPublicKeyStr(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\key\\kmpay\\public_key.pem"));
            }
        }

        //康美支付接口地址
        public static readonly string pay_url = "https://www.kmpay518.com/kame-checkout/mchexcashier/index.htm";

        //康美接口接口地址
        public static readonly string gatway_url = "https://www.kmpay518.com/kame-api/gw/router";

        //退款方法，固定值
        public static readonly string refund_method = "kmpay.refund.req";

        public static readonly string orderget_method = "kmpay.order.get";

        //签名方式
        public static readonly string sign_type = "RSA";

        //调试用，创建TXT日志文件夹路径，见AlipayCore.cs类中的LogResult(string sWord)打印方法。
        public static readonly string log_path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "log");

        //字符编码格式 目前支持 gbk 或 utf-8
        public static readonly string input_charset = "UTF-8";

        //支付类型 ，无需修改
        public static readonly string payment_type = "2";

        //防钓鱼时间戳  若要使用请调用类文件submit中的Query_timestamp函数
        public static readonly string anti_phishing_key = "";

        //客户端的IP地址 非局域网的外网IP地址，如：221.0.0.1
        public static readonly string exter_invoke_ip = "112.74.101.192";

        /// <summary>
        /// 从文件读取私字符串
        /// </summary>
        /// <param name="Path">私钥文件路径</param>
        public static string getPrivateKeyStr(string Path)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Path);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN RSA PRIVATE KEY-----", "");
                pubkey = pubkey.Replace("-----END RSA PRIVATE KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

        /// <summary>
        /// 从文件读取私字符串
        /// </summary>
        /// <param name="Path">私钥文件路径</param>
        public static string getPublicKeyStr(string Path)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Path);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }
    }
}
