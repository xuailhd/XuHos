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
    public class RequestUserQueryOPDRegisterDTO : Common.IPagerRequest, Common.IRequestKeywordQuery, Common.IRequestSegmentQuery
    {
        public RequestUserQueryOPDRegisterDTO()
        {
            PageSize = int.MaxValue;
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

        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgnazitionID { get; set; }

        public string MemberID { get; set; }

        public EnumDoctorServiceType? OPDType { get; set; }

        public EnumOrderState? OrderState { get; set; }

        public bool WithoutNotSigned { get; set; }

        public bool? FilterRecipeAndSchedule { get; set; }
    }
}