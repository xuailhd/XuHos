using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{
    /// <summary>
    /// Redis服务配置
    /// </summary>
    public class Redis: IConfigSection
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否自动启动
        /// </summary>
        public string AutoStart
        { get; set; }

        /// <summary>
        /// 读线程池大小
        /// </summary>
        public string MaxReadPoolSize
        { get; set; }

        /// <summary>
        /// 写线程池大小
        /// </summary>
        public string MaxWritePoolSize
        { get; set; }

        /// <summary>
        /// 读服务器列表
        /// </summary>
        public string ReadServerList
        { get; set; }

        /// <summary>
        /// 写入服务器列表
        /// </summary>
        public string WriteServerList
        { get; set; }

        /// <summary>
        /// 哨兵列表
        /// </summary>
        public string SentineList { get; set; }

        public string KeyPrefix { get; set; }
        public string Ssl { get; set; }

        public string DBNum { get; set; }
    }
}
