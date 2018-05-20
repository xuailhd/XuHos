

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;


namespace XuHos.Extensions
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class Extensions_Enum
    {
        #region public method

        /// <summary>
        /// 获取枚举类型描述
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string GetEnumDescript(this Type enumType)
        {
            DescriptionAttribute attr = null;
            attr = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute));
            if (attr != null && !string.IsNullOrEmpty(attr.Description))
            {
                return attr.Description;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据枚举项的值获取枚举项的描述信息。
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static string GetEnumDescript(this object enumValue)
        {
            Type enumType = enumValue.GetType();
            DescriptionAttribute attr = null;

            // 获取枚举常数名称。
            string name =System.Enum.GetName(enumType, enumValue);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                }
            }
            if (attr != null && !string.IsNullOrEmpty(attr.Description))
            {
                return attr.Description;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetEnumDescript(string enumValue, Type enumType)
        {
            int value = 0;
            try
            {
                value = Convert.ToInt32(enumValue);
            }
            catch(Exception ex)
            {
                return enumValue;
            }

            DescriptionAttribute attr = null;

            // 获取枚举常数名称。
            string name = System.Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                }
            }
            if (attr != null && !string.IsNullOrEmpty(attr.Description))
            {
                return attr.Description;
            }
            else
            {
                return string.Empty;
            }
        }

        

        /// <summary>
        /// 根据枚举类型和项的值获取枚举项的名称。
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static string GetEnumFieldName(this Type enumType, object enumValue)
        {
            string name = System.Enum.GetName(enumType, enumValue);
            if (name != null)
            {
                return name;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

    }

    public static class EnumHelper
    {
        private static ConcurrentDictionary<Type, CodeDictionary<string>> cache = new ConcurrentDictionary<Type, CodeDictionary<string>>();

        public static CodeDictionary<string> ToDictionary<TEnum>()
        {
            CodeDictionary<string> dic;
            if (cache.TryGetValue(typeof(TEnum), out dic))
            {
                return dic;
            }
            dic = new CodeDictionary<string>();
            foreach (TEnum type in Enum.GetValues(typeof(TEnum)))
            {
                dic.Add((Convert.ToInt16(type)).ToString(), type.GetEnumDescript());
            }
            cache[typeof(TEnum)] = dic;
            return dic;
        }
    }


    public class CodeDictionary<TKey> : Dictionary<TKey, string>
    {
        public new string this[TKey key]
        {
            get
            {
                string value = "";
                if (!this.TryGetValue(key, out value))
                {
                    value = "";
                }
                return value;
            }
            set
            {
                this[key] = value;
            }
        }
    }
}
