using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.UI.WinSvr.JobService.Enum
{

    /// <summary>
    /// 服务启动类型
    /// </summary>
    public enum ServiceStartType
    {
        Boot,
        System,
        Auto,
        Manual,
        Disabled
    }
}
