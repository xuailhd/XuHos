using System.Web.Http;
using XuHos.Common.Enum;
using XuHos.DTO.Common;
using XuHos.BLL;
using XuHos.Service.Infrastructure.Filters;
using System.Text;
using XuHos.Common;
using XuHos.Common.Pay.WxPay;
using System;
using XuHos.WebApi.Controllers;
using System.Web;

namespace XuHos.WebApi.Platform.Cashier
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class WXPayController : ApiBaseController
    {

        /**
         * @api {GET} /Cashier/WxPay 114401/微信预支付
         * @apiGroup 114 Payment
         * @apiVersion 4.0.0
         * @apiDescription 微信预支付 
         * @apiPermission 已登录
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空 
         * @apiParam {string} OrderNo 订单编号
         * @apiParam {string} [SellerID=wxf1b0cceac4c331e3] 收款账号
         * @apiParam {string} [SignType=0] 签名类型（APP=0,Web=1,Wap=2,JS=3）
         * @apiParam {string} [OpenId] 用户编号
         * @apiParamExample 请求样例：
         * ?OrderNo=42FF1C61132E443F862510FF3BC3B03A
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {Array} Data 配置信息
         * @apiSuccessExample {json} 返回样例:
         *{
                "Data": {
                    "appId": "wx7833444576404bcb",  
                    "timeStamp":"134134132413",
                    "package":"Sign=WXPay",
                    "prepay_id": "wx2016082211541365c425a5bc0232230911",
                    "nonce_str": "ECb1tzPtvX1FcKU8",
                    "partnerId": "1263997401",
                    "sign": "923EDEF1288CD891E95D288DCCB3B5B4",
                },
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
        }
         **/
        /// <summary>
        /// 微信预支付
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [UserAuthenticate(IsValidUserType = false)]
        [Route("~/Cashier/WxPay")]
        public DTO.Common.ApiResult Index(string OrderNo, string SellerID= "wxf1b0cceac4c331e3",EnumPaySignType SignType= EnumPaySignType.App,string ReturnUrl="",string OpenId= "")
        {
            var orderService = new OrderService(CurrentOperatorUserID);
            var orderEntity = orderService.GetOrder(OrderNo);

            if (orderService.SetTradeLog(OrderNo, EnumPayType.WxPay, EnumTradeState.WAIT_BUYER_PAY, "", orderEntity.TotalFee, SellerID))
            {
                var bll = BLL.Platform.Cashier.CashierFactoryService.Create(EnumPayType.WxPay, CurrentOperatorUserID);
                return bll.GetPaySign(OrderNo, SellerID, orderEntity.TotalFee, SignType,ReturnUrl, OpenId).ToApiResultForObject();
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }

        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [Route("~/Cashier/WxPay/NotifyUrl")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult NotifyUrl(string seller_id = "")
        {
            try
            {
                var SellerID = "";
                var out_trade_no = "";


                /*：Receive data from WeChat : <xml><appid><![CDATA[wxf1b0cceac4c331e3]]></appid>
                <attach><![CDATA[视频咨询]]></attach>
                <bank_type><![CDATA[CCB_DEBIT]]></bank_type>
                <cash_fee><![CDATA[1]]></cash_fee>
                <fee_type><![CDATA[CNY]]></fee_type>
                <is_subscribe><![CDATA[N]]></is_subscribe>
                <mch_id><![CDATA[1270127201]]></mch_id>
                <nonce_str><![CDATA[6529dfb8ff07445f9f102dc3983fd435]]></nonce_str>
                <openid><![CDATA[o56u0wINo_JMQuYJ5gI8lZB5TmDE]]></openid>
                <out_trade_no><![CDATA[SP2016101114235527570973]]></out_trade_no>
                <result_code><![CDATA[SUCCESS]]></result_code>
                <return_code><![CDATA[SUCCESS]]></return_code>
                <sign><![CDATA[06973EC5F2CC6692E581711CC19BC37E]]></sign>
                <time_end><![CDATA[20161011142425]]></time_end>
                <total_fee>1</total_fee>
                <trade_type><![CDATA[APP]]></trade_type>
                <transaction_id><![CDATA[4000592001201610116392636086]]></transaction_id>
                </xml>*/

                //接收从微信后台POST过来的数据
                System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();

                var xml = builder.ToString();

                LogHelper.WriteInfo("WXPay NotifyUrl data=" + xml);
                if (xml != "")
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(xml);
                    var appidNode = doc.SelectSingleNode("xml/appid");
                    var out_trade_noNode = doc.SelectSingleNode("xml/out_trade_no");

                    if (appidNode != null && out_trade_noNode != null)
                    {
                        SellerID = appidNode.InnerText;
                        out_trade_no = out_trade_noNode.InnerText;

                        if (updateWxPayStatus(out_trade_no, SellerID))
                        {

                            WxPayData res = new WxPayData();
                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "OK");
                            LogHelper.WriteInfo("order query success : " + res.ToXml());
                            
                            System.Web.HttpContext.Current.Response.Write(res.ToXml());
                            System.Web.HttpContext.Current.Response.End();
                        }
                        else
                        {
                            WxPayData res = new WxPayData();
                            res.SetValue("return_code", "FAIL");
                            res.SetValue("return_msg", "更新支付状态失败");
                            System.Web.HttpContext.Current.Response.Write(res.ToXml());
                            System.Web.HttpContext.Current.Response.End();
                        }
                    }
                    else
                    {
                        WxPayData res = new WxPayData();
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "参数错误");
                        System.Web.HttpContext.Current.Response.Write(res.ToXml());
                        System.Web.HttpContext.Current.Response.End();

                    }
                }
                else
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "参数错误");
                    System.Web.HttpContext.Current.Response.Write(res.ToXml());
                    System.Web.HttpContext.Current.Response.End();

                }



            }
            catch (Exception E)
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", E.Message);
                System.Web.HttpContext.Current.Response.Write(res.ToXml());
                System.Web.HttpContext.Current.Response.End();

                LogHelper.WriteError(E);
            }

            return Content(System.Net.HttpStatusCode.OK, "");
        }


        /// <summary>
        /// 更新微信支付状态
        
        /// 日期：2016年6月25日
        /// </summary>
        /// <param name="OrderNo">订单编号</param>
        /// <returns></returns>
        bool updateWxPayStatus(string OrderNo, string SellerID)
        {
            XuHos.BLL.OrderService service = new OrderService(CurrentOperatorUserID);

            var result = XuHos.Common.Pay.WxPay.OrderQuery.Run("", OrderNo, SellerID);

            if (result.GetValue("result_code").ToString() == "SUCCESS")
            {

                var trade_state = result.GetValue("trade_state").ToString();//订单状态

                //尚未支付
                if (trade_state == "NOTPAY")
                {
                    return false;
                }

                var transaction_id = result.GetValue("transaction_id").ToString();//交易编号
                var total_fee = int.Parse(result.GetValue("total_fee").ToString()) * 100;//转化成元

                //已支付
                if (trade_state == "SUCCESS")
                {

                    using (XuHos.EventBus.MQChannel mqChanne = new EventBus.MQChannel())
                    {
                        return mqChanne.Publish<EventBus.Events.OrderPayNotifyEvent>(new EventBus.Events.OrderPayNotifyEvent()
                        {
                            OrderNo = OrderNo,
                            TradeNo = transaction_id,
                            PayType = EnumPayType.WxPay,
                            SelllerID = SellerID
                        });
                    }

                }

                return false;
            }
            else
            {
                return false;
            }
        }


        /**
              * 
              * 网页授权获取用户基本信息的全部过程
              * 详情请参看网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
              * 第一步：利用url跳转获取code
              * 第二步：利用code去获取openid和access_token
              * 
          */
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [HttpGet]
        [Route("~/Cashier/WxPay/Click")]
        public IHttpActionResult Click(string url = "", string code = "")
        {
            try
            {
                /**
                * 
                * 通过code换取网页授权access_token和openid的返回数据，正确时返回的JSON数据包如下：
                * {
                *  "access_token":"ACCESS_TOKEN",
                *  "expires_in":7200,
                *  "refresh_token":"REFRESH_TOKEN",
                *  "openid":"OPENID",
                *  "scope":"SCOPE",
                *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
                * }
                * 其中access_token可用于获取共享收货地址
                * openid是微信支付jsapi支付接口统一下单时必须的参数
                * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
                * @失败时抛异常WxPayException
                */
                if (!string.IsNullOrEmpty(code))
                {
                    //构造获取openid及access_token的url
                    WxPayData data = new WxPayData();
                    data.SetValue("appid", "");
                    data.SetValue("secret", "");
                    data.SetValue("code", code);
                    data.SetValue("grant_type", "authorization_code");

                    //请求url以获取数据
                    string response = XuHos.Common.Utility.WebAPIHelper.HttpGet($"https://api.weixin.qq.com/sns/oauth2/access_token?{data.ToUrl()}", "");

                    XuHos.Common.LogHelper.WriteDebug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + response);

                    var jd = XuHos.Common.JsonHelper.FromJson<LitJson.JsonData>(response);
                    var access_token = (string)jd["access_token"];
                    var openid = (string)jd["openid"]; //获取用户openid

                    XuHos.Common.LogHelper.WriteDebug(this.GetType().ToString(), "Get openid : " + openid);
                    XuHos.Common.LogHelper.WriteDebug(this.GetType().ToString(), "Get access_token : " + access_token);

                    if (url.EndsWith("&") || url.EndsWith("?"))
                    {
                        //触发微信返回code码         
                        return Redirect($"{url}openid={openid}&access_token={access_token}");
                    }
                    else
                    {
                        //触发微信返回code码         
                        return Redirect($"{url}?openid={openid}&access_token={access_token}");
                    }
                }
                else
                {
                    //构造网页授权获取code的URL
                    string host = Request.RequestUri.Host;
                    string path = Request.RequestUri.AbsolutePath;
                    string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);

                    WxPayData data = new WxPayData();
                    data.SetValue("appid", "");
                    data.SetValue("redirect_uri", redirect_uri);
                    data.SetValue("response_type", "code");
                    data.SetValue("scope", "snsapi_base");
                    data.SetValue("state", "STATE" + "#wechat_redirect");
                    //触发微信返回code码         
                    return Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl());
                }
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                throw new WxPayException(ex.ToString());
            }
        }
    }
}