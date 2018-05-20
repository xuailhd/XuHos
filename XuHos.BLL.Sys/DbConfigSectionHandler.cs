using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Cache;
using XuHos.Common.Config;
using XuHos.Common;
namespace XuHos.BLL.Sys
{ 
    public class DbConfigSectionHandler : IConfigSectionHandler
    {
        static DAL.EF.Base.Repository<Entity.SysConfig> db = new DAL.EF.Base.Repository<Entity.SysConfig>();



        public TConfigSection GetSection<TConfigSection>(string PropNameSurfix)
                where TConfigSection : XuHos.Common.Config.IConfigSection
        {
            return GetConfigByDb<TConfigSection>(PropNameSurfix);
        }

        public bool SetSection<TConfigSection>(TConfigSection config, string Surfix)
            where TConfigSection : XuHos.Common.Config.IConfigSection
        {
            return SetConfigToDb<TConfigSection>(config, Surfix);
        }

        /// <summary>
        /// 从数据库获取某个配置项
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <typeparam name="TResult">配置项类型</typeparam>
        /// <returns></returns>
        static TConfigSection GetConfigByDb<TConfigSection>(string PropNameSurfix)
            where TConfigSection : XuHos.Common.Config.IConfigSection
        {
            Type type = typeof(TConfigSection);
            var config = Activator.CreateInstance(type);
            var configTypeName = type.Name;
            var props = type.GetProperties();

            //循环所有属性
            foreach (var p in props)
            {
                var value = GetSingleSectionValue(configTypeName, p.Name + PropNameSurfix);
                if (p.CanWrite)
                {
                    //一次读取数据库获取
                    try
                    {
                        p.SetValue(config, value, null);
                    }
                    catch(Exception ex) {
                        LogHelper.WriteDebug(ex.Message);
                    };

                }
            }

            return (TConfigSection)config;
        }

        static bool SetConfigToDb<TConfigSection>(TConfigSection config, string Surfix)
        {
            Type type = typeof(TConfigSection);
            var configTypeName = type.Name;
            var props = type.GetProperties();
            //循环所有属性
            foreach (var p in props)
            {
                //一次读取数据库获取
                db.Update(new Entity.SysConfig()
                {
                    ConfigKey = getConfigKey(configTypeName, p.Name) + Surfix,
                    ConfigValue = p.GetValue(config).ToString()
                });
            }
            return true;
        }

        static string GetSingleSectionValue(string ConfigTypeName, string PropName)
        {
            
            var result = db.Single(getConfigKey(ConfigTypeName, PropName));
            LogHelper.WriteDebug(getConfigKey(ConfigTypeName, PropName));
            if (result != null)
            {
                return result.ConfigValue;
            }
            else
            {
                return "";
            }

        }

        static string getConfigKey(string ConfigTypeName, string PropName)
        {
            return string.Format("{0}.{1}", ConfigTypeName, PropName);
        }
    }
}
