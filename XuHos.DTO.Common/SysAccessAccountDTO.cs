using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    public class SysAccessAccountDTO
    {

        public string AccessID { get; set; }


        public string AppId { get; set; }

        public string AppSecret { get; set; }


        public string AppKey { get; set; }

        /// <summary>
        /// JS，IOS，Android等
        /// </summary>
        public string SourceType { get; set; }

        public string UserKey { get; set; }

        public string OrgID { get; set; }

        public bool? IgnoreApiAuth { get; set; }
    }
}
