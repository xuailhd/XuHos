using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{   
    public  class Agora: IConfigSection
    {
        public string Secret
        { get; set; }

        public string Key
        { get; set; }
    }
}
