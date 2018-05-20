using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using System.ComponentModel.DataAnnotations;
namespace XuHos.DTO.Platform
{
    /// <summary>
    /// 支付业务类
    /// </summary>
    public class OrderDTO
    {
        public OrderDTO()
        {
            PayType = EnumPayType.None;
            OrderType = EnumProductType.Other;
            TradeNo = "";
            OrderNo = "";
            LogisticNo = "";           

        }


        /// <summary>
        /// 物流公司
        /// </summary>
        public string LogisticCompanyName { get; set; }

        /// <summary>
        /// 物流运单号码
        /// </summary>
        public string LogisticWayBillNo { get; set; }

        public string EditBatchNo { get; set; }


        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string OrderNo { get; set; }


        /// <summary>
        /// 交易编号（支付宝交易编号或微信交易编号）
        /// </summary>
        public string TradeNo
        { get; set; }

        /// <summary>
        ///外部订单号码
        /// </summary>
        public string OrderOutID { get; set; }

        /// <summary>
        /// 物流编号
        /// </summary>
        public string LogisticNo { get; set; }

        /// <summary>
        /// 支付类型（0-康美支付；1-微信支付；2-支付宝）
        /// </summary>
        public EnumPayType PayType { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public EnumProductType OrderType { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public EnumOrderState OrderState { get; set; }
        public string OrderStateName { get; set; }


        /// <summary>
        /// 退款状态
        /// </summary>
        public EnumRefundState RefundState { get; set; }
        public string RefundStateName { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public EnumLogisticState LogisticState { get; set; }

        public string  LogisticStateName { get; set; }

        /// <summary>
        /// 消费类型（0-付费、1-免费、2-义诊、3-套餐、4-会员、5-家庭医生）
        /// </summary>
        public EnumCostType CostType { get; set; }

        /// <summary>
        /// 订单提交时间
        /// </summary>
        public DateTime OrderTime
        { get; set; }

        /// <summary>
        /// 订单过期时间
        /// </summary>
        public DateTime OrderExpireTime { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? TradeTime
        { get; set; }

        /// <summary>
        /// 订单取消时间
        /// </summary>
        public DateTime? CancelTime
        { get; set; }

        public string CancelReason
        { get; set; }

        /// <summary>
        /// 订单完成时间
        /// </summary>
        public DateTime? FinishTime
        { get; set; }


        /// <summary>
        /// 商品出库时间
        /// </summary>
        public DateTime? StoreTime
        { get; set; }


        /// <summary>
        /// 物流派送时间
        /// </summary>
        public DateTime? ExpressTime
        { get; set; }


        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime? RefundTime
        { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal? RefundFee
        { get; set; }

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

        public string RefundNo { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgnazitionID { get; set; }


        /// <summary>
        /// 是否已评价
        /// </summary>
        public bool IsEvaluated { get; set; }

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

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserCNName
        { get; set; }

        /// <summary>
        /// 收款账号
        /// </summary>
        public string SellerID
        { get; set; }

        bool? _Cancelable;
        /// <summary>
        /// 是否可取消（未退款、待确认、待支付、已支付 且 物流状态不能是已送达、已发货和配送中的）
        /// </summary>
        public bool Cancelable
        {
            get
            {
                if(_Cancelable.HasValue)
                {
                    return _Cancelable.Value;
                }

                return 
                    this.RefundState == EnumRefundState.NoRefund &&
                    (this.OrderState == EnumOrderState.Paid ||
                    this.OrderState == EnumOrderState.NoPay || 
                    this.OrderState == EnumOrderState.NoConfirm) && 
                    !(
                    this.LogisticState == EnumLogisticState.配送中 ||
                    this.LogisticState == EnumLogisticState.已发货 || 
                    this.LogisticState == EnumLogisticState.已送达);
            }
            set
            {
                _Cancelable = value;
            }
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }


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
        public decimal Discount {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductId { get; set;}

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

    public class OrderRepeatReturnDTO
    {
        /// <summary>
        ///外部订单号码
        /// </summary>
        public string OrderOutID { get; set; }

        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public EnumOrderState OrderState { get; set; }

        public int ChannelID { get; set; }

        /// <summary>
        /// 医生编号
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 是否可以取消
        /// </summary>
        public bool Cancelable { get; set; }
    }


    /// <summary>
    /// 消费折扣 传递DTO
    /// </summary>
    public class DiscountSumitDTO
    {
        /// <summary>
        /// 订单外部关联ID
        /// </summary>
        public string OrderOutID { get; set; }

        public string OrderNo { get; set; }

        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 消费的 医生/商品  的ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 消费折扣的机构
        /// </summary>
        public string OrganizationID { get; set; }

        /// <summary>
        /// 消费折扣的UserID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 服务类型 不能用枚举 有外部系统类型 11-健康指标检测 12-人体经络检测  13-中医体质检测 14-经络梳理按摩 15-养生调理方案 16- 体验店版经络梳理按摩 17 -技师咨询 18-体验店版人体经络检测
        /// </summary>
        public int ServiceType { get; set; }

        /// <summary>
        /// 服务子类型 （ServiceType=14时，1-酸痛检测及生成的按摩程序,2-经典按摩,3-部位按摩,4-技师推送的程序,5-下载的名师按摩）
        /// </summary>
        public int ServiceSubType { get; set; }

        /// <summary>
        /// 消费的设备 ID 
        /// 蒙发利 按摩椅设备编号
        /// </summary>
        public string ConsumerEquipmentID { get; set; }

        /// <summary>
        /// 消费渠道ID
        /// 设备 所在门店ID
        /// </summary>
        public string SalesChannelsID { get; set; }

        /// <summary>
        /// 是否预消费
        /// </summary>
        public bool IsPreUse { get; set; }

        /// <summary>
        /// 折扣类型
        /// </summary>
        public EnumPayPrivilege PayPrivilegeType { get; set; }

        /// <summary>
        /// 消费家庭成员ID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 优惠或折扣ID
        /// </summary>
        public string PrivilegeOutID { get; set; }

        /// <summary>
        /// 用户套餐ID，为空时代表不指定
        /// </summary>
        public string UserPackageID { get; set; }
        /// <summary>
        /// 套餐标识（1-使用时代社区版套餐）
        /// </summary>
        public int UserPackageFlag { get; set; }
    }

}
