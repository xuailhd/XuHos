using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Response
{
    public class ResponseUserOPDRegisterDTO
    {
        /// <summary>
        /// 预约登记ID
        /// </summary>
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        [Required]
        /// <summary>
        /// 排班ID
        /// </summary>
        public string ScheduleID { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime RegDate { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime OPDDate { get; set; }


        [Required]
        /// <summary>
        /// 预约类型（0-挂号、1-图文、2-语音、3-视频）
        /// </summary>
        public EnumDoctorServiceType OPDType { get; set; }


        /// <summary>
        /// 预约金额
        /// </summary>
        public decimal Fee { get; set; }


        [Required]
        public string MemberID { get; set; }

        /// <summary>
        /// 成员姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }

        /// <summary>
        /// 婚姻情况(0-未婚、1-已婚、2-未知)
        /// </summary>
        public EnumUserMaritalStatus Marriage { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 证件类型（0-身份证）
        /// </summary>
        public EnumUserCardType IDType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public string OrgnazitionID { get; set; }


        public string OrgName { set; get; }

        public EnumOrgType? OrgType { set; get; }

        public string RecipeFileID { get; set; }

        public DTO.Platform.OrderDTO Order { get; set; }

        public DTO.UserMemberDTO Member { get; set; }

        public DTO.UserDTO User { get; set; }

        public DTO.DoctorDto Doctor { get; set; }

        public DTO.ConversationRoomDTO Room { get; set; }

        public string ConsultContent { get; set; }

        /// <summary>
        /// 处方文件路径
        /// </summary>
        public string RecipeFileUrl { get; set; }


        /// <summary>
        /// 就诊开始时间
        /// </summary>    
        public string OPDBeginTime { get; set; }

        /// <summary>
        /// 就诊结束时间
        /// </summary>

        public string OPDEndTime { get; set; }

        /// <summary>
        /// 问诊疾病
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(128)]
        public string ConsultDisease { get; set; }

    }
}
