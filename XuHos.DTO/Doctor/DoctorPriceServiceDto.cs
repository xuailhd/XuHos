using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class DoctorPriceServiceDto
    {
        public DoctorPriceServiceDto()
        {
            ServiceID = "";
            ServiceType = 0;
            ServiceSwitch = 1;
            ServicePrice = 0;
        }

        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public EnumDoctorServiceType ServiceType { get; set; }

        /// <summary>
        /// 服务类型名称
        /// </summary>
        public string ServiceTypeName { get; set; }

        /// <summary>
        /// 服务开关(0-关闭服务、1-开启服务)
        /// </summary>
        public int ServiceSwitch { get; set; }

        /// <summary>
        /// 服务价格
        /// </summary>
        public decimal ServicePrice { get; set; }

        /// <summary>
        /// 服务价格下线
        /// </summary>
        public decimal ServicePriceDown { get; set; }

        /// <summary>
        /// 服务价格上线
        /// </summary>
        public decimal ServicePriceUp { get; set; }
    }

    public class DoctorServiceSaveDto
    {
        /// <summary>
        /// 保存医生服务数据
        /// </summary>
        public List<DoctorPriceServiceDto> Data { get; set; }

        /// <summary>
        /// 保存医生服务数据(此参数为app提供)
        /// </summary>
        public string ServiceData { get; set; }

    }
}
