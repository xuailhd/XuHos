using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 会员套餐支付完成事件
    /// </summary>
    public class UserPackagePayCompletedEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 套餐ID
        /// </summary>
        public string UserPackageID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        ///// <summary>
        ///// 用户Code = Users.CreateUserID
        ///// </summary>
        //public string UserCode { get; set; }

        /// <summary>
        /// 操作人员用户ID
        /// </summary>
        public string OperateUserID { get; set; }

        ///// <summary>
        ///// 订单编号
        ///// </summary>
        //public string OrderID { get; set; }

        public string OrgID { get; set; }

        ///// <summary>
        ///// 销售渠道ID 网店ID(易创店) 易创vip-(null)
        ///// </summary>
        //public string SalesChannelsID { get; set; }

        ///// <summary>
        ///// 销售渠道类型 易创vip 0 | 易创店 1
        ///// </summary>
        //public int SalesChannelsType { get; set; }

        ///// <summary>
        ///// 订购方式 1 赠送 |2 现金购买
        ///// </summary>
        //public int BuyType { get; set; }

        /// <summary>
        /// 订单支付时间，有的话则已这个做套餐开始时间
        /// </summary>
        public DateTime? OrderPayTime { get; set; }


        public string OrderNo { get; set; }
    }
}