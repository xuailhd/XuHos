using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{

    /// <summary>
    /// 订单被创建
    /// </summary>
    public class OrderCreateEvent: BaseEvent, IEvent
    {
        public DateTime OrderTime { get; set; }

        public DateTime OrderExpireTime { get; set; }


        /// <summary>
        ///外部订单号码
        /// </summary>
        public string OrderOutID { get; set; }


        /// <summary>
        /// 订单类型
        /// </summary>
        public EnumProductType OrderType { get; set; }
        
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice
        { get; set; }

        decimal _TotalFee = 0;
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TotalFee
        {
            get
            {
                if (Details != null)
                {
                    return Details.Sum(a => a.Fee);
                }
                else
                {
                    return _TotalFee;
                }
            }
            set
            {
                _TotalFee = value;
            }
        }




        /// <summary>
        /// 订单详情
        /// </summary>
        public List<OrderDetailDTO> Details { get; set; }



        /// <summary>
        /// 收货人详细信息
        /// </summary>
        public OrderConsigneeDTO Consignee { get; set; }


        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID
        { get; set; }
        public string OrgnazitionID { get; set; }

        public class OrderDetailDTO
        {
            public OrderDetailDTO()
            {
                Subject = "";
                Body = "";
                UnitPrice = 0;
                Quantity = 0;
                ProductId = "";
                ProductType = EnumProductType.ImageText;
            }
            /// <summary>
            /// 主题
            /// </summary>
            public string Subject
            { get; set; }

            /// <summary>
            /// 订单说明
            /// </summary>
            public string Body
            { get; set; }

            /// <summary>
            /// 单价
            /// </summary>
            public decimal UnitPrice
            { get; set; }

            /// <summary>
            /// 购买数量
            /// </summary>
            public int Quantity { get; set; }


            /// <summary>
            /// 费用（小计）
            /// </summary>
            public decimal Fee
            {
                get
                {
                    return UnitPrice * Quantity - Discount;
                }
            }

            /// <summary>
            /// 优惠
            /// </summary>
            public decimal Discount
            {
                get
                {
                    return 0;
                }
            }

            /// <summary>
            /// 产品编号
            /// </summary>
            public string ProductId { get; set; }

            public EnumProductType ProductType { get; set; }

        }




        /// <summary>
        /// 订单收货侵袭
        /// </summary>
        public class OrderConsigneeDTO
        {
            /// <summary>
            /// 收货人编号
            /// </summary>
            public string Id { get; set; }

            public string Address { get; set; }
            public string Name { get; set; }
            public string Tel { get; set; }
            public Nullable<System.DateTime> SendGoodsTime { get; set; }
            public decimal IsHosAddress { get; set; }
        }
    }



}
