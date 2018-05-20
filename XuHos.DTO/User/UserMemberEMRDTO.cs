using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class UserMemberEMRDTO
    {
        /// <summary>
        /// 患者电子病历ID
        /// </summary>
        public string UserMemberEMRID { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 医生患者ID
        /// </summary>
        public string DoctorMemberID { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 病历名称
        /// </summary>
        [MaxLength(64)]
        public string EMRName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        [MaxLength(128)]
        public string HospitalName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [MaxLength(1024)]
        public string Remark { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual List<UserFileDTO> Files { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }

    public class UserMemeberQueryDTO
    {
        public string MemberId { get; set; }

        public string DoctorMemberID { get; set; }

        private int _currentPage;
        public int CurrentPage
        {
            get
            {
                if(_currentPage==0)
                {
                    return 1;
                }
                else
                {
                    return _currentPage;
                }
            }
            set
            {
                _currentPage = value;
            }
        }
        private int _pageSize;
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    return 10;
                }
                else
                {
                    return _pageSize;
                }
            }
            set
            {
                _pageSize = value;
            }
        }
        

        public string Keyword { get; set; }
    }
}
