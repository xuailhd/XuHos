using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    public interface IResponse<TEntity>
    {
        TEntity Data { get; set; }

        //总记录数
        int Total { get; set; }
    }
}
