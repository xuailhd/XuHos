using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{
    public  class IMGStore:IConfigSection
    {
        

        public IMGStore()
        {
        }


        public string UrlPrefix
        { get; set; }

        /// <summary>
        /// 目录（E://）
        /// </summary>
        public string DirectorRootPath
        { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// CDN前缀
        /// </summary>
        public string CDNUrlPrefix{ get; set; }

    }
}
