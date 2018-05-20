using System.Web.Http;
using XuHos.Common.Enum;
using XuHos.DTO.Common;
using XuHos.BLL;
using System.Collections.Generic;
using XuHos.Common.Pay.AliPay;
using System.Collections.Specialized;
using XuHos.Common;
using System;
using XuHos.Service.Infrastructure.Filters;
using System.Net.Http;
using System.Text;
using XuHos.BLL.Sys;
using XuHos.WebApi.Controllers;
using XuHos.BLL.Sys.Implements;

namespace XuHos.WebApi.Platform.Cashier
{
    /// <summary>
    /// 收银台接口
    /// </summary>
    public class AliPayController : ApiBaseController
    {
        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            var Request = System.Web.HttpContext.Current.Request;
            SortedDictionary <string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;
            
            // Get names of all forms into a string array.
            var requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            var Request = System.Web.HttpContext.Current.Request;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            var requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        /**
         * @api {GET} /Cashier/AliPay 114001/支付宝预支付
         * @apiGroup 114 Payment
         * @apiVersion 4.0.0
         * @apiDescription 获取云通信独立认证配置 
         * @apiPermission 已登录
         * @apiHeader {String} apptoken appToken
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
         * @apiParam {string} OrderNo 订单编号
         * @apiParam {string} [SellerID=wxf1b0cceac4c331e3]收款账号
         * @apiParam {string} [SignType=0] 签名类型（APP=0,Web=1,Wap=2）
         * @apiParam {string} [ReturnUrl] 返回地址
         * @apiParamExample 请求样例：
         * ?OrderNo=42FF1C61132E443F862510FF3BC3B03A         
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {Array} Data 配置信息
         * @apiSuccessExample {json} 返回样例:
         *{
            "Data": "_input_charset=utf-8&notify_url=http%3a%2f%2fwww.kmwlyy.com%2f%2fCashier%2fAliPay%NotifyUrl&out_trade_no=KM2016081214142447185198&partner=2088021337472610&payment_type=1&return_url=http%3a%2f%2fwww.kmwlyy.com%2f%2fCashier%2fAliPay%2fReturnUrl&seller_id=2088021337472610&service=create_direct_pay_by_user&total_fee=2.76&sign=696f5a2a70356988a384259323b7635e&sign_type=MD5",
            "Total": 0,
            "Status": 0,
            "Msg": "操作成功"
        }
         **/
        /// <summary>
        /// 支付宝预支付
        /// </summary>
        /// <param name="OrderNo"></param>   
        /// <returns></returns>
        [HttpGet]
        [Route("~/Cashier/AliPay")]
        [UserAuthenticate(IsValidUserType =false) ]
        public DTO.Common.ApiResult Index(string OrderNo,string SellerID="",EnumPaySignType SignType= EnumPaySignType.App,string ReturnUrl="")
        {
            //设置默认收款人
            if (string.IsNullOrEmpty(SellerID))
            {
                var Config = SysConfigService.Get<XuHos.Common.Config.Sections.Pay>();
                SellerID = Config.AliPayDefaultSellerId;
            }

       
            var orderService = new OrderService(CurrentOperatorUserID);
            var orderEntity = orderService.GetOrder(OrderNo);
            if (orderService.SetTradeLog(OrderNo, EnumPayType.AliPay, EnumTradeState.WAIT_BUYER_PAY, "", orderEntity.TotalFee, SellerID))
            {
                var bll = BLL.Platform.Cashier.CashierFactoryService.Create(EnumPayType.AliPay, CurrentOperatorUserID);
                return bll.GetPaySign(OrderNo, SellerID, orderEntity.TotalFee, SignType,ReturnUrl).ToApiResultForObject();
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }


        /// <summary>
        /// 退款异步通知页面
        /// </summary>
        /// <returns></returns>
        [Route("~/Cashier/AliPay/RefundNotifyUrl")]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage RefundNotifyUrl()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();
            var ret = "fail";
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                XuHos.BLL.OrderService service = new OrderService(CurrentOperatorUserID);
                var Request = System.Web.HttpContext.Current.Request;
                //批次号
                string batch_no = Request.Form["batch_no"];              
            
                //批量退款数据中转账成功的笔数
                string success_num = Request.Form["success_num"];
                //批量退款数据中的详细信息
                string result_details = Request.Form["result_details"];
                //通知编号
                string notify_id = Request.Form["notify_id"];
                //签名
                string sign = Request.Form["sign"];

                var order = service.GetOrder(batch_no);

                var alipay = new WebPay(order.SellerID);

                if (alipay.VerifySign(sPara, notify_id, sign))//验证成功
                {
                    if (service.RefundCompleted(order.OrderNo, EnumPayType.AliPay))
                    {
                        ret = "success";
                    }
                }             
            }

         


            //返回处理结果
            return new HttpResponseMessage
            {
                Content = new StringContent(ret, Encoding.GetEncoding("UTF-8"), "text/plain")
            };
        }

        /// <summary>
        /// 支付完成异步通知
        /// </summary>
        /// <returns></returns>
        [Route("~/Cashier/AliPay/NotifyUrl")]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage NotifyUrl()
        {
            var Request = System.Web.HttpContext.Current.Request;
            var content = "";
            try
            {
                LogHelper.WriteDebug("Cashier/AliPay/NotifyUrl", Request.Params.ToJson());
                SortedDictionary<string, string> sPara = GetRequestPost();

                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    //通知编号
                    string seller_id = Request.Form["seller_id"];
                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];
                    //支付宝交易号
                    string trade_no = Request.Form["trade_no"];
                    //交易状态
                    string trade_status = Request.Form["trade_status"];
                    //总金额
                    string total_fee = Request["total_fee"];
                    //通知编号
                    string notify_id = Request.Form["notify_id"];
                  
                    //签名
                    string sign = Request.Form["sign"];

                    var alipay = new WebPay(seller_id);

                    if ((trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS") && alipay.VerifySign(sPara, notify_id, sign))//验证成功
                    {
                        using (XuHos.EventBus.MQChannel mqChanne = new EventBus.MQChannel())
                        {
                            if (mqChanne.Publish<EventBus.Events.OrderPayNotifyEvent>(new EventBus.Events.OrderPayNotifyEvent()
                            {

                                OrderNo = out_trade_no,
                                TradeNo = trade_no,
                                PayType = EnumPayType.AliPay,
                                SelllerID = seller_id
                            }))
                            {
                                content = "success";                              
                            }
                        }
                                           
                    }
                  
                }

                content = "fail";

            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
                content = "fail";
            }

            //返回处理结果
            return new HttpResponseMessage
            {
                Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "text/plain")
            };
        }


        /// <summary>
        /// 付款完成返回
        
        /// 日期：2016年6月24日
        /// </summary>
        /// <returns></returns>
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [HttpGet]
        [HttpPost]
        [Route("~/Cashier/AliPay/ReturnUrl")]
        public IHttpActionResult ReturnUrl()
        {
            var payConfig = SysConfigService.Get<XuHos.Common.Config.Sections.Pay>();

            try
            {

                var Request = System.Web.HttpContext.Current.Request;
                var sPara = GetRequestGet();
            

                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    string seller_id = Request.QueryString["seller_id"];
                    //商户订单号
                    string out_trade_no = Request.QueryString["out_trade_no"];

                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];

                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];

                    //总金额
                    string total_fee = Request["total_fee"];
                    //通知编号
                    string notify_id = Request.QueryString["notify_id"];
                    //签名
                    string sign = Request.QueryString["sign"];
                    var alipay = new WebPay(seller_id);
                    //验证成功
                    if ((trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS"))
                    {
                        if (alipay.VerifySign(sPara, notify_id, sign))
                        {
                            XuHos.BLL.OrderService service = new OrderService(CurrentOperatorUserID);

                            var order = service.GetOrder(out_trade_no);

                            if (order != null)
                            {
                                if (order.TotalFee == decimal.Parse(total_fee))
                                {
                                    if (service.PayCompleted(out_trade_no, trade_no, XuHos.Common.Enum.EnumPayType.AliPay, seller_id))
                                    {
                                        return Redirect($"{payConfig.ReturnUrlPrefix}/Trade/Order/PaySuccessfully?OrderNo={out_trade_no}");
                                    }
                                    else
                                    {
                                        return Redirect($"{payConfig.ReturnUrlPrefix}/Trade/Order/Error?message=系统错误，更新订单状态失败");
                                    }
                                }
                                else
                                {
                                    return Redirect($"{payConfig.ReturnUrlPrefix}/Trade/Order/Error?message=参数非法，参数“totalFee”或 “seller_id”不正确");
                                }
                            }
                            else
                            {
                                return Redirect($"{payConfig.ReturnUrlPrefix}/Trade/Order/Error?message=参数非法，订单不存在");
                            }
                        }
                        else
                        {
                            return Redirect($"{payConfig.ReturnUrlPrefix}/Trade/Order/PaySuccessfully?OrderNo={out_trade_no}");
                        }
                    }

                  
                }
                return Redirect($"{payConfig.ReturnUrlPrefix}/Trade/Order/Error?message=参数非法，不是合法的支付请求");
            }
            catch (Exception E)
            {
                return Redirect($"{payConfig.ReturnUrlPrefix}/Trade/Order/Error?message={E.Message}");
            }


        }

    }
}