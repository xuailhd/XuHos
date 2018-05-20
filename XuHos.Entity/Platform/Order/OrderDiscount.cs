using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;

namespace XuHos.Entity
{
    /// <summary>
    /// 订单折扣
    /// </summary>

    public partial class OrderDiscount : AuditableEntity
    {
        /// <summary>
        /// 订单日志ID
        /// </summary>
        [Key]
        [MaxLength(32)]
        public string OrderDiscountID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(32)]
        [Required]
        public string OrderNo { get; set; }

        /// <summary>
        /// 特权类型
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public XuHos.Common.Enum.EnumPayPrivilege Privilege { get; set; }

        /// <summary>
        /// 优惠价格
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 消费状态
        /// </summary>
        public EnumDiscountConsumeState State { get; set;}

        [MaxLength(32)]
        public string PrivilegeOutID { get; set; }
    }
}
