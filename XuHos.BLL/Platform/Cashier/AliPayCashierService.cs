using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common;
using XuHos.Common.Pay.AliPay;
using XuHos.Extensions;
using XuHos.Common.Enum;

namespace XuHos.BLL.Platform.Cashier
{
    public class AliPayCashierService :BaseCashierService,ICashierService
    {
        public string CurrentOperatorUserID { get; set; }

        public AliPayCashierService(string CurrentOperatorUserID)
        {
            this.CurrentOperatorUserID = CurrentOperatorUserID;
        }

        /// <summary>
        /// 阿里支付
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public override object GetPaySign(string OrderNo, string SellerID, decimal TotalPrice,EnumPaySignType SignType,string ReturnUrl="",string OpenId="")
        {
            BLL.OrderService bll = new BLL.OrderService(CurrentOperatorUserID);
            DTO.Platform.OrderDTO order = bll.GetOrder(OrderNo);
            
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

                    NavivePay pay = new NavivePay(SellerID);

                    if (SignType == EnumPaySignType.App)
                    {
                        return pay.GetMobilePayParams(OrderNo, Subject, Body, TotalPrice.ToString(), ReturnUrl);
                    }
                    else
                    {
                        return pay.GetWapPayParams(OrderNo, Subject, Body, TotalPrice.ToString(), ReturnUrl);
                    }
            }
            else
            {
                throw new System.ArgumentException("订单不存在");
            }
        }
        

        /// <summary>
        /// 支付宝退款
        
        /// 日期：2016年10月17日
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public override bool ApplyRefund(string OrderNo,string SellerID, string TradeNo, decimal TotalFee, string RefundNo,decimal RefundFee,string OnlineTransactionNo)
        {
            if (base.ApplyRefund(OrderNo,SellerID,TradeNo,TotalFee,RefundNo,RefundFee, OnlineTransactionNo))
            {
                //没有收款人
                if (SellerID == "")
                {
                    return true;
                }

                XuHos.Common.Pay.AliPay.WebPay alipay = new WebPay(SellerID);
                return alipay.ApplyRefund(SellerID, TradeNo, "", RefundFee, RefundNo);

            }

            return false;
        }

    }
}
