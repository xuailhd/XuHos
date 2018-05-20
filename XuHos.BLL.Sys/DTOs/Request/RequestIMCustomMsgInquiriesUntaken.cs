using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs
{
    public class RequestIMCustomMsgInquiriesUntaken : IRequestIMCustomMsg<Dictionary<int, long>>
    {
        public Dictionary<int, long> Data
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
                return EnumIMCustomMsgType.InquiriesUntaken.GetEnumDescript();
            }
        }
    }
}
