using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Integration.WechatApp
{ 
    public static class Configuration
    {
        public static string WechatAppID { get; set; }

        public static string WechatAppSec { get; set; }
        public static void RegisterConfig(
            XuHos.Common.Config.Sections.IM config)
        {
            if (config != null)
            {
                WechatAppID = config.WechatAppID;
                WechatAppSec = config.WechatAppSec;
            }

        }
    }
}
