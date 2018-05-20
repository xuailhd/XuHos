using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestIMCustomMsgRoomReadyTurnOnDataDTO
    {
        public int ChannelID { get; set; }

        public string ServiceID { get; set; }

        public string DoctorID { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepartmentName { get; set; }

        public string PhotoUrl { get; set; }
    }
        

    public class RequestIMCustomMsgRoomReadyTurnOn : IRequestIMCustomMsg<RequestIMCustomMsgRoomReadyTurnOnDataDTO>
    {
        public RequestIMCustomMsgRoomReadyTurnOnDataDTO Data
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
                return EnumIMCustomMsgType.Room_ReadyTurnOn.GetEnumDescript();
            }
        }
    }
}
