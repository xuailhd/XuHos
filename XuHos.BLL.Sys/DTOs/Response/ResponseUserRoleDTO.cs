using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class ResponseUserRoleDTO
    {
        
        public string RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        public string UserAccount { set; get; }
        public string UserCNName { set; get; }
        public List<SysModuleTreeDTO> Modules { set; get; }
        public List<SysModuleMenuDTO> RoleModules { set; get; }
        public string Memo { set; get; }

    }
}
