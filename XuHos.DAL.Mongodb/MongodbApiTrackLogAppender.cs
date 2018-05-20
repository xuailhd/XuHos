using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Log.ApiTrack;
using XuHos.Common;

namespace XuHos.DAL.Mongodb
{
    public class MongodbApiTrackLogAppender : XuHos.Common.Log.ApiTrack.ITrackLogLogAppender
    {
        public void WriteLog(string CollectName, object log)
        {
            try
            {
                XuHos.DAL.Mongodb.MongoDbHelper.Insert(CollectName, log);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }
    }
}