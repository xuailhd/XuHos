using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;

namespace XuHos.Common.Log.ApiTrack
{

    public class WebApiTrackLogStatistics
    {
        /// <summary>
        /// 提交开始执行接口的时间
        /// </summary>
        public DateTime enterTime { get; set; }


        /// <summary>
        /// 接口执行消耗毫秒的时间
        /// </summary>
        public double costTime { get; set; }
    }


    public class WebApiTrackLogGeneral
    {
        
        public int statusCode { get; set; }

        /// <summary>
        /// 请求的地址
        /// </summary>
        public string requestUri { get; set; }

        public string requestType { get; set; }

        /// <summary>
        /// controller名字
        /// </summary>
        public string controllerName { get; set; }
        /// <summary>
        /// 操作的action
        /// </summary>
        public string actionName { get; set; }

        /// <summary>
        /// 客户端hostname
        /// </summary>
        public string remoteHostName { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string remoteBrowser { get; set; }

        public string urlReferrer { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 浏览器标示
        /// </summary>
        public string navigator { get; set; }

        /// <summary>
        ///操作人的id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comments { get; set; }

        public string appId
        { get; set; }

    }

    public class ApiTrackLog
    {

        public System.Collections.Generic.Dictionary<string, string> RequestHeaders { get; set; }

        /// <summary>
        /// 请求携带的参数
        /// </summary>
        public string RequestParamters { get; set; }



        /// <summary>
        /// logId
        /// </summary> 
        public Guid Id { get; set; }



        public WebApiTrackLogGeneral General { get; set; }

        public WebApiTrackLogStatistics Statistics { get; set; }


        /// <summary>
        /// 执行结果
        /// </summary>
        public string Response { get; set; }


    }
}
