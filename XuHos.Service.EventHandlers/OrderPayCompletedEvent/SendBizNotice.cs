using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL.Sys;
using XuHos.Common.Enum;
using XuHos.BLL.Sys.Implements;
using XuHos.Common.Cache;

namespace XuHos.Service.EventHandlers.OrderPayCompletedEvent
{
    public class SendBizNotice : IEventHandler<EventBus.Events.OrderPayCompletedEvent>
    {
        XuHos.BLL.UserOPDRegisterService opdService = new XuHos.BLL.UserOPDRegisterService("");

        public bool Handle(EventBus.Events.OrderPayCompletedEvent evt)
        {
            var LockName = $"{typeof(SendBizNotice)}:{evt.OrderNo}";

            var lockValue = Guid.NewGuid().ToString("N");

            //获取分布式锁，获取锁失败时进行锁等待（锁超时时间5秒）
            if (LockName.Lock(lockValue, TimeSpan.FromSeconds(5)))
            {
                try
                {
                    #region 发送业务通知
                    var opd = opdService.Single(evt.OrderOutID);
                    #region 需分诊的订单，等分诊后再处理
                    if (opd.DoctorTriage.IsToGuidance == true)
                    {
                        return true;
                    }
                    #endregion

                    if (opd != null)
                    {
                        var noticeType = EnumNoticeSecondType.AllNotice;
                        var ServiceType = EnumDoctorServiceType.VidServiceType;

                        //图文消息
                        if (evt.OrderType == EnumProductType.ImageText)
                        {
                            noticeType = EnumNoticeSecondType.DoctorPicNotice;
                            ServiceType = EnumDoctorServiceType.PicServiceType;
                        }
                        //家庭医生
                        else if (evt.OrderType == EnumProductType.FamilyDoctor)
                        {
                            noticeType = EnumNoticeSecondType.DoctorFamilyNotice;
                            ServiceType = EnumDoctorServiceType.FamilyDoctor;
                        }
                        //语音或视频咨询
                        else if (evt.OrderType == EnumProductType.video)
                        {
                            noticeType = EnumNoticeSecondType.DoctorVidNotice;
                            ServiceType = EnumDoctorServiceType.VidServiceType;
                        }
                        //语音或视频咨询
                        else if (evt.OrderType == EnumProductType.Phone)
                        {
                            noticeType = EnumNoticeSecondType.DoctorVidNotice;
                            ServiceType = EnumDoctorServiceType.AudServiceType;
                        }
                        //远程会诊
                        else if (evt.OrderType == EnumProductType.Consultation)
                        {
                            noticeType = EnumNoticeSecondType.DoctorConsulNotice;
                            ServiceType = EnumDoctorServiceType.Consultation;
                        }


                        using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                        {
                            return mqChannel.Publish<EventBus.Events.UserNoticeEvent>(new EventBus.Events.UserNoticeEvent()
                            {
                                NoticeType = noticeType,
                                ServiceID = evt.OrderOutID,
                                ServiceType = ServiceType
                            });
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Common.LogHelper.WriteError(ex);
                    return false;
                }
                finally
                {
                    LockName.UnLock(lockValue);
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
