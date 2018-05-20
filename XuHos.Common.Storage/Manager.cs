using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Storage
{
    /// <summary>
    /// 存储服务
    
    /// 日期：2016年7月29日
    /// </summary>
    public static class Manager
    {
        static Lazy<IFileStorage> _instance = null;

        public static void Register(Func<IFileStorage> getInstanceHandler)
        {
            _instance = new Lazy<IFileStorage>(getInstanceHandler);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        public static IFileStorage Instance
        {
            get
            {
                return _instance.Value;
            }
        }

    }
}
