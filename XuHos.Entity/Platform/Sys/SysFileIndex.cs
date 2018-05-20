using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.Entity
{
    public class SysFileIndex:AuditableEntity
    {
        /// <summary>
        /// 银行ID
        /// </summary>
        [Key,Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string MD5 { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(500)]
        public string FileUrl { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string FileType { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Remark { get; set; }

        [Required]
        public long ReadCount { get; set; }

        [Required]
        public long FileSize { get; set; }

        /// <summary>
        /// 签名Key，通过此Key可获取访问文件权限
        /// </summary>
        [Required]
        public string AccessKey { get; set; }
    }
}
