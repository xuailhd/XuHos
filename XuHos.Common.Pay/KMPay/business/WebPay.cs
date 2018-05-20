using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Pay.KMPay
{
    public class WebPay
    {
        public XuHos.Common.Config.Sections.Pay.KMPay Config { get; set; }

        public WebPay(string appId)
        {
            Config = Configuration.GetAppPayConfig<XuHos.Common.Config.Sections.Pay.KMPay>(appId);
        }


        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="TradeNo"></param>
        /// <param name="Subject"></param>
        /// <param name="TotalFee"></param>
        /// <param name="RefundFee"></param>
        /// <param name="RefundNo"></param>
        /// <returns></returns>
        public bool ApplyRefund(string SellerID, string TradeNo, string Subject, decimal TotalFee, decimal RefundFee, string RefundNo)
        {
            var KMConfig = XuHos.Common.Pay.Configuration.GetAppPayConfig<XuHos.Common.Config.Sections.Pay.KMPay>(SellerID);

            #region 计算退款金额      
            //转换单位，元>分
            float _RefundFee = float.Parse(RefundFee.ToString()) * 100;
            float _TotalFee = float.Parse(TotalFee.ToString()) * 100;
            #endregion

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
            sParaTemp.Add("method", define.refund_method);
            sParaTemp.Add("partner", KMConfig.partner);
            sParaTemp.Add("inputCharset", define.input_charset);
            sParaTemp.Add("clientIp", clientIp);
            sParaTemp.Add("timestamp", timestamp);
            sParaTemp.Add("sellerEmail", KMConfig.seller_email);
            sParaTemp.Add("refundDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            sParaTemp.Add("batchNo", RefundNo);//批次号，不能重复
            sParaTemp.Add("batchNum", "1");
            sParaTemp.Add("detailData", string.Format("{0}^{1}^退款", TradeNo, _RefundFee));//多批退款用#隔开，如a^1^退款#b^1^退款

            //参数签名
            var dic = XuHos.Common.Pay.KMPay.KMSubmit.BuildSignPara(sParaTemp, KMConfig);

            //发起退款请求
            var result = XuHos.Common.Pay.KMPay.Utility.HttpPostToEntity<XuHos.Common.Pay.KMPay.Model.RefundResult>(define.gatway_url, dic);

            //退款成功或/批次号重复（意味着已经退款了）
            if (result.success || result.code == "valid-batch-exist")
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sPara"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public bool VerifySign(SortedDictionary<string, string> sPara,string sign)
        {
       
            KMNotify notify = new KMNotify();
            return notify.Verify(sPara, sign, Config);
        }
    }
}
