using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class SysModuleTreeDTO
    {
        public string id { set; get; }
        public string text { set; get; }
        public string state { set; get; }
        public string url { set; get; }
        public bool @checked { set; get; }

        public string target { set; get; }
        public string cssclass { set; get; }

        public List<SysModuleTreeDTO> children { set; get; }
    }
}
