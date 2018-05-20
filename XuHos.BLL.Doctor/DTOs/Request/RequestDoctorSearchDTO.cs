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
    /// 医生列表查询条件
    /// </summary>
    public class RequestDoctorSearchDTO : RequestSearchCondition
    {
        /// <summary>
        /// 公共科室ID
        /// </summary>
        public List<string> CAT_NOs { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public List<string> DepartmentIDs { get; set; }

        /// <summary>
        /// 医生职称
        /// </summary>
        public List<string> DoctorTitles { get; set; }

        /// <summary>
        /// 是否义诊
        /// </summary>
        public bool IsFreeClinicr { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public EnumDoctorOrderBy SortFiled { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public bool SortIsAsc { get; set; }

        /// <summary>
        /// 是否家庭医生
        /// </summary>
        public bool IsFamilyDoctor { get; set; }

    }

    public class RequestHospitalDoctorSearchDTO : RequestSearchCondition
    {
        /// <summary>
        /// 一级公共科室ID
        /// </summary>
        public string PARENT_CAT_NO { get; set; }

        /// <summary>
        /// 二级公共科室ID
        /// </summary>
        public string CAT_NO { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public string DepartmentID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 医生职称
        /// </summary>
        public List<string> DoctorTitles { get; set; }

        /// <summary>
        /// 是否义诊
        /// </summary>
        public bool IsFreeClinicr { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public EnumDoctorOrderBy SortFiled { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public bool SortIsAsc { get; set; }

        /// <summary>
        /// 是否家庭医生
        /// </summary>
        public bool IsFamilyDoctor { get; set; }

    }
}
