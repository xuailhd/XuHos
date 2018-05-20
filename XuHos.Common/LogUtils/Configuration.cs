using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Log
{
    public static class Configuration
    {
        internal static ApiTrack.ITrackLogLogAppender LogAppender
        { get; set; }


        /// <summary>
        /// 注册日志写入实例
        /// </summary>
        /// <param name="_LogAppender"></param>
        public static void Register(ApiTrack.ITrackLogLogAppender _LogAppender)
        {
            if (_LogAppender != null)
                LogAppender = _LogAppender;
        }
    }
}
