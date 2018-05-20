using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace XuHos.Entity
{
    public class UserFile: AuditableEntity,IUserBaseEntity
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string FileID { get; set; }

        /// <summary>
        /// 外部关联ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string OutID { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string FileName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(128)]
        public string FileUrl { get; set; }


        /// <summary>
        /// 文件类型(0-图片、1=语音、3=视频、4=病历、5=处方、6=会诊报告、7处方封面图、8家庭医生签约协议书)
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public int FileType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(512)]
        public string Remark { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        /// <summary>
        /// 访问秘钥
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string AccessKey { get; set; }

        /// <summary>
        /// 资源编号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string ResourceID { get; set; }

    }
}
