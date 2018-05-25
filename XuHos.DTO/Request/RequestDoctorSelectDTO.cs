using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XuHos.Common.Enum;

namespace XuHos.DTO
{
    /// <summary>
    /// 搜索请求
    /// </summary>
    public class RequestDoctorSelectDTO : Common.IPagerRequest, Common.IRequestKeywordQuery
    {
        public RequestDoctorSelectDTO()
        {
            this.Keyword = "";
            this.CurrentPage = 1;
            this.PageSize = 10;
        }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Keyword { get; set; }

        /// <summary>
        /// 医院Id
        /// </summary>
        public string HospitalId { get; set; }

        /// <summary>
        /// 按科室查询
        /// </summary>
        public string DepartmentName { get; set; }
        
        /// <summary>
        /// 按科室模糊查询
        /// </summary>
        public string DepartmentKeyword { get; set; }

        /// <summary>
        /// 按医院查询
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 按医院模糊查询
        /// </summary>
        public string HospitalKeyword { get; set; }

        /// <summary>
        /// 分页索引
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排班时间
        /// </summary>
        public DateTime? ScheduleDate { get; set; }

        /// <summary>
        /// 是否包含套餐可用状态
        /// </summary>
        public bool IncludePkgStatus { get; set; } = false;

        /// <summary>
        /// 是否包含回复数
        /// </summary>
        public bool IncludeReplyCount { get; set; } = false;

        /// <summary>
        /// 排序类型（4：按照有无排班排序；5：按照有无套餐排序；6、按照评分降序排序）
        /// </summary>
        public List<EnumDoctorOrderBy> OrderBy { get; set; }
    }
}