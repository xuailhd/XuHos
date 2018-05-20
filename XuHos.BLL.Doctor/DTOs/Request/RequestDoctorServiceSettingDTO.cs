using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    public class RequestDoctorServiceSettingDTO
    {

        /// <summary>
        /// 服务类型
        /// </summary>
        public EnumDoctorServiceType ServiceType { get; set; }

        /// <summary>
        /// 服务开关(0-关闭服务、1-开启服务)
        /// </summary>
        public int ServiceSwitch { get; set; }

        /// <summary>
        /// 服务价格
        /// </summary>
        public decimal ServicePrice { get; set; }

    }
}
