using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public class User : AuditableEntity, IUserBaseEntity
    {
        public User()
        {
            //Score = 0;
            //Star = 1;
            //Comment = 0;
            //Good = 0;
            //Fans = 0;
            //Grade = 1;
            //Checked = 1;
            RegTime = DateTime.Now;
            CancelTime = DateTime.Now;
            UserState =  EnumUserState.Disabled;
            UserLevel =  0;
            OrgCode = "";
            UserAccount = "";
        }


        /// <summary>
        /// 用户ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID {
            get;
            set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserAccount {
            get;
            set; }

        /// <summary>
        /// 用户中文名
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(64)]
        public string UserCNName { get; set; }

        /// <summary>
        /// 用户英文名
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserENName { get; set; }

        /// <summary>
        /// 用户类型(0-管理员、1-患者、2-医生)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public Common.Enum.EnumUserType UserType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(16)]
        public string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string Password { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 注销时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CancelTime { get; set; }

        /// <summary>
        /// 用户状态(-1=未开户,0-正常、1-冻结、2-销户)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumUserState UserState { get; set; }

        /// <summary>
        /// 用户级别(0-普通用户、10 以上 Vip等级)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int UserLevel { get; set; }

        /// <summary>
        /// 机构编码 ,  只标识用户注册来源  不要和机构会员混淆，医生所属机构混淆
        /// </summary>
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string OrgCode { get; set; }

    }
}
