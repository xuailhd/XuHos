using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ExamResultAttachmentDTO
    {
        public string AttachmentID { get; set; }

        /// <summary>
        /// 检查结果ID
        /// </summary>
        public string ExamResultID { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
