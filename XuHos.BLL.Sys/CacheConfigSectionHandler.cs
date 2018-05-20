using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Cache;
namespace XuHos.BLL.Sys
{
    /// <summary>
    /// 配置节点处理程序
    
    /// 日期：2016年8月1日
    /// </summary>
    public class CacheConfigSectionHandler:XuHos.Common.Config.IConfigSectionHandler
    {
        public TConfigSection GetSection<TConfigSection>(string PropNameSurfix="") where TConfigSection : XuHos.Common.Config.IConfigSection
        {
            if (Manager.Instance != null)
                return (TConfigSection)Manager.Instance.StringGet<TConfigSection>(typeof(TConfigSection).FullName+PropNameSurfix);
            else
                return default(TConfigSection);
        }

        public bool SetSection<TConfigSection>(TConfigSection config,string Surfix="") where TConfigSection : XuHos.Common.Config.IConfigSection
        {
            if (Manager.Instance != null)
            {
                Manager.Instance.StringSet<TConfigSection>(typeof(TConfigSection).FullName+Surfix, config);
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
