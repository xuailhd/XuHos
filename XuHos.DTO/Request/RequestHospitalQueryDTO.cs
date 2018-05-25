using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    /// <summary>
    /// 搜索请求
    /// </summary>
    public class RequestHospitalQueryDTO : Common.IPagerRequest,Common.IRequestKeywordQuery
    {

        ///查询关键字
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 分页索引
        /// </summary>
        public int CurrentPage
        {
            get; set;
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get; set;
        }
    }

}
