using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 家庭医生签约已确认    
    /// </summary>
    public class FamilyDoctorSignatureConfirmed : BaseEvent, IEvent
    {
        /// <summary>
        /// 签约编号
        /// </summary>
        public string SignatureID { get; set; }

        /// <summary>
        /// 医生编号
        /// </summary>
        public string DoctorID { get; set;}

        public List<string> IDNumbers { get; set; }
    }
}
