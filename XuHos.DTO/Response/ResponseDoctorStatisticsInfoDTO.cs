using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    /// <summary>
    /// 医生个人信息
    /// </summary>
    public class ResponseDoctorStatisticsInfoDTO : Common.ImageBaseDTO
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        string _PhotoUrl { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PhotoUrl))
                {
                    return PaddingUrlPrefix("images/doctor/unknow.png");

                  
                }
                else
                {
                    return PaddingUrlPrefix(_PhotoUrl);
                }
            }
            set
            {
                _PhotoUrl = value;
            }
        }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal Income { get; set; }
        /// <summary>
        /// 服务次数
        /// </summary>
        public int ServiceTimes { get; set; }
        /// <summary>
        /// 关注量
        /// </summary>
        public int FollowedCount { get; set; }
        /// <summary>
        /// 评价量
        /// </summary>
        public int EvaluatedCount { get; set; }


        /// <summary>
        /// 问诊量
        /// </summary>
        public int DiagnoseTimes { get; set; }
        public int Sort { get; set; }

    }
}
