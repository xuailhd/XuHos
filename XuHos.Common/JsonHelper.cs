using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System.IO;


namespace XuHos.Common
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {

        #region Object转json
        /// <summary>
        /// 将对象转为json格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isDateFomat"></param>
        /// <param name="dateFomat"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return ToJson(obj, false, string.Empty);
        }

        /// <summary>
        /// 将对象转为json格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isDateFomat"></param>
        /// <param name="dateFomat"></param>
        /// <returns></returns>
        public static string ToJson(object obj, bool isDateFomat, string dateFomat)
        {
            if (obj == null) return string.Empty;
            IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter()
            {
                DateTimeFormat = (isDateFomat && !string.IsNullOrEmpty(dateFomat)) ? dateFomat : "yyyy-MM-dd HH:mm:ss"
            };
            JsonConverter[] converter = new JsonConverter[] { dateTimeConverter };
            return JsonConvert.SerializeObject(obj, converter);
        }
        #endregion

        #region json转T对象
        public static T FromJson<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion
    }

    //public class TimestampConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType == typeof(DateTime);
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        writer.WriteValue(ConvertDateTimeString((DateTime)value));
    //    }

    //    public static DateTime ConvertIntDateTime(int aSeconds)
    //    {
    //        return new DateTime(1970, 1, 1).AddSeconds(aSeconds);
    //    }

    //    public static string ConvertDateTimeString(DateTime aDT)
    //    {

    //        return (string)(aDT - new DateTime(1970, 1, 1)).TotalSeconds;
    //    }
    //}

}

