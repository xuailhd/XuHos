using XuHos.Common;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace XuHos.Integration.ShortMessage.YunZX
{
    internal enum EBodyType : uint
    {
        EType_XML = 0,
        EType_JSON
    };

    public class SMSHelper
    {
        static string serverIp = "api.ucpaas.com";
        static string serverPort = "443";
        static string account = "5479ec4b5e6af10c32300bbb331e80f0";    //用户sid
        static string token = "ca6251edc09cddad5ed4ad364e5af2e1";      //用户sid对应的token
        static string appId = "b5632048da80458c9c9a37517d8ef510";      //对应的应用id，非测试应用需上线使用
        //static string clientNum = "60000000000001";
        //static string clientpwd = "";
        //static string friendName = "";
        //static string clientType = "0";
        //static string charge = "0";
        //static string phone = "";
        //static string date = "day";
        //static uint start = 0;
        //static uint limit = 100;
        //static string toPhone = "13751071821";                                    //发送短信手机号码，群发逗号区分
        //static string templatedId = "1";                               //短信模板id，需通过审核
        //static string param = "a,b,c";                                     //短信参数
        //static string verifyCode = "1234";
        //static string fromSerNum = "4000000000";
        //static string toSerNum = "4000000000";
        //static string maxallowtime = "60";

        static UCSRestRequest api = new UCSRestRequest();
        //查询主账号
        //api.QueryAccountInfo();

        //申请client账号
        //api.CreateClient(friendName, clientType, charge, phone);

        //查询账号信息(账号)
        //api.QueryClientNumber(clientNum);

        //查询账号信息(电话号码)
        //api.QueryClientMobile(phone);

        //查询账号列表
        //api.GetClient(start, limit);

        //删除一个账号
        //api.DropClient(clientNum);

        //查询应用话单
        //api.GetBillList(date);

        //查询账号话单
        //api.GetClientBillList(clientNum, date);

        //账号充值
        //api.ChargeClient(clientNum, clientType, charge);

        //回拨
        //api.CallBack(clientNum, toPhone, fromSerNum, toSerNum, maxallowtime);

        //短信
        //api.SendSMS(toPhone, templatedId, param);

        //语音验证码
        //api.VoiceCode(toPhone, "1234");

        //public static void Register(Common.Config.Sections.SMSYunZX config)
        //{
        //    if(config != null && !string.IsNullOrEmpty(config.ServerIp))
        //    {
        //        api.init(config.ServerIp, config.ServerPort);
        //        api.setAccount(config.Account, config.Token);
        //        api.setAppId(config.AppId);
        //    }
        //    else
        //    {
        //        api.init(serverIp, serverPort);
        //        api.setAccount(account, token);
        //        //api.enabeLog(true);
        //        api.setAppId(appId);
        //        //api.enabeLog(true);
        //    }
        //}


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="toPhone">发送的手机号码</param>
        /// <param name="templatedId">短信模板ID</param>
        /// <param name="param">参数1，参数2，参数3，……</param>
        /// <returns></returns>
        public static bool SendSMS(string toPhone, string templatedId, string param, out string reasoon)
        {
            var responseResult = api.SendSMS(toPhone, templatedId, param);
            reasoon = "";
            if (responseResult.Contains("\"respCode\":\"000000\""))
            {
                return true;
            }
            else if (responseResult.Contains("\"respCode\":\"105147\""))
            {
                reasoon = $"【短信发送超频】Phone: {toPhone}, templatedId: {templatedId}, param: {param}";
                XuHos.Common.LogHelper.WriteError(new Exception(reasoon));
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 发送语音信息
        /// </summary>
        /// <param name="toPhone">发送的手机号码</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns></returns>
        public static string VoiceCode(string toPhone, string verifyCode)
        {
            return api.VoiceCode(toPhone, verifyCode);
        }
    }
}
