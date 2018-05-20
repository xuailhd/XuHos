using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XuHos.DTO.Common
{
    /// <summary>
    /// 用于下拉框等
    /// </summary>
    public class ResponseTextAndValue
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public object Data { get; set; }
    }
}