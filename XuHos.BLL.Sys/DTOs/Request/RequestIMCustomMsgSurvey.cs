using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    /// <summary>
    /// 单选题
    /// </summary>
    public class RadioTopic
    {
        public string Type { get { return "RadioTopic"; } }

        public List<string> Answer { get; set; }
    }

    public class RequestIMCustomMsgSurvey : IRequestIMCustomMsg<RadioTopic>
    {
        public RadioTopic Data
        {
            get; set;
        }

        public string Desc
        {
            get; set;
        }

        public string Ext
        {
            get
            {
                return EnumIMCustomMsgType.Survey_Question.GetEnumDescript();
            }
        }
    }
}
