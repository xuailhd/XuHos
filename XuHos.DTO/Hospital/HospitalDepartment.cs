using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO
{
    /// <summary>
    /// 医院科室
    /// </summary>
    public partial class HospitalDepartmentDTO
    {

        /// <summary>
        /// 科室ID
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }   
        
        public HospitalDto Hospital { get; set; }     

        /// <summary>
        /// 医院医生
        /// </summary>
        public List<DoctorDto> Doctors { get; set; }

        /// <summary>
        /// 外接系统数据关联
        /// </summary>
        public ConversationExternalDTO ConversationExternals { get; set; }
    }
}
