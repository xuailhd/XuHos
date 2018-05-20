using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMEHosp.Common.Pay.WxPay
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    internal class ResultNotify:Notify
    {

        public override void ProcessNotify(UpdateOrderStateHandler UpdateOrderState,string appId)
        {
            var WxPayConfig = Configuration.GetAppPayConfig<Config.Sections.Pay.WXPay>(appId);


            Log.Info(this.GetType().ToString(), "ProcessNotify");
            WxPayData notifyData = GetNotifyData(WxPayConfig.KEY);
            
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id,appId))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }
            //查询订单成功
            else
            {
                string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                
                if (UpdateOrderState(out_trade_no, transaction_id))
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                    HttpContext.Current.Response.Write(res.ToXml());
                    HttpContext.Current.Response.End();
                }
                else
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "更新订单:"+ out_trade_no + "状态失败");
                    Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                    HttpContext.Current.Response.Write(res.ToXml());
                    HttpContext.Current.Response.End();
                }
             
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id,string appId)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req,6,appId);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}