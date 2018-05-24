using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using XuHos.Common;
using XuHos.Common.Utility;
using XuHos.Common.Cache;
using XuHos.Common.Enum;
using XuHos.Entity;
using XuHos.BLL;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Extensions;
using XuHos.Service.Infrastructure.Filters;
using XuHos.DTO.Platform;
namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrdersController : ApiBaseController
    {
        BLL.OrderService bll;

        public OrdersController()
        {
        }

        /**
         * @apiIgnore Not finished Method
          * @api {POST} /Orders/Create 118001/创建支付订单
          * @apiGroup 118 Order
          * @apiVersion 4.0.0
          * @apiDescription 新增订单 
          * @apiPermission 已登录用户
          * @apiHeader {String} apptoken Users unique access-key.
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
          * @apiParam {string} OrderType=0,1,2,3,4,5 订单类型
          * @apiParam {string} OrderOutID 外部订单编号（预约编号、处方订单编号..）
          * @apiParam {Array} Details 订单详情
          * @apiParamExample {json} 请求样例：
          *{
               "OrderType": "XXXX", ///订单类型（ 挂号=0,图文咨询=1,电话咨询=2,视频咨询=3,处方支付=4，家庭医生=5，会员套餐=6,远程会诊=7,影像判读=8,充值=9,续费升级=11）    
               "OrderOutID": "XXXX",//医生排版编号
               "Details":[{
                "Subject":'主题',
                "Body":'内容',
                "UnitPrice":'0.1',//单价（元）
                "Quantity":'0',//数量
                "ProductId":'XXX',//产品编号
                "ProductType":''//产品类型（ 挂号=0,图文咨询=1,电话咨询=2,视频咨询=3,处方支付=4，家庭医生=5，会员套餐=6,远程会诊=7,影像判读=8,充值=9,续费升级=11）
                }],
                "Consignee": {
                    "Id": "sample string 1",//收货人编号
                    "Address": "sample string 2",//收货地址
                    "Name": "sample string 3",//收货人姓名
                    "Tel": "sample string 4"//收货人电话
                  }   
           *}
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录     
           * @apiSuccess (Response) {string} Data 订单编号
           * @apiSuccessExample {json} 返回样例:
           *{
               "Data":"OrderNo 订单编号",
               "Total":0,
               "Status":0,
               "Msg":""
           *}
       */
        /// <summary>
        /// 创建支付订单
        /// 前置条件：已登录

        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult Create(OrderDTO order)
        {
            if (string.IsNullOrEmpty(order.OrgnazitionID))
                order.OrgnazitionID = CurrentOperatorOrgID;
            bll = new OrderService(CurrentOperatorUserID);
            return bll.CreateOrder(order).OrderNo.ToApiResultForObject();
        }



        /**
         * @apiIgnore Not finished Method
          * @api {POST} /Orders/Cancel 118002/取消订单
          * @apiGroup 118 Order
          * @apiVersion 4.0.0
          * @apiDescription 新增订单 
          * @apiPermission 已登录用户
          * @apiHeader {String} apptoken Users unique access-key.
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
          * @apiParam {string} OrderNo 订单编号
          * @apiParamExample 请求样例：
          * ?OrderNo=42FF1C61132E443F862510FF3BC3B03A
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {bool} Data 是否成功（true:成功，false 失败）
           * @apiSuccessExample {json} 返回样例:
           *{
               "Data":true,
               "Status":0,
               "Msg":""
           *}
       */
        /// <summary>
        /// 取消订单
        /// 前置条件：已登录

        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult Cancel(string OrderNo,string Reason="暂无")
        {
            bll = new OrderService(CurrentOperatorUserID);
            return bll.Cancel(OrderNo, Reason).ToApiResultForBoolean();
        }


        /**
         * @apiIgnore Not finished Method
        * @api {POST} /Orders/Confirm 118003/确认订单
        * @apiGroup 118 Order
        * @apiVersion 4.0.0
        * @apiDescription 设置收货人并确认订单相关信息 
        * @apiPermission 已登录用户
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam (URI Parameters) {string} OrderNo 订单编号
        * @apiParam (URI Parameters) {int} [Privilege] 折扣类型 (默认为0不使用特权)
        * @apiParam (URI Parameters) {string} [UserPackageID] 用户套餐ID (需要指定使用特定套餐的情况下使用)
        * @apiParamExample {object} (Body Parameters)请求样例：
        *{
              OrderNo:"42FF1C61132E443F862510FF3BC3B03A",
              "Privilege":0 // 折扣类型(不使用特权=0,义诊=1,套餐=3,家庭医生=5,机构折扣=6)
              "Consignee": {
                    "Id": "sample string 1",    
                    "Address": "sample string 2",//收货地址
                    "Name": "sample string 3",//收货人姓名
                    "Tel": "sample string 4"//收货人电话    
                }                             
         }
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {bool} Data 是否成功（true:成功，false 取消失败）
         * @apiSuccessExample {json} 返回样例:
         * *{
              "Data":{
                  "OrderNo": "sample string 1",//订单编号
                  "TradeNo": "sample string 2",//交易（支付）编号
                  "OrderOutID": "sample string 3",//外部订单编号
                  "LogisticNo": "sample string 4",//物流编号
                  "PayType": 0,//支付类型 -1=免支付,0=康美支付,1=微信支付,2=支付宝,3=中国银联,4=MasterCard,5=PayPal,6=VISA
                  "OrderType": 0,//订单类型 0=挂号,1=图文咨询,2=电话咨询,3=视频咨询,4=处方支付，5=其他
                  "OrderState": 0,//订单状态（state：0-待支付、1-已支付、2-已完成、3-已取消）
                  "RefundState": 1,//退款状态 1=申请退款,2=已退款,3=拒绝退款
                  "LogisticState": 0,//物流状态 -1=待审核,0=已审核,1=已备货,2=已发货,3=配送中,4=已送达
                  "OrderTime": "2016-08-09T13:21:11.6382466+08:00",//订单时间
                  "TradeTime": "2016-08-09T13:21:11.6412468+08:00",//交易（支付）时间
                  "CancelTime": "2016-08-09T13:21:11.6412468+08:00",//取消订单时间
                  "CancelReason": "sample string 6",//取消原因
                  "FinishTime": "2016-08-09T13:21:11.6422469+08:00",//订单完成时间
                  "StoreTime": "2016-08-09T13:21:11.6422469+08:00",//仓库出库时间
                  "ExpressTime": "2016-08-09T13:21:11.6422469+08:00",//物流发货时间
                  "RefundTime": "2016-08-09T13:21:11.6422469+08:00",//退款时间
                  "RefundFee": 1.0,//退款金额
                  "TotalFee": 24.0,//交易总金额
                  "Details": [
                    {
                      "Subject": "sample string 1"//订单明细（主题）
                      "Body": "sample string 2",//内容
                      "UnitPrice": 3.0,//单价
                      "Quantity": 4,//数量
                      "Fee": 12.0,//费用
                      "Discount": 0.0,//折扣
                      "ProductId": "sample string 5",//产品编号
                      "ProductType": 0//产品类型
                    },
                    {
                      "Subject": "sample string 1",、、
                      "Body": "sample string 2",
                      "UnitPrice": 3.0,
                      "Quantity": 4,
                      "Fee": 12.0,
                      "Discount": 0.0,
                      "ProductId": "sample string 5",
                      "ProductType": 0
                    }
                    ...
                  ],
                  "Consignee": {
                    "Id": "sample string 1",//收货人编号
                    "Address": "sample string 2",//收货地址
                    "Name": "sample string 3",//收货人姓名
                    "Tel": "sample string 4"//收货人电话
                  }
                },
              "Status":0,
              "Msg":""
          *}
     */
        /// <summary>
        /// 确认订单
        /// 前置条件：已登录

        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult Confirm(RequestOrderConfirmDTO request)
        {
            bll = new OrderService(CurrentOperatorUserID);
            EnumApiStatus status = bll.Confirm(request.OrderNo, request);
            if (status == EnumApiStatus.BizOK)
            {
                return bll.GetOrder(request.OrderNo).ToApiResultForObject();
            }
            else
            {
                return status.ToApiResultForApiStatus();
            }
        }


        /**
         * @apiIgnore Not finished Method
         * @api {POST} /Orders/Complete 118004/交易完成
         * @apiGroup 118 Order
         * @apiVersion 4.0.0
         * @apiDescription 交易完成后调用 
         * @apiPermission 已登录用户
         * @apiHeader {String} apptoken Users unique access-key.
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
         * @apiParam {string} OrderNo 订单编号
         * @apiParamExample 请求样例：
         * ?OrderNo=42FF1C61132E443F862510FF3BC3B03A&RefundNo=
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {bool} Data 是否成功（true:成功，false 取消失败）
          * @apiSuccessExample {json} 返回样例:
          *{
              "Data":true,
              "Status":0,
              "Msg":""
          *}
      */
        /// <summary>
        /// 交易完成
        /// 前置条件：已登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult Complete(string OrderNo)
        {
            bll = new OrderService(CurrentOperatorUserID);
            return bll.Complete(OrderNo).ToApiResultForBoolean();
        }



        /**
         * @apiIgnore Not finished Method
         * @api {GET} /Orders 118005/获取订单详情
         * @apiGroup 118 Order
         * @apiDescription 交易完成后调用 
         * @apiPermission 已登录用户
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空   
         * @apiParam {string} OrderNo 订单编号
         * @apiParamExample 请求样例：
         * ?OrderNo=42FF1C61132E443F862510FF3BC3B03A
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {object} Data 订单详情
          * @apiSuccessExample {json} 返回样例:
          *{
              "Data":{
                  "OrderNo": "sample string 1",//订单编号
                  "TradeNo": "sample string 2",//交易（支付）编号
                  "OrderOutID": "sample string 3",//外部订单编号
                  "LogisticNo": "sample string 4",//物流编号
                  "PayType": 0,//支付类型 -1=免支付,0=康美支付,1=微信支付,2=支付宝,3=中国银联,4=MasterCard,5=PayPal,6=VISA
                  "OrderType": 0,//订单类型 0=挂号,1=图文咨询,2=电话咨询,3=视频咨询,4=处方支付，5=其他
                  "OrderState": 0,// 订单状态（state：-1=待确认、0=待支付、1=已支付、2=已完成、3=已取消）
                  "RefundState": 1,//退款状态 1=申请退款,2=已退款,3=拒绝退款
                  "LogisticState": 0,//物流状态 -1=待审核,0=已审核,1=已备货,2=已发货,3=配送中,4=已送达
                  "OrderTime": "2016-08-09T13:21:11.6382466+08:00",//订单时间
                  "TradeTime": "2016-08-09T13:21:11.6412468+08:00",//交易（支付）时间
                  "CancelTime": "2016-08-09T13:21:11.6412468+08:00",//取消订单时间
                  "CancelReason": "sample string 6",//取消原因
                  "FinishTime": "2016-08-09T13:21:11.6422469+08:00",//订单完成时间
                  "StoreTime": "2016-08-09T13:21:11.6422469+08:00",//仓库出库时间
                  "ExpressTime": "2016-08-09T13:21:11.6422469+08:00",//物流发货时间
                  "RefundTime": "2016-08-09T13:21:11.6422469+08:00",//退款时间
                  "RefundFee": 1.0,//退款金额
                  "TotalFee": 24.0,//交易总金额
                  "Details": [
                    {
                      "Subject": "sample string 1"//订单明细（主题）
                      "Body": "sample string 2",//内容
                      "UnitPrice": 3.0,//单价
                      "Quantity": 4,//数量
                      "Fee": 12.0,//费用
                      "Discount": 0.0,//折扣
                      "ProductId": "sample string 5",//产品编号
                      "ProductType": 0//产品类型
                    },
                    {
                      "Subject": "sample string 1",、、
                      "Body": "sample string 2",
                      "UnitPrice": 3.0,
                      "Quantity": 4,
                      "Fee": 12.0,
                      "Discount": 0.0,
                      "ProductId": "sample string 5",
                      "ProductType": 0
                    }
                    ...
                  ],
                  "Consignee": {
                    "Id": "sample string 1",//收货人编号
                    "Address": "sample string 2",//收货地址
                    "Name": "sample string 3",//收货人姓名
                    "Tel": "sample string 4"//收货人电话
                  }
                },
              "Status":0,
              "Msg":""
          *}
      */
        /// <summary>
        /// 获取订单详情
        /// 前置条件：已登录

        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [Route("~/Orders")]
        [HttpGet]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetOrder(string OrderNo)
        {
            bll = new OrderService(CurrentOperatorUserID);
            return bll.GetOrder(OrderNo).ToApiResultForObject();
        }


        /**
         * @apiIgnore Not finished Method
     * @api {GET} /Orders/LogisticWithDelivery 118007/开始发货
     * @apiGroup 118 Order
     * @apiVersion 4.0.0
     * @apiDescription 支付完成后发货（系统会自动发货，一般情况不需要调用） 
     * @apiPermission 已登录用户
     * @apiHeader {String} apptoken Users unique access-key.
     * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
     * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
     * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
     * @apiParam {string} LogisticNo 物流编号
     * @apiParamExample 请求样例：
     * ?LogisticNo=TD16070400010
      * @apiSuccess (Response) {String} Msg 提示信息 
      * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
      * @apiSuccess (Response) {object} Data 订单详情
      * @apiSuccessExample {json} 返回样例:
      *{
                "Data": true,
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
    }
    **/
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="LogisticNo">物流编号</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult LogisticWithDelivery(string OrderNo)
        {
            bll = new OrderService(CurrentOperatorUserID);
            return bll.LogisticWithDelivery(OrderNo).ToApiResultForBoolean();
        }

        /// <summary>
        /// 118008
        /// 提供给BAT同步订单数据
        /// </summary>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpGet]
        public ApiResult GetOrderListBAT(string startTime, string endTime)
        {
            bll = new OrderService(CurrentOperatorUserID);
            var result = bll.QueryOrderListBAT(DateTime.Parse(startTime), DateTime.Parse(endTime));
            return result.ToApiResultForList();
        }
    }
}
