using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Integration.WechatApp
{
    public class AuthTokeDTO
    {
        public string openid { get; set; }

        public string session_key { get; set; }

        public string unionid { get; set; }

        public string errcode { get; set; }

        public string errmsg { get; set; }
    }
}
