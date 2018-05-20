using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XuHos.Common;
using XuHos.Common.Pay.WxPay;
using XuHos.DTO;
using XuHos.Extensions;
using XuHos.DTO;
using XuHos.Common.Enum;

namespace XuHos.BLL.Platform.Cashier
{
    public class WXPayCashierService : BaseCashierService, ICashierService
    {
        public string CurrentOperatorUserID { get; set; }
        public WXPayCashierService(string CurrentOperatorUserID)
        {
            this.CurrentOperatorUserID = CurrentOperatorUserID;
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public override object GetPaySign(string OrderNo, string SellerID, decimal TotalPrice, EnumPaySignType SignType, string ReturnUrl = "", string OpenId = "")
        {
            BLL.OrderService orderService = new BLL.OrderService(CurrentOperatorUserID);
            DTO.Platform.OrderDTO order = orderService.GetOrder(OrderNo);

            if (order != null)
            {
                #region 获取订单说明和描述

                string Subject = "-";
                string Body = order.OrderType.GetEnumDescript();
                if (order.Details != null && order.Details.Count == 1)
                {
                    Subject = order.Details[0].Subject;
                }
                else if (order.Details != null)
                {
                    Subject = string.Format("共{0}件商品", order.Details.Count);
                }
                else
                {
                    Subject = string.Format("共{0}件商品", 0);
                }

                #endregion

                UnifiedOrder pay = new UnifiedOrder();
                
                if (SignType == EnumPaySignType.Web)
                {
                    return pay.GetQRCodeUrlWhenWeb(OrderNo, Subject, Body, TotalPrice.ToString(), SellerID);
                }
                //H5支付方式
                else if (SignType == EnumPaySignType.Wap)
                {
                    return pay.GetJumpUrlWhenMWeb(OrderNo, Subject, Body, TotalPrice.ToString(), SellerID);
                }
                //JS支付（公众号）
                else if (SignType == EnumPaySignType.Js)
                {
                    var pre_payId = pay.GetPrepayIdWhenJsSdk(OrderNo, Subject, Body, TotalPrice.ToString(), SellerID, OpenId, order.OrderOutID);
                    return GetJsApiParameters(pre_payId, SellerID);
                }
                else if (SignType == EnumPaySignType.App)
                {
                    var pre_payId = pay.GetPrepayIdWhenApp(OrderNo, Subject, Body, TotalPrice.ToString(), SellerID);
                    return GetAppApiParamters(pre_payId, SellerID);
                }
                else
                {
                    return pay.GetQRCodeUrlWhenWeb(OrderNo, Subject, Body, TotalPrice.ToString(), SellerID);
                }
            }
            else
            {
                throw new System.ArgumentException("订单不存在");
            }
        }

        public override bool ApplyRefund(string OrderNo, string SellerID, string TradeNo, decimal TotalFee, string RefundNo, decimal RefundFee, string OnlineTransactionNo)
        {
            if (base.ApplyRefund(OrderNo, SellerID, TradeNo, TotalFee, RefundNo, RefundFee, OnlineTransactionNo))
            {
                //调用订单退款接口,如果内部出现异常则在页面上显示异常原因
                try
                {
                    XuHos.BLL.OrderService service = new OrderService(CurrentOperatorUserID);

                    var order = service.GetOrder(OrderNo);

                    #region 计算退款金额      
                    //转换单位，元>分
                    float _RefundFee = float.Parse(RefundFee.ToString()) * 100;
                    float _TotalFee = float.Parse(order.TotalFee.ToString()) * 100;
                    #endregion

                    var queryResult = XuHos.Common.Pay.WxPay.OrderQuery.Run("", OrderNo, SellerID);

                    if (queryResult.GetValue("result_code").ToString() == "SUCCESS")
                    {
                        var trade_state = queryResult.GetValue("trade_state").ToString();//订单状态

                        //已经退款
                        if (trade_state == "REFUND")
                        {
                            return service.RefundCompleted(OrderNo, EnumPayType.WxPay);
                        }
                        else
                        {
                            if (_RefundFee <= 0)
                            {
                                return true;
                            }
                            else
                            {
                                //发起退款申请
                                var result = XuHos.Common.Pay.WxPay.Refund.Run(TradeNo, OrderNo, _TotalFee.ToString(), _RefundFee.ToString(), SellerID);

                                //退款成功
                                if (result.GetValue("result_code").ToString() == "SUCCESS")
                                {
                                    return true;
                                }
                            }
                        }
                    }

                }
                catch (WxPayException ex)
                {
                    XuHos.Common.LogHelper.WriteError(ex);
                }
                catch (Exception ex)
                {
                    XuHos.Common.LogHelper.WriteError(ex);
                }
            }

            return false;
        }


        /**
       *  
       * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
       * 微信浏览器调起JSAPI时的输入参数格式如下：
       * {
       *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
       *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
       *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
       *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
       *   "signType" : "MD5",         //微信签名方式:    
       *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
       * }
       * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
       * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
       * 
       */
        ResponseWXPayDTO GetJsApiParameters(string prepay_id, string sellerId)
        {
            var WxPayConfig = XuHos.Common.Pay.Configuration.GetAppPayConfig<XuHos.Common.Config.Sections.Pay.WXPay>(sellerId);

            var nonce_str = System.Guid.NewGuid().ToString("N");
            var timeStamp = System.DateTime.Now.ToTimeStamp().ToString();
            var package = "prepay_id=" + prepay_id;
            var signType = "MD5";
            var paySign = "";

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", WxPayConfig.APPID);
            jsApiParam.SetValue("timeStamp", timeStamp);
            jsApiParam.SetValue("nonceStr", nonce_str);
            jsApiParam.SetValue("package", package);
            jsApiParam.SetValue("signType", signType);
            paySign = jsApiParam.MakeSign(WxPayConfig.KEY);
            jsApiParam.SetValue("paySign", paySign);

            return new ResponseWXPayDTO()
            {
                appid = WxPayConfig.APPID,
                timeStamp = timeStamp,
                nonce_str = nonce_str,
                package = package,
                signType = signType,
                sign = paySign,
                partnerId = WxPayConfig.MCHID,
                prepay_id = prepay_id
            };
        }


        ResponseWXPayDTO GetAppApiParamters(string prepay_id, string SellerID)
        {
            if (prepay_id != "")
            {
                var WxPayConfig = XuHos.Common.Pay.Configuration.GetAppPayConfig<XuHos.Common.Config.Sections.Pay.WXPay>(SellerID);

                ResponseWXPayDTO result = new ResponseWXPayDTO();
                result.timeStamp = System.DateTime.Now.ToTimeStamp().ToString();
                result.package = "Sign=WXPay";
                result.appid = WxPayConfig.APPID;
                result.prepay_id = prepay_id;
                result.nonce_str = System.Guid.NewGuid().ToString("N");
                result.partnerId = WxPayConfig.MCHID;
                result.signType = "MD5";
                result.sign = "";


                WxPayData data = new WxPayData();
                data.SetValue("appid", result.appid);
                data.SetValue("partnerid", result.partnerId);
                data.SetValue("prepayid", result.prepay_id);
                data.SetValue("noncestr", result.nonce_str);
                data.SetValue("timestamp", result.timeStamp);
                data.SetValue("package", result.package);
                result.sign = data.MakeSign(WxPayConfig.KEY);
                return result;
            }
            else
            {
                return new ResponseWXPayDTO();
            }
        }


    }
}
