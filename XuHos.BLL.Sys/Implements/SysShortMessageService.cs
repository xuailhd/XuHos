using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.Common;
using XuHos.DAL.EF;
using XuHos.Entity;
using XuHos.Common.Cache.Keys;
using XuHos.Extensions;
using XuHos.Common.Cache;
using XuHos.Common.Enum;
using XuHos.DTO.Common;
using XuHos.EventBus.Events;
using XuHos.Integration.ShortMessage;
using System.Web;

namespace XuHos.BLL.Sys.Implements
{
    public class SysShortMessageService
    {
        /// <summary>
        /// 获取所有模板
        /// </summary>
        /// <returns></returns>
        List<SysShortMessageTemplate> GetAllTemplates()
        {
            StringCacheKey shortMsgTemplatesKey = new StringCacheKey(StringCacheKeyType.Sys_ShortMsgTemplates);
            var shortMsgTemplates = shortMsgTemplatesKey.FromCache<List<SysShortMessageTemplate>>();

            if(shortMsgTemplates == null)
            {
                using (DBEntities db = new DBEntities())
                {
                    shortMsgTemplates = db.SysShortMessageTemplates.ToList();
                    if (shortMsgTemplates == null)
                    {
                        shortMsgTemplates = new List<SysShortMessageTemplate>();
                    }

                    shortMsgTemplates.ToCache(shortMsgTemplatesKey);
                }
            }
            return shortMsgTemplates;
        }

        /// <summary>
        /// 获取短信默默，默认取网络医院。
        /// </summary>
        /// <param name="MsgType"></param>
        /// <param name="OrgID"></param>
        /// <returns></returns>
        public SysShortMessageTemplate GetTemplate(string templateID)
        {
            var shortMsgTemplates = GetAllTemplates();
            var template = shortMsgTemplates.FindAll(a => a.TemplateID == templateID).FirstOrDefault();
            return template;
        }

        /// <summary>
        /// 获取发送频率消息
        
        /// 日期：2017年8月29日
        /// </summary>
        /// <param name="rates"></param>
        /// <returns></returns>
        public string GetSendRateMessage(Dictionary<string, int> rates)
        {
            if (rates["1M"] > 1)
            {
                return "验证码发送失败，已超过1分钟内发送1次的限制";
            }

            if (rates["1H"] > 3)
            {
                return "验证码发送失败，已超过1小时发送3次的限制";
            }


            if (rates["24H"] > 10)
            {
                return "验证码发送失败，已超过一天内发送10次的限制";
            }

            return "";

        }

        /// <summary>
        /// 检查发送频率
        
        /// 日期：2017年8月29日
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public Dictionary<string, int> CheckSendRate(string Mobile,string tempateID)
        {
            var result = new Dictionary<string, int>();
            var smsDuplicateCacheKey = new EntityCacheKey<int?>(StringCacheKeyType.SMS_Duplicate, $"{Mobile}:{tempateID}");
            var now = DateTime.Now;
            foreach (var point in new Dictionary<string, int>
            {
                { "1M", 1 },
                { "1H", 60 },
                { "24H", 1440 }
            })
            {
                var key = $"{smsDuplicateCacheKey.KeyName}:{point.Key}";
                var list = XuHos.Common.Cache.Manager.Instance.StringGet<List<DateTime>>(key) ?? new List<DateTime>();
                var delCount = list.RemoveAll(x => x.AddMinutes(point.Value) <= now);
                if (list.Count == 0)
                {
                    Manager.Instance.RemoveCache(key);
                }
                else if (delCount > 0)
                {
                    Manager.Instance.StringSet(key, list);
                    Manager.Instance.ExpireEntryAt(key, TimeSpan.FromMinutes(point.Value));
                }
                result.Add(point.Key, list.Count);
            }
            return result;
        }

        /// <summary>
        /// 更新发送频率
        
        /// 日期：2017年8月29日
        /// </summary>
        /// <param name="Mobile"></param>
        public void UpdateSendRate(string Mobile, Dictionary<string, int> rates)
        {
            var smsDuplicateCacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<int?>(XuHos.Common.Cache.Keys.StringCacheKeyType.SMS_Duplicate, Mobile);
            var now = DateTime.Now;

            foreach (var point in new Dictionary<string, int>
            {
                { "1M", 1 },
                { "30M", 30 },
                { "24H", 1440 }
            })
            {
                var key = $"{smsDuplicateCacheKey.KeyName}:{point.Key}";
                var list = XuHos.Common.Cache.Manager.Instance.StringGet<List<DateTime>>(key) ?? new List<DateTime>();
                list.RemoveAll(x => x.AddMinutes(point.Value) <= now);
                list.Add(now);
                Manager.Instance.StringSet(key, list);
                //if (!rates.ContainsKey(point.Key))
                //{
                XuHos.Common.Cache.Manager.Instance.ExpireEntryAt(key, TimeSpan.FromMinutes(point.Value));
                //}
            }
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="MsgType"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public bool CheckVerifyCode(string Mobile, string MsgType, string Title)
        {
            var cacheKey = new StringCacheKey(StringCacheKeyType.SYS_SMSVerifyCode, $"{Mobile}/{MsgType}/{Title}");
            UserShortMessageLog model = cacheKey.FromCache<UserShortMessageLog>();
            return model != null;
        }

        /// <summary>
        /// 根据手机号查询最后一条短信验证码信息
        /// </summary>
        /// <param name="telePhone"></param>
        public UserShortMessageLog GetLastSMSLog(string telePhone, int type)
        {
            UserShortMessageLog model = null;
            try
            {
                var db = new DBEntities();
                var msgList = (from p in db.UserShortMessageLogs.Where(m => m.IsDeleted == false
                               && m.TelePhoneNum == telePhone && m.MsgLogType == type).
                               OrderByDescending(m => m.CreateTime)
                               select p).ToList<UserShortMessageLog>();
                if (msgList != null && msgList.Count > 0)
                {
                    model = msgList[0];
                }
            }
            catch (Exception ex)
            {
                model = null;
                throw ex;
            }
            return model;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public ApiResult SendMsg(RequestSendSMSDTO evt)
        {
            string reason = "发送失败";
            try
            {
                using (var db = new DBEntities())
                {
                    UserShortMessageLog model = new UserShortMessageLog()
                    {
                        ShortMessageLogID = Guid.NewGuid().ToString("N"),
                        MsgLogType = evt.MsgType,
                        UserID = evt.UserID ?? "",
                        TelePhoneNum = evt.Mobile,
                        MsgTitle = evt.Title ?? "",
                        MsgContent = evt.Content ?? "",
                        OutTime = evt.OutTime.HasValue ? evt.OutTime.Value : DateTime.MaxValue,
                        IsDeleted = true //代表未生效
                    };

                    #region  校验：短信发送频率控制 (本系统缓存控制)
                    var smsCheckResult = CheckSendRate(evt.Mobile,evt.TemplateID);
                    var Reason = GetSendRateMessage(smsCheckResult);
                    if (!string.IsNullOrEmpty(Reason))
                    {
                        model.MsgContent = Reason;
                        db.UserShortMessageLogs.Add(model);
                        db.SaveChanges();
                        return EnumApiStatus.BizSMSOverclock.ToApiResultForApiStatus("您获取验证码太过频繁，请稍后再试");
                    }
                    #endregion

                    db.UserShortMessageLogs.Add(model);
                    var i = db.SaveChanges();

                    if (i > 0)
                    {
                        var flag = SMSHelper.SendSMS(evt.Mobile, evt.TemplateID, BuildSMSParaStr(evt.MsgParms, evt.SMSVender), evt.SMSVender, out reason);
                        if (!flag)
                        {
                            if (!string.IsNullOrEmpty(reason))
                            {
                                model.MsgContent = reason;
                                db.SaveChanges();
                                //return new ApiResult() {  Status = EnumApiStatus.BizSMSOverclock, Msg = "该手机号短信发送频率太高，请稍候重试"};
                            }
                            //接口返回失败的话 暂时都认为是超频。前端便于做是否重试控制
                            return new ApiResult() { Status = EnumApiStatus.BizSMSOverclock, Msg = "您获取验证码太过频繁，请稍后再试" };
                        }
                        else
                        {
                            //生效
                            model.IsDeleted = false;
                            db.SaveChanges();

                            UpdateSendRate(evt.Mobile, smsCheckResult);

                            if (evt.OutTime.HasValue)
                            {
                                StringCacheKey cacheKey = new StringCacheKey(StringCacheKeyType.SYS_SMSVerifyCode, $"{evt.Mobile}/{evt.MsgType}/{evt.Title}");
                                model.ToCache(cacheKey, evt.OutTime.Value - DateTime.Now);
                            }
                            return true.ToApiResultForBoolean();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
            }

            return reason.ToApiResultForObject(EnumApiStatus.BizError, reason);
        }

        public string BuildSMSParaStr(List<string> paras,int SMSVender)
        {
            if(paras == null && paras.Count < 1)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            switch (SMSVender)
            {
                case 1:
                    for (int i = 0; i < paras.Count; i++)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append("&");
                        }
                        sb.Append(HttpUtility.UrlEncode($"#para{i.ToString()}#", Encoding.UTF8) 
                            + "=" + HttpUtility.UrlEncode($"{paras[i]}", Encoding.UTF8));
                    }
                    return HttpUtility.UrlEncode(sb.ToString());
                default:
                    for(int i = 0; i < paras.Count; i++)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(paras[i]);
                    }
                    return sb.ToString();
            }
        }
    }
}
