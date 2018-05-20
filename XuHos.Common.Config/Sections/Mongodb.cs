using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Config.Sections
{   
    public  class Mongodb: IConfigSection
    {
        public string ConnectionString
        { get; set; }
        public string CollectionName
        { get; set; }
    }
}
