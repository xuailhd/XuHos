using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ResponseUserFileDTO : ImageBaseDTO
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public string FileID { get; set; }

        /// <summary>
        /// 外部关联ID
        /// </summary>
        public string OutID { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        public string FileName { get; set; }

        string _FileUrl;
        /// <summary>
        /// 文件地址
        /// </summary>
        [Required]
        public string FileUrl
        {
            set
            {
                _FileUrl = value;
            }
            get
            {
                return PaddingUrlPrefix(_FileUrl);
            }
        }

        /// <summary>
        /// 文件类型(0-图片,1-文件，2-语音,3-视频)
        /// </summary>
        [Required]
        public int FileType { get; set; }

        public long FileSize { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        public string Remark { get; set; }
    }
}
