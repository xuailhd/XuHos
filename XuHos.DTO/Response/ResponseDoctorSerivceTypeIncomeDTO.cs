using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{


    public class ResponseDoctorSerivceTypeIncomeDTO
    {
        /// <summary>
        /// 0-挂号、1-图文咨询、2-语音问诊、3-视频问诊、4-处方支付、5-家庭医生、6-会员套餐、7-远程会诊、8-影像判读、100-其它
        /// </summary>
        public int SerivceType { get; set; }
        public int TimesCount { get; set; }
        public decimal Income { get; set; }
    }
}
