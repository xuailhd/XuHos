using XuHos.BLL;
using XuHos.Common.Enum;
using XuHos.DTO;
using XuHos.Entity;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.OrderEvaluationEvent
{
    public class InsertEvaluation : IEventHandler<EventBus.Events.OrderEvaluationEvent>
    {
        XuHos.BLL.OrderService orderService = new OrderService("");
        BLL.Doctor.Implements.DoctorService doctorService = new BLL.Doctor.Implements.DoctorService();


        public bool Handle(EventBus.Events.OrderEvaluationEvent evt)
        {
            if (evt == null)
            {
                return true;
            }

            try
            {
                var ProviderID = "";
                var UserID = "";

                #region 获取服务提供者的编号

                var order = orderService.GetOrder("", evt.OuterID);

                if (order != null)
                {
                    UserID = order.UserID;

                    switch (order.OrderType)
                    {
                        case EnumProductType.video:
                            {
                                ProviderID = new XuHos.BLL.UserOPDRegisterService(order.UserID).Single<XuHos.DTO.UserOPDRegisterDTO>(order.OrderOutID).DoctorID;
                            }; break;
                    }
                }
                #endregion


                if (string.IsNullOrEmpty(ProviderID) || string.IsNullOrEmpty(UserID))
                {
                    return true;
                }

                #region 写评论记录
                ServiceEvaluation model = new ServiceEvaluation()
                {
                    UserID = UserID,
                    EvaluationTags = evt.EvaluationTags,
                    ProviderID = ProviderID,
                    Content = evt.Content,
                    CreateTime = DateTime.Now,
                    CreateUserID = string.IsNullOrEmpty(evt.CreateUserID) ? UserID : evt.CreateUserID,
                    OuterID = evt.OuterID,
                    Score = evt.Score,
                    ServiceType = order.OrderType
                };

                if (!doctorService.Evaluation(model))
                {
                    return false;
                }
                #endregion

                #region 写领域消息，评论完成
                using (XuHos.EventBus.MQChannel mqChannel = new MQChannel())
                {
                    if (!mqChannel.Publish<XuHos.EventBus.Events.OrderEvaluationCompletedEvent>(new EventBus.Events.OrderEvaluationCompletedEvent()
                    {
                        Content = evt.Content,
                        CreateUserID = evt.CreateUserID,
                        EvaluationTags = evt.EvaluationTags,
                        OuterID = evt.OuterID,
                        ProviderID = ProviderID,
                        Score = evt.Score
                    }))
                    {
                        return false;
                    }
                }
                #endregion

            }
            catch (Exception Ex)
            {
                XuHos.Common.LogHelper.WriteError(Ex);
                return false;
            }

            return true;
        }
    }


}
