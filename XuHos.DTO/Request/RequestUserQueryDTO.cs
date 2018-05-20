using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;

namespace XuHos.DTO
{
    /// <summary>
    /// 搜索请求
    
    /// 日期：2016年8月4日
    /// </summary>
    public class RequestUserQueryDTO : Common.IPagerRequest, Common.IRequestKeywordQuery, Common.IRequestSegmentQuery
    {
        public RequestUserQueryDTO()
        {
            this.Keyword = "";
            this.CurrentPage = 1;
            this.PageSize = 10;
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }


        ///查询关键字
        /// <summary>
        /// 搜索关键字
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Keyword { get; set; }

        /// <summary>
        /// 分页索引
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        public string MemberId { get; set; }

        public string DoctorMemberID { get; set; }
    }

    /// <summary>
    /// 查询中医体质
    /// </summary>
    public class RequestMemberExaminedQueryDTO : Common.IPagerRequest
    {
        public enum EnumUnit
        {
            Day,

            Count,
        }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分页索引
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EnumUnit Unit { get; set; }
    }
}