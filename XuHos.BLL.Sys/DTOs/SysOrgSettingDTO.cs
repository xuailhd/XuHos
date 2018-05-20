using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs
{
    public class SysOrgSettingDTO
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgID { get; set; }

        /// <summary>
        /// 支付回调地址
        /// </summary>
        public string PaymentCallbackURL { get; set; }
    }
}
