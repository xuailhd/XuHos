using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using XuHos.Service.Infrastructure.Filters;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using XuHos.Service.Infrastructure.Handlers;

namespace XuHos.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            #region 配置路由
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }
            );

            #endregion

            config.MessageHandlers.Add(new ApiCancelledTaskBugWorkaroundMessageHandler());
            
            #region 设置过滤器
            //记录操作（写入mongodb）
            config.Filters.Add(new ApiOperateTrackAttribute());
            //模型验证
            config.Filters.Add(new ApiModelValidateFilterAttribute());
            //统一权限验证
            config.Filters.Add(new ApiAuthorizationFilterAttribute());  
            //异常处理
            config.Filters.Add(new ApiExceptionFilterAttribute());
            #endregion

            #region 设置api的返回结果类型

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //默认返回 json  
            config.Formatters
                .JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "json", "application/json"));


            //json 序列化设置  
            config.Formatters
                .JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, //设置忽略值为 null 的属性  
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,  //DateTime默认为本地时区(中国时区，东8区)
                                                                      //DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffzzz"

                 };
            #endregion

        }
    }
}

