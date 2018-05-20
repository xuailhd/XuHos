using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    /// <summary>
    /// 订单日志
    /// </summary>
    
    public partial class OrderLog : AuditableEntity
    {
        /// <summary>
        /// 订单日志ID
        /// </summary>
        [Key]
        [MaxLength(32)]
        public string OrderLogID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(32)]
        [Required]
        public string OrderNo { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public XuHos.Common.Enum.EnumEnumOrderLogOperationType OperationType { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        [Required,MaxLength(200)]
        public string OperationDesc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required,MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Required]
        public System.DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        [Required,MaxLength(32)]
        public string OperationUserID { get; set; }
    }
}
