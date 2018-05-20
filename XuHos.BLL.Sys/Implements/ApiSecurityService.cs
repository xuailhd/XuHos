using XuHos.BLL;
using XuHos.BLL.Common.DTOs.Response;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.BLL.Sys.DTOs.Request;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using XuHos.DTO.Common;
using System;

namespace XuHos.BLL.Sys.Implements
{
    public class ApiSecurityService
    {
      
        #region 校验方法


        /// <summary>
        /// app接入端是否合法
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static bool CheckAppAccessAccount(string appId, string appSecret, out SysAccessAccountDTO account)
        {

            account = new SysAccessAccountDTO();

            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret))
            { return false;
            }

            //检查id,secret是否有效
            account = GetAppAccessAccountByAppID(appId);
           
            if (account != null && account.AppSecret == appSecret)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static bool CheckSign(
                        string appkey,
                        string sign,
                        string apptoken,
                        string noncestr,
                        string usertoken = "",
                        string userId = "")
        {
            if (string.IsNullOrEmpty(sign))
                return false;

            //签名参数不固定（参数按照字母排列）
            var signParam = GetSignParam(appkey,
                                         apptoken,
                                         noncestr,
                                         usertoken,
                                         userId);

            if (GetSign(signParam) == sign)
            {
                return true;
            }
            //兼容老版本签名方式（签名参数顺序需固定）
            else if (GetSign(
                "apptoken=" + apptoken +
                "&noncestr=" + noncestr +
                (!string.IsNullOrWhiteSpace(userId) ? ("&userid=" + userId) : "") +
                (!string.IsNullOrWhiteSpace(usertoken) ? ("&usertoken=" + usertoken) : "") +
                "&appkey=" + appkey) == sign)
            {
                return true;
            }


            return false;
        }


        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public static bool CheckAppToken(string tokenStr)
        {
            if (string.IsNullOrEmpty(tokenStr))
                return false;

            var token = GetAppToken(tokenStr);

            if (token == null)
                return false;


            return true;
        }

        /// <summary>
        /// 检查登录用户
        /// </summary>
        /// <param name="userTokenStr"></param>
        /// <returns></returns>
        public static bool CheckUserTicket(string userTokenStr)
        {
            if (string.IsNullOrEmpty(userTokenStr))
                return false;

            var ticket = GetUserTicket(userTokenStr);

            if (ticket == null || string.IsNullOrEmpty(ticket.UserID))
            {
                return false;
            }
            else
            {

                return true;
            }
        }

        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static bool CheckTimestamp(string timeStamp)
        {
            if (string.IsNullOrEmpty(timeStamp))
                return false;

            var reqTimeStamp = Convert.ToInt64(timeStamp);//客户端请求时的时间戳
            var nowTimestamp = Convert.ToInt64(TimeHelper.GetTimestamp());//服务器当前时间戳
            var allowSeconds = 5 * 60;//允许时间波动范围,5分钟
            if (reqTimeStamp < nowTimestamp - allowSeconds || reqTimeStamp > nowTimestamp + allowSeconds)
                return false;
            return true;
        }

        /// <summary>
        /// 验证随机数(在指定时间内随机数不能重复)
        /// </summary>
        /// <param name="nonceStr"></param>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public static bool CheckNonceStr(string nonceStr, string tokenStr)
        {
            if (string.IsNullOrEmpty(nonceStr) || nonceStr.Length < 10 || nonceStr.Length > 40)
                return false;

            var CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.api_noncestr, $"{tokenStr}:{nonceStr}");
            
            string cacheNonceStr = CacheKey.FromCache<string>();

            if (cacheNonceStr == "1")
                return false;

            return true;
        }

        #endregion

        #region 设置方法

        /// <summary>
        /// 保存token
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <param name="appId"></param>
        public static void SetAppToken(string tokenStr, SysAccessAccountDTO account)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.API_apptoken, tokenStr);
            var token = CreateAppToken(tokenStr, account);
            token.ToCache(CacheKey, token.ExpireDate);
        }

        /// <summary>
        /// 创建AppToken
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static XuHos.BLL.Common.DTOs.Response.ResponseToken CreateAppToken(string tokenStr, SysAccessAccountDTO account)
        {
            var expireMinute = _GetAppTokenExpireMinute();
            var nowTime = DateTime.Now;
            var token = new XuHos.BLL.Common.DTOs.Response.ResponseToken()
            {

                Token = tokenStr,
                AppId = account.AppId,
                SourceType = account.SourceType,
                Time = nowTime,
                ExpireDate =TimeSpan.FromMinutes(expireMinute),
                OrgID = account.OrgID
            };

            return token;

        }


        /// <summary>
        /// 记录本次请求的随机数
        /// </summary>
        /// <param name="nonceStr"></param>
        /// <param name="tokenStr"></param>
        public static void SetNonceStr(string nonceStr, string tokenStr)
        {
            if (!string.IsNullOrWhiteSpace(nonceStr) && !string.IsNullOrWhiteSpace(tokenStr))
            {
                var CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.api_noncestr,$"{tokenStr}:{nonceStr}");

                "1".ToCache(CacheKey,TimeSpan.FromMinutes(5));
            }
        }


        /// <summary>
        /// 登入，保存用户信息
        /// </summary>
        /// <param name="userTokenStr"></param>
        /// <param name="user"></param>
        public static void SetUserTicket(UserLoginServerTicketDTO user)
        {
            //设置缓存
            var expireMinute = _GetUserTokenExpireMinute();
            var UserTicket_CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<UserLoginServerTicketDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Ticket, user.UserToken);
            user.ToCache(UserTicket_CacheKey,TimeSpan.FromMinutes(expireMinute));
        }

        /// <summary>
        ///设置登录用户的相对过期
        /// </summary>
        public static void SetUserTokenExpire(string userToken)
        {
            if (!string.IsNullOrEmpty(userToken))
            {
                //设置过期时间
                var expireMinute = _GetUserTokenExpireMinute();
                var userTokenCacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.API_usertoken, userToken);
                var userTicketCacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Ticket, userToken);

                userTokenCacheKey.ExpireEntryAt(TimeSpan.FromMinutes(expireMinute));
                userTicketCacheKey.ExpireEntryAt(TimeSpan.FromMinutes(expireMinute));
            }
        }

        #endregion

        #region 获取方法


        /// <summary>
        /// 获取接入端账户信息
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static SysAccessAccountDTO GetAppAccessAccountByAppID(string appId)
        {

            return new BLL.Sys.Implements.SysAccessAccountService().GetAllAccount().FirstOrDefault(i => i.AppId == appId);
        }

        /// <summary>
        /// 获取验证通过的接入端信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static XuHos.BLL.Common.DTOs.Response.ResponseToken GetAppToken(string apptoken)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<XuHos.BLL.Common.DTOs.Response.ResponseToken>(XuHos.Common.Cache.Keys.StringCacheKeyType.API_apptoken, apptoken);
            return CacheKey.FromCache();
        }


        /// <summary>
        /// 获取签名参数
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static SortedList GetSignParam(
                        string appkey,                      
                        string apptoken,
                        string noncestr,
                        string usertoken,
                        string userId = "")
        {

            var signParam = new SortedList();
            signParam.Add("appkey", appkey);
            signParam.Add("apptoken", apptoken);
            signParam.Add("noncestr", noncestr);

            if (!string.IsNullOrEmpty(userId))
            {
                signParam.Add("userid", userId);
            }

            if (!string.IsNullOrEmpty(usertoken))
            {
                signParam.Add("usertoken", usertoken);
            }

            return signParam;
        }

   
        /// <summary>
        /// 获取参数签名
        /// </summary>
        /// <param name="reqParams"></param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public static string GetSign(SortedList reqParams)
        {
            var sb = new StringBuilder();
            foreach (string key in reqParams.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                sb.Append(key + "=" + reqParams[key]);
            }

            return GetSign(sb.ToString());
        }

        public static string GetSign(string str)
        {
            string sign = StringEncrypt.GetMD5(str, "UTF-8").ToUpper();
            return sign;
        }


     


        public static UserLoginServerTicketDTO GetUserTicket(string userTokenStr)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<UserLoginServerTicketDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Ticket, userTokenStr);
            var ticket = CacheKey.FromCache();
            if (ticket != null)
            {
                return ticket;

            }
            else
            {
                return default(UserLoginServerTicketDTO);
            }
        }



        /// <summary>
        /// 获取接入端密钥
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public static string GetAppKey(string tokenStr)
        {
            var token = GetAppToken(tokenStr);
            if (token != null)
            {
                var account = GetAppAccessAccountByAppID(token.AppId);
                if (account != null)
                {
                    return account.AppKey;
                }
            }

            return "";
        }

        /// <summary>
        /// 获取接入端编号
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public static string GetAppId(string tokenStr)
        {
            var token = GetAppToken(tokenStr);
            if (token != null)
            {
                var account = GetAppAccessAccountByAppID(token.AppId);
                if (account != null)
                {
                    return account.AppId;
                }
            }

            return "";
        }

        /// <summary>
        /// 是否忽略验证
        /// </summary>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public static bool IsIgnoreAuthByAppToken(string tokenStr)
        {
            var token = GetAppToken(tokenStr);
            if (token != null)
            {
                var account = GetAppAccessAccountByAppID(token.AppId);
                if (account != null)
                {
                    return account.IgnoreApiAuth.HasValue ? account.IgnoreApiAuth.Value : false;
                }
            }

            return false;
        }

        public static bool IsIgnoreAuthByAppId(string appId)
        {
            var account = GetAppAccessAccountByAppID(appId);
            if (account != null)
            {
                return account.IgnoreApiAuth.HasValue ? account.IgnoreApiAuth.Value : false;
            }
            return false;
        }

        #endregion

        public static void RemoveUserToken(string userTokenStr)
        {
            var CacheKey_API_usertoken = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.API_usertoken, userTokenStr);
            var CacheKey_MAP_GetUserIDByUserToken = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Ticket, userTokenStr);

            CacheKey_MAP_GetUserIDByUserToken.RemoveCache();
            CacheKey_API_usertoken.RemoveCache();
        }

        /// <summary>
        /// 解密userId
        /// </summary>
        /// <param name="encryptUserId"></param>
        /// <param name="appTokenStr"></param>
        /// <param name="userSign"></param>
        /// <returns></returns>
        public static string DecryptUserIdByAppToken(string encryptUserId, string appTokenStr)
        {
            if (string.IsNullOrEmpty(encryptUserId))
                return "";

            var appToken = GetAppToken(appTokenStr);
            var  appAccessAccount = GetAppAccessAccountByAppID(appToken.AppId);

            //没有分配userKey的，则验证失败
            var userKey = appAccessAccount.UserKey;

            if (string.IsNullOrEmpty(userKey))
            {
                return "";
            }

            //解密userId
            var userId = StringEncrypt.Decrypt(encryptUserId, userKey);

            return userId;
        }

        /// <summary>
        /// 解密userId
        /// </summary>
        /// <param name="encryptUserId"></param>
        /// <param name="appTokenStr"></param>
        /// <param name="userSign"></param>
        /// <returns></returns>
        public static string DecryptUserIdByAppId(string encryptUserId, string appId = null)
        {
            if (string.IsNullOrEmpty(encryptUserId))
                return "";

           
            var appAccessAccount = GetAppAccessAccountByAppID(appId);

            //没有分配userKey的，则验证失败
            var userKey = appAccessAccount.UserKey;
            if (string.IsNullOrEmpty(userKey))
                return "";

            //解密userId
            var userId = StringEncrypt.Decrypt(encryptUserId, userKey);
            return userId;
        }

        /// <summary>
        /// appToken过期时间(分钟)
        /// </summary>
        static int _GetAppTokenExpireMinute()
        {
            return 30;
        }


        /// <summary>
        /// userToken过期时间(分钟)
        /// </summary>
        static int _GetUserTokenExpireMinute()
        {
            return 30;
        }
        
    }
}