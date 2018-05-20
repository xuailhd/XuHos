using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception GetDetailException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetDetailException();
        }
    }
}
