using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.Infrastructure.Filters
{
    /// <summary>
    /// 忽略医生认证特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreCheckDoctorStateAttribute : Attribute
    {

    }
}
