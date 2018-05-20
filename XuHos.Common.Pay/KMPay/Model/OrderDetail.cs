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
    internal class OrderResult
    {
        public string code { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
        public OrderDetail data { get; set; }
    }

    internal class OrderDetail
    {
        public string outTradeNo { get; set; }
        public string tradeNo { get; set; }
        public string orderStatus { get; set; }
        public string sellerId { get; set; }
        public decimal totalAmount { get; set; }
        public string sign { get; set; }
    }
}
