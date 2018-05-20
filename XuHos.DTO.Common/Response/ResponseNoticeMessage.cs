using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common.Response
{

    /// <summary>
    /// 消息
    /// </summary>
    public class ResponseNoticeMessage
    {
        public string MessageID { get; set; }

     
        public string Title { get; set; }

        public string Summary { get; set; }

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
        /// 发送目标(所有用户，患者，医生，指定用户)
        /// </summary>
        public EnumNoticeTarget Target { get; set; }

        /// <summary>
        /// 点击消息进入业务页面的参数
        /// </summary>
        public string PageArgs { get; set; }

    }
}
