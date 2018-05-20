using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace XuHos.Entity
{
  
    /// <summary>
    /// 订单收货人
    /// </summary>
    public partial class OrderConsignee : AuditableEntity
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        [Key]
        [MaxLength(32)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string ConsigneeAddress { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [MaxLength(50)]
        [Required]

        public string ConsigneeName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string ConsigneeTel { get; set; }

        /// <summary>
        /// 送货时间
        /// </summary>
        public DateTime? SendGoodsTime { get; set; }

        /// <summary>
        /// 是否家庭地址
        /// </summary>
        public decimal IsHosAddress { get; set; }     
    }
}
