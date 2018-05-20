using System;
using System.Collections.Generic;
using System.Linq;

namespace XuHos.Common.Config.Sections
{
    public class FamilyWechatConfig : IWechatConfig, IConfigSection
    {
        public string AppID { get; set; }

        public string AppSecret { get; set; }

        public string WebAuthCallbackUrl { get; set; }

        public string OrgID { get; set; }

        public string HealthReport { get; set; }

        public string TCMReport { get; set; }

        public string HealthReportKey { get; set; }

        public string HealthReportSecret { get; set; }
    }
}