using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ResponseDoctorDiagnosisFormularDTO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string DiagnosisFormularID { set; get; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 初步诊断
        /// </summary>
        public string PreliminaryDiagnosis { set; get; }

        /// <summary>
        /// 医生建议
        /// </summary>
        public string Advised { set; get; }
    }
}
