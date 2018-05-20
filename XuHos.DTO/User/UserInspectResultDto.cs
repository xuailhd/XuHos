using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace XuHos.DTO
{
    public class UserInspectResultDto
    {
        /// <summary>
        /// 检查结果ID
        /// </summary>
        public string InspectResultID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        [Required]
        public string MemberID { get; set; }

        /// <summary>
        /// 检查类型
        /// </summary>
        [Required]
        public string InspectType { get; set; }

        /// <summary>
        /// 检查部位
        /// </summary>
        [Required]
        public string InspectPoint { get; set; }

        /// <summary>
        /// 检查日期
        /// </summary>
        //[Required]
        public string InspectDate { get; set; }

        /// <summary>
        /// 医生建议
        /// </summary>
        //[Required]
        public string DoctorSuggest { get; set; }

        /// <summary>
        /// 上传文件名称
        /// </summary>
        //[Required]
        public string FileUploadName { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// 案例ID
        /// </summary>
        //[Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CaseID { get; set; }

        public string StudyID { get; set; }

        public string StuUID { get; set; }

        public UserMemberDTO UserMember { get; set; }

        public string ImgMD5 { get; set; }

        public string UserId { get; set; }
    }

    public class DcmDTO : ResponseUploadFileDTO
    {
        public string CaseID { get; set; }
        public string StudyID { get; set; }
        public string StuUID { get; set; }
    }
}
