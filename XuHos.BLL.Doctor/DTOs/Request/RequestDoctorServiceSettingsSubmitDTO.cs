using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生服务设置提交
    
    /// 日期：2017年4月19日
    /// </summary>
    public class RequestDoctorServiceSettingsSubmitDTO
    {
        /// <summary>
        /// 保存医生服务数据
        /// </summary>
        public List<RequestDoctorServiceSettingDTO> Data { get; set; }

        /// <summary>
        /// 保存医生服务数据(此参数为app提供)
        /// </summary>
        public string ServiceData { get; set; }

        /// <summary>
        /// 义诊设置
        /// </summary>
        public RequestDoctorClinicSettingDTO FreeClinicSetting { get; set; }


        public class RequestDoctorClinicSettingDTO
        {

            /// <summary>
            /// 每天接受咨询次数
            /// </summary>
            public int AcceptCount { get; set; }

            /// <summary>
            /// 状态(0-关闭、1-开通)
            /// </summary>
            public bool State { get; set; }
            /// <summary>
            /// 义诊月份
            /// </summary>
            public string ClinicMonth { get; set; } = DateTime.Now.ToString("yyyyMM");

        }

        /// <summary>
        /// 医生服务设置
        /// </summary>
        public class RequestDoctorServiceSettingDTO
        {
            public RequestDoctorServiceSettingDTO()
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
    }
}
