using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMEHosp.DTO
{
    /// <summary>
    /// 医生个人信息
    /// </summary>
    public class RequestDoctorPersonalInfoDTO
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 特长
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoUrl { get; set; }

    }
}
