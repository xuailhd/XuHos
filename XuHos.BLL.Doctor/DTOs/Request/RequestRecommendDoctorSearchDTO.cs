using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Common;
using XuHos.Common.Enum;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 推荐医生列表查询条件
    /// </summary>
    public class RequestRecommendDoctorSearchDTO : RequestSearchCondition
    {
        public string HospitalID { get; set; }
        public string DepartmentID { get; set; }

        public string CurrentDoctorID { get; set; }

        /// <summary>
        /// 医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生
        /// </summary>
        public int? DoctorType { get; set; }

        /// <summary>
        /// 是否专家
        /// </summary>
        public bool? IsExpert { get; set; }

    }
}
