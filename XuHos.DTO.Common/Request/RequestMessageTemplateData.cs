using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common.Request
{

    /// <summary>
    /// 消息数据，替换消息占位符
    /// </summary>
    public class RequestMessageTemplateData
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string HospName { get; set; }

        public string DrugStoreName { get; set; }

        public string ServiceTypeName { get; set; }

        /// <summary>
        /// 修改模板中的字符串，格式："原字符串\t新字符串"，多个用"\n"分割
        /// </summary>
        public string Replace { get; set; }
    }
}
