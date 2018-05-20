using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.DTO;
using System.ComponentModel.DataAnnotations;

namespace XuHos.BLL.Common.DTOs.Response.Platform
{
    public class ResponseBannerDTO : DTO.Common.ImageBaseDTO
    {

        /// <summary>
        /// Banner名称
        /// </summary>
        public string BannerName { get; set; }

        /// <summary>
        /// Banner图地址
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// Banner链接跳转地址
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 图片轮播时间(毫秒)
        /// </summary>
        public int SliderTime { get; set; }


    }
}
