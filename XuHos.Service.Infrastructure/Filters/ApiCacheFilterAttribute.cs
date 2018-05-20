using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace XuHos.Service.Infrastructure.Filters
{
    /// <summary>
    /// 接口缓存
    
    /// 日期：2017年1月19日
    /// </summary>
    public class ApiOutputCacheFilterAttribute : ActionFilterAttribute
    {
        // 缓存时间 /秒
        private int _timespan;
        // 客户端缓存时间 /秒
        private int _clientTimeSpan;
        // 是否为匿名用户缓存keys
        private bool _anonymousOnly;
        // 缓存索引键
        private string _cachekey;
        // 缓存仓库
        private XuHos.Common.Cache.ICacheManager WebApiCache
        {
            get
            {
                return XuHos.Common.Cache.Manager.UseDb(3);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timespan">缓存时间 /秒</param>
        /// <param name="clientTimeSpan">客户端缓存时间 /秒</param>
        /// <param name="anonymousOnly">是否为匿名用户缓存</param>
        public ApiOutputCacheFilterAttribute(int timespan, int clientTimeSpan, bool anonymousOnly)
        {
            _timespan = timespan;
            _clientTimeSpan = clientTimeSpan;
            _anonymousOnly = anonymousOnly;
        }

        //是否缓存
        private bool _isCacheable(HttpActionContext ac)
        {
            if (_timespan > 0 && _clientTimeSpan > 0)
            {
                if (_anonymousOnly)
                    if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        return false;
                if (ac.Request.Method == HttpMethod.Get) return true;
            }
            else
            {
                throw new InvalidOperationException("Wrong Arguments");
            }
            return false;
        }

        private CacheControlHeaderValue setClientCache()
        {
            var cachecontrol = new CacheControlHeaderValue();
            cachecontrol.MaxAge = TimeSpan.FromSeconds(_clientTimeSpan);
            cachecontrol.MustRevalidate = true;
            return cachecontrol;
        }


        public override void OnActionExecuting(HttpActionContext ac)
        {
            if (ac != null)
            {
                try
                {
                    if (_isCacheable(ac))
                    {
                        if (ac.Request.Headers.Accept.FirstOrDefault() != null)
                        {
                            _cachekey = string.Join(":", new string[] { ac.Request.RequestUri.PathAndQuery, ac.Request.Headers.Accept.FirstOrDefault().ToString() });
                        }
                        else
                        {
                            _cachekey = string.Join(":", new string[] { ac.Request.RequestUri.PathAndQuery });
                        }


                        var val = WebApiCache.StringGet<string>(_cachekey);

                        if (val != null)
                        {
                            ac.Response = ac.Request.CreateResponse();
                            ac.Response.Content = new StringContent(val);
                            var contenttype = WebApiCache.StringGet<MediaTypeHeaderValue>(_cachekey + ":response-ct");

                            if (contenttype == null)
                            {
                                contenttype = new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                            }
                            ac.Response.Content.Headers.ContentType = contenttype;
                            ac.Response.Headers.CacheControl = setClientCache();
                            return;
                        }
                    }
                }
                catch (Exception E)
                {
                    XuHos.Common.LogHelper.WriteError(E);
                }
            }
            else
            {
                throw new ArgumentNullException("actionContext");
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var val = WebApiCache.StringGet<string>(_cachekey);

                //缓存不存在则添加
                if (val == null)
                {
                    var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                    WebApiCache.StringSet(_cachekey, body,TimeSpan.FromSeconds(_timespan));
                    WebApiCache.StringSet(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, TimeSpan.FromSeconds(_timespan));
                }

                if (_isCacheable(actionExecutedContext.ActionContext))
                {
                    actionExecutedContext.ActionContext.Response.Headers.CacheControl = setClientCache();
                }
            }
            catch(Exception E)
            {
                XuHos.Common.LogHelper.WriteError(E);
            }
        }

    }
}
