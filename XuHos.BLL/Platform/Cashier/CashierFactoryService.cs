using System;
using System.Net;
using XuHos.Common.Pay.AliPay;
using XuHos.Common.Pay.WxPay;
using XuHos.DTO.Common;
using XuHos.DTO;
using XuHos.Extensions;
using System.Collections.Generic;
namespace XuHos.BLL.Platform.Cashier
{


    /// <summary>
    /// 收银台
    
    /// 日期：2016年8月22日
    /// </summary>
    public class CashierFactoryService
    {

        public static ICashierService Create(XuHos.Common.Enum.EnumPayType type,string CurrentOperatorUserID)
        {
            switch (type)
            {
                case XuHos.Common.Enum.EnumPayType.WxPay:
                    return new WXPayCashierService(CurrentOperatorUserID);
                case XuHos.Common.Enum.EnumPayType.AliPay:
                    return new AliPayCashierService(CurrentOperatorUserID);
                case XuHos.Common.Enum.EnumPayType.UnionPay:
                case XuHos.Common.Enum.EnumPayType.MasterCard:
                case XuHos.Common.Enum.EnumPayType.PayPal:
                case XuHos.Common.Enum.EnumPayType.VISA:
              
                default:
                    return null;
            }

        }

    }
}
