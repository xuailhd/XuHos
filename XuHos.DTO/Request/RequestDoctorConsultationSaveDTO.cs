using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Request
{
    /// <summary>
    /// 医生会诊，字段与表单对应
    /// </summary>
    public class RequestDoctorConsultationSaveDTO
    {
        /// <summary>
        /// 医生会诊ID
        /// </summary>
        public string ConsultationID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 会诊主题
        /// </summary>
        public string ConsultationSubject { get; set; }

        /// <summary>
        /// 会诊内容
        /// </summary>
        public string ConsultationContent { get; set; }

        /// <summary>
        /// 提醒患者内容
        /// </summary>
        public string ConsultationRemind { get; set; }

        /// <summary>
        /// 预计会诊时间
        /// </summary>
        public DateTime? ConsultationDate { get; set; }

        /// <summary>
        /// 会诊开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 会诊完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        ///  会诊费用
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        ///  会诊结果
        /// </summary>
        public string ConsultationResult { get; set; }

        /// <summary>
        ///  现病史
        /// </summary>
        public string PresentHistoryIllness { get; set; }

        /// <summary>
        /// 既往病史
        /// </summary>
        public string PastMedicalHistory { get; set; }

        /// <summary>
        ///  既往手术史
        /// </summary>
        public string PastOperatedHistory { get; set; }

        /// <summary>
        /// 家族史 
        /// </summary>
        public string FamilyMedicalHistory { get; set; }

        /// <summary>
        /// 诊断检查
        /// </summary>
        public string DiagnosticTests { get; set; }

        /// <summary>
        /// 病案号
        /// </summary>
        public string MedicalRecordNo { get; set; }

        /// <summary>
        ///  会诊状态(0-待支付、1-待开始、2-进行中、3-已完成)
        /// </summary>
        public EnumConsultationStatus ConsultationStatus { get; set; }

        /// <summary>
        /// 会诊医生ID列表
        /// </summary>
        public List<string> DoctorIDList { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
