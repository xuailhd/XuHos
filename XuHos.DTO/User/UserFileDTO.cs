using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;
using XuHos.DTO.Common;

namespace XuHos.DTO
{
    public class UserFileDTO
    {
        //兼容旧版APP
        public string UrlPrefix
        {
            get
            {
                return ImageBaseDTO.UrlPrefix;
            }
        }
        public string FullFileUrl
        {
            get
            {
                return ImageBaseDTO.PaddingUrlPrefix(FileUrl);
            }
        }
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
        public string FileName { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }


        /// <summary>
        /// 文件类型(0-图片、1=语音、3=视频、4=病历、5=处方、6=会诊报告)
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
    }



}
