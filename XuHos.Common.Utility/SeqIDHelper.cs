using XuHos.Common.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Utility
{
    public  class SeqIDHelper
    {
        static IdWorker worker = null;

        public static void RegisterSeqWorker(long workerId,long dataCenterId)
        {
            worker = new IdWorker(workerId, dataCenterId);
        }

        /// <summary>
        /// 获取18位长整型唯一编号
        /// </summary>
        /// <returns></returns>
        public static long GetSeqId()
        {
            if (worker != null)
                return worker.NextId();

            throw new ArgumentNullException("SeqWorker");
        }

        /// <summary>
        /// 获取流水号（长度大于18位）,格式：Prefix+SeqId
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public static string GetSeqNo(string Prefix)
        {
            var SeqID = GetSeqId();

            return $"{Prefix}{SeqID}";
        }
        
    }
}
