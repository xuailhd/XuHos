using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common
{
    //public class IOCContainer
    //{
    //    private static volatile IOCContainer instance;
    //    private static object syncRoot = new Object();
    //    private IUnityContainer container = null;
    //    private IOCContainer() {
    //        var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

    //        container = new UnityContainer().LoadConfiguration(section);
        
    //    }
    //    public static IOCContainer Instance
    //    {
    //        get
    //        {
    //            if (instance == null)
    //            {
    //                lock (syncRoot)
    //                {
    //                    if (instance == null)
    //                        instance = new IOCContainer();
    //                }
    //            }
    //            return instance;
    //        }
    //    }

    //    public T Resolve<T>()
    //    {
    //        return container.Resolve<T>();
    //    }
    //}
}
