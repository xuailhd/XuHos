using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Config;
using XuHos.Common.Config.Sections;
namespace XuHos.Common.Pay
{
    public static class Configuration
    {
        static Configuration()
        {
            Config = new Common.Config.Sections.Pay();
            AppConfigs = new Dictionary<string, Common.Config.Sections.Pay.IAppPayConfig>();

        }

        internal static Config.Sections.Pay Config
        {
            get;
            set;
        }


        static System.Collections.Generic.Dictionary<string,Config.Sections.Pay.IAppPayConfig> AppConfigs
        {
            get;
            set;
        }



        public static void Register(Config.Sections.Pay _Config)
        {
            Config = _Config;
        }

        public static void Register(Config.Sections.Pay.IAppPayConfig _AppPayConfig,string AppID)
        {
            if (AppConfigs.ContainsKey(AppID))
            {
                AppConfigs.Remove(AppID);
            }

            AppConfigs.Add(AppID, _AppPayConfig);
        }


        public static T GetAppPayConfig<T>(string AppId)
            where T: class,Config.Sections.Pay.IAppPayConfig,new()
        {
            if (AppConfigs.ContainsKey(AppId))
                return AppConfigs[AppId] as T;
            else
                return default(T);

        }

    }
}
