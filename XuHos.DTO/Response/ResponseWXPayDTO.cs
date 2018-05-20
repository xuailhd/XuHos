using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ResponseWXPayDTO
    {
        public string signType { get; set; }

        public string appid
        { get; set; }

        public string partnerId
        { get; set; }

        public string nonce_str { get; set; }
        public string prepay_id { get; set; }

        public string sign { get; set; }


        public string package { get; set; }

        public string timeStamp { get; set; }
       

    }
}
