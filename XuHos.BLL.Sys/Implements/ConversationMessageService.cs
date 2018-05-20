using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Entity;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using XuHos.Extensions;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.Implements
{
    /// <summary>
    /// 会话消息业务逻辑
    /// </summary>
    public class ConversationMessageService : BLL.Common.CommonBaseService<XuHos.Entity.ConversationMessage>

    {
        public ConversationMessageService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }

        public Response<List<DTO.ConversationMessageReturnDTO>> GetMessages(int ChannelID,int CurrentPage,int PageSize)
        {

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                int totalCount = 0;
                Response<List<DTO.ConversationMessageReturnDTO>> result = new Response<List<DTO.ConversationMessageReturnDTO>>();

                var query = from message in db.Set<ConversationMessage>().AsQueryable().Where(a => a.IsDeleted == false)
                            where message.ConversationRoomID == ChannelID
                            group message by message.MessageSeq into gps
                            select new DTO.ConversationMessageReturnDTO()
                            {
                                MsgSeq = gps.Key,                               
                                FromAccount = gps.FirstOrDefault().UserID,
                                ToGroupId = gps.FirstOrDefault().ConversationRoomID.ToString(),
                                MsgTime = gps.FirstOrDefault().MessageTime,
                                MsgBody = gps.OrderBy(a=>a.MessageIndex).Select(a => a.MessageContent).ToList(),
                            };

                query = query.OrderBy(a => new { a.MsgTime });


                var fTotal = query.FutureCount();
                var fList = query.Skip((CurrentPage - 1) * PageSize).Take(PageSize).Future();
                totalCount = fTotal.Value;
                result.Data = fList.ToList();
                result.Total = totalCount;
          
                return result;

            }
        }

    }
}
