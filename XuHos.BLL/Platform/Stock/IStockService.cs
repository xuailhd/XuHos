using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Platform;

namespace XuHos.BLL.Platform.Order
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public interface IStockService
    {
        
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="order"></param>
        /// <param name="LogisticNo"></param>
        /// <param name="LogisticState"></param>
        /// <returns></returns>
        bool Delivery(string OrderOutID, out string LogisticNo, out EnumLogisticState LogisticState);

        /// <summary>
        /// 预扣库存
        /// </summary>
        /// <param name="DeductionId">库存编号</param>
        /// <returns></returns>
        bool PreDeduction(out string DeductionId);

        /// <summary>
        /// 确认扣库存
        /// </summary>
        /// <param name="DeductionId"></param>
        /// <returns></returns>
        bool ConfirmDeduction(string DeductionId);

        /// <summary>
        /// 恢复库存
        /// </summary>
        /// <param name="OutID"></param>
        /// <returns></returns>
        EnumStockRestoreResult Restore(OrderDTO order,string CancelReason);

    }
}
