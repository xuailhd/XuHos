using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Pay.AliPay
{
    public class WebPay
    {
        public XuHos.Common.Config.Sections.Pay.AliPay Config
        { get; set; }


        // 退款服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        public string refundNotify_url
        {
            get
            {
                return string.Format("{0}/Cashier/AliPay/RefundNotifyUrl", Configuration.Config.NotifyUrlPrefix);
            }
        }

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

        public WebPay(string appId)
        {
            Config = Configuration.GetAppPayConfig<XuHos.Common.Config.Sections.Pay.AliPay>(appId);
        }

        /// <summary>
        /// 获取支付表单
        
        /// 日期：2016年8月22日
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="Subject"></param>
        /// <param name="TotalFee"></param>
        /// <param name="Body"></param>
        /// <returns></returns>
        public string GetWebPayForm(string OrderNo, string Subject, string TotalFee, string Body)
        {
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", define.payService);
            sParaTemp.Add("partner", Config.partner);
            sParaTemp.Add("seller_id", Config.seller_id);
            sParaTemp.Add("_input_charset", define.input_charset.ToLower());
            sParaTemp.Add("payment_type", define.payment_type);
            sParaTemp.Add("notify_url", payNotify_url);
            sParaTemp.Add("return_url", payReturn_url);
            sParaTemp.Add("anti_phishing_key", define.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", define.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", OrderNo);
            sParaTemp.Add("subject", Subject);
            sParaTemp.Add("total_fee", TotalFee);
            sParaTemp.Add("body", Body);
            return XuHos.Common.Pay.AliPay.Submit.BuildRequest(sParaTemp, "get", "确认",Config);         

            
        }



        /// <summary>
        /// 获取支付表单
        
        /// 日期：2016年8月22日
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="Subject"></param>
        /// <param name="TotalFee"></param>
        /// <param name="Body"></param>
        /// <returns></returns>
        public string GetWapForm(string OrderNo, string Subject, string TotalFee, string Body)
        {
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", define.WapPayService);
            sParaTemp.Add("partner", Config.partner);
            sParaTemp.Add("seller_id", Config.seller_id);
            sParaTemp.Add("_input_charset", define.input_charset.ToLower());
            sParaTemp.Add("payment_type", define.payment_type);
            sParaTemp.Add("notify_url", payNotify_url);
            sParaTemp.Add("return_url", payReturn_url);
            sParaTemp.Add("anti_phishing_key", define.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", define.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", OrderNo);
            sParaTemp.Add("subject", Subject);
            sParaTemp.Add("total_fee", TotalFee);
            sParaTemp.Add("body", Body);
            return XuHos.Common.Pay.AliPay.Submit.BuildRequest(sParaTemp, "get", "确认", Config);


        }
        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="TradeNo"></param>
        /// <param name="Subject"></param>
        /// <param name="RefundFee"></param>
        /// <param name="RefundNo"></param>
        /// <returns></returns>
        public bool ApplyRefund(string SellerID,string TradeNo,string Subject,decimal RefundFee,string RefundNo)
        {
            //把请求参数打包成数组
            System.Collections.Generic.SortedDictionary<string, string> values = new SortedDictionary<string, string>();
            values.Add("service", define.refundService);
            values.Add("partner", Config.partner);
            values.Add("_input_charset", define.input_charset.ToLower());
            values.Add("notify_url", refundNotify_url);
            values.Add("seller_user_id", Config.seller_id);
            values.Add("refund_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            values.Add("batch_no", RefundNo);
            values.Add("batch_num", "1");
            values.Add("detail_data", string.Format("{0}^{1}^{2}", TradeNo, RefundFee, Subject));

            //<?xml version=\"1.0\" encoding=\"GBK\"?>\n<alipay><is_success>F</is_success><error>ILLEGAL_SIGN</error></alipay>
            var xml = Submit.AjaxRequest(values, Config);
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            LogHelper.WriteDebug("支付宝退款返回:" + xml);

            var rootNode = doc.SelectSingleNode("alipay");

            if (rootNode.SelectSingleNode("is_success").InnerText == "T")
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
        public bool VerifySign(SortedDictionary<string, string> sPara,string notify_id, string sign)
        {
            Notify notify = new Notify(Config);
            return notify.Verify(sPara, notify_id,sign);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="OrderOutId"></param>
        /// <param name="TradeStatus">交易状态：WAIT_BUYER_PAY（交易创建，等待买家付款）、TRADE_CLOSED（未付款交易超时关闭，或支付完成后全额退款）、TRADE_SUCCESS（交易支付成功）、TRADE_FINISHED（交易结束，不可退款）</param>
        /// <param name="TradeNo">交易编号</param>
        /// <returns></returns>
        public bool GetOrderInfo(string OrderOutId,out string TradeStatus,out string TradeNo,out string SellerID)
        {
            TradeStatus = "";
            TradeNo = "";
            SellerID = "";
            try
            {

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.partner);
                sParaTemp.Add("_input_charset", define.input_charset.ToLower());
                sParaTemp.Add("service", "single_trade_query");
                sParaTemp.Add("out_trade_no", OrderOutId);

                /*<?xml version="1.0" encoding="utf-8"?>
                <alipay>
                <is_success>T</is_success>
                <request>
                <param name="_input_charset">utf-8</param>
                <param name="service">single_trade_query</param>
                <param name="partner">2088021337472610</param>
                <param name="out_trade_no">TW2017011317030001</param>
                </request>
                <response>
                    <trade>
                        <additional_trade_status>DAEMON_CONFIRM_CLOSE</additional_trade_status>
                        <buyer_email>geniusming@qq.com</buyer_email>
                        <buyer_id>2088002935623932</buyer_id>
                        <discount>0.00</discount>
                        <flag_trade_locked>0</flag_trade_locked>
                        <gmt_close>2017-01-14 17:03:56</gmt_close>
                        <gmt_create>2017-01-13 17:07:40</gmt_create>
                        <gmt_last_modified_time>2017-01-14 17:03:56</gmt_last_modified_time>
                        <gmt_payment>2017-01-13 17:07:49</gmt_payment>
                        <gmt_refund>2017-01-14 17:03:56.744</gmt_refund>
                        <is_total_fee_adjust>F</is_total_fee_adjust>
                        <operator_role>B</operator_role>
                        <out_trade_no>TW2017011317030001</out_trade_no>
                        <payment_type>1</payment_type>
                        <price>0.10</price>
                        <quantity>1</quantity>
                        <refund_fee>0.10</refund_fee>
                        <refund_flow_type>3</refund_flow_type>
                        <refund_id>24162081</refund_id>
                        <refund_status>REFUND_SUCCESS</refund_status>
                        <seller_email>3249695045@qq.com</seller_email>
                        <seller_id>2088021337472610</seller_id>
                        <subject>鍥炬枃鍜ㄨ</subject>
                        <to_buyer_fee>0.10</to_buyer_fee>
                        <to_seller_fee>0.10</to_seller_fee>
                        <total_fee>0.10</total_fee>
                        <trade_no>2017011321001004930274617544</trade_no>
                        <trade_status>TRADE_CLOSED</trade_status>
                        <use_coupon>F</use_coupon>
                    </trade>
                </response><sign>a89a41271be1a494988144449cd87183</sign>
                <sign_type>MD5</sign_type>
                </alipay>*/
                var xml = Submit.AjaxRequest(sParaTemp, Config);
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(xml);
                var rootNode = doc.SelectSingleNode("alipay");

                if (rootNode.SelectSingleNode("is_success").InnerText == "T")
                {
                    SellerID = rootNode.SelectSingleNode("response/trade/seller_id").InnerText;
                    TradeNo = rootNode.SelectSingleNode("response/trade/trade_no").InnerText;
                    //TRADE_CLOSED
                    TradeStatus = rootNode.SelectSingleNode("response/trade/trade_status").InnerText;
                    return true;
                }
                else
                {
                    //TRADE_NOT_EXIST
                    TradeStatus = rootNode.SelectSingleNode("error").InnerText;
                    return false;

                }
            }
            catch(Exception E)
            {
                LogHelper.WriteError(E);
            }

            return false;
        }
    }
}
