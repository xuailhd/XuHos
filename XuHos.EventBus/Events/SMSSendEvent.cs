using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 短信发送
    /// </summary>
    public class SMSSendEvent : BaseEvent, IEvent
    {
        public string Mobile { get; set; }

        public string TemplateID { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 短信验证码类型(1:用户注册,2:找回密码,3设置支付密码,4绑定手机号,5登陆账号开通密码通知,6会诊单创建通知，7会诊单付款通知，8会诊单支付成功通知)
        /// </summary>
        public int MsgType { get; set; }

        /// <summary>
        /// 标题/验证码，空选项
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 用户Id，可选项
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 参数列表， 按模板参数顺序有序排列
        /// </summary>
        public List<string> MsgParms { get; set; }

        public DateTime? OutTime { get; set; }
    }
}