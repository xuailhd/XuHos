using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using XuHos.BLL.User.DTOs;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;
using XuHos.DAL.EF;
using XuHos.DTO;
using XuHos.Entity;

namespace XuHos.BLL.User.Implements
{
    public class UserWechatService
    {
        private const string NOTEXISTS = "NotExists";

        public string GetUserIdByOpenId(string openId, string appId)
        {
            var cacheKey = new EntityCacheKey<string>(StringCacheKeyType.User_OpenID, $"{appId}_{openId}");
            var userId = cacheKey.FromCache();
            if (userId == NOTEXISTS)
            {
                return null;
            }
            using (var db = new DBEntities())
            {
                userId = db.UserWechatMaps.Where(x => x.OpenID == openId && x.AppID == appId && !x.IsDeleted).Select(x => x.UserID).FirstOrDefault();
                (userId ?? NOTEXISTS).ToCache(cacheKey, TimeSpan.FromHours(4));
                return userId;
            }
        }

        /// <summary>
        /// 绑定OpenId
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="appId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool BindOpenId(string openId, string appId, string userId)
        {
            var cacheKey = new EntityCacheKey<string>(StringCacheKeyType.User_OpenID, $"{appId}_{openId}");
            var cUserId = cacheKey.FromCache();
            if (cUserId == userId)
            {
                return true;
            }
            try
            {
                using (var db = new DBEntities())
                {
                    if (!db.UserWechatMaps.Any(x => x.OpenID == openId && x.AppID == appId && !x.IsDeleted))
                    {
                        var entity = new UserWechatMap()
                        {
                            OpenID = openId,
                            AppID = appId,
                            UserID = userId,
                            CreateTime = DateTime.Now,
                            CreateUserID = "",
                            UserWechatMapID = Guid.NewGuid().ToString("N"),
                        };
                        db.UserWechatMaps.Add(entity);
                        db.SaveChanges();
                        return true;
                    }
                    return db.UserWechatMaps.Where(x => x.OpenID == openId && x.AppID == appId && !x.IsDeleted && (x.UserID == null || x.UserID == "")).Update(x => new UserWechatMap()
                    {
                        UserID = userId,
                        ModifyTime = DateTime.Now,
                    }) > 0;
                }
            }
            finally
            {
                cacheKey.RemoveCache();
            }
         
        }
    }
}

namespace XuHos.BLL.User.DTOs
{
}
