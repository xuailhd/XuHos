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
    /// 医生价格服务
    /// </summary>
    public partial class DoctorServiceDTO
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public EnumDoctorServiceType ServiceType { get; set; }

        /// <summary>
        /// 服务类型名称
        /// </summary>
        public string ServiceTypeName
        {
            get
            {
                return this.ServiceType.GetEnumDescript();
            }
        }

        /// <summary>
        /// 服务开关(0-关闭服务、1-开启服务)
        /// </summary>
        public int ServiceSwitch { get; set; }

        /// <summary>
        /// 服务价格
        /// </summary>
        public decimal ServicePrice { get; set; }

        public bool HasSchedule { get; set; }
    }
}
