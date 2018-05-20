using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Integration.QQCloudy
{
    public static class Configuration
    {

        public static XuHos.Common.Config.Sections.IM IMConfig { get; private set; }
        public static string adminAccount { get; private set; }
        public static void RegisterConfig(
            XuHos.Common.Config.Sections.IM config)
        {
            IMConfig = config;

            if (IMConfig != null)
            {
                adminAccount = IMConfig.AdminAccount;
            }

        }
    }
}
