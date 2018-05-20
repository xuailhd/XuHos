using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XuHos.BLL.Common.DTOs.Response
{
    public class ResponseToken
    {
        public DateTime Time { get; set; }
        public string AppId { get; set; }
        public string AppKey { get; set; }

        /// <summary>
        /// 等同于 机构ID， 医院表的医院ID
        /// </summary>
        public string OrgID { get; set; }

        public TimeSpan ExpireDate { get; set; }

        public string Token { get; set; }

        public string SourceType { get; set; }

    }

}