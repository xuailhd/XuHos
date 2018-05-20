using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;
using XuHos.Common.Cache;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.Sys.DTOs.Request;

namespace XuHos.Service.EventHandlers.OrderPayCompletedEvent
{
    /// <summary>
    /// 更新统计指标
    
    /// 日期：2017年4月20日
    /// </summary>
    public class UpdateMonitorIndex : IEventHandler<EventBus.Events.OrderPayCompletedEvent>
    {
        XuHos.BLL.UserOPDRegisterService opdService = new BLL.UserOPDRegisterService("");
        XuHos.BLL.User.Implements.UserService userService = new BLL.User.Implements.UserService();
        XuHos.BLL.Doctor.Implements.DoctorService doctorService = new BLL.Doctor.Implements.DoctorService();

        public bool Handle(EventBus.Events.OrderPayCompletedEvent evt)
        {
            if (evt == null || evt.OrderNo == "")
            {
                return true;
            }

            var opd = opdService.Single(evt.OrderOutID);
            if (opd != null)
            {
                #region 需分诊的订单，等分诊后再处理
                if (opd.DoctorTriage.IsToGuidance == true)
                {
                    return true;
                }
                #endregion

                var user = userService.GetUserInfoByUserId(opd.UserID);

                if (user != null)
                {
                    var DoctorName = "-";
                    var DoctorID = "";

                    //正常预约的记录
                    if (opd.DoctorID != "")
                    {
                        var doctor = doctorService.GetDoctorDetail(opd.DoctorID);
                        DoctorID = doctor.DoctorID;
                        DoctorName = doctor.DoctorName;
                    }

                    SysMonitorIndexService service = new SysMonitorIndexService();
                    var values = new Dictionary<string, string>();
                    values.Add("UserID", opd.UserID + "");
                    values.Add("UserName", user.UserCNName + "");
                    values.Add("UserSource", user.OrgID + "");
                    values.Add("UserLevel", user.UserLevel.ToString());

                    values.Add("DoctorID", DoctorID);//医生编号 DoctorAcceptEvent  中维护
                    values.Add("DoctorName", DoctorName); //医生名称 DoctorAcceptEvent  中维护

                    values.Add("WaitingElapsedSeconds", "-");//候诊消耗时长 ChannelStateChangedEvent  中维护

                    values.Add("VisitingRoomState", "-");//就诊结束标志 ChannelChargingEvent/ChannelStateChangedEvent  中维护
                    values.Add("VisitingServiceChargingState", "-");//服务计费标志 ChannelChargingEvent 中维护
                    values.Add("VisitingServiceDurationSeconds", "-");//就诊服务时长 ChannelChargingEvent 中维护
                    values.Add("VisitingServiceElapsedSeconds", "-");//就诊消耗时长 ChannelChargingEvent 中维护

                    values.Add("RecipeTotalCount", "-");//处方总数量 RecipeSignSubmitEvent 中维护
                    values.Add("RecipeSignedCount", "-");//处方签名数量 RecipeSignCallbackEvent 中维护
                    values.Add("FallbackRemark", "-");
                    values.Add("FallbackFlag", "-");
                    values.Add("DrKangState", "-");//康博士问诊状态
                    service.InsertAndUpdate(new RequestSysMonitorIndexUpdateDTO()
                    {
                        Category = "UserConsult",
                        OutID = evt.OrderNo,
                        Values = values
                    });
                }
            }

            return true;
        }
    }
}
