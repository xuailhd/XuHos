using XuHos.BLL.Doctor.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    public class ResponseDoctorServiceSettingsDTO
    {
        /// <summary>
        /// 保存医生服务数据
        /// </summary>
        public List<ResponseDoctorServicePriceDTO> Data { get; set; } 
        
        /// <summary>
        /// 当前时间是否处于停诊（APP用）
        /// </summary>
        public object StopDiagnosis { get; set; }

    }
}
