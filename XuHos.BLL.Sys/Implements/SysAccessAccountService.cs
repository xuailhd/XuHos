using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.Common.Utility;
using XuHos.DAL.EF;
using XuHos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XuHos.BLL.Common.DTOs.Response;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.Implements
{
    public class SysAccessAccountService : Common.CommonBaseService<SysAccessAccount>
    {
        XuHos.Common.Cache.Keys.EntityListCacheKey<SysAccessAccountDTO> CacheKey;


        public SysAccessAccountService() :base("")
        {
            CacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<SysAccessAccountDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.SysAccessAccount,"");
        }
        
        /// <summary>
        /// 所有接入账户
        /// </summary>
        /// <returns></returns>
        public List<SysAccessAccountDTO> GetAllAccount()
        {
          
            var _SysAccountList = CacheKey.FromCache();

            //缓存中不存在，从数据库中获取
            if (_SysAccountList == null)
            {
                _SysAccountList = base.GetPageList<SysAccessAccountDTO>(1, int.MaxValue, i => i.IsDeleted == false).Data;
                if (_SysAccountList != null)
                    _SysAccountList.ToCache<List<SysAccessAccountDTO>>(CacheKey);
                else
                    _SysAccountList = new List<SysAccessAccountDTO>();
            }
        
          
            return _SysAccountList;
        }

        public SysAccessAccountDTO GetById(string appId)
        {
            return GetAllAccount().FirstOrDefault(i => i.AppId == appId);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public bool ClearAccountCache()
        {
       
            CacheKey.RemoveCache();
            return true;
        }

        #region //要清除缓存，重写了增删改
        public override bool Insert(SysAccessAccount model)
        {
            var res = base.Insert(model);
            if (res == true)
                ClearAccountCache();
            return res;
        }
        public override bool Update(SysAccessAccount model)
        {
            var res = base.Update(model);
            if (res == true)
                ClearAccountCache();
            return res;
        }
        public override bool Update(string ID, Expression<Func<SysAccessAccount, SysAccessAccount>> updateExpress)
        {
            var res = base.Update(ID, updateExpress);
            if (res == true)
                ClearAccountCache();
            return res;

        }

        public override bool Delete(string ID)
        {
            var res = base.Delete(ID);
            if (res == true)
                ClearAccountCache();
            return res;
        }
        #endregion

    }
}
