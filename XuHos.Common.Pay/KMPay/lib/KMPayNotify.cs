using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace XuHos.Common.Pay.KMPay
{
    /// <summary>
    /// 类名：Notify
    /// 功能：康美支付回调通知处理类
    /// 详细：处理通知返回
    /// author：Tang
    /// data：2016-7-1
    /// </summary>
    internal class KMNotify
    {
      
		
		 /// <summary>
        /// 从文件读取公钥转公钥字符串
        /// </summary>
        /// <param name="Path">公钥文件路径</param>
        public static string getPublicKeyStr(string Path)
        {
            StreamReader sr = new StreamReader(Path);
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

        /// <summary>
        ///  验证消息是否是回调的合法消息
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="notify_id">通知验证ID</param>
        /// <param name="sign">支付宝生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string sign, XuHos.Common.Config.Sections.Pay.KMPay KMConfig)
        {
           return GetSignVeryfy(inputPara, sign,KMConfig);
        }


        /// <summary>
        /// 获取返回时的签名验证结果
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="sign">对比的签名结果</param>
        /// <returns>签名验证结果</returns>
        private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign, XuHos.Common.Config.Sections.Pay.KMPay KMConfig)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤空值、sign与sign_type参数
            sPara = KMCore.FilterPara(inputPara);
            
            //获取待签名字符串
            string preSignStr = KMCore.CreateLinkString(sPara);

            //获得签名验证结果
            bool isSgin = false;
            if (sign != null && sign != "")
            {
                switch (define.sign_type)
                {
                    case "RSA":
                        isSgin = RSAFromPkcs8.verify(preSignStr, sign,define.kmpay_public_key,define.input_charset);
                        break;
                    default:
                        break;
                }
            }

            return isSgin;
        }


   
    }
}