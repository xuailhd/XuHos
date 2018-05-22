using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Response
{
    /// <summary>
    /// 我的医生和医生收藏的查询结果合并到这个类
    /// </summary>
    public class ResponseUserCollectedDoctorDTO : ImageBaseDTO
    {
        public string OPDRegisterID { get; set; }
        public string DoctorAttentionID { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentID { get; set; }
        public string Gender { get; set; }
        string _Portait { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string Portait
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Portait))
                {
                    return PaddingUrlPrefix("images/doctor/default.jpg");
                }
                else
                {
                    return PaddingUrlPrefix(_Portait);
                }
            }
            set
            {
                _Portait = value;
            }
        }
        public string Position { get; set; }
        public bool? IsExpert { get; set; }
        public string Specialties { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 是否已关注
        /// </summary>
        public bool IsFollowed { get; set; }
    }
}
