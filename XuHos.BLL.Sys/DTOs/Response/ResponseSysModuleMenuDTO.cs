using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class SysModuleMenuDTO
    {
        public string menuid { set; get; }
        public string menuname { set; get; }
        public string icon { set; get; }
        public string url { set; get; }

        public string CSSClass { set; get; }
        public string Target { set; get; }
        public List<SysModuleMenuDTO> menus { set; get; }
        
    }
}
