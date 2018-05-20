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
    public class RequestNoticeMessageDTO
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// 通知标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 用户应用端(医生端，患者端)
        /// </summary>
        public EnumUserNoticeType UserNoticeType { get; set; }

        /// <summary>
        /// 一级消息分类(医生端：系统公告、订单消息、业务消息，患者端：系统公告、服务消息)
        /// </summary>
        public EnumNoticeFirstType NoticeFirstType { get; set; }

        /// <summary>
        /// 二级消息分类
        /// </summary>
        public EnumNoticeSecondType NoticeSecondType { get; set; }

        /// <summary>
        /// 通知日期
        /// </summary>
        public DateTime NoticeDate { get; set; }

        /// <summary>
        /// 点击消息进入业务页面的参数
        /// </summary>
        public string PageArgs { get; set; }

        public string ClientName { get; set; }
    }



}
