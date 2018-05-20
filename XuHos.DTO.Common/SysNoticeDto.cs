using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO.Common
{
    /// <summary>
    /// 系统公告
    /// </summary>
    public class SysNoticeDto
    {
        [MaxLength(32)]
        public string SysNoticeID { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [MaxLength(512)]
        public string Summary { get; set; }

        /// <summary>
        /// 公告内容
        /// </summary>
        [Required]
        [MaxLength(4000)]
        public string Content { get; set; }

        /// <summary>
        /// 公告日期
        /// </summary>
        [Required]
        public DateTime NoticeDate { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [Required]
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Required]
        public bool IsShow { get; set; }

    }
}
