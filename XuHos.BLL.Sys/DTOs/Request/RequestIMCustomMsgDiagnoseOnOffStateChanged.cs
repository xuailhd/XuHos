using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.BLL.Sys.DTOs.Request;

namespace XuHos.BLL.Sys.DTOs
{
    public class RequestIMCustomMsgDiagnoseOnOffStateChanged : IRequestIMCustomMsg<RequestDoctorDiagnoseStateDTO>
    {
        public RequestDoctorDiagnoseStateDTO Data
        {
            get; set;
        }

        public string Desc
        {
            get; set;
        }

        public string Ext
        {
            get
            {
                return EnumIMCustomMsgType.Diagnose_OnOff_StateChanged.GetEnumDescript();
            }
        }
    }
}
