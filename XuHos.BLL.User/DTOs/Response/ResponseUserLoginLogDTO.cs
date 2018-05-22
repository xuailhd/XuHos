using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;

namespace XuHos.BLL.User.DTOs
{
    public class ResponseUserLoginLogDTO
    {

        /// <summary>
        /// UserID
        /// 
      
        public string UserID { get; set; }

        public string UserAccount { get; set; }

        public string LoginAccount { get; set; }

        public int RoleType { get; set; }

        public string UserRoleName { get; set; }

        public string HospitalName { get; set; }

        public string TopDrugStoreName { get; set; }
        public string TopDrugStoreID { get; set; }
        public string TopPath { get; set; }
        /// <summary>
        /// OrgID 登录用户所属机构ID
        /// </summary>

        public string OrgID { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
       
        public EnumLoginType LoginType { get; set; }

        public string LoginTypeName { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary> 
        public DateTime LoginTime { get; set; }
    }
}
