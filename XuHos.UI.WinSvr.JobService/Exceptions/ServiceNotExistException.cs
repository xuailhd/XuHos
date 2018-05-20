using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.UI.WinSvr.JobService.Exceptions
{
    /// <summary>
    /// 服务不存在异常
    /// </summary>
    public class ServiceNotExistException : ApplicationException
    {
        public ServiceNotExistException() : base("服务不存在！") { }

        public ServiceNotExistException(string message) : base(message) { }

    }
}
