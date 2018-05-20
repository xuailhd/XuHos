using System;
using System.Collections.Generic;
using System.Web;

namespace KMEHosp.Common.Pay.WxPay
{
    public class NativePay
    {

        /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
        public string GetWebPayUrl(string out_trade_no, string subject,string body,string total_fee, string appId)
        {

            //因为微信的单位是分所以这里需要将元转换成分
            int fee = int.Parse((double.Parse(total_fee) * 100).ToString());

            WxPayData data = new WxPayData();
            data.SetValue("body", subject);//商品描述
            data.SetValue("attach", subject);//附加数据           
            data.SetValue("out_trade_no", out_trade_no);//随机字符串 //data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());//随机字符串
            data.SetValue("total_fee", fee);//总金额，
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(30).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", "jjj");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", "");//商品ID

            WxPayData result = WxPayApi.UnifiedOrder(data,6,appId);//调用统一下单接口
            var result_code = result.GetValue("result_code");


            if (result_code.ToString() == "FAIL")
            {
                throw new Exception(result.GetValue("err_code_des").ToString());
            }
            else
            {
                string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

                return url;
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
        public string UnifiedOrder(string out_trade_no, string subject, string body, string total_fee, string appId,string trade_type)
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
            WxPayData wxReturn = WxPayApi.UnifiedOrder(req,6,appId);
            if (wxReturn.IsSet("prepay_id"))
            {
                return  wxReturn.GetValue("prepay_id").ToString();      
            }
            else
            {
                return "";
            }


        }

    }
}