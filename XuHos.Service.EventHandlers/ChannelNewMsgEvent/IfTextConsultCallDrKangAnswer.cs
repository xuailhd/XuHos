using KMEHosp.BLL.Sys.DTOs.Request;
using KMEHosp.BLL.Sys.Implements;
using KMEHosp.Common;
using KMEHosp.Common.Cache;
using KMEHosp.Common.Enum;
using KMEHosp.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMEHosp.BLL.Doctor.Implements;

namespace KMEHosp.Service.EventHandlers.ChannelNewMsgEvent
{
    /// <summary>
    ///呼叫康博士回答
    /// 作者：郭明
    /// 日期：2017年5月5日
    /// </summary>
    public class IfTextConsultCallDrKangAnswer : IEventHandler<EventBus.Events.ChannelNewMsgEvent>
    {
        public class Msg
        {

            public string MsgType { get; set; }

            public Dictionary<string, string> MsgContent { get; set; }

        }

        KMEHosp.BLL.User.Implements.UserService userService = new BLL.User.Implements.UserService();

        KMEHosp.Integration.KMBAT.DrKangService drKangService = new Integration.KMBAT.DrKangService();
        SysMonitorIndexService moniorIndexService = new SysMonitorIndexService();
        KMEHosp.BLL.OrderService orderService = new BLL.OrderService("");
        KMEHosp.BLL.Sys.Implements.ConversationRoomService roomService = new ConversationRoomService();

        

        public bool Handle(EventBus.Events.ChannelNewMsgEvent evt)
        {
            if (evt == null)
                return true;

            //已经启用了康博士
            if (!string.IsNullOrEmpty(KMEHosp.Integration.KMBAT.Configuration.Config.drKangEnable) &&
                             (KMEHosp.Integration.KMBAT.Configuration.Config.drKangEnable == "1" ||
                             KMEHosp.Integration.KMBAT.Configuration.Config.drKangEnable.ToUpper() == bool.TrueString.ToUpper())
                             )
            {
                try
                {
                    //是通过客户端发送的
                    if (evt.OptPlatform != "RESTAPI" && evt.ServiceType == EnumDoctorServiceType.PicServiceType)
                    {
                        #region 检查当前咨询中康博士回答情况，判断是否还需要继续使用康博士
                        var cacheKey_Channel_DrKangState = new KMEHosp.Common.Cache.Keys.StringCacheKey(Common.Cache.Keys.StringCacheKeyType.Channel_DrKangState, evt.ChannelID.ToString());
                        var Channel_DrKangState = cacheKey_Channel_DrKangState.FromCache<string>();

                        switch (Channel_DrKangState)
                        {
                            //问答结束，没有匹配的疾病            
                            case "nullMatchDisease":
                            //问答结束，已有明确诊断
                            case "diagnosis":
                            //无法响应回复
                            case "nullMatchResponse":
                            //禁用(医生已回复)
                            case "disabled":
                            //出现异常
                            case "exception":
                                return true;
                        }
                        #endregion

                        //文字内容才识别
                        if (evt.Messages.Length > 0 && evt.Messages[0].MessageContent.Contains("\"MsgType\":\"TIMTextElem\""))
                        {
                            var room = roomService.GetChannelInfo(evt.ChannelID);

                            //医生未接诊
                            if (room != null && room.RoomState == EnumRoomState.NoTreatment)
                            {
                                #region 医生未回答
                                //获取用户的信息
                                var userInfo = roomService.GetChannelUsersInfo(evt.ChannelID).Where(t =>
                                    t.identifier == Convert.ToInt32(evt.FromAccount)).FirstOrDefault();
                                
                                //获取医生信息
                                var sendMsgFromAccount = 0;

                                //用户回复了
                                if (userInfo != null && userInfo.UserType == EnumUserType.User)
                                {
                                    var order = orderService.GetOrder("", evt.ServiceID);

                                    if (order != null)
                                    {
                                        //  { "MsgContent":{ "Text":"头疼"},"MsgType":"TIMTextElem"}
                                        var msg = KMEHosp.Common.JsonHelper.FromJson<Msg>(evt.Messages[0].MessageContent);

                                        if (msg != null && msg.MsgType == "TIMTextElem" && msg.MsgContent != null)
                                        {
                                            var text = msg.MsgContent["Text"];

                                            try
                                            {


                                                #region 使用康博士，记录最后的处理状态，并返回康博士的回答

                                                Integration.KMBAT.Model.ResponseResultDataDTO ret =null;

                                                //首次的时候没有设置基本信息，如果是通过一键呼叫或者其他服务转过来的则没有设置。

                                                if (Channel_DrKangState == "notSetBaseMsg")
                                                {
                                                    var user = userService.GetUserInfoByUserId(userInfo.UserID);
                                                    ret = drKangService.setBaseMsg(userInfo.UserCNName,"",text,user.Gender == EnumUserGender.Male ? "男" : "女",room.ServiceID);
                                                }
                                                else
                                                {
                                                    //调用康博士导诊接口
                                                    ret = drKangService.drKangGuide(text, evt.ServiceID);
                                                }

                                                var QuestionTopic = "";
                                                var QuestionAnswer = new List<string>();
                                                var DrKangTrueSymptom = "";
                                                var DrKangDisease = "";

                                                //没有与症状匹配的模板
                                                if (ret.type == "nullMatchSymptom")
                                                {
                                                    QuestionTopic = "您的情况我已转达给了医生，请耐心等待医生的回复。";
                                                    //康博士无能为力，医生参与吧
                                                }
                                                //没有与症状匹配的模板
                                                else if (ret.type == "nullMatchTemplate")
                                                {
                                                    //康博士无能为力，医生参与吧
                                                    QuestionTopic = "您的情况我已转达给了医生，请耐心等待医生的回复。";
                                                }
                                                //匹配到多个模板需要跟用户确认（）
                                                else if (ret.type == "confirmTemplate")
                                                {
                                                    QuestionTopic = ret.body;
                                                    //返回提示内容,需要在跟患者确认。
                                                }
                                                //问答阶段
                                                else if (ret.type == "acking")
                                                {
                                                    QuestionTopic = ret.body;
                                                    QuestionAnswer = ret.answer;
                                                    //返回提示信息，正在问答阶段，医生这时候是否能够介入？
                                                }
                                                //问答结束，没有匹配的疾病
                                                else if (ret.type == "nullMatchDisease")
                                                {
                                                    QuestionTopic = "您的情况我已转达给了医生，请耐心等待医生的回复。";
                                                    //没有明确的诊断，需要医生参与
                                                }
                                                //问答结束，已有明确诊断
                                                else if (ret.type == "diagnosis")
                                                {
                                                    var diagnosisRet = drKangService.getInterrogationRecord(evt.ServiceID);
                                                    DrKangTrueSymptom = diagnosisRet.trueSymptom;
                                                    DrKangDisease = diagnosisRet.disease;

                                                    //QuestionTopic = $"您的症状为:{diagnosisRet.trueSymptom},可能患有{diagnosisRet.disease}疾病。该情况我已转达给了医生，请耐心等待医生的正式回复。";
                                                    QuestionTopic = $"您的症状为:{diagnosisRet.trueSymptom}。该情况我已转达给了医生，请耐心等待医生的正式回复。";
                                                }
                                                //无法回答
                                                else if (ret.type == "nullMatchResponse")
                                                {
                                                    QuestionTopic = "您的情况我已转达给了医生，请耐心等待医生的回复。";
                                                    //康博士无法回答的问题，需人工介入
                                                }

                                                //记录最后一次问答的状态
                                                ret.type.ToCache(cacheKey_Channel_DrKangState);
                                                #endregion

                                                #region 更新监控指标
                                                var values = new Dictionary<string, string>();
                                                values.Add("DrKangState", ret.type);//康博士问诊状态

                                                if (!string.IsNullOrEmpty(DrKangDisease))
                                                {
                                                    values.Add("DrKangDisease", DrKangDisease);//康博士问诊状态
                                                }

                                                if (!string.IsNullOrEmpty(DrKangTrueSymptom))
                                                {
                                                    values.Add("DrKangTrueSymptom", DrKangTrueSymptom);//康博士问诊状态
                                                }

                                                if (!moniorIndexService.InsertAndUpdate(new RequestSysMonitorIndexUpdateDTO()
                                                {
                                                    Category = "UserConsult",
                                                    OutID = order.OrderNo,
                                                    Values = values
                                                }))
                                                {
                                                    return false;
                                                }
                                                #endregion

                                                #region 使用非医生的身份，回答给用户
                                                using (MQChannel channle = new MQChannel())
                                                {
                                                    if (QuestionAnswer.Count > 0)
                                                    {
                                                        return channle.Publish<EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestIMCustomMsgSurvey>>(new EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestIMCustomMsgSurvey>()
                                                        {
                                                            ChannelID = evt.ChannelID,
                                                            FromAccount = sendMsgFromAccount,
                                                            Msg = new RequestIMCustomMsgSurvey()
                                                            {
                                                                Desc = QuestionTopic,
                                                                Data = new RadioTopic()
                                                                {
                                                                    Answer = QuestionAnswer
                                                                }
                                                            }
                                                        });
                                                    }
                                                    else
                                                    {
                                                        return channle.Publish<EventBus.Events.ChannelSendGroupMsgEvent<string>>(new EventBus.Events.ChannelSendGroupMsgEvent<string>()
                                                        {
                                                            ChannelID = evt.ChannelID,
                                                            FromAccount = sendMsgFromAccount,
                                                            Msg = QuestionTopic
                                                        });
                                                    }
                                                }
                                                #endregion
                                            }
                                            catch
                                            {
                                                #region 出现异常，则记录下来
                                                var values = new Dictionary<string, string>();
                                                values.Add("DrKangState", "exception");//康博士问诊状态
                                                if (!moniorIndexService.InsertAndUpdate(new RequestSysMonitorIndexUpdateDTO()
                                                {
                                                    Category = "UserConsult",
                                                    OutID = order.OrderNo,
                                                    Values = values
                                                }))
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    return true;
                                                }
                                                #endregion
                                            }
                                        }

                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                //医生已经回答
                                "disabled".ToCache(cacheKey_Channel_DrKangState, TimeSpan.FromHours(24));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    KMEHosp.Common.LogHelper.WriteError(ex);
                    return false;
                }
            }
            else
            {
                return true;
            }

            return true;
        }

        
    }


}
