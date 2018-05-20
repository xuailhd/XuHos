using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Log.ApiTrack
{
    public interface ITrackLogLogAppender
    {
        void WriteLog(string CollectName, object log);
    }
}
