using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Pay.KMPay.Model
{
    /// <summary>
    /// 退款结果实体类
    /// </summary>
    internal class RefundResult
    {
        public string code { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
        public RefundResultData data { get; set; }

        //public string params{get;set;}

    }

    internal class RefundResultData
    {
        public string batchNo { get; set; }
        public string successNum { get; set; }
        public string resultDetails { get; set; }
        public string sign { get; set; }
    }

}
