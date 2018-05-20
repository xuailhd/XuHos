using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestSysNoticeQueryDTO
    {
        public string keyWord { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }
    }
}
