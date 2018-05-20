using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Platform.Cashier
{
    public interface ICashierService
    {

        /// <summary>
        /// 签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="OrderNo"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        object GetPaySign(string OrderNo, string SellerID,decimal TotalPrice, EnumPaySignType SignType, string ReturnUrl,string openid="");

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        bool ApplyRefund(string OrderNo, string SellerID, string TradeNo,decimal TotalFee, string RefundNo, decimal RefundFee,string OnlineTransactionNo);

        /// <summary>
        /// 是否不许需要等待退款通知。当申请退款的时候
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="PayType"></param>
        /// <returns></returns>
        bool IsNotWaitRefundNotifyIfApplyRefund(string OrderNo, EnumPayType PayType);
    }
}
