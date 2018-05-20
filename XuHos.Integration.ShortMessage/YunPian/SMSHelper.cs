using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace XuHos.Integration.ShortMessage.YunPian
{
    public static class SMSHelper 
    {
        //private static Common.Config.Sections.SMSYunPian config;
        //public static void Register(Common.Config.Sections.SMSYunPian _config)
        //{
        //    if(_config != null && !string.IsNullOrEmpty( _config.Sms_host))
        //    {
        //        config = _config;
        //    }
        //    else
        //    {
        //        config = new Common.Config.Sections.SMSYunPian()
        //        {
        //            Apikey = "00ca2cb0d09d098c9a4679a3f8a088a7",
        //            Sms_ApiVersion = "/v2",
        //            Sms_host = "https://sms.yunpian.com"
        //        };
        //    }
        //}
        //public Result singleSend(Dictionary<string, string> parms)
        //{
        //    parms.Add("apikey", config.apikey);
        //    return HttpUtil.HttpPost(Config.url_send_single_sms, parms);
        //}
        //public Result batchSend(Dictionary<string, string> parms)
        //{
        //    parms.Add("apikey", config.apikey);
        //    return HttpUtil.HttpPost(Config.url_send_batch_sms, parms);
        //}
        //public Result multiSend(Dictionary<string, string> parms)
        //{
        //    parms.Add("apikey", config.apikey);
        //    string text = parms["text"];
        //    string[] textArr = text.Split(',');
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var s in textArr)
        //    {
        //        if (sb.Length != 0)
        //            sb.Append(",");
        //        sb.Append(HttpUtility.UrlEncode(s));
        //    }
        //    parms["text"] = sb.ToString();
        //    return HttpUtil.HttpPost(Config.url_send_multi_sms, parms);
        //}
        public static bool SendSMS(string toPhone, string templatedId, string param, out string reason)
        {
            reason = "";
            return true;
            //StringBuilder sb = new StringBuilder();
            //sb.Append($"mobile={toPhone}");
            //sb.Append($"&tpl_id={templatedId}");
            //sb.Append($"&tpl_value={param}");
            //sb.Append($"&apikey={config.Apikey}");
            //var ret = HttpUtil.HttpPost(config.Sms_host + config.Sms_ApiVersion + "/sms/tpl_single_send.json", sb.ToString());
            //if (ret.success)
            //{
            //    reason = "";
            //    return true;
            //}
            //else if(ret.data!=null)
            //{
            //    reason = ret.data.msg;
            //    return false;
            //}
            //else
            //{
            //    reason = ret.ToString();
            //    return false;
            //}
        }


        //public static Result TplSingleSend(Dictionary<string, string> parms)
        //{
        //    parms.Add("apikey", config.Apikey);
        //    return HttpUtil.HttpPost(config.Sms_host + config.Sms_ApiVersion + "/sms/tpl_single_send.json", parms);
        //}
        //public static Result TplBatchSend(Dictionary<string, string> parms)
        //{
        //    parms.Add("apikey", config.Apikey);
        //    return HttpUtil.HttpPost(config.Sms_host + config.Sms_ApiVersion + "/sms/tpl_batch_send.json", parms);
        //}
    }
}
