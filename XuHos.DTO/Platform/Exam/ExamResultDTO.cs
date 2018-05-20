using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ExamResultDTO
    {
        /// <summary>
        /// 检查结果ID
        /// </summary>
        public string ExamResultID { get; set; }

        /// <summary>
        /// 检查类型ID
        /// </summary>
        public string ExamItemTypeID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 检查时间
        /// </summary>
        public DateTime ExamTime { get; set; }

        /// <summary>
        /// 检查结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 检查状态（0-正常、1-高于正常范围、-1-低于正常范围）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public string StatusMsg { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        public string ExamItemTypeName { get; set; }
        public string UnifiedUnit { get; set; }

        public string ExamItemTypeParentID { get; set; }

        public List<ExamResultDTO> SubExamResults { get; set; }

        public List<ExamResultAttachmentDTO> ExamResultAttachments { get; set; }

    }
}
