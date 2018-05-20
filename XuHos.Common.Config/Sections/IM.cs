using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{
    /// <summary>
    /// 短信配置
    /// </summary>
    public class IM: IConfigSection
    {
        public string AccountType
        { get; set; }

        public string SDKAppID
        { get; set; }

        /// <summary>
        /// 管理员账号（服务端集成消息是需要）
        /// </summary>
        public string AdminAccount { get; set; }

        public string WechatAppID { get; set; }

        public string WechatAppSec { get; set; }
    }
}
