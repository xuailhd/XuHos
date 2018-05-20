using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Pay
{
    internal static class TrackApiLog
    {
        public static void WriteLog(string payType, string requestUri, string comments, string RequestParamters, DateTime requestEnterTime, string Response)
        {
            XuHos.Common.LogHelper.WriteTrackLog("TrackPay" + payType + "ApiOperatorLog",
                requestUri: requestUri,
                comments: comments,
                RequestParamters: RequestParamters,
                requestEnterTime: requestEnterTime,
                Response: Response);
        }

    }
}
