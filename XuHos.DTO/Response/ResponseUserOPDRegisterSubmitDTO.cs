using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    public class ResponseUserOPDRegisterSubmitDTO
    {
        public string OPDRegisterID
        { get; set; }

        public string ActionStatus { get; set; }

        public string ErrorInfo
        { get; set; }

        public string OrderNO { get; set; }

        public EnumOrderState OrderState { get; set; }

        public int ChannelID { get; set; }
    }
}
