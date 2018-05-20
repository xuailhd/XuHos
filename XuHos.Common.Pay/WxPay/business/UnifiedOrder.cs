using System;
using System.Collections.Generic;
using System.Web;

namespace XuHos.Common.Pay.WxPay
{
    public class UnifiedOrder
    {
        /// <summary>
        /// 统一下单并返回跳转地址        
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="openId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public string GetJumpUrlWhenMWeb(string out_trade_no, string subject, string body, string total_fee, string appId)
        {
            WxPayData result = Request(out_trade_no, subject, body, total_fee, appId, "MWEB");

            if (result.IsSet("prepay_id"))
            {
                return result.GetValue("mweb_url").ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 统一下单获取并返回下单编号
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="total_fee"></param>
        /// <param name="appId"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string GetPrepayIdWhenJsSdk(string out_trade_no, string subject, string body, string total_fee, string appId, string openid,string product_id)
        {
            WxPayData result = Request(out_trade_no, subject, body, total_fee, appId, "JSAPI", openid, product_id);

            if (result.ToString() == "FAIL")
            {
                throw new Exception(result.GetValue("err_code_des").ToString());
            }
            else
            {
                if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
                {
                    throw new WxPayException($"UnifiedOrder response error,{result.ToXml()}");
                }

                return result.GetValue("prepay_id").ToString();
            }

        }

         /// <summary>
         /// 统一下单并返回二维码支付地址
         /// </summary>
         /// <param name="out_trade_no"></param>
         /// <param name="subject"></param>
         /// <param name="body"></param>
         /// <param name="total_fee"></param>
         /// <param name="appId"></param>
         /// <returns></returns>
        public string GetQRCodeUrlWhenWeb(string out_trade_no, string subject, string body, string total_fee, string appId)
        {
            WxPayData result = Request(out_trade_no, subject, body, total_fee, appId, "NATIVE", "");
            var result_code = result.GetValue("result_code");
            if (result_code.ToString() == "FAIL")
            {
                throw new Exception(result.GetValue("err_code_des").ToString());
            }
            else
            {
                //获得统一下单接口返回的二维码链接
                string url = result.GetValue("code_url").ToString();
                return url;
            }
        }


        /// <summary>
        /// 统一下单并返回订单编号
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="total_fee"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public string GetPrepayIdWhenApp(string out_trade_no, string subject, string body, string total_fee, string appId)
        {
            WxPayData result = Request(out_trade_no, subject, body, total_fee, appId, "APP", "");
        
            var result_code = result.GetValue("result_code");
            if (result_code.ToString() == "FAIL")
            {
                throw new Exception(result.GetValue("err_code_des").ToString());
            }
            else
            {
                if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
                {
                    throw new WxPayException($"UnifiedOrder response error,{result.ToXml()}");
                }
                return result.GetValue("prepay_id").ToString();
            }
            
        }

        /// <summary>
        /// 统一下单接口
        /// 调用该接口在微信支付服务后台生成预支付交易单，返回正确的预支付交易回话标识后再在APP里面调起支付。
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="openId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public WxPayData Request(string out_trade_no, string subject, string body, string total_fee, string appId,string trade_type,string openid="", string product_id="")
        {
            //因为微信的单位是分所以这里需要将元转换成分
            int fee = int.Parse((double.Parse(total_fee) * 100).ToString());

            //统一下单
            WxPayData req = new WxPayData();
            req.SetValue("body", body);
            req.SetValue("attach", subject);
            req.SetValue("out_trade_no", out_trade_no);
            req.SetValue("total_fee", fee);
            req.SetValue("trade_type",trade_type);
            req.SetValue("openid", openid);
            req.SetValue("product_id", product_id);

            
            WxPayData wxReturn = WxPayApi.UnifiedOrder(req,6,appId);
            return wxReturn;
        }
    }
}