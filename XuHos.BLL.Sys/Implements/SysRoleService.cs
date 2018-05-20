using XuHos.BLL.Sys.DTOs.Response;
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
    public class SysRoleService:Common.CommonBaseService<Entity.UserRole>
    {
        public SysRoleService() : base("")
        {

        }
        List<string> RoleRolePrevileges { get; set; }

        public ResponseUserRoleDTO GetRoleInfo(string RoleId,string SystemType)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.UserRoles
                            where item.RoleID == RoleId && item.IsDeleted == false
                            select new ResponseUserRoleDTO()
                            {
                                RoleID = item.RoleID,
                                RoleName = item.RoleName,
                                Memo = item.Memo
                            };

                var userRole = query.FirstOrDefault();

                RoleRolePrevileges = (from item in db.UserRolePrevileges
                                      where item.RoleID == RoleId && item.IsDeleted == false
                                      select item.ModuleID).ToList();
                if(SystemType=="Admin")
                { 
                userRole.Modules = GetAllModuleTree("",0);
                }
                else if(SystemType == "Doctor")
                {
                    userRole.Modules = GetDoctorAllModuleTree("",0);
                }
                else if (SystemType == "Org")
                {
                    userRole.Modules = GetOrgAllModuleTree("", 0);
                }


                return userRole;

            }
        }


        /// <summary>
        /// 总店选择框(后台用)
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetRoleList(EnumRoleType RoleType)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.UserRoles
                            orderby item.ModifyTime descending
                            where item.RoleType == RoleType && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.RoleName,
                                Value = item.RoleID
                            };
                return query.ToList();
            }
        }

        public List<SysModuleTreeDTO> GetSystemModuleTree()
        {
            var topNodes = new List<SysModuleTreeDTO>()
            {
                new SysModuleTreeDTO() { id = "Admin",
                                text = "Admin",
                                url = "",
                                state = "closed",
                                children=GetAllModuleTree("",0),
                },
                 new SysModuleTreeDTO() { id = "Doctor",
                                text = "Doctor",
                                url = "",
                                state =  "closed",
                 children=GetDoctorAllModuleTree("",0)}

            };
            return topNodes;
        }

        /// <summary>
        /// 获取权限(后台用)
        /// </summary>
        /// <returns></returns>
        public List<SysModuleTreeDTO> GetAllModuleTree(string parentId, int level)
        {
            
            using (var db = new DBEntities())
            {
                var query = from item in db.SysModules
                            orderby item.Sort
                            where item.IsDeleted == false && item.Level == level && item.ParentModuleID == parentId &&item.TopModuleID=="Admin"
                            select new SysModuleTreeDTO()
                            {
                                id = item.ModuleID,
                                text = item.ModuleName,
                                url = item.ModuleUrl,
                                state = level == 0 ? "closed" : "",
                            };
                var list = query.ToList();

                list.ForEach(o =>
                {
                    o.children = GetAllModuleTree(o.id, level + 1);
                    if (RoleRolePrevileges != null)
                        o.@checked = string.IsNullOrEmpty(o.url.Trim()) ? false : RoleRolePrevileges.Contains(o.id);
                });
                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<SysModuleTreeDTO> GetDoctorAllModuleTree(string parentId, int level)
        {

            using (var db = new DBEntities())
            {
                var query = from item in db.SysModules
                            orderby item.Sort
                            where item.IsDeleted == false && item.Level == level && item.ParentModuleID == parentId && item.TopModuleID == "Doctor"
                            select new SysModuleTreeDTO()
                            {
                                id = item.ModuleID,
                                text = item.ModuleName,
                                url = item.ModuleUrl,
                                state =  "",
                                target=item.Target,
                                cssclass=item.CSSClass,
                            };
                var list = query.ToList();

                list.ForEach(o =>
                {
                    o.children = GetDoctorAllModuleTree(o.id, level + 1);
                    if (RoleRolePrevileges != null)
                        o.@checked = string.IsNullOrEmpty(o.url.Trim()) ? false : RoleRolePrevileges.Contains(o.id);
                });
                return list;
            }
        }

        /// <summary>
        /// 获取权限(后台用)
        /// </summary>
        /// <returns></returns>
        public List<SysModuleTreeDTO> GetOrgAllModuleTree(string parentId, int level)
        {

            using (var db = new DBEntities())
            {
                var query = from item in db.SysModules
                            orderby item.Sort
                            where item.IsDeleted == false && item.Level == level && item.ParentModuleID == parentId && item.TopModuleID == "Org"
                            select new SysModuleTreeDTO()
                            {
                                id = item.ModuleID,
                                text = item.ModuleName,
                                url = item.ModuleUrl,
                                state = level == 0 ? "closed" : "",
                            };
                var list = query.ToList();

                list.ForEach(o =>
                {
                    o.children = GetOrgAllModuleTree(o.id, level + 1);
                    if (RoleRolePrevileges != null)
                        o.@checked = string.IsNullOrEmpty(o.url.Trim()) ? false : RoleRolePrevileges.Contains(o.id);
                });
                return list;
            }
        }
    }
}
