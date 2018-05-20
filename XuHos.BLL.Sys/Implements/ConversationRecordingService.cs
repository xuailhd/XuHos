using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Entity;
using System.Linq.Expressions;
namespace XuHos.BLL.Sys.Implements
{
    /// <summary>
    /// 会话消息业务逻辑
    /// </summary>
    public class ConversationRecordingService : BLL.Common.CommonBaseService<XuHos.Entity.ConversationRecording>       
    {

        public ConversationRecordingService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }



    }
}
