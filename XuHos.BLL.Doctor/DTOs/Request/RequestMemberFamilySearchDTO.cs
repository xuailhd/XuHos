using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Common;
using XuHos.Common.Enum;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 家庭成员查询条件
    /// </summary>
    public class RequestFamilyMemberSearchDTO : RequestSearchCondition
    {

        public string FamilyDoctorID { get; set; }

        public string CurrentOperatorDoctorID { get; set; }

        public string MemberID { get; set; }

        public string UserMemberEMRID { get; set; }

        public bool IsContainsSelf { get; set; }

    }
}
