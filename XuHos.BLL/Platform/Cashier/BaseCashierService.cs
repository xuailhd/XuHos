using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Platform.Cashier
{

    public abstract class BaseCashierService : ICashierService
    {

        public abstract object GetPaySign(string OrderNo, string SellerID, decimal TotalPrice, EnumPaySignType SignType, string ReturnUrl, string OpenId);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public virtual bool ApplyRefund(string OrderNo, string SellerID, string TradeNo, decimal TotalFee, string RefundNo, decimal RefundFee, string OnlineTransactionNo)
        {
            return true;
        }

        public virtual bool IsNotWaitRefundNotifyIfApplyRefund(string OrderNo, EnumPayType PayType)
        {
            return true;
        }
    }
}
