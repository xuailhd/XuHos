using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ExamItemDTO
    {
        /// <summary>
        /// 检查项目ID
        /// </summary>
        public string ExamItemID { get; set; }
        /// <summary>
        /// 检查类型ID
        /// </summary>
        public string ExamItemTypeID { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        public string MemberID { get; set; }
    }
    public class ExamItemWithResultDTO : ExamItemDTO
    {
        //public int AccountID { get; set; }
        public int DataType { get; set; }
        public string ExamItemTypeName { get; set; }
        public string UnifiedUnit { get; set; }
        public List<ExamResultDTO> Results { get; set; }
        public string LastResult { get; set; }
        public System.DateTime? LastExamTime { get; set; }
    }
}
