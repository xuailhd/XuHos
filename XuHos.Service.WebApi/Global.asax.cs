using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using XuHos.Extensions;
using XuHos.Common.Cache;

namespace XuHos.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            #region api路由配置
            //web api帮助文档
            //AreaRegistration.RegisterAllAreas();
            //WebApi配置
            GlobalConfiguration.Configure(WebApiConfig.Register);
            #endregion

            XuHos.Service.Infrastructure.BundleConfig.RegisterApplicationConfig();

            //监听刷新系统配置消息（分布式环境下不用重启则可能够系统配置）
            XuHos.Common.Cache.Manager.Instance.Subscribe<object>("Sys/Config/Refresh", (a) =>
            {
                XuHos.Service.Infrastructure.BundleConfig.RegisterApplicationConfig();

            });
            
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {


            //指定HTTP 请求内容的类型 (云通信回调时需要)
            Request.Headers.Add("Content-Type", "application/json; charset=utf-8");

            #region  Cors跨域设置
            //Cors跨域设置(这些配制放在HttpMethod=="OPTIONS"里面就调用出错，放出来就没事，不知原因)
            var response = HttpContext.Current.Response;

            response.AddHeader("Access-Control-Allow-Origin", "*"); //正式环境注意改成具体网站，*代表允许所有网站
            response.AddHeader("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type,X-Requested-With,x-apptoken,x-noncestr,x-usertoken,x-sign,x-userid,userid,apptoken,noncestr,usertoken,sign");//Content-Type
            response.AddHeader("Access-Control-Max-Age", "36000");//设置跨域缓存，减少浏览器OPTIONS访问次数

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                response.End();
            }

            #endregion

            

        }

    }
}