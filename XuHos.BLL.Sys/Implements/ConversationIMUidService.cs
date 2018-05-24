using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Entity;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using XuHos.DAL.EF;

using EntityFramework.Caching;
using XuHos.Common.Cache;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.Implements
{
    /// <summary>
    /// 房间相关业务逻辑
    /// </summary>
    public class ConversationIMUidService : BLL.Common.CommonBaseService<XuHos.Entity.ConversationIMUid>
    {
        public ConversationIMUidService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        {
          
        }

        /// <summary>
        /// 获取医生的通信唯一标识
        /// </summary>
        /// <param name="doctorIDList"></param>
        /// <returns></returns>
        public List<int> GetDoctorIMUids(params string[] doctorIDList)
        {
            using (DBEntities db = new DBEntities())
            {
                #region 根据医生编号查询医生的所有用户标识
                var identifiers = (from doctor in db.Doctors.Where(a => !a.IsDeleted)
                                   join doctorId in doctorIDList on doctor.DoctorID equals doctorId
                                   join user in db.Users.Where(a => !a.IsDeleted) on doctor.DoctorID equals user.UserID
                                   join uid in db.ConversationIMUids.Where(a => !a.IsDeleted) on user.UserID equals uid.UserID
                                   select uid.Identifier).ToList();

                return identifiers;
                #endregion
            }
        }

        /// <summary>
        /// 获取医生的通信唯一标识
        /// </summary>
        /// <param name="doctorIDList"></param>
        /// <returns></returns>
        public int GetDoctorIMUid(string doctorID)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetIMUidByDoctorID, doctorID);

            int? uid = cacheKey.FromCache<int?>();
            if (uid == null || !uid.HasValue || uid.Value == 0)
            {
                uid = GetDoctorIMUids(doctorID).FirstOrDefault();
                uid.ToCache(cacheKey);
            }

            return uid.Value;

        }

        /// <summary>
        /// 获取用户的通信唯一标识
        /// </summary>
        /// <param name="doctorIDList"></param>
        /// <returns></returns>
        public List<int> GetUserIMUids(params string[] UserIDList)
        {


            using (DBEntities db = new DBEntities())
            {
                #region 根据医生编号查询医生的所有用户标识
                var identifiers = (from user in db.Users.Where(a => !a.IsDeleted)
                                   join userId in UserIDList on user.UserID equals userId
                                   join uid in db.ConversationIMUids.Where(a => !a.IsDeleted) on user.UserID equals uid.UserID
                                   select uid.Identifier).ToList();

                return identifiers;
                #endregion
            }
        }

        /// <summary>
        /// 获取用户的通信唯一标识
        /// </summary>
        /// <param name="doctorIDList"></param>
        /// <returns></returns>
        public int GetUserIMUid(string UserID)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetIMUidByUserID, UserID);
            int? uid = cacheKey.FromCache<int?>();
            if (uid == null || !uid.HasValue || uid.Value == 0)
            {
                uid = GetUserIMUids(UserID).FirstOrDefault();
                uid.ToCache(cacheKey);
            }
            return uid.Value;
        }

        /// <summary>
        /// 获取用户的通信唯一标识
        /// </summary>
        /// <param name="doctorIDList"></param>
        /// <returns></returns>
        public List<int> GetUserMemberIMUids(params string[] UserMemberIDList)
        {
            using (DBEntities db = new DBEntities())
            {
                #region 根据医生编号查询医生的所有用户标识

                var identifiers = (from user in db.Users
                                   join member in db.UserMembers on user.UserID equals member.UserID
                                   join memberId in UserMemberIDList on member.MemberID equals memberId
                                   join uid in db.ConversationIMUids.Where(a => !a.IsDeleted) on user.UserID equals uid.UserID
                                   select uid.Identifier).ToList();

                return identifiers;
                #endregion
            }
        }

        /// <summary>
        /// 获取用户的通信唯一标识
        /// </summary>
        /// <param name="doctorIDList"></param>
        /// <returns></returns>
        public int GetUserMemberIMUid(string UserMemberID)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetIMUidByMemberID, UserMemberID);

            int? uid = cacheKey.FromCache<int?>();
            if (uid == null || !uid.HasValue || uid.Value == 0)
            {
                uid = GetUserMemberIMUids(UserMemberID).FirstOrDefault();
                uid.ToCache(cacheKey);
            }

            return uid.Value;
        }

        /// <summary>
        /// 通过IM唯一标识获取用户编号
        /// </summary>
        /// <param name="Identifiers"></param>
        /// <returns></returns>
        public List<string> GetUserIdByUids(params int[] Identifiers)
        {
            using (DBEntities db = new DBEntities())
            {
             
                return (from user in db.Users.Where(a => !a.IsDeleted)
                        join uid in db.ConversationIMUids.Where(a => !a.IsDeleted) on user.UserID equals uid.UserID
                        join identifier in Identifiers on uid.Identifier equals identifier
                        select user.UserID).ToList();
             
            }
        }


        /// <summary>
        /// 当前通信唯一标识有效
        
        /// 日期：2016年12月28日
        /// </summary>
        /// <param name="Identifier"></param>
        /// <returns></returns>
        public bool EnableUid(int[] Identifiers)
        {
            using (DBEntities db = new DBEntities())
            {
                db.ConversationIMUids.Where(a => Identifiers.Contains(a.Identifier) && !a.Enable).Update(a => new ConversationIMUid()
                {

                    Enable = true,
                    ModifyTime = DateTime.Now
                });

                db.SaveChanges();
                return true;
            }
        }
    }
}
