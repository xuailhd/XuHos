using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Entity;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using XuHos.DTO;
using XuHos.BLL.Sys;
using XuHos.DAL.EF;
using XuHos.Common.Cache;
using XuHos.Extensions;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.BLL.Sys.DTOs.Response;
using XuHos.Common.Cache.Keys;
using XuHos.EventBus;
using XuHos.EventBus.Events;
using XuHos.BLL.Common.DTOs.Request;
using XuHos.BLL.Common.DTOs.Response;
using System.Data.Entity.Infrastructure;
using XuHos.DTO.Platform;

namespace XuHos.BLL.Sys.Implements
{
    /// <summary>
    /// 房间相关业务逻辑
    /// </summary>
    public class ConversationRoomService
    {

        #region Query

        public bool ChannelExists(int ChannelID)
        {

            if (GetChannelInfo(ChannelID) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ChannelExists(string ServiceID)
        {
            if (GetChannelInfo(ServiceID) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 频道下存在某用户
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ChannelUserExist(int ChannelID, string UserId)
        {
            var result = GetChannelUsersInfo(ChannelID);
            return result.Where(a => a.UserID == UserId).Count() > 0;
        }

        /// <summary>
        /// 频道下存在某用户
        /// </summary>
        /// <param name="ServiceID"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ChannelUserExist(string ServiceID, string UserId)
        {
            var ChannelID = GetChannelIDByServiceID(ServiceID);
            var result = GetChannelUsersInfo(ChannelID);
            return result.Where(a => a.UserID == UserId).Count() > 0;
        }


        /// <summary>
        /// 获取多个用户的信息
        /// </summary>
        /// <param name="Identifiers"></param>
        /// <returns></returns>
        public List<ResponseConversationRoomMemberDTO> GetChannelUsersInfo(int ChannelID, params int[] Identifiers)
        {
            var result = GetChannelUsersInfo(ChannelID);

            if (Identifiers!=null && Identifiers.Length > 0)
            {
                return result.Where(a => Identifiers.Contains(a.identifier)).ToList();
            }
            else
            {
                return result;
            }
        }


        /// <summary>
        /// 获取多个用户的信息
        /// </summary>
        /// <param name="Identifiers"></param>
        /// <returns></returns>
        public List<ResponseConversationRoomMemberDTO> GetChannelUsersInfo(int ChannelID)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<ResponseConversationRoomMemberDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Channel_Member, ChannelID.ToString());
            var result = cacheKey.FromCache();
            if (result == null)
            {
                using (DAL.EF.DBEntities db = new DBEntities())
                {
                    result = (
                         from roomMember in db.ConversationRoomUids
                         join room in db.ConversationRooms on roomMember.ConversationRoomID equals room.ConversationRoomID
                         where room.ChannelID==ChannelID
                        select new ResponseConversationRoomMemberDTO
                        {
                            identifier = roomMember.Identifier,
                            UserCNName =roomMember.UserCNName,
                            UserENName = roomMember.UserENName,
                            PhotoUrl = roomMember.PhotoUrl,
                            UserType = roomMember.UserType,
                            UserID = roomMember.UserID,
                        }).ToList();

                        result.ToCache(cacheKey,TimeSpan.FromHours(2));
                }
            }
            return result;
        }

        /// <summary>
        /// 获取所有好友
        
        /// 日期：2017年6月26日
        /// </summary>
        /// <param name="FromUserID"></param>
        public List<RespnoseConversactionFriendDTO> GetAllFriends(string UserID)
        {
            using (DBEntities db = new DBEntities())
            {
                var query = from friend in db.ConversationFriends.Where(a => a.FromUserID == UserID)
                            join channel in db.ConversationRooms on friend.ConversationRoomID equals channel.ConversationRoomID
                            join uid in db.ConversationRoomUids on new { Identifier=friend.ToUserIdentifier, ConversationRoomID=friend.ConversationRoomID } equals new { Identifier=uid.Identifier, ConversationRoomID=uid.ConversationRoomID}
                            join user in db.Users on uid.UserID equals user.UserID
                            join member in db.UserMembers on uid.UserMemberID equals member.MemberID into leftJoinMember
                            from memberIfEmpty in leftJoinMember.DefaultIfEmpty()
                            select new RespnoseConversactionFriendDTO
                            {
                                ChannelID = channel.ChannelID,
                                AddWording = friend.AddWording,
                                GroupName = friend.GroupName,
                                Identifier = friend.ToUserIdentifier,
                                Remark = friend.Remark,
                                UserID = friend.ToUserID,
                                NickName = memberIfEmpty!=null? memberIfEmpty.MemberName:user.UserCNName,
                                Avatar = user.PhotoUrl
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// 获取所有会话
        
        /// 日期：2017年6月26日
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<ResponseConversactionSessionDTO> GetSessions(string UserID, DateTime LastTime, EnumRoomType? SessionType)
        {
            using (DBEntities db = new DBEntities())
            {
                //查询出最后的消息
                var queryChannels = from friend in db.ConversationFriends.Where(a => a.FromUserID == UserID)
                                    join channel in db.ConversationRooms on friend.ConversationRoomID equals channel.ConversationRoomID
                                    join message in db.ConversationMessages.Where(a => a.MessageTime >= LastTime) on channel.ChannelID equals message.ConversationRoomID
                                    where (!SessionType.HasValue) || (channel.RoomType == SessionType.Value)
                                    group message by channel.ConversationRoomID into gps
                                    select new { ConversationRoomID = gps.Key };

                //查询频道的永不编号（不包括自己）
                var queryChannelUserIds = from room in queryChannels
                                          join roomUid in db.ConversationRoomUids on room.ConversationRoomID equals roomUid.ConversationRoomID
                                          where roomUid.UserID != UserID
                                          select new {
                                              ConversationRoomID = roomUid.ConversationRoomID,
                                              UserID = roomUid.UserID
                                          };

                //频道的用户信息（会话列表显示名字和头像，显示对方的）
                var queryChannelUserInfo = from roomUid in db.ConversationRoomUids
                                           join channel in queryChannelUserIds on new { roomUid.UserID, roomUid.ConversationRoomID } equals new { channel.UserID, channel.ConversationRoomID }
                                           join user in db.Users on roomUid.UserID equals user.UserID
                                           join member in db.UserMembers on user.UserID equals member.UserID
                                           select new
                                           {
                                               channel.ConversationRoomID,
                                               Avatar = user.PhotoUrl,
                                               NickName = member.MemberName
                                           };

                var queryLastMessage = from friend in db.ConversationFriends.Where(a => a.FromUserID == UserID)
                                       join channel in db.ConversationRooms on friend.ConversationRoomID equals channel.ConversationRoomID
                                       join message in db.ConversationMessages.Where(a => a.MessageTime >= LastTime) on channel.ChannelID equals message.ConversationRoomID
                                       where (!SessionType.HasValue) || (channel.RoomType == SessionType.Value)
                                       group message by message.ConversationRoomID into gps
                                       select new {
                                           ConversationRoomID = gps.Key,
                                           MsgID = gps.Max(d => d.MessageTime)
                                       };


                var query = from lastMessage in queryLastMessage
                            join channel in db.ConversationRooms on lastMessage.ConversationRoomID equals channel.ChannelID
                            join user in queryChannelUserInfo on channel.ConversationRoomID equals user.ConversationRoomID
                            join message in db.ConversationMessages on new {
                                ConversationRoomID=lastMessage.ConversationRoomID,
                                lastMessage.MsgID } equals new {
                                    ConversationRoomID=message.ConversationRoomID,
                                    MsgID=message.MessageTime
                                }
                            orderby message.MessageTime descending
                            select new ResponseConversactionSessionDTO()
                            {
                                MessageContent = message.MessageContent,
                                MessageTime = message.MessageTime,
                                ChannelID = channel.ChannelID,
                                ServiceID = channel.ServiceID,
                                ServiceType = channel.ServiceType,
                                NickName = user.NickName,
                                Avatar = user.Avatar
                            };

                var list = query.ToList();
                List<ResponseConversactionSessionDTO> result = new List<ResponseConversactionSessionDTO>();
                list.ForEach(a =>
                {
                    if (!result.Any(b=>b.ChannelID==a.ChannelID))
                    {
                        result.Add(a);
                    }
                });
            
                return result;
            }
        }

        /// <summary>
        /// 获取房间信息
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public DTO.ConversationRoomDTO GetChannelInfo(int ChannelID)
        {
            if (ChannelID > 0)
            {

                var cacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.ConversationRoomDTO>(StringCacheKeyType.Channel, ChannelID.ToString());
                DTO.ConversationRoomDTO room = cacheKey.FromCache();
                if (room == null)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        var model = db.ConversationRooms.Where(a => a.ChannelID == ChannelID).FirstOrDefault();
                        if (model != null)
                        {
                            room = model.Map<Entity.ConversationRoom, DTO.ConversationRoomDTO>();
                            room.ToCache(cacheKey,TimeSpan.FromHours(1));                            
                        }
                    }
                }
                return room;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取房间信息
        /// </summary>
        /// <param name="ServiceID"></param>
        /// <returns></returns>
        public DTO.ConversationRoomDTO GetChannelInfo(string ServiceID)
        {
            var ChannelID = GetChannelIDByServiceID(ServiceID);
            if (ChannelID > 0)
                return GetChannelInfo(ChannelID);
            else
                return null;
        }

  
        /// <summary>
        /// 根据服务编号获取频道编号
        
        /// 日期：2017年4月21日
        /// </summary>
        /// <param name="ServiceID"></param>
        /// <returns></returns>
        public int GetChannelIDByServiceID(string ServiceID)
        {
            if (ServiceID != "")
            {
                var cacheKey_ChannelID = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetChannelIDByServiceID, ServiceID);
                int? channelID = cacheKey_ChannelID.FromCache<int?>();
                if (channelID == null)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        var model = db.ConversationRooms.Where(a => a.ServiceID == ServiceID).Select(a => new { a.ChannelID }).FirstOrDefault();
                        if (model != null)
                        {
                            channelID = model.ChannelID;
                            channelID.ToCache(cacheKey_ChannelID,TimeSpan.FromHours(1));                            
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }

                return channelID.Value;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 查询用户前面还有多少人在候诊
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <param name="DoctorID">医生</param>
        /// <param name="ChannelID">房间编号</param>
        /// <returns></returns>
        public int GetWaitingCount(string DoctorID, int ChannelID)
        {

            DateTime today = DateTime.Now;
            int Year = today.Year;
            int Month = today.Month;
            int Day = today.Day;
            long TriageID = 0;

            if (ChannelID > 0)
            {
                var roomInfo = GetChannelInfo(ChannelID);
                TriageID = roomInfo.TriageID;
            }

            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = from room in db.Set<Entity.ConversationRoom>().Where(room => 
                                       !room.IsDeleted
                                       && room.RoomState!= EnumRoomState.AlreadyVisit
                                       && (room.TriageID != long.MaxValue && ( room.TriageID < TriageID || TriageID == 0))
                                       && (room.ServiceType == EnumDoctorServiceType.AudServiceType || room.ServiceType == EnumDoctorServiceType.VidServiceType)
                                    
                            
                                   )
                            join opd in db.Set<Entity.UserOPDRegister>().Where(opd =>
                                                                                //opd.DoctorID == DoctorID &&
                                                                                opd.OPDDate.Year == Year &&
                                                                                opd.OPDDate.Month == Month &&
                                                                                opd.OPDDate.Day == Day)
                                                                                on room.ServiceID equals opd.OPDRegisterID
                            join order in db.Orders on room.ServiceID equals order.OrderOutID
                            where order.OrderState == EnumOrderState.Paid
                            select room.ChannelID;

                    //查询排前面的患者数量
                    return query.Count();
            }
        }

     

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="orderOutID"></param>
        /// <returns></returns>
        public EnumOrderState? GetOrderState(string orderOutID)
        {
            DTO.Platform.OrderDTO order = null;

            var cacheKey_orderNoOutIDMap = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetOrderNoByOrderOutID, orderOutID);
            string orderNo = cacheKey_orderNoOutIDMap.FromCache<string>();
            
            if (orderNo != null)
            {
                var cacheKey_order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, orderNo);
                order = cacheKey_order.FromCache();
            }

            if (order != null)
                return order.OrderState;

            //缓存没有命中，从数据库获取
            using (DBEntities db = new DBEntities())
            {
                Order entity = db.Set<Order>().Where(x => x.OrderOutID == orderOutID).FirstOrDefault();

                if (entity == null)
                    return null;

                return entity.OrderState;
            }
            
        }

        #endregion


        #region Command

        /// <summary>
        /// 发送候诊队列通知
        /// </summary>
        /// <param name="DoctorID">医生编号</param>
        /// <returns>发送通知的数量</returns>
        public int SendWaitingQueueChangeNotice(string DoctorID)
        {
            int result = -1;

            DateTime today = DateTime.Now;
            int Year = today.Year;
            int Month = today.Month;
            int Day = today.Day;

            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                //查询排前面的患者数量
                var queue = (from room in db.Set<Entity.ConversationRoom>().Where(a =>                
                    !a.IsDeleted
                    && a.RoomState != EnumRoomState.AlreadyVisit
                    && (a.TriageID != long.MaxValue)
                    && (a.ServiceType == EnumDoctorServiceType.AudServiceType || a.ServiceType == EnumDoctorServiceType.VidServiceType))           
                             join opd in db.Set<Entity.UserOPDRegister>().Where(opd =>
                             //opd.DoctorID== DoctorID && 
                             (opd.OPDDate.Year == Year && opd.OPDDate.Month == Month && opd.OPDDate.Day == Day))
                             on room.ServiceID equals opd.OPDRegisterID
                             join order in db.Orders on room.ServiceID equals order.OrderOutID
                             where order.OrderState == EnumOrderState.Paid
                             orderby
                             room.TriageID ascending
                             select room.ChannelID).ToList();

                if (queue.Count > 0)
                {
                    result = 0;
                }

                var imService = new XuHos.Integration.QQCloudy.IMHelper();

                for (var i = 0; i < queue.Count; i++)
                {
                    #region 通知其他候诊人员候诊人数有编号
                    var uidService = new BLL.Sys.Implements.ConversationIMUidService("");
                    var DoctorUid = uidService.GetDoctorIMUid(DoctorID);

                    //发送实时消息，告诉用户处方订单已生成，用户可以在线支付
                    if (imService.SendGroupCustomMsg(queue[i], DoctorUid, new BLL.Sys.DTOs.Request.RequestCustomMsgQueueChanged()
                    {
                        Data = i,
                        Desc = "您前面有" + i + "位患者，请等待医生呼叫"
                    }))
                    {
                        result++;
                    }
                    #endregion
                }

            }

            return result;
        }


        /// <summary>
        /// 批量将正在候诊状态的用户修改成离开
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public void Disconnection(string UserID)
        {
            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var roomList = (from room in db.ConversationRooms.Where(a =>   
                    (a.RoomState!= EnumRoomState.Disconnection &&
                    a.RoomState!= EnumRoomState.NoTreatment &&
                    a.RoomState!= EnumRoomState.AlreadyVisit)          
                   && (a.ServiceType == EnumDoctorServiceType.AudServiceType ||
                   a.ServiceType == EnumDoctorServiceType.VidServiceType))
                                join opd in db.UserOpdRegisters.Where(a => a.UserID == UserID)
                                on room.ServiceID equals opd.OPDRegisterID
                                select new {
                                    room.ChannelID,
                                    room.RoomState,
                                    room.DisableWebSdkInteroperability
                                }).ToList();

              
                foreach (var room in roomList)
                {
                    var ExpectedState = room.RoomState;

                    CompareAndSetChannelState(room.ChannelID, UserID, EnumRoomState.Disconnection,room.DisableWebSdkInteroperability, ref ExpectedState);
                }
            }
        }


        /// <summary>
        /// 替换频道成员
        
        /// 日期：2017年4月23日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public bool ReplaceChannelMembers(int ChannelID, List<RequestChannelMemberDTO> members)
        {
            using (DBEntities db = new DBEntities())
            {
                var room = GetChannelInfo(ChannelID);
                var ConversationRoomID = room.ConversationRoomID;
                
                db.ConversationRoomUids.Where(a => a.ConversationRoomID == ConversationRoomID).Delete();

                foreach (var member in members)
                {
                    var roomUid = new ConversationRoomUid()
                    {
                        ConversationRoomID= ConversationRoomID,
                        ChannelID = ChannelID,
                        UserType = member.UserType,
                        Identifier = member.Identifier,
                        UserMemberID=member.UserMemberID.IfNull(""),    
                        UserCNName=member.UserCNName.IfNull(""),
                        UserENName=member.UserENName.IfNull(""),
                        PhotoUrl=member.PhotoUrl.IfNull(""),
                        UserID =member.UserID                        
                    };
                    db.ConversationRoomUids.Add(roomUid);
                }

                if (db.SaveChanges() > 0)
                {
                    var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<ResponseConversationRoomMemberDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Channel_Member, ChannelID.ToString());
                    cacheKey.RemoveCache();

                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }


        /// <summary>
        /// 新增频道成员
        
        /// 日期：2017年4月23日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public bool InsertChannelMembers(int ChannelID, List<RequestChannelMemberDTO> members)
        {
            using (DBEntities db = new DBEntities())
            {
                var room = GetChannelInfo(ChannelID);
                var ConversationRoomID = room.ConversationRoomID;

                foreach (var member in members)
                {
                    if (!db.ConversationRoomUids.Where(a => a.ConversationRoomID == ConversationRoomID && a.Identifier==member.Identifier).Any())
                    {
                        var roomUid = new ConversationRoomUid()
                        {
                            ConversationRoomID = ConversationRoomID,
                            ChannelID = ChannelID,
                            UserType = member.UserType,
                            Identifier = member.Identifier,
                            UserMemberID = member.UserMemberID.IfNull(""),
                            UserID = member.UserID,
                            UserCNName = member.UserCNName.IfNull(""),
                            UserENName = member.UserENName.IfNull(""),
                            PhotoUrl = member.PhotoUrl.IfNull(""),
                        };
                        db.ConversationRoomUids.Add(roomUid);
                    }
                }

                if (db.SaveChanges() >= 0)
                {
                    var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<ResponseConversationRoomMemberDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Channel_Member, ChannelID.ToString());
                    cacheKey.RemoveCache();

                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// 比较并设置频道信息(已增加同步锁)
        
        /// 日期：2017年6月22日
        /// </summary>
        /// <param name="room"></param>
        /// <param name="Version"></param>
        /// <returns></returns>
        public bool CompareAndSetChannelInfo(DTO.ConversationRoomDTO room, byte[] Version=null)
        {          
            try
            {
                XuHos.DAL.EF.Base.Repository<ConversationRoom> helper = new DAL.EF.Base.Repository<ConversationRoom>();

                //更新成功
                return helper.Update(room.Map<DTO.ConversationRoomDTO, ConversationRoom>());
            }
            //并发更新异常，交给调用一侧去更新
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
            }                
            finally
            {
                var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<ResponseConversationRoomMemberDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Channel,room.ChannelID.ToString());
                cacheKey.RemoveCache();

            }


            return false;

        }

 
        /// <summary>
        /// 比较并设置房间状态
        
        /// 日期：2017年6月22日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public EnumApiStatus CompareAndSetChannelState(int ChannelID, string FromUserID, EnumRoomState State, bool DisableWebSdkInteroperability,ref EnumRoomState ExpectedState)
        {
   
            try
            {
                var room = GetChannelInfo(ChannelID);

                #region 校验：房间已标记为失效的，不可设置房间状态
                if (room.Close)
                {
                    return EnumApiStatus.BizChannelSetStateIfClose;
                }

                // 订单已取消房间也定义为失效
                var orderState = GetOrderState(room.ServiceID);
                if (orderState.HasValue && orderState.Value == EnumOrderState.Canceled)
                {
                    return EnumApiStatus.BizChannelSetStateIfClose;
                }
                
                #endregion

                #region 校验：房间状态未启用
                if (!room.Enable)
                {
                    //频道未就绪（客户端重试）
                    return EnumApiStatus.BizChannelNotReady;
                }
                #endregion

                #region 校验：当前房间状态和预期的房间状态一致才允许设置

                #region 处理特殊状态情况 客户端没有WaitAgain 状态需要统一
                if (ExpectedState == EnumRoomState.Waiting && room.RoomState == EnumRoomState.WaitAgain)
                {
                    ExpectedState = room.RoomState;
                }
                #endregion

                if (room.RoomState != ExpectedState)
                {
                    //返回新的状态给客户端，客户端需要同步
                    ExpectedState = room.RoomState;

                    //前端用户只需要知道有候诊状态即可，不需要知道有重复候诊的状态
                    if (ExpectedState == EnumRoomState.WaitAgain)
                    {
                        ExpectedState = EnumRoomState.Waiting;
                    }

                    //当前状态不是预期状态
                    return EnumApiStatus.BizChannelRejectSetStateIfNotExpectedState;
                }
                #endregion

                #region 校验：只能在预约时间内进入诊室或提前30分钟进入诊室
                if ((State == EnumRoomState.WaitAgain || State == EnumRoomState.Waiting))
                {
                    if (DateTime.Now <= room.BeginTime.AddMinutes(-30))
                    {
                        return EnumApiStatus.BizChannelRejectConnectIfNoReservationTime;
                    }
                }
                #endregion

                #region 房间状态切换的规则
                switch (room.RoomState)
                {
                    case EnumRoomState.AlreadyVisit:
                        {
                            //就诊已经结束不能在设置状态
                            return EnumApiStatus.BizOK;
                        }
                    case EnumRoomState.NoTreatment:
                        {
                            if (State == EnumRoomState.NoTreatment)
                            {
                                State = EnumRoomState.NoTreatment;
                            }
                            else if (State == EnumRoomState.InMedicalTreatment)
                            {
                                State = EnumRoomState.InMedicalTreatment;
                            }
                            else
                            {
                                State = EnumRoomState.Waiting;
                            }
                            break;
                        }
                    case EnumRoomState.Waiting:
                        {                          
                           
                            //重试
                            if (State == EnumRoomState.Waiting || State == EnumRoomState.WaitAgain)
                            {
                                State = EnumRoomState.Waiting;
                            }
                            //医生呼叫
                            else if (State == EnumRoomState.Calling)
                            {
                                State = EnumRoomState.Calling;
                            }
                            //接听
                            else if (State == EnumRoomState.InMedicalTreatment)
                            {
                                State = EnumRoomState.InMedicalTreatment;
                            }
                            //候诊界面，用户点击离开或者异常断开都是未就诊
                            else
                            {
                                State = EnumRoomState.NoTreatment;
                            }
                            break;
                        }
                    case EnumRoomState.Calling:
                        {                 
                            //重试         
                            if (State == EnumRoomState.Calling)
                            {
                                State = EnumRoomState.Calling;
                            }
                            //接听
                            else if (State == EnumRoomState.InMedicalTreatment)
                            {
                                State = EnumRoomState.InMedicalTreatment;
                            }
                            //取消呼叫 或者拒绝
                            else if (State == EnumRoomState.Waiting || State == EnumRoomState.WaitAgain)
                            {
                                State = EnumRoomState.WaitAgain;
                            }
                            else
                            {
                                State = EnumRoomState.Disconnection;
                            }
                            break;
                        }
                    case EnumRoomState.InMedicalTreatment:
                        {
                            //重试
                            if (State == EnumRoomState.InMedicalTreatment)
                            {
                                State = EnumRoomState.InMedicalTreatment;
                            }
                            //医生挂断
                            else if (State == EnumRoomState.AlreadyVisit)
                            {
                                State = EnumRoomState.AlreadyVisit;
                            }
                            //重试候诊
                            else if (State == EnumRoomState.Waiting || State == EnumRoomState.WaitAgain)
                            {
                                State = EnumRoomState.WaitAgain;
                            }
                            //患者离开，一会再来
                            else
                            {
                                State = EnumRoomState.Disconnection;
                            }
                            break;
                        }
                    case EnumRoomState.Disconnection:
                        {
                            //医生挂断
                            if (State == EnumRoomState.AlreadyVisit)
                            {
                                State = EnumRoomState.AlreadyVisit;
                            }
                            //取消呼叫 或者拒绝
                            else if (State == EnumRoomState.Waiting || State == EnumRoomState.WaitAgain)
                            {
                                State = EnumRoomState.WaitAgain;
                            }
                            else
                            {
                                State = EnumRoomState.Disconnection;
                            }
                            break;
                        }
                    case EnumRoomState.WaitAgain:
                        {
                            //医生呼叫
                            if (State == EnumRoomState.Calling)
                            {
                                State = EnumRoomState.Calling;
                            }
                            //取消呼叫 或者拒绝
                            else if (State == EnumRoomState.Waiting || State == EnumRoomState.WaitAgain)
                            {
                                State = EnumRoomState.WaitAgain;
                            }
                            else
                            {
                                State = EnumRoomState.Disconnection;
                            }
                            break;
                        }
                }
                #endregion

                //默认高版本，当前禁用互通性（SDK直接可以互通的情况下）
                if (room.DisableWebSdkInteroperability)
                {
                    room.DisableWebSdkInteroperability = DisableWebSdkInteroperability;
                }

                room.RoomState = State;
                room.ModifyTime = DateTime.Now;

                #region 推送状态变更消息，并修改状态
                using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                {
                    mqChannel.BeginTransaction();
                    if (!mqChannel.Publish<EventBus.Events.ChannelStateChangedEvent>(new EventBus.Events.ChannelStateChangedEvent()
                    {
                        ChannelID = ChannelID,
                        FromUserID = FromUserID,
                        State = State,
                        ExpectedState = ExpectedState,
                        DisableWebSdkInteroperability = room.DisableWebSdkInteroperability
                    }))
                    {
                        return EnumApiStatus.BizError;
                    }

                    if (CompareAndSetChannelInfo(room))
                    {
                        mqChannel.Commit();
                        return EnumApiStatus.BizOK;
                    }
                    else
                    {
                        return EnumApiStatus.BizError;
                    }
                }
                #endregion

            }
            //出现数据库并发更新异常需要重试
            catch (DbUpdateConcurrencyException ex)
            {
                return CompareAndSetChannelState(ChannelID, FromUserID, State, DisableWebSdkInteroperability, ref ExpectedState);
            }
            catch(Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                //业务错误
                return EnumApiStatus.BizError;
            }      

        }


        string Format(int duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";

            if (ts.Days > 0)
            {
                str += $"{ts.Days}天";
            }

            if (ts.Hours > 0)
            {
                str += $"{ts.Hours.ToString()}小时";
            }

            if (ts.Minutes > 0)
            {
                str += $"{ts.Minutes.ToString()}分钟";
            }

            if (ts.Seconds > 0)
            {
                str += $"{ts.Seconds}秒";
            }

            return str;
        }

        /// <summary>
        /// 重新开始计时
        
        /// 日期：2017年7月30日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public bool RestartCharging(int ChannelID)
        {
            try
            {
                //获取房间信息
                var room = GetChannelInfo(ChannelID);
                var doctorUser = GetChannelUsersInfo(ChannelID).Find(a => a.UserType == EnumUserType.Doctor);

                #region 房间不存在或医生不存在则忽略
                if (room == null && doctorUser == null)
                {
                    return true;
                }
                #endregion

                #region 更新房间计费状态
                if (room.ChargingState != EnumRoomChargingState.Started)
                {
                    room.ChargingState = EnumRoomChargingState.Started;

                    if (!CompareAndSetChannelInfo(room))
                    {
                        return false;
                    }
                }
                #endregion

                using (MQChannel mqChannel = new MQChannel())
                {
                    mqChannel.BeginTransaction();

                    #region 发布延时消息，15秒为一个周期。消费端收到消息后重新计算房间已通话时间。
                    if (!mqChannel.Publish<EventBus.Events.ChannelChargingEvent>(new EventBus.Events.ChannelChargingEvent()
                    {
                        ChannelID = ChannelID,
                        Seq = room.ChargingSeq + 1,
                        ChargingTime = room.ChargingTime,
                        Interval = room.ChargingInterval
                    }))
                    {
                        return false;
                    }
                    #endregion

                    #region 发送服务时长变更消息
                    if (!mqChannel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestCustomMsgRoomDurationChanged>>(new ChannelSendGroupMsgEvent<RequestCustomMsgRoomDurationChanged>()
                    {
                        Msg = new BLL.Sys.DTOs.Request.RequestCustomMsgRoomDurationChanged()
                        {
                            Data = new RequestConversationRoomStatusDTO()
                            {
                                ChannelID = room.ChannelID,
                                State = room.RoomState,
                                ServiceID = room.ServiceID,
                                ServiceType = room.ServiceType,
                                DisableWebSdkInteroperability = room.DisableWebSdkInteroperability,
                                ChargingState = EnumRoomChargingState.Started,
                                Duration = room.Duration, //总时长
                                TotalTime = room.TotalTime// 消耗
                            },
                            Desc = $"服务计时已启动，总时长{Format(room.Duration)}, 剩余{Format(room.Duration - room.TotalTime)}"
                        },
                        ChannelID = room.ChannelID,
                        FromAccount = doctorUser.identifier
                    }))
                    {

                        return false;
                    }
                    #endregion

                    mqChannel.Commit();

                    return true;
                }
                
            }
            //出现并发更新异常，重试即可
            catch (DbUpdateConcurrencyException ex)
            {
                return RestartCharging(ChannelID);
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }
        }

        /// <summary>
        /// 暂停计时
        
        /// 日期：2017年7月30日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public bool PauseCharging(int ChannelID)
        {
            try
            {
                //获取房间信息
                var room = GetChannelInfo(ChannelID);

                //房间存在且状态不是暂停的则执行暂停操作（去重复）
                if (room != null && room.RoomState != EnumRoomState.AlreadyVisit && (room.ServiceType == EnumDoctorServiceType.AudServiceType || room.ServiceType == EnumDoctorServiceType.VidServiceType))
                {
                    var doctorUser = GetChannelUsersInfo(ChannelID).Find(a => a.UserType == EnumUserType.Doctor);

                    if (doctorUser != null)
                    {
                        using (MQChannel channel = new MQChannel())
                        {
                            channel.BeginTransaction();

                            if (!channel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestCustomMsgRoomDurationChanged>>(new ChannelSendGroupMsgEvent<RequestCustomMsgRoomDurationChanged>()
                            {
                                Msg = new BLL.Sys.DTOs.Request.RequestCustomMsgRoomDurationChanged()
                                {
                                    Data = new RequestConversationRoomStatusDTO()
                                    {
                                        ChannelID = room.ChannelID,
                                        State = room.RoomState,
                                        ServiceID = room.ServiceID,
                                        ServiceType = room.ServiceType,
                                        DisableWebSdkInteroperability = room.DisableWebSdkInteroperability,
                                        ChargingState = EnumRoomChargingState.Paused,
                                        Duration = room.Duration, //总时长
                                        TotalTime = room.TotalTime// 消耗
                                    },
                                    Desc = $"服务计时已暂停，总时长{Format(room.Duration)}, 剩余{Format(room.Duration - room.TotalTime)}"
                                },
                                ChannelID = room.ChannelID,
                                FromAccount = doctorUser.identifier
                            }))
                            {
                                return false;
                            }

                            room.ChargingState = EnumRoomChargingState.Paused;

                            if (CompareAndSetChannelInfo(room))
                            {
                                channel.Commit();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            //出现并发更新异常，重试即可
            catch (DbUpdateConcurrencyException ex)
            {
                return PauseCharging(ChannelID);
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }

            return true;

        }

        /// <summary>
        /// 开始计时
        
        /// 日期：2017年6月22日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="Duration"></param>
        /// <param name="ServiceID"></param>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public bool StartCharging(int ChannelID,int Duration,string ServiceID,string OrderNo)
        {
            try
            {
                var room = GetChannelInfo(ChannelID);

                #region 修改房间开始时间和已经消耗的时间
                //第一次进入诊室的时候
                if (room.TotalTime < 1)
                {
                    room.BeginTime = DateTime.Now;
                    room.TotalTime = 1;

                    if (!CompareAndSetChannelInfo(room))
                    {
                        return false;
                    }
                }
                #endregion

                using (MQChannel mqChannel = new MQChannel())
                {
                    mqChannel.BeginTransaction();

                    //存在重复的请求，消费端需要去重复
                    if (!mqChannel.Publish<EventBus.Events.ChannelDurationChangeEvent>(new EventBus.Events.ChannelDurationChangeEvent()
                    {
                        Duration = Duration, //套餐里面的服务时长单位是分钟需要转换成秒
                        ServiceID = ServiceID,
                        OrderNo = OrderNo,
                        NewUpgradeOrderNo = OrderNo
                    }))
                    {
                        return false;
                    }

                    //存在重复的请求，消费端需要去重复
                    //发布延时消息，15秒为一个周期。消费端收到消息后重新计算房间已通话时间。
                    if (!mqChannel.Publish<EventBus.Events.ChannelChargingEvent>(new EventBus.Events.ChannelChargingEvent()
                    {
                        ChannelID = ChannelID,
                        Seq = 0,
                        ChargingTime = room.BeginTime,
                        Interval = 15
                    }))
                    {
                        return false;
                    }

                    room.ChargingState = EnumRoomChargingState.Started;

                    if (CompareAndSetChannelInfo(room))
                    {
                        mqChannel.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //出现并发更新异常，重试即可
            catch (DbUpdateConcurrencyException ex)
            {
                return StartCharging(ChannelID, Duration, ServiceID, OrderNo);
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }

        }

        /// <summary>
        /// 服务时间递增（已增加同步锁）
        
        /// 日期：2017年4月20日
        /// </summary>
        /// <param name="ServiceID">业务编号</param>
        /// <param name="Seconds">需要递增的服务时长，小于等于0意味着不限制服务时长（秒）</param>
        /// <returns></returns>
        public bool IncrementChannelDuration(int ChannelID, string ServiceID, int Seconds, string OrderNo, string NewUpgradeOrderNo)
        {          
            return false;               

        }


        /// <summary>
        /// 新增频道成员
        
        /// 日期：2017年4月23日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public bool InsertChannelLog(
            string ConversationRoomID,
            string OperationUserID,
            string OperatorUserName,
            string OperatorType, 
            string OperationDesc="",
            string OperationRemark="")
        {
            using (DBEntities db = new DBEntities())
            {

                db.ConversationRoomLogs.Add(new ConversationRoomLog()
                {   
                    ConversationRoomID = ConversationRoomID,                    
                    ConversationRoomLogID = Guid.NewGuid().ToString("N"),
                    OperationDesc = string.IsNullOrEmpty(OperationDesc) ? "" : OperationDesc,
                    OperationRemark = string.IsNullOrEmpty(OperationRemark) ? "" : OperationRemark,
                    OperationTime = DateTime.Now,
                    OperatorType= OperatorType,
                    OperationUserID = string.IsNullOrEmpty(OperationUserID) ? "" : OperationUserID,
                    OperatorUserName =string.IsNullOrEmpty(OperatorUserName)?"":OperatorUserName
                });

                if (db.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// 获取最后的日志
        /// </summary>
        /// <param name="ConversationRoomID"></param>
        /// <returns></returns>
        public ConversationRoomLog GetChannelLastLog(string ConversationRoomID,string OperatorType)
        {
            using (DBEntities db = new DBEntities())
            {
                var log=db.ConversationRoomLogs.Where(a => a.ConversationRoomID == ConversationRoomID && a.OperatorType== OperatorType).OrderByDescending(a => a.OperationTime).FirstOrDefault();
                return log;
            }
        }

        /// <summary>
        /// 添加好友
        
        /// 日期：2017年6月23日
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Dictionary<int, ConversationRoomDTO> ApplyAddFriend(RequestConversactionApplyAddFriendDTO request)
        {
            Dictionary<int, ConversationRoomDTO> result = new Dictionary<int, ConversationRoomDTO>();
            using (DBEntities db = new DBEntities())
            {
                request.AddFriendItem.ForEach(account =>
                {
                    var channelInfo = (from friend in db.ConversationFriends.Where(a => a.FromUserID==request.FromUserID && a.ToUserID == account.ToUserID)
                                       join room in db.ConversationRooms on friend.ConversationRoomID equals room.ConversationRoomID
                                       where room.RoomType== EnumRoomType.C2C
                                       select room).FirstOrDefault();

                    if (channelInfo == null)
                    {
                        var ConversationRoomID = Guid.NewGuid().ToString("N");

                        channelInfo = new ConversationRoom()
                        {
                            ServiceID = "",
                            Priority = 5,
                            ChargingInterval = 0,
                            ChargingSeq = 0,
                            ChargingState = EnumRoomChargingState.Stoped,
                            ChargingTime = DateTime.Now,
                            Duration = 0,
                            TotalTime = 0,
                            Enable = false,
                            ServiceType = EnumDoctorServiceType.PicServiceType,
                            Secret = "",
                            BeginTime = DateTime.Now,
                            EndTime = DateTime.Now,
                            ConversationRoomID = ConversationRoomID,
                            CreateTime = DateTime.Now,
                            IsDeleted = false,
                            RoomState = EnumRoomState.NoTreatment,
                            RoomType = EnumRoomType.C2C,
                            TriageID =long.MaxValue
                        };

                        db.ConversationRooms.Add(channelInfo);

                        db.ConversationRoomUids.Add(new ConversationRoomUid()
                        {
                            ConversationRoomID = ConversationRoomID,
                            Identifier = account.ToUserIdentifier,
                            UserType = account.ToUserType,
                            IsDeleted = false,
                            UserMemberID = account.ToUserMemberID,
                            CreateUserID = request.FromUserID,
                            UserID = account.ToUserID,
                            UserCNName = account.ToUserName,
                            UserENName =account.ToUserName,               
                            PhotoUrl = "",
                        });

                        if (request.FromUserIdentifier != request.FromUserIdentifier)
                        {
                            db.ConversationRoomUids.Add(new ConversationRoomUid()
                            {
                                ConversationRoomID = ConversationRoomID,
                                Identifier = request.FromUserIdentifier,
                                UserType = request.FromUserType,
                                IsDeleted = false,
                                UserMemberID = request.FromUserMemberID,
                                CreateUserID = request.FromUserID,
                                UserID = request.FromUserID,
                                UserCNName = account.ToUserName,
                                UserENName = account.ToUserName,
                                PhotoUrl = "",
                            });
                        }

                        db.ConversationFriends.Add(new ConversationFriend()
                        {
                            ConversationRoomID = ConversationRoomID,
                            FromUserID = request.FromUserID,
                            FromUserIdentifier = request.FromUserIdentifier,
                            ToUserIdentifier = account.ToUserIdentifier,
                            ToUserID = account.ToUserID,
                        
                       
                            AddWording = account.AddWording,
                            CreateTime = DateTime.Now,
                            GroupName = account.GroupName,
                            Remark = account.Remark,
                            IsDeleted = false,
                       
                            FriendID = Guid.NewGuid().ToString("N")
                        });

                        db.ConversationFriends.Add(new ConversationFriend()
                        {
                            ConversationRoomID = ConversationRoomID,
                            FromUserID = account.ToUserID,
                            FromUserIdentifier = account.ToUserIdentifier,
                            ToUserIdentifier = request.FromUserIdentifier,
                            ToUserID = request.FromUserID,
                        
                      
                            AddWording = account.AddWording,
                            CreateTime = DateTime.Now,
                            GroupName = account.GroupName,
                            Remark = account.Remark,
                            IsDeleted = false,
                        
                            FriendID = Guid.NewGuid().ToString("N")
                        });

                        db.SaveChanges();

                        result.Add(account.ToUserIdentifier, channelInfo.Map<ConversationRoom, ConversationRoomDTO>());
                    }
                    else
                    {
                        result.Add(account.ToUserIdentifier, channelInfo.Map<ConversationRoom, ConversationRoomDTO>());
                    }
                });

              
            }

            return result;
        }

        /// <summary>
        /// 关闭房间
        /// </summary>
        /// <returns></returns>
        public bool CloseRoom(int channelID, bool close)
        {
            try
            {
                var room = GetChannelInfo(channelID);
                room.Close = close;
                return CompareAndSetChannelInfo(room);
            }
            //出现并发更新异常，重试即可
            catch (DbUpdateConcurrencyException ex)
            {
                return CloseRoom(channelID, close);
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }
        }


        /// <summary>
        /// 获取没有候诊的放假
        /// </summary>
        public List<ConversationRoomDTO> GetNoWaitingRooms()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddMinutes(30);


            using (DBEntities db = new DBEntities())
            {
                var query = from room in db.ConversationRooms.Where(a => !a.IsDeleted && (

                            (endDate>=a.BeginTime && startDate <=a.EndTime)

                            ) && a.RoomState== EnumRoomState.NoTreatment && a.RoomType== EnumRoomType.Group)
                            join order in db.Orders on room.ServiceID equals order.OrderOutID 
                            join derep in db.SysDereplications.Where(i => i.DereplicationType == EnumDereplicationType.RoomReadyClinic && i.TableName == "ConversationRooms") on room.ServiceID equals derep.OutID into leftJoinDerep
                            from derepIfEmpty in leftJoinDerep.DefaultIfEmpty()
                            where derepIfEmpty == null && order.OrderState== EnumOrderState.Paid
                            select  new ConversationRoomDTO()
                            {
                                ServiceID = room.ServiceID,
                                Priority =room.Priority,
                                ChargingInterval = room.ChargingInterval,
                                ChargingSeq = room.ChargingSeq,
                                ChargingState =room.ChargingState,
                                ChargingTime = room.ChargingTime,
                                Duration = room.Duration,
                                TotalTime = room.TotalTime,
                                Enable = room.Enable,
                                ServiceType = room.ServiceType,
                                Secret = room.Secret,
                                BeginTime =room.BeginTime,
                                EndTime =room.EndTime,
                                ConversationRoomID =room.ConversationRoomID,                             
                                RoomState = room.RoomState,
                                RoomType = room.RoomType,
                                TriageID = room.TriageID
                            };

                return query.ToList();
            }
        }
        #endregion

    }
}
