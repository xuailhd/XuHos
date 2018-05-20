using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;
namespace XuHos.Entity
{

    public class Order : AuditableEntity
    {
        public Order()
        {
            PayType = EnumPayType.None;
            OrderTime = DateTime.Now;
            OrderExpireTime = OrderTime.AddMinutes(30);
            RefundState = EnumRefundState.NoRefund;
            OrderState = EnumOrderState.NoConfirm;
            totalFee = 0;
            RefundFee = 0;
            LogisticState = EnumLogisticState.待审核;
        }

        /// <summary>
        /// 订单号
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

  

        /// <summary>
        /// 订单外部ID(关联外部订单)
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string OrderOutID { get; set; }

        /// <summary>
        /// 订单状态（state：0-待支付、1-已支付、2-已完成、3-已取消）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumOrderState OrderState { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumProductType OrderType { get; set; }

        /// <summary>
        /// 消费类型（0-付费、1-免费、2-义诊、3-套餐、4-会员、5-家庭医生）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumCostType CostType { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [Required]
        [Column(TypeName = "decimal")]
        public decimal RefundFee { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string RefundNo { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumRefundState RefundState { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime RefundTime { get; set; }

        /// <summary>
        /// 支付类型（state：-1-免支付、0-康美支付、1-微信支付、2-支付宝、3-中国银联、4-MasterCard、5-PayPal、6-VISA）
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumPayType PayType { get; set; }

        /// <summary>
        /// 第三方支付交易号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime StoreTime { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ExpressTime { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        [Required]
        [Column(TypeName = "decimal")]
        public decimal OriginalPrice
        { get; set; }

        /// <summary>
        /// 交易金额（打折后的应付价格）
        /// </summary>
        [Required]
        [Column(TypeName = "decimal")]
        public decimal totalFee { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 订单过期时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime OrderExpireTime { get; set; }

        /// <summary>
        /// 订单取消时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CancelTime { get; set; }

        /// <summary>
        /// 订单完成时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string CancelReason { get; set; }


        /// <summary>
        /// 物流编号
        /// </summary>
        [MaxLength(32)]
        public string LogisticNo { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public EnumLogisticState LogisticState { get; set; }

        /// <summary>
        /// 物流公司
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string LogisticCompanyName { get; set; }

        /// <summary>
        /// 物流运单号码
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string LogisticWayBillNo { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        /// <summary>
        /// 收款账号
        /// </summary>
        public string SellerID
        { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OrgnazitionID { get; set; }

        /// <summary>
        /// 是否已评价
        /// </summary>
        [Column(TypeName = "bit")]
        public bool IsEvaluated { get; set; }
    }


}
