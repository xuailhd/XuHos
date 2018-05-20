using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Response
{
    public class ResponseServiceItemStatDTO
    {
        public int SerivceType { get; set; }
        public int TimesCount { get; set; }
        public decimal Income { get; set; }

        public string SerivceTypeName {
            get {
                string ret = "";
                switch(this.SerivceType)
                {
                    case 0: ret = "挂号"; break;
                    case 1: ret = "图文咨询"; break;
                    case 2: ret = "语音咨询"; break;
                    case 3: ret = "视频咨询"; break;
                    case 5: ret = "家庭医生"; break;
                    case 7: ret = "远程会诊"; break;
                }
                return ret;
            }
        }
    }
    public class ResponseDoctorServiceItemStatDTO : ResponseServiceItemStatDTO
    {
        public string DoctorName { get; set; }
    }
}
