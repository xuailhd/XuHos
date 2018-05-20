using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    /// <summary>
    /// 日期分段查询请求
    /// </summary>
    public interface IRequestSegmentQuery : IRequest
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        DateTime? EndDate { get; set; }
    }
}
