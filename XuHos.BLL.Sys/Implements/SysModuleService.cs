using XuHos.BLL.Sys.DTOs.Response;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.Implements
{
    public class SysModuleService : XuHos.BLL.Common.CommonBaseService<Entity.SysModule>
    {
        public SysModuleService(string CurrentOperatorUserID) :base(CurrentOperatorUserID)
        { }

        /// <summary>
        /// 依据角色,系统标识 获取菜单功能权限
        /// </summary>
        /// <param name="roletypes"></param>
        /// <param name="topModuleID"></param>
        /// <param name="type">0 菜单 1 功能</param>
        /// <returns></returns>
        public List<ResponseSysModuleForDocDTO> GetRoleMenus(List<EnumRoleType> roletypes,string topModuleID,int type = -1 )
        {
            StringCacheKey SysModuleskey = new StringCacheKey(StringCacheKeyType.Sys_SysModules);

            var sysModules = SysModuleskey.FromCache<List<ResponseRoleSysModuleDTO>>();

            if (sysModules == null)
            {
                using(var db = new DBEntities())
                {
                    sysModules = (from sys in db.SysModules
                                  join sysmap in db.UserRolePrevileges on sys.ModuleID equals sysmap.ModuleID
                                  join role in db.UserRoles on sysmap.RoleID equals role.RoleID
                                  where !sys.IsDeleted
                                  select new ResponseRoleSysModuleDTO()
                                  {
                                      CSSClass = sys.CSSClass,
                                      Level = sys.Level,
                                      ModuleID = sys.ModuleID,
                                      ModuleName = sys.ModuleName,
                                      ModuleType = sys.ModuleType,
                                      ModuleUrl = sys.ModuleUrl,
                                      ParentModuleID = sys.ParentModuleID,
                                      Sort = sys.Sort,
                                      Target = sys.Target,
                                      TopModuleID = sys.TopModuleID,
                                      RoleID = role.RoleID,
                                      RoleType = (int)role.RoleType
                                  }).ToList();
                    sysModules.ToCache(SysModuleskey);
                }
            }

            if(sysModules == null)
            {
                return new List<ResponseSysModuleForDocDTO>();
            }
            else
            {
                var query = (from sys in sysModules
                             join role in roletypes on sys.RoleType equals (int)role
                             where sys.TopModuleID == topModuleID && !string.IsNullOrEmpty(sys.ModuleUrl)
                             group new { sys.CSSClass , sys.ModuleUrl , sys.Target, sys.ModuleName, sys.Sort, sys.ModuleType }
                             by new { sys.CSSClass, sys.ModuleUrl, sys.Target, sys.ModuleName, sys.Sort, sys.ModuleType } into gro
                             select new ResponseSysModuleForDocDTO()
                             {
                                 cssClass = gro.Key.CSSClass,
                                 href = gro.Key.ModuleUrl,
                                 target = gro.Key.Target,
                                 title = gro.Key.ModuleName,
                                 Sort = gro.Key.Sort,
                                 ModuleType = gro.Key.ModuleType,
                                 id = gro.Key.ModuleUrl
                             });

                if (type >= 0)
                {
                    query = query.Where(t => t.ModuleType == type);
                }
                return query.OrderBy(t=>t.Sort).ToList();
            }
        }
    }
}
