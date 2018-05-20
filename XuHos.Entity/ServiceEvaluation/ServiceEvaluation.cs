using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public partial class ServiceEvaluation : AuditableEntity
    {
        /// <summary>
        /// ServiceEvaluationID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ServiceEvaluationID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        /// <summary>
        /// 外部订单ID
        /// </summary>		
        public string OuterID { get; set; }

        /// <summary>
        /// 评价分值
        /// </summary>		
        public int Score { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        /// <summary>
        /// 评价标签，多个标签以(;)分割
        /// </summary>		
        public string EvaluationTags { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(1024)]
        public string Content { get; set; }

        /// <summary>
        /// 服务提供者ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ProviderID { get; set; }

        /// <summary>
        /// 服务类型(0-挂号、1-图文咨询、2-语音问诊、3-视频问诊、4-处方支付、5-家庭医生、6-会员套餐、7-远程会诊、8-影像判读、100-其它)
        /// </summary>
        [Required]
        public EnumProductType ServiceType { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }
    }
}
