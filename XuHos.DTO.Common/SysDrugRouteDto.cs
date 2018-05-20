using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    public class SysDrugRouteDto
    {
        public SysDrugRouteDto(string name, string pyname, string wbname)
        {
            Name = name;
            PYName = pyname;
            WBName = wbname;
        }
        public string Name { get; set; }
        public string PYName { get; set; }
        public string WBName { get; set; }
    }
}
