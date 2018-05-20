using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;

namespace XuHos.Service.EventHandlers.OrderCreateEvent
{
    /// <summary>
    /// 默认处理
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.OrderCreateEvent>
    {
        public bool Handle(EventBus.Events.OrderCreateEvent evt)
        {
            try
            {
                if (evt == null)
                    return true;

                if (string.IsNullOrEmpty(evt.OrderOutID))
                    return true;

                XuHos.BLL.OrderService order = new BLL.OrderService(evt.UserID);
                var result = order.CreateOrder(new XuHos.DTO.Platform.OrderDTO()
                {
                    UserID = evt.UserID,
                    OrderOutID = evt.OrderOutID,
                    OrderTime = evt.OrderTime,
                    OrderType = evt.OrderType,
                    OrderExpireTime = evt.OrderExpireTime,
                    TotalFee = evt.TotalFee,
                    OrgnazitionID=evt.OrgnazitionID,
                    Details = evt.Details!=null?evt.Details.Select(a => new DTO.Platform.OrderDetailDTO()
                    {
                        Body = a.Body,
                        ProductId = a.ProductId,
                        ProductType = a.ProductType,
                        Quantity = a.Quantity,
                        Subject = a.Subject,
                        UnitPrice = a.UnitPrice

                    }).ToList():new List<DTO.Platform.OrderDetailDTO>(),
                    Consignee = evt.Consignee!=null? new DTO.Platform.OrderConsigneeDTO()
                    {
                        Address = evt.Consignee.Address,
                        IsHosAddress = evt.Consignee.IsHosAddress,
                        Name = evt.Consignee.Name,
                        SendGoodsTime = evt.Consignee.SendGoodsTime,
                        Tel = evt.Consignee.Tel,
                        Id = evt.Consignee.Id
                    }:null
                });
                if (result.OrderNo != "")
                    return true;
               
            }
            catch(Exception E)
            {
                XuHos.Common.LogHelper.WriteError(E);
                
            }
            return false;
        }
    }
}
