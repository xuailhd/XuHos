using System;
using System.Collections.Generic;
using XuHos.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace XuHos.Entity
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public partial class OrderDetail : AuditableEntity
    {
        /// <summary>
        /// 编号
        /// </summary>        
        [Key, MaxLength(32)]
        public string OrderDetailID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(32)]
        public string OrderNO { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [MaxLength(32)]
        [Column(TypeName = "varchar")]
        [Required]
        public string ProductId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>        
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 优惠
        /// </summary>        
        [Required]
        public decimal Discount { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        [Required]

        public decimal Fee { get; set; }  
              

        public string Remarks { get; set; }

        /// <summary>
        /// 分组编号
        /// </summary>
        public int GroupNo { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        [Required]
        public EnumProductType ProductType { get; set; }
    }
}
