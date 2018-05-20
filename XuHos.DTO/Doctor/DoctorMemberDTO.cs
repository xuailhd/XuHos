using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.DTO.Common;

namespace XuHos.DTO
{
    /// <summary>
    /// 医生的患者
    /// </summary>
    public class DoctorMemberDTO
    {

        public string DoctorMemberID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        public string MemberID { get; set; }

        public string MemberName { get; set; }

        public EnumUserGender Gender { get; set; }
        public string GenderName { get; set; }
        public string Birthday { get; set; }
        public string Mobile { get; set; }
        public decimal ConsulServicePrice { get; set; }
        public string DoctorName { get; set; }

        int _Age;
        public int Age
        {
            get
            {
                if (_Age > 0)
                {
                    return _Age;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Birthday))
                    {
                        DateTime dt;

                        if (DateTime.TryParse(Birthday, out dt) == true)
                        {
                            _Age = DateTime.Now.Year - dt.Year;
                            if (DateTime.Now.Month >= dt.Month && DateTime.Now.Day >= dt.Day)
                            {
                                _Age += 1;
                            }
                            return _Age;
                        }
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            set
            {
                _Age = value;
            }
        }
    }

    /// <summary>
    /// 查询类
    /// </summary>
    public class DoctorMemberCondition : RequestSearchCondition
    {
        public string DoctorMemberID { get; set; }

        public string MemberID { get; set; }
        public string UserID { get; set; }
  
        public string PatientName { get; set; }

        public string DoctorID { get; set; }
    }
}
