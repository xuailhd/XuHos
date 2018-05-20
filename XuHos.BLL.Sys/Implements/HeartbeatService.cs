using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Cache.Keys;

namespace XuHos.BLL.Sys.Implements
{
    public class HeartbeatService
    {
        private const int userOnLineStateDbNum = 1;
        private const int  userTypeStateDbNum = 2;

        int getDbNum(int dbNum)
        {
            return Manager.dbNum + dbNum;
        }

        /// <summary>
        /// 检查某个用户是否在诊室
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public bool HasChannelHeartBeat(string UserID, int ChannelID)
        {
            var roomService = new ConversationRoomService();
          
            var cacheKey_HeartbeatApp = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_App, $"{UserID}");
            
            if (Manager.UseDb(getDbNum(userOnLineStateDbNum)).StringGet<DateTime?>(cacheKey_HeartbeatApp.KeyName) != null)
            {
                return true;
            }
            else
            {
                var cacheKey_Heartbeat_Web = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_Web, $"{UserID}:user.room:{ChannelID}");

                if (Manager.UseDb(getDbNum(userTypeStateDbNum)).StringGet<DateTime?>(cacheKey_Heartbeat_Web.KeyName) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public List<string> GetOnlineUserList(EnumUserType userType)
        {
            return Manager.Instance.SetMembers<string>(new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Heartbeat_Web, $"OnlineSet:{userType:G}").KeyName);
        }

        /// <summary>
        /// 检查APP用户是否还有心跳
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool HasUserAppHeartbeat(string UserID)
        {
       
            var cacheKey_HeartbeatApp = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_App, $"{UserID}");
            var appHeartbeat = Manager.UseDb(getDbNum(userOnLineStateDbNum)).StringGet<DateTime?>(cacheKey_HeartbeatApp.KeyName);

            if (appHeartbeat != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查WEB用户是否还有心跳
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool HasUserWebHeartbeat(string UserID)
        {

            var cacheKey_Heartbeat_Web = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_Web, $"{UserID}");
            var webHeartbeat = Manager.UseDb(getDbNum(userOnLineStateDbNum)).StringGet<DateTime?>(cacheKey_Heartbeat_Web.KeyName);

            if (webHeartbeat != null)
            {
                return true;
            }

            return false;
        }

        public void SetAppHeartBeat(string UserID,bool flag=true, EnumUserType userType = EnumUserType.User)
        {
            var cacheKey_HeartbeatApp = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_App, $"{UserID}");

            if (flag)
            {
                //加入到在线用户set
                Manager.Instance.SetAdd(new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Heartbeat_App, $"OnlineSet:{userType:G}").KeyName, UserID);
                Manager.UseDb(getDbNum(userOnLineStateDbNum)).StringSet(cacheKey_HeartbeatApp.KeyName, DateTime.Now);
            }
            else
            {
                Manager.Instance.SetRemove(new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Heartbeat_App, $"OnlineSet:{userType:G}").KeyName, UserID);
                Manager.UseDb(getDbNum(userOnLineStateDbNum)).RemoveCache(cacheKey_HeartbeatApp.KeyName);
            }
        }

        public void SetWebHeartBeat(string UserID,string type,bool flag=true, EnumUserType userType = EnumUserType.User)
        {
            var cacheKey_HeartbeatWeb = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_Web, $"{UserID}");
            var cacheKey_HeartbeatWeb_Type = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_Web, $"{UserID}:{type}");

            if (flag)
            {
                //加入到在线用户set
                Manager.Instance.SetAdd(new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Heartbeat_Web, $"OnlineSet:{userType:G}").KeyName, UserID);
                Manager.UseDb(getDbNum(userOnLineStateDbNum)).StringSet(cacheKey_HeartbeatWeb.KeyName, DateTime.Now,TimeSpan.FromSeconds(15));
                Manager.UseDb(getDbNum(userTypeStateDbNum)).StringSet(cacheKey_HeartbeatWeb_Type.KeyName, DateTime.Now, TimeSpan.FromSeconds(15));
            }
            else
            {
                Manager.Instance.SetRemove(new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Heartbeat_Web, $"OnlineSet:{userType:G}").KeyName, UserID);
                Manager.UseDb(getDbNum(userOnLineStateDbNum)).RemoveCache(cacheKey_HeartbeatWeb.KeyName);
                Manager.UseDb(getDbNum(userTypeStateDbNum)).RemoveCache(cacheKey_HeartbeatWeb_Type.KeyName);
            }
        }

        public void ListernHeartBeat()
        {
            var userTypes = Enum.GetNames(typeof(EnumUserType));

            //订阅缓存过期
            XuHos.Common.Cache.Manager.Instance.Subscribe($"__keyevent@{getDbNum(userOnLineStateDbNum)}__:expired", cacheKey =>
            {
                var service = new XuHos.BLL.Sys.Implements.HeartbeatService();
                XuHos.Common.LogHelper.WriteDebug("缓存过期:" + cacheKey);

                var key = cacheKey.ToString();


                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(
                    $".*:" + XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_Web + ":(?<UserID>.*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                var match = reg.Match(key);
                if (match?.Success == true)
                {
                    XuHos.Common.LogHelper.WriteInfo("用户心跳缓存过期:" + key);
                    if (match.Groups["UserID"].Success)
                    {
                        var UserID = match.Groups["UserID"].Value;
                        foreach (var userType in userTypes)
                        {
                            Manager.Instance.SetRemove(new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Heartbeat_Web, $"OnlineSet:{userType}").KeyName, UserID);
                            Manager.Instance.SetRemove(new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Heartbeat_App, $"OnlineSet:{userType}").KeyName, UserID);
                        }
                    }
                }
            });

            //订阅缓存过期 __keyevent@0__:expired               
            XuHos.Common.Cache.Manager.Instance.Subscribe($"__keyevent@{getDbNum(userTypeStateDbNum)}__:expired", cacheKey =>
            {
                try
                {
                    var service = new XuHos.BLL.Sys.Implements.HeartbeatService();
                    XuHos.Common.LogHelper.WriteDebug("缓存过期:" + cacheKey);

                    var key = cacheKey.ToString();

                  
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(
                                 $".*:" + XuHos.Common.Cache.Keys.StringCacheKeyType.Heartbeat_Web + ":(?<UserID>.*):(?<Type>.*):(?<ChannelID>.*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    if (reg.IsMatch(key))
                    {
                        var group = reg.Match(key);

                        XuHos.Common.LogHelper.WriteInfo("用户心跳缓存过期:" + key);

                        if (group.Groups.Count >= 3)
                        {
                            //匹配成功
                            if (group.Groups["UserID"].Success && group.Groups["Type"].Success && group.Groups["ChannelID"].Success)
                            {
                                var UserID = group.Groups["UserID"].Value;
                                var Type = group.Groups["Type"].Value;
                                var ChannelID = group.Groups["ChannelID"].Value;

                                using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                                {
                                    mqChannel.Publish<EventBus.Events.ChannelStateChangedEvent>(new EventBus.Events.ChannelStateChangedEvent()
                                    {
                                        ChannelID = int.Parse(ChannelID),
                                        FromUserID = UserID,
                                        FromPlatform = "WEB",
                                        State = EnumRoomState.Disconnection

                                    });
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            });
        }
    }
}
