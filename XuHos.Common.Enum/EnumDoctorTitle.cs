using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    [Description("医生职称")]
    public enum EnumDoctorTitle
    {
        /// <summary>
        /// 医生职称
        /// </summary>
        [Description("住院医师")]
        Resident = 1,
        /// <summary>
        /// 医生职称
        /// </summary>
        [Description("主治医师")]
        AttendingPhysician =2,
        /// <summary>
        /// 医生职称
        /// </summary>
        [Description("副主任医师")]
        AssociateChiefPhysician = 3,
        
        /// <summary>
        /// 医生职称
        /// </summary>
        [Description("主任医师")]
        ChiefPhysician = 4
    }
}
