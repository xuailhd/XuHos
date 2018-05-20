using XuHos.Common;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace XuHos.Integration.ShortMessage
{
    public class SMSHelper
    {
        //public static void RegisterYunPian(Common.Config.Sections.SMSYunPian _config)
        //{
        //    YunPian.SMSHelper.Register(_config);
        //}

        //public static void RegisterYunZX(Common.Config.Sections.SMSYunZX _config)
        //{
        //    YunZX.SMSHelper.Register(_config);
        //}

        public static bool SendSMS(string toPhone, string templatedId, string param, int SMSVender, out string reasoon)
        {
            switch (SMSVender)
            {
                case 1:
                    return YunPian.SMSHelper.SendSMS(toPhone, templatedId, param, out reasoon);
                default:
                    return YunZX.SMSHelper.SendSMS(toPhone, templatedId, param, out reasoon);

            }
        }
    }

}
