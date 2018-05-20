using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{
    public class MQ : IConfigSection
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string HostName
        { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName
        { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password
        { get; set; }

        /// <summary>
        /// 端口 5672
        /// </summary>
        public string Port
        { get; set; }


        /// <summary>
        /// 虚拟主机（不同环境使用不同的 开发环境=/dev，测试环境/test 生产环境 /release）
        /// </summary>
        public string VirtualHost
        { get; set; }

    }
}
