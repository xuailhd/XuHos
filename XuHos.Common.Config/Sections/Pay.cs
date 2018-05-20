using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{

    /// <summary>
    /// 支付配置
    /// </summary>
    public class Pay : IConfigSection
    {
        public interface IAppPayConfig
        {
             string Type
            { get; set; }            
        }

        public class WXPay : IConfigSection, IAppPayConfig
        {

            public string APPID
            { get; set; }

            public string MCHID
            { get; set; }

            public string KEY
            { get; set; }

            public string APPSECRET
            { get; set; }

            public string Type
            { get; set; }
        }

        public class AliPay : IConfigSection, IAppPayConfig
        {


            public string partner
            { get; set; }

            public string key
            { get; set; }


            public string Type
            { get; set; }      

            public string seller_id
            {
                get
                {
                    return partner;
                }
            }
        }

        public class KMPay : IConfigSection, IAppPayConfig
        {
            public string seller_id
            { get; set; }

            public string seller_email
            { get; set; }

            public string Type
            { get; set; }

            //商户编号
            public string partner
            {
                get
                {
                    return seller_id;
                }
            }
        }


        /// <summary>
        /// 返回地址前缀
        /// </summary>
        public string ReturnUrlPrefix { get; set; }

        /// <summary>
        /// 订单号前缀（区分不同环境）
        /// </summary>
        public string SeqPrefix { get; set; }

           /// <summary>
        /// 通知地址（支付通知，退款通知）
        /// </summary>
        public string NotifyUrlPrefix
        { get; set; }

        public string WXPaySellerIds
        { get; set; }

        public string AliPaySellerIds
        { get; set; }

        public string KMPaySellerIds
        { get; set; }

        /// <summary>
        /// 微信支付默认收款人
        /// </summary>
        public string WXPayDefaultSellerId
        { get; set; }
        
        /// <summary>
        /// 阿里支付默认收款人
        /// </summary>
        public string AliPayDefaultSellerId
        { get; set; }

        /// <summary>
        /// 康美支付默认收款人
        /// </summary>
        public string KMPayDefaultSellerId
        { get; set; }

    }


}
