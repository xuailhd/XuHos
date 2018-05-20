using System;
using System.Net;
using XuHos.Common.Pay.AliPay;
using XuHos.Common.Pay.WxPay;
using XuHos.DTO.Common;
using XuHos.DTO;
using XuHos.Extensions;
using System.Collections.Generic;
namespace XuHos.BLL.Platform.Order
{
    /// <summary>
    /// 外部订单
    
    /// 日期：2016年8月22日
    /// </summary>
    public class StockFactoryService
    {
        public static IStockService Create(XuHos.Common.Enum.EnumProductType type)
        {
            switch (type)
            {
                case XuHos.Common.Enum.EnumProductType.Phone:
                    return new XuHos.BLL.Platform.Order.VideoConsultStockService();
                case XuHos.Common.Enum.EnumProductType.video:
                    return new XuHos.BLL.Platform.Order.VideoConsultStockService();
                case XuHos.Common.Enum.EnumProductType.Other:
                    break;
                default:
                    break;
                   
            }
            return null;
        }

    }
}
