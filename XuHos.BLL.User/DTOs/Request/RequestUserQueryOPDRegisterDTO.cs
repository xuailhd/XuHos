using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Request
{
    public class RequestUserQueryOPDRegisterDTO : RequestSearchCondition
    {

        public RequestUserQueryOPDRegisterDTO()
        {
            PageSize = int.MaxValue;
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate
        {
            get; set;
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate
        {
            get; set;
        }
        
        /// <summary>
        /// 机构ID
        /// </summary>
        public string OrgnazitionID { get; set; }

        public string MemberID { get; set; }
        public EnumDoctorServiceType? OPDType { get; set; }
        public EnumOrderState? OrderState { get; set; }

        public bool WithoutNotSigned { get; set; }
    }
}
