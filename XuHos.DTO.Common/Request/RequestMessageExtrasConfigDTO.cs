using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;

namespace XuHos.DTO.Common
{
    public class RequestMessageExtrasConfigDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ExtrasConfigID { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public EnumTerminalType TerminalType { get; set; }

        /// <summary>
        /// 二级消息分类
        /// </summary>
        public EnumNoticeSecondType MsgType { get; set; }

        /// <summary>
        /// 通知模板
        /// </summary>
        public string MsgTitle { get; set; }

        /// <summary>
        /// 跳转页面
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// //页面类型（HTML/Native）
        /// </summary>
        public string PageType { get; set; }

        /// <summary>
        /// 打开目标(_Blank/_Parent/_Self/_Top)
        /// </summary>
        public string PageTarget { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object PageArgs { get; set; }

    }


    public class RequestMessageExtrasDTO
    {
        public string MsgID { get; set; }

        public List<RequestMessageExtrasConfigDTO> MsgBody { get; set; }
    }

}
