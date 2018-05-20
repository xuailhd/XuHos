using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Extensions;

namespace XuHos.DTO
{
    /// <summary>
    /// 分诊医生
    /// </summary>
    public partial class DoctorTriageDTO
    {
        /// <summary>
        /// 预约ID
        /// </summary>
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 分诊医生ID
        /// </summary>
        public string TriageDoctorID { get; set; }

        /// <summary>
        /// 分诊状态（0无，1待分诊，2分诊中，3已分诊）
        /// </summary>
        public EnumTriageStatus TriageStatus { get; set; }

        /// <summary>
        /// 是否要经过导诊系统分诊
        /// </summary>
        public bool IsToGuidance { get; set; }

    }
}
