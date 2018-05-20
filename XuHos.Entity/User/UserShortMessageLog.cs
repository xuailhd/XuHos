using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    

    /// <summary>
    /// 短信记录表
    /// </summary>
    public partial class UserShortMessageLog : AuditableEntity
    {
        /// <summary>
        /// 日志编号
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ShortMessageLogID { get; set; }

        /// <summary>
        /// 短信验证码类型(1:用户注册,2:找回密码,3设置支付密码,4绑定手机号,5登陆账号开通密码通知,6会诊单创建通知，7会诊单付款通知，8会诊单支付成功通知)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int MsgLogType { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(20)]
        public string TelePhoneNum { get; set; }

        /// <summary>
        /// 短信标题，验证码
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string MsgTitle { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string MsgContent { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public System.DateTime OutTime { get; set; }

    }
}
