using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class UserNoticeSendEvent: BaseEvent, IEvent
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
        /// 发送者用户编号
        /// </summary>
        public string FromUserID { get; set; }

        public List<string> ToUserList { get; set; }

        public EnumTargetUserCodeType ToUserListType { get; set; }

        public string ClientName { get; set; }

        /// <summary>
        /// 二级消息分类
        /// </summary>
        public EnumNoticeSecondType NoticeSecondType { get; set; }

        /// <summary>
        /// 通知日期
        /// </summary>
        public DateTime NoticeDate { get; set; }

        public EnumNoticeTarget Target { get; set; }

        /// <summary>
        /// 点击消息进入业务页面的参数
        /// </summary>
        public string PageArgs { get; set; }

        public bool silence { get; set; }

        public Dictionary<string, object> extrasIOS { get; set;}

        public Dictionary<string, object> extrasAndroid { get; set; }

        public string serviceID { get; set; }

    }
}
