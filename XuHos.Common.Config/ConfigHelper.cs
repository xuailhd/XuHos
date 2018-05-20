using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace XuHos.Common.Config
{
    /// <summary>
    /// 获取配置文件的工具类
    /// </summary>
    public class ConfigHelper
    {
        #region public method

       /// <summary>
       /// 得到AppSettings中的配置字符串信息
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            string keyValue = "";
            object objKey = ConfigurationManager.AppSettings[key];
            if (objKey != null)
            {
                keyValue = objKey.ToString();
            }
            return keyValue;
        }

        /// <summary>
        /// 得到ConnectionString中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            string keyValue = "";
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[key].ConnectionString))
            {
                keyValue = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            return keyValue;
        }

        #endregion



    }
}
