using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    public class Response<TEntity> : IResponse<TEntity>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public TEntity Data { get; set; }

        //总记录数
        public int Total { get; set; }
    }
}
