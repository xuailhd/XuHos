
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public class OrderCallbackLog : AuditableEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string CallbackLogID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OrgID { get; set; }

        /// <summary>
        /// 回调状态：0-失败，1-成功
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 第一回调失败后，尝试回调次数
        /// </summary>
        public int TriedTimes { get; set; }

        /// <summary>
        /// 回调返回信息
        /// </summary>
        [Column(TypeName = "nvarchar")]
        public string Message { get; set; }
    }
}
