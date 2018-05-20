using System;
using System.Collections.Generic;
using System.Linq;
using XuHos.Entity;
using XuHos.DAL.EF;
using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO;
using System.Data.Entity;
using EntityFramework.Extensions;
using XuHos.DTO.Common;
using XuHos.Common;
using XuHos.DTO.Platform;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.User.DTOs.Request;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.BLL.User.Implements;
using System.Linq.Dynamic;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;
using XuHos.BLL.User.DTOs.Response;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using RequestUserQueryOPDRegisterDTO = XuHos.DTO.RequestUserQueryOPDRegisterDTO;

namespace XuHos.BLL
{
    /// <summary>
    /// 用户预约相关业务    
    /// </summary>
    public class UserOPDRegisterService : BLL.User.Implements.UserBaseService<XuHos.Entity.UserOPDRegister>
    {
        public UserOPDRegisterService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        {

        }

        public UserOPDRegisterService(string CurrentOperatorUserID, string CurrentOperatorOrgID) : base(CurrentOperatorUserID)
        {
            this.CurrentOperatorOrgID = CurrentOperatorOrgID;
        }


        public bool SendConsultContent(int ChannelID, string OPDRegisterID)
        {
            var imservice = new XuHos.Integration.QQCloudy.IMHelper();
            var sysDerepService = new BLL.Sys.SysDereplicationService();
            var imUidService = new ConversationIMUidService(CurrentOperatorUserID);

            using (DBEntities db = new DBEntities())
            {
                var model = db.UserOpdRegisters.Where(a => a.OPDRegisterID == OPDRegisterID && !a.IsDeleted).FirstOrDefault();

                if (model != null)
                {
                    //用户唯一标识
                    var userIdentifier = imUidService.GetUserIMUid(model.UserID);

                    //发送图片消息
                    var ImageFiles = db.UserFiles.Where(a => a.OutID == OPDRegisterID).ToList();

                    using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                    {
                        mqChannel.BeginTransaction();

                        foreach (var File in ImageFiles)
                        {
                            if (!mqChannel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>>(new EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>()
                            {
                                ChannelID = ChannelID,
                                FromAccount = userIdentifier,
                                Msg = File.Map<UserFile, ResponseUserFileDTO>()

                            }))
                            {
                                return false;
                            }
                        }

                        if (!mqChannel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<string>>(new EventBus.Events.ChannelSendGroupMsgEvent<string>()
                        {
                            ChannelID = ChannelID,
                            FromAccount = userIdentifier,
                            Msg = model.ConsultContent

                        }))
                        {
                            return false;
                        }

                        mqChannel.Commit();

                        return true;
                    }
                }
            }

            return true;
        }

        public UserOPDRegisterDTO Single(string OPDRegisterID, bool WithoutUnSignedRecipe = true)
        {
            var model = this.Single<UserOPDRegisterDTO>(a => a.OPDRegisterID == OPDRegisterID);

            if (model != null)
            {

                #region 订单信息
                model.Order = new BLL.OrderService("").GetOrder("", model.OPDRegisterID);
                #endregion

                #region 房间信息
                model.Room = new BLL.Sys.Implements.ConversationRoomService().GetChannelInfo(model.OPDRegisterID);
                #endregion

                #region 用户资料
                model.User = (from m in this.DBEntities.Users
                              where m.UserID == model.UserID && m.IsDeleted == false
                              select new UserDTO
                              {
                                  UserID = m.UserID,
                                  UserCNName = m.UserCNName,
                                  UserENName = m.UserENName,
                                  UserLevel = m.UserLevel,
                                  UserType = m.UserType
                              }).FutureFirstOrDefault();
                #endregion

                #region 用户病历
                model.UserMedicalRecord = (from m in this.DBEntities.UserMedicalRecords
                                           where m.OPDRegisterID == OPDRegisterID && m.IsDeleted == false
                                           orderby m.CreateTime descending
                                           select new UserMedicalRecordDTO()
                                           {

                                               Advised = m.Advised,
                                               AllergicHistory = m.AllergicHistory,
                                               ConsultationSubject = m.ConsultationSubject,
                                               Description = m.Description,
                                               FamilyMedicalHistory = m.FamilyMedicalHistory,
                                               PastMedicalHistory = m.PastMedicalHistory,
                                               PastOperatedHistory = m.PastOperatedHistory,
                                               PreliminaryDiagnosis = m.PreliminaryDiagnosis,
                                               PresentHistoryIllness = m.PresentHistoryIllness,
                                               Sympton = m.Sympton,
                                               UserMedicalRecordID = m.UserMedicalRecordID,

                                           }).FirstOrDefault();

                #endregion

                #region 附件
                IQueryable<UserFile> queryAttachFiles = null;

                if (model.OPDType == EnumDoctorServiceType.AudServiceType || model.OPDType == EnumDoctorServiceType.VidServiceType)
                {
                    queryAttachFiles = (from m in this.DBEntities.UserFiles
                                        where m.OutID == model.OPDRegisterID
                                        select m);
                }

                if (queryAttachFiles != null)
                {
                    model.AttachFiles = queryAttachFiles.Where(i => i.FileType == 0).Select(m => new UserFileDTO
                    {
                        FileID = m.FileID,
                        FileName = m.FileName,
                        FileType = m.FileType,
                        FileUrl = m.FileUrl,
                        OutID = m.OutID,
                        Remark = m.Remark
                    }).ToList();
                }
                #endregion

                #region 医生信息
                if (!string.IsNullOrEmpty(model.DoctorID))
                {
                    XuHos.BLL.Doctor.Implements.DoctorService doctorService = new Doctor.Implements.DoctorService();
                    var doctor = doctorService.GetDoctorDetail(model.DoctorID);
                    if (doctor != null)
                    {
                        model.Doctor = new DoctorDto()
                        {
                            Title = Sys.Implements.CodeService.GetDoctorTitles()[doctor.Title],
                            DoctorName = doctor.DoctorName,
                        };
                    }
                }
                #endregion

                #region 患者信息
                if (!string.IsNullOrEmpty(model.MemberID))
                {
                    model.Member = new UserMemberDTO()
                    {
                        MemberID = model.MemberID,
                        MemberName = model.MemberName,
                        Gender = model.Gender,
                        Birthday = model.Birthday,
                        Mobile = model.Mobile
                    };
                }
                #endregion

                #region 订单来源
                if (!string.IsNullOrEmpty(model.OrgnazitionID))
                {
                    List<string> paths = new XuHos.BLL.HospitalService("").GetOrgFullPathByOrgID(model.OrgnazitionID);//.Single<DTO.HospitalDto>(x => x.HospitalID == model.OrgnazitionID);
                    model.OrgName = paths == null ? null : string.Join(" - ", paths);
                    model.OrgType = null;
                }
                #endregion
            }
            return model;
        }
        /// <summary>
        /// 检查是否已经预约
        
        /// 日期：2016年11月25日
        /// </summary>
        /// <param name="requst"></param>
        /// <returns></returns>
        public bool ExistsWithSubmitRequest(RequestUserOPDRegisterSubmitDTO requst, out OrderRepeatReturnDTO order)
        {
            using (DBEntities db = new DBEntities())
            {
                int Year = DateTime.Now.Year;
                int Month = DateTime.Now.Month;
                int Day = DateTime.Now.Day;

                OrderRepeatReturnDTO opdModel = null;

                //未指定医生（医生领单的业务）
                if (string.IsNullOrEmpty(requst.DoctorID))
                {
                    //有已经支付过，就诊未结束的就返回
                    opdModel = (from opd in db.UserOpdRegisters.Where(a =>
                                     a.IsUseTaskPool
                                     && a.UserID == requst.UserID
                                     && a.MemberID == requst.MemberID
                                     && a.OPDType == requst.OPDType
                                     && (
                                         a.RegDate.Year == Year &&
                                         a.RegDate.Month == Month &&
                                         a.RegDate.Day == Day)
                                        && a.IsDeleted == false)
                                join opdOrder in db.Orders on opd.OPDRegisterID equals opdOrder.OrderOutID
                                join room in db.ConversationRooms on opd.OPDRegisterID equals room.ServiceID
                                where
                                (
                                       opdOrder.OrderState == EnumOrderState.Paid ||
                                       opdOrder.OrderState == EnumOrderState.Finish
                                )
                                && room.RoomState != EnumRoomState.AlreadyVisit
                                //排除已经关闭的订单
                                && !room.Close
                                orderby opdOrder.OrderTime descending
                                select new OrderRepeatReturnDTO
                                {
                                    OrderOutID = opd.OPDRegisterID,
                                    OrderNo = opdOrder.OrderNo,
                                    OrderState = opdOrder.OrderState,
                                    ChannelID = room.ChannelID,
                                    DoctorID = opd.DoctorID,
                                    Cancelable = (

                                          opdOrder.RefundState == EnumRefundState.NoRefund &&
                                         (opdOrder.OrderState == EnumOrderState.Paid || opdOrder.OrderState == EnumOrderState.NoPay || opdOrder.OrderState == EnumOrderState.NoConfirm) &&
                                         !(opdOrder.LogisticState == EnumLogisticState.配送中 || opdOrder.LogisticState == EnumLogisticState.已发货 || opdOrder.LogisticState == EnumLogisticState.已送达)
                                        )
                                }).FirstOrDefault();
                }
                else
                {
                    //如果有未完成或未取消的订单则返回
                    opdModel = (from opd in db.UserOpdRegisters.Where(a =>
                                     !a.IsUseTaskPool
                                     && (a.DoctorID == requst.DoctorID)
                                     && a.UserID == requst.UserID
                                     && a.MemberID == requst.MemberID
                                     && a.OPDType == requst.OPDType
                                     && (
                                         a.RegDate.Year == Year &&
                                         a.RegDate.Month == Month &&
                                         a.RegDate.Day == Day)
                                        && a.IsDeleted == false)
                                join opdOrder in db.Orders on opd.OPDRegisterID equals opdOrder.OrderOutID
                                join room in db.ConversationRooms on opd.OPDRegisterID equals room.ServiceID
                                where
                                 (opdOrder.OrderState == EnumOrderState.NoConfirm ||
                                 opdOrder.OrderState == EnumOrderState.NoPay ||
                                 opdOrder.OrderState == EnumOrderState.Paid)
                                orderby opdOrder.OrderTime descending
                                select new OrderRepeatReturnDTO
                                {
                                    OrderOutID = opd.OPDRegisterID,
                                    OrderNo = opdOrder.OrderNo,
                                    OrderState = opdOrder.OrderState,
                                    ChannelID = room.ChannelID,
                                    DoctorID = opd.DoctorID,
                                    Cancelable = (
                                          opdOrder.RefundState == EnumRefundState.NoRefund &&
                                         (opdOrder.OrderState == EnumOrderState.Paid || opdOrder.OrderState == EnumOrderState.NoPay || opdOrder.OrderState == EnumOrderState.NoConfirm) &&
                                         !(opdOrder.LogisticState == EnumLogisticState.配送中 || opdOrder.LogisticState == EnumLogisticState.已发货 || opdOrder.LogisticState == EnumLogisticState.已送达)
                                        )
                                }).FirstOrDefault();

                }

                //已经预约了
                if (opdModel != null)
                {
                    order = opdModel;
                    return true;
                }
                else
                {
                    order = null;
                    return false;
                }


            }
        }

        /// <summary>
        /// 获取医生服务价格
        
        /// 日期：2016年11月25日
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        decimal GetServicePriceWithSubmitRequest(RequestUserOPDRegisterSubmitDTO request)
        {
            var bll = new BLL.Doctor.Implements.DoctorService();

            //医生服务
            var docServiceInfo = bll.GetDoctorServicePriceSetting(request.DoctorID, request.OPDType);

            //医生排版和医生服务正常
            if (docServiceInfo != null)
            {
                //医生开通了服务
                if (docServiceInfo.ServiceSwitch == 1)
                {
                    if (request.OPDType == EnumDoctorServiceType.Registration)
                    {
                        return 0;
                    }
                    //如果是药房的预约那么则不需要收费           
                    else if (request.UserType == EnumUserType.Drugstore)
                    {
                        return 0;
                    }
                    else
                    {
                        return docServiceInfo.ServicePrice;
                    }
                }
            }

            return -1;
        }

        string GetBoilWayName(int BoilWay, int DecoctNum = 1, decimal DecoctTotalWater = 500, decimal DecoctTargetWater = 300)
        {
            if (BoilWay == 2)
            {
                return "制法：研粉末冲服";
            }
            else if (BoilWay == 3)
            {
                return "制法：制作成药丸服用";
            }
            else
            {
                return $"制法：水煎，{DecoctNum} 煎，清水 {DecoctTotalWater} 毫升，煎至 {DecoctTargetWater} 毫升";
            }
        }

        string GetSigName(decimal Dose, string DoseUnit, string DrugRouteName, string Frequency)
        {
            var sigName = "Sig：";
            sigName = sigName + Dose.ToString() + " " + DoseUnit;
            sigName = sigName + "  ";
            if (!string.IsNullOrEmpty(DrugRouteName))
            {
                sigName = sigName + DrugRouteName;
            }
            sigName = sigName + "  ";
            if (!string.IsNullOrEmpty(Frequency))
            {
                sigName = sigName + Frequency;
            }
            return sigName;
        }

        string GetUsageName(int FreqDay, string Usage, int FreqTimes = 1, int Times = 1)
        {
            if (FreqDay == 1)
            {
                return $"用法：{Usage}，每日 {FreqTimes} 剂，分 {Times} 次服";
            }
            else
            {
                return $"用法：{Usage}，每 {FreqDay} 日 {FreqTimes} 剂，分 {Times} 次服";
            }
        }


        /// <summary>
        /// 提交预约
        
        /// 日期：2016年11月25日
        /// </summary>
        /// <param name="request"></param>
        /// <param name="CheckExists">是否检查重复预约</param>
        /// <returns></returns>
        public ResponseUserOPDRegisterSubmitDTO Submit(RequestUserOPDRegisterSubmitDTO request, bool CheckExists = true)
        {
            using (DAL.EF.DBEntities db = new DBEntities())
            {
                var Reason = "";

                var orderService = new OrderService(CurrentOperatorUserID);

                //获取服务价格
                decimal ServicePrice = 0M;
                var OPDDate = DateTime.Now;
                var OPDBeginTime = DateTime.Now.ToString("HH:mm");
                var OPDEndTime = DateTime.Now.AddMinutes(30).ToString("HH:mm");
                var OrderExpireTime = DateTime.Now.AddMinutes(30);
                var IsUseTaskPool = false;

                RequestUserMemberDTO member = null;

                #region 没有指定就诊人
                if (string.IsNullOrEmpty(request.MemberID))
                {
                    //获取默认就诊人
                    if (string.IsNullOrEmpty(request.Name))
                    {
                        member = new BLL.User.Implements.UserMemberService().
                            GetDefaultMemberInfo(request.UserID).Map<ResponseUserMemberDTO, RequestUserMemberDTO>();

                        request.MemberID = member.MemberID;

                    }
                    else
                    {
                        member = new RequestUserMemberDTO()
                        {
                            UserID = request.UserID,
                            MemberName = request.Name,
                            Relation = EnumUserRelation.Other, //自己关系只能有一个，默认其他
                            Gender = request.Sex,
                            Birthday = request.Birth,
                            Marriage = request.Marriage,
                            Mobile = request.Mobile,
                            Address = "",
                            Email = "",
                            PostCode = "",
                        };

                        //设置默认成员
                        request.MemberID = member.MemberID;
                        
                    }
                }

                #endregion

                #region 校验失败：就诊人信息不合法
                UserMemberService umService = new UserMemberService();
                if (string.IsNullOrEmpty(request.MemberID) && !umService.CheckMemberProfileWithSubmitRequest(request.MemberID, out Reason))
                {
                    return new ResponseUserOPDRegisterSubmitDTO
                    {
                        ErrorInfo = Reason,
                        ActionStatus = "Fail",
                    };
                }
                #endregion

                #region 一键呼叫和预约处理



                //指定医生呼叫
                if (!string.IsNullOrEmpty(request.ScheduleID))
                {
                    if (request.OPDType == EnumDoctorServiceType.AudServiceType || request.OPDType == EnumDoctorServiceType.VidServiceType)
                    {
                        //获取排版信息
                        var schInfo = new DoctorSchduleService(CurrentOperatorUserID).Single<Entity.DoctorSchedule>(request.ScheduleID);

                        #region 校验失败：视频或语音在线文字的时候没有设置医生排版
                        if (request.OPDType == EnumDoctorServiceType.AudServiceType || request.OPDType == EnumDoctorServiceType.VidServiceType)
                        {
                            if (schInfo == null || schInfo.Number == 0)
                            {
                                return new ResponseUserOPDRegisterSubmitDTO
                                {
                                    ErrorInfo = "医生排班不存在",
                                    ActionStatus = "UnSupport",
                                };
                            }
                        }
                        #endregion

                        #region 通过医生排版填充预约信息
                        if (schInfo != null)
                        {
                            request.DoctorID = schInfo.DoctorID;
                            OPDDate = new DateTime(int.Parse(schInfo.OPDate.Substring(0, 4)), int.Parse(schInfo.OPDate.Substring(4, 2)), int.Parse(schInfo.OPDate.Substring(6, 2)));//预约日期            
                            OPDBeginTime = schInfo.StartTime;
                            OPDEndTime = schInfo.EndTime;
                        }
                        #endregion

                        //获取服务价格
                        ServicePrice = GetServicePriceWithSubmitRequest(request);

                        #region 校验失败：医生未开通服务
                        if (ServicePrice < 0)
                        {
                            return new ResponseUserOPDRegisterSubmitDTO
                            {
                                ErrorInfo = "医生未开通" + request.OPDType.GetEnumDescript(),
                                ActionStatus = "UnSupport",
                            };
                        }
                        #endregion
                    }
                }
                //一键呼叫
                else
                {
                    //不需要家庭成员
                    if (request.MemberID == "-")
                    {
                        request.MemberID = "";
                    }

                    if (string.IsNullOrEmpty(request.DoctorID))
                    {
                        request.DoctorID = "";
                    }
                    // 如果指定医生的情况，但又没有排班的情况，参考，药店看诊端，沈腾飞
                    else
                    {
                        var schedules = new DoctorSchduleService(CurrentOperatorUserID).GetDoctorScheduleList(
                            request.DoctorID, DateTime.Now, DateTime.Now.AddDays(1), false);

                        #region 校验失败：视频或语音在线文字的时候没有设置医生排版
                        if (request.OPDType == EnumDoctorServiceType.AudServiceType || request.OPDType == EnumDoctorServiceType.VidServiceType)
                        {
                            if (schedules.Data == null || schedules.Data.Count == 0)
                            {
                                return new ResponseUserOPDRegisterSubmitDTO
                                {
                                    ErrorInfo = "医生排班不存在",
                                    ActionStatus = "UnSupport",
                                };
                            }

                            if (schedules.Data.Where(x => x.AppointmentCounts.Sum(y => y.Value) < x.Number).FirstOrDefault() == null)
                            {
                                return new ResponseUserOPDRegisterSubmitDTO
                                {
                                    ErrorInfo = "该时间段已约满",
                                    ActionStatus = "UnSupport",
                                };
                            }
                        }
                        #endregion

                        //获取服务价格
                        ServicePrice = GetServicePriceWithSubmitRequest(request);

                        #region 校验失败：医生未开通服务
                        if (ServicePrice < 0)
                        {
                            return new ResponseUserOPDRegisterSubmitDTO
                            {
                                ErrorInfo = "医生未开通" + request.OPDType.GetEnumDescript(),
                                ActionStatus = "UnSupport",
                            };
                        }
                        #endregion
                    }

                    if (string.IsNullOrEmpty(request.ScheduleID))
                    {
                        request.ScheduleID = "";
                    }

                    IsUseTaskPool = true;
                    //订单过期时间5分钟
                    OrderExpireTime = DateTime.Now.AddMinutes(5);
                }
                #endregion

                #region 校验失败：重复预约
                var existsOrder = new OrderRepeatReturnDTO();

                if (CheckExists && ExistsWithSubmitRequest(request, out existsOrder))
                {
                    //一键呼叫时，且订单已经被医生领取，如果订单能够被取消掉则先取消订单再预约
                    if (string.IsNullOrEmpty(request.DoctorID) &&
                        !string.IsNullOrEmpty(existsOrder.DoctorID) &&
                        existsOrder.Cancelable)
                    {
                        //取消掉原订单
                        if (!orderService.Cancel(existsOrder.OrderNo))
                        {
                            return new ResponseUserOPDRegisterSubmitDTO
                            {
                                ErrorInfo = "取消未完成的订单失败，请重试",
                                ActionStatus = "Fail",
                            };
                        }
                    }
                    else
                    {
                        return new ResponseUserOPDRegisterSubmitDTO
                        {
                            ErrorInfo = "当天已有未完成的预约，不能重复预约",
                            OPDRegisterID = existsOrder.OrderOutID,
                            OrderNO = existsOrder.OrderNo,
                            OrderState = existsOrder.OrderState,
                            ChannelID = existsOrder.ChannelID,
                            ActionStatus = "Repeat"
                        };

                    }
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(request.MemberID) && member == null)
                {
                    member = umService.GetMemberInfo(request.MemberID).Map<ResponseUserMemberDTO, RequestUserMemberDTO>();
                }

                #region 新增预约记录
                UserOPDRegister model = new UserOPDRegister()
                {
                    IsUseTaskPool = IsUseTaskPool,
                    CreateTime = DateTime.Now,
                    ScheduleID = request.ScheduleID,
                    DeleteTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    IsDeleted = false,
                    DoctorGroupID = string.IsNullOrEmpty(request.DoctorGroupID) ? "" : request.DoctorGroupID,
                    CreateUserID = request.UserID,
                    OrgnazitionID = request.OrgnazitionID,
                    OPDType = request.OPDType,
                    MemberID = request.MemberID,
                    RegDate = DateTime.Now,
                    OPDBeginTime = OPDBeginTime,//服务预约时间段开始
                    OPDEndTime = OPDEndTime,//服务预约时间端结束
                    Fee = ServicePrice,//服务价格
                    DoctorID = string.IsNullOrEmpty(request.DoctorID) ? "" : request.DoctorID,//医生编号
                    OPDDate = OPDDate,//预约日期
                    OPDRegisterID = Guid.NewGuid().ToString("N"),
                    UserID = request.UserID,
                    ConsultContent = string.IsNullOrEmpty(request.ConsultContent) ? "" : request.ConsultContent,
                    ConsultDisease = string.IsNullOrEmpty(request.ConsultDisease) ? "" : request.ConsultDisease,
                };

                if (member != null)
                {
                    model.MemberName = member.MemberName;
                    model.Gender = member.Gender;
                    model.Marriage = member.Marriage;
                    model.Age = member.Age;
                    model.IDNumber = string.IsNullOrEmpty(member.IDNumber) ? "" : member.IDNumber;
                    model.IDType = member.IDType;
                    model.Mobile = member.Mobile;
                    model.Address = member.Address;
                    model.Birthday = member.Birthday;
                }

                db.UserOpdRegisters.Add(model);

                #endregion

                #region 添加过敏史
                // 如果填写了过敏史，则增加病历记录
                if (!string.IsNullOrEmpty(request.AllergicHistory))
                {
                    db.Set<UserMedicalRecord>().Add(new UserMedicalRecord
                    {
                        OPDRegisterID = model.OPDRegisterID,
                        DoctorID = model.DoctorID,
                        OrgnazitionID = "",
                        MemberID = model.MemberID,
                        UserID = model.UserID,
                        UserMedicalRecordID = Guid.NewGuid().ToString("N"),
                        Sympton = "",//主诉
                        PresentHistoryIllness = "",//现病史
                        PastMedicalHistory = "",//既往病史
                        Advised = "", //医嘱
                        PreliminaryDiagnosis = "", //初步诊断
                        AllergicHistory = request.AllergicHistory//过敏史
                    });
                }
                #endregion

                #region 添加医生患者
                if (!string.IsNullOrEmpty(request.DoctorID) && !string.IsNullOrEmpty(request.MemberID) &&
                    db.DoctorMembers.FirstOrDefault(x => x.DoctorID == request.DoctorID && x.MemberID == request.MemberID && !x.IsDeleted) == null)
                {
                    var doctorMember = new DoctorMember()
                    {
                        DoctorID = request.DoctorID,
                        MemberID = request.MemberID,
                        CreateTime = DateTime.Now,
                        CreateUserID = request.UserID,
                        DoctorMemberID = Guid.NewGuid().ToString("N"),
                    };

                    db.DoctorMembers.Add(doctorMember);
                }
                #endregion

                #region 添加附件
                if (request.Files != null)
                {
                    foreach (var file in request.Files)
                    {
                        db.UserFiles.Add(new UserFile()
                        {
                            FileID = Guid.NewGuid().ToString("N"),
                            FileName = file.FileUrl,
                            FileUrl = file.FileUrl,
                            FileType = 0,
                            Remark = file.Remark,
                            AccessKey = "",
                            ResourceID = "",
                            OutID = model.OPDRegisterID,
                            UserID = request.UserID
                        });
                    }
                }
                #endregion

                #region 创建房间
                var room = new Entity.ConversationRoom();
                room.EndTime = DateTime.Parse(OPDDate.ToString("yyyy-MM-dd ") + OPDEndTime);
                room.BeginTime = DateTime.Parse(OPDDate.ToString("yyyy-MM-dd ") + OPDBeginTime);
                room.TotalTime = 0;
                room.RoomState = EnumRoomState.NoTreatment;//状态
                room.ConversationRoomID = Guid.NewGuid().ToString("N");
                room.ServiceID = model.OPDRegisterID;
                room.ServiceType = model.OPDType;
                //如果预约类型是挂号那么房间类型就是线下看诊，否则是线上看诊
                room.RoomType = EnumRoomType.Group;
                room.TriageID = long.MaxValue;
                room.Priority = request.UserLevel;
                room.DisableWebSdkInteroperability = true;
                db.Set<ConversationRoom>().Add(room);
                #endregion

                #region 创建订单

                EnumProductType ProductType = EnumProductType.Other;

                switch (model.OPDType)
                {
                    case EnumDoctorServiceType.PicServiceType:
                        ProductType = EnumProductType.ImageText;
                        break;
                    case EnumDoctorServiceType.AudServiceType:
                        ProductType = EnumProductType.Phone;
                        break;
                    case EnumDoctorServiceType.VidServiceType:
                        ProductType = EnumProductType.video;
                        break;
                    case EnumDoctorServiceType.FamilyDoctor:
                        ProductType = EnumProductType.Other;
                        break;
                    case EnumDoctorServiceType.Registration:
                        ProductType = EnumProductType.Registration;
                        break;
                    default:
                        break;
                }

                var order = orderService.CreateOrder(new DTO.Platform.OrderDTO()
                {
                    OrderType = ProductType,
                    OrderOutID = model.OPDRegisterID,
                    UserID = model.UserID,
                    OrderTime = DateTime.Now,
                    OrderExpireTime = OrderExpireTime,
                    OrgnazitionID = request.OrgnazitionID,
                    Details = new List<DTO.Platform.OrderDetailDTO>()
                                    {
                                        new DTO.Platform.OrderDetailDTO() {

                                            Subject=ProductType.GetEnumDescript(),
                                            Body="",
                                            ProductId="",
                                            ProductType = ProductType,
                                            Quantity=1,
                                            UnitPrice =model.Fee
                                        }
                                    }
                }, db);

                #endregion

                if (db.SaveChanges() > 0)
                {
                    //确定订单
                    if (orderService.Confirm(order.OrderNo, new RequestOrderConfirmDTO()
                    {
                        OrderNo = order.OrderOutID,
                        Privilege = request.Privilege,
                        UserPackageID = request.UserPackageID
                    }) == EnumApiStatus.BizOK)
                    {
                        order = orderService.GetOrder(order.OrderNo);

                        return new ResponseUserOPDRegisterSubmitDTO
                        {
                            ErrorInfo = "预约成功",
                            ActionStatus = "Success",
                            OPDRegisterID = model.OPDRegisterID,
                            OrderNO = order.OrderNo,
                            OrderState = order.OrderState,
                            ChannelID = room.ChannelID
                        };
                    }
                    else
                    {
                        orderService.Cancel(order.OrderNo);
                    }
                }

                return new ResponseUserOPDRegisterSubmitDTO
                {
                    ErrorInfo = "预约失败",
                    ActionStatus = "Fail",
                };

            }
        }


        /// <summary>
        /// 获取预约记录
        
        /// 日期：2016年8月3日
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="OPDType"></param>
        /// <param name="Keyword"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public Response<List<DTO.UserOPDRegisterDTO>> GetPageList(DTO.RequestUserQueryOPDRegisterDTO request)
        {

            Response<List<DTO.UserOPDRegisterDTO>> result = new Response<List<DTO.UserOPDRegisterDTO>>();

            var today = DateTime.Now;

            using (XuHos.DAL.EF.DBEntities db = new DBEntities())
            {
                int Total = 0;

                string queryPredicate = "IsDeleted = @0 AND (OPDType = @1 OR OPDType = @2)";
                List<object> paramValues = new List<object> { false, EnumDoctorServiceType.VidServiceType, EnumDoctorServiceType.AudServiceType };

                if (!string.IsNullOrEmpty(request.MemberID))
                {
                    BLL.User.Implements.UserMemberService memser = new UserMemberService();
                    var member = memser.GetMemberInfo(request.MemberID);
                    if (!string.IsNullOrEmpty(member.IDNumber))
                    {
                        queryPredicate += " AND IDNumber = @3";
                        paramValues.Add(member.IDNumber);
                    }
                    else
                    {
                        queryPredicate += " AND MemberID = @3";
                        paramValues.Add(request.MemberID);
                    }
                }
                else
                {
                    queryPredicate += " AND UserID = @3";
                    paramValues.Add(CurrentOperatorUserID);
                }

                IQueryable<UserOPDRegisterDTO> query = null;
                // 过滤排班与处方明细，app不需要用到提高查询效率
                if (request.FilterRecipeAndSchedule.HasValue && request.FilterRecipeAndSchedule.Value)
                {
                    // 指定用户条件筛选，建议在UserOpdRegisters表Where条件进行
                    query = from opd in db.UserOpdRegisters.Where(queryPredicate, paramValues.ToArray())
                            join doctor in db.Doctors.Where(a => !a.IsDeleted) on opd.DoctorID equals doctor.DoctorID 
                            join dept in db.HospitalDepartments on doctor.DepartmentID equals dept.DepartmentID into leftJoinDept
                            from deptIfEmpty in leftJoinDept.DefaultIfEmpty()
                            join hosp in db.Hospitals on deptIfEmpty.HospitalID equals hosp.HospitalID into leftJoinHosp
                            from hsopIfEmpty in leftJoinHosp.DefaultIfEmpty()
                            join order in db.Orders.Where(a => !a.IsDeleted) on opd.OPDRegisterID equals order.OrderOutID
                            join room in db.Set<Entity.ConversationRoom>().Where(a => a.IsDeleted == false) on opd.OPDRegisterID equals room.ServiceID into leftJoinRoom
                            from roomIfEmpty in leftJoinRoom.DefaultIfEmpty()
                            select new DTO.UserOPDRegisterDTO()
                            {
                                OPDRegisterID = opd.OPDRegisterID,//预约编号
                                OPDDate = opd.OPDDate,//排版日期
                                OPDType = opd.OPDType,//预约类型
                                RegDate = opd.RegDate,//预约时间                                                    
                                UserID = opd.UserID,//用户编号                             
                                Fee = opd.Fee,//费用
                                ConsultContent = opd.ConsultContent,
                                IDNumber = opd.IDNumber,
                                MemberID = opd.MemberID,
                                Age = opd.Age,
                                Room = new DTO.ConversationRoomDTO()
                                {
                                    //就诊当天，没有就诊，用户已经支付
                                    ChannelID = roomIfEmpty != null && (order.OrderState == EnumOrderState.Finish ||
                                    (order.OrderState == EnumOrderState.Paid &&
                                    (opd.OPDDate.Year == today.Year &&
                                    opd.OPDDate.Month == today.Month &&
                                    opd.OPDDate.Day == today.Day))
                                    ) ? roomIfEmpty.ChannelID : 0,//就诊房间
                                    RoomState = roomIfEmpty != null ? roomIfEmpty.RoomState : EnumRoomState.NoTreatment,//预约状态
                                    Secret = roomIfEmpty != null ? roomIfEmpty.Secret : "",//房间密码
                                },
                                Order = new DTO.Platform.OrderDTO()
                                {
                                    OrderNo = order.OrderNo,//订单编号
                                    OrderTime = order.OrderTime,//订单时间
                                    OrderState = order.OrderState,
                                    LogisticNo = order.LogisticNo,
                                    LogisticState = order.LogisticState,
                                    PayType = order.PayType,
                                    CostType = order.CostType,
                                    TotalFee = order.totalFee,
                                    TradeNo = order.TradeNo,
                                    IsEvaluated = order.IsEvaluated,
                                    RefundState = order.RefundState
                                },
                                Member = new DTO.UserMemberDTO()
                                {
                                    UserID = opd.UserID,
                                    MemberID = opd.MemberID,//成员编号
                                    MemberName = opd.MemberName,
                                    Gender = opd.Gender,
                                    IDNumber = opd.IDNumber,
                                    Age = opd.Age
                                },
                                Doctor = new DTO.DoctorDto()
                                {

                                    DoctorID = opd.DoctorID,//医生编号
                                    DoctorName = doctor.DoctorName,
                                    DepartmentID = doctor.DepartmentID,
                                    DepartmentName = deptIfEmpty != null ? deptIfEmpty.DepartmentName : "",
                                    HospitalID = hsopIfEmpty != null ? hsopIfEmpty.HospitalID : "",
                                    HospitalName = hsopIfEmpty != null ? hsopIfEmpty.HospitalName : "",
                                    Specialty = doctor.Specialty,
                                    Title = doctor.Title,
                                    Duties = doctor.Duties,
                                }
                            };
                }
                else
                {
                    // 指定用户条件筛选，建议在UserOpdRegisters表Where条件进行
                    query = from opd in db.UserOpdRegisters.Where(queryPredicate, paramValues.ToArray())
                            join doctor in db.Doctors.Where(a => !a.IsDeleted) on opd.DoctorID equals doctor.DoctorID into doctorLeftMid
                            from doctorLeft in doctorLeftMid.DefaultIfEmpty()
                            join dept in db.HospitalDepartments on doctorLeft.DepartmentID equals dept.DepartmentID into leftJoinDept
                            from deptIfEmpty in leftJoinDept.DefaultIfEmpty()
                            join hosp in db.Hospitals on deptIfEmpty.HospitalID equals hosp.HospitalID into leftJoinHosp
                            from hsopIfEmpty in leftJoinHosp.DefaultIfEmpty()
                            join emr in db.UserMedicalRecords.Where(a => !a.IsDeleted && a.DoctorID != null && a.DoctorID != "") on opd.OPDRegisterID equals emr.OPDRegisterID into leftJoinEmr
                            from emrIfEmpty in leftJoinEmr.DefaultIfEmpty()
                                //join depart in db.HospitalDepartments on doctor.DepartmentID equals depart.DepartmentID
                                //join hosp in db.Hospitals on depart.HospitalID equals hosp.HospitalID
                            join order in db.Orders.Where(a => !a.IsDeleted) on opd.OPDRegisterID equals order.OrderOutID
                            join sch in db.DoctorSchedules.Where(a => !a.IsDeleted) on opd.ScheduleID equals sch.ScheduleID into leftJoinSch
                            from schIfEmpty in leftJoinSch.DefaultIfEmpty()
                            join room in db.Set<Entity.ConversationRoom>().Where(a => a.IsDeleted == false) on opd.OPDRegisterID equals room.ServiceID into leftJoinRoom
                            from roomIfEmpty in leftJoinRoom.DefaultIfEmpty()
                            select new DTO.UserOPDRegisterDTO()
                            {
                                OPDRegisterID = opd.OPDRegisterID,//预约编号
                                OPDDate = opd.OPDDate,//排版日期
                                OPDType = opd.OPDType,//预约类型
                                RegDate = opd.RegDate,//预约时间                                                    
                                UserID = opd.UserID,//用户编号                             
                                Fee = opd.Fee,//费用
                                ConsultContent = opd.ConsultContent,
                                IDNumber = opd.IDNumber,
                                MemberID = opd.MemberID,
                                Age = opd.Age,
                                Room = new DTO.ConversationRoomDTO()
                                {
                                    //就诊当天，没有就诊，用户已经支付
                                    ChannelID = roomIfEmpty != null && (order.OrderState == EnumOrderState.Finish ||
                                    (order.OrderState == EnumOrderState.Paid &&
                                    (opd.OPDDate.Year == today.Year &&
                                    opd.OPDDate.Month == today.Month &&
                                    opd.OPDDate.Day == today.Day))
                                    ) ? roomIfEmpty.ChannelID : 0,//就诊房间
                                    RoomState = roomIfEmpty != null ? roomIfEmpty.RoomState : EnumRoomState.NoTreatment,//预约状态
                                    Secret = roomIfEmpty != null ? roomIfEmpty.Secret : "",//房间密码
                                },
                                Order = new DTO.Platform.OrderDTO()
                                {
                                    OrderNo = order.OrderNo,//订单编号
                                    OrderTime = order.OrderTime,//订单时间
                                    OrderState = order.OrderState,
                                    LogisticNo = order.LogisticNo,
                                    LogisticState = order.LogisticState,
                                    PayType = order.PayType,
                                    CostType = order.CostType,
                                    TotalFee = order.totalFee,
                                    TradeNo = order.TradeNo,
                                    IsEvaluated = order.IsEvaluated,
                                    RefundState = order.RefundState
                                },
                                Member = new DTO.UserMemberDTO()
                                {
                                    UserID = opd.UserID,
                                    MemberID = opd.MemberID,//成员编号
                                    MemberName = opd.MemberName,
                                    Gender = opd.Gender,
                                    IDNumber = opd.IDNumber,
                                    Age = opd.Age
                                },
                                Doctor = new DTO.DoctorDto()
                                {
                                    DoctorID = opd.DoctorID,//医生编号
                                    DoctorName = doctorLeft == null ? "" : doctorLeft.DoctorName,
                                    DepartmentID = doctorLeft != null ? doctorLeft.DepartmentID : "",
                                    DepartmentName = deptIfEmpty != null ? deptIfEmpty.DepartmentName : "",
                                    HospitalID = hsopIfEmpty != null ? hsopIfEmpty.HospitalID : "",
                                    HospitalName = hsopIfEmpty != null ? hsopIfEmpty.HospitalName : "",
                                    Specialty = doctorLeft == null ? "" : doctorLeft.Specialty,
                                    Title = doctorLeft == null ? "" : doctorLeft.Title,
                                    Duties = doctorLeft == null ? "" : doctorLeft.Duties,
                                },
                                Schedule = new DTO.DoctorScheduleDto
                                {
                                    ScheduleID = schIfEmpty.ScheduleID,
                                    StartTime = schIfEmpty != null ? schIfEmpty.StartTime : opd.OPDBeginTime,
                                    EndTime = schIfEmpty != null ? schIfEmpty.EndTime : opd.OPDEndTime,
                                },
                                IsExistMedicalRecord = emrIfEmpty != null
                            };
                }

                #region 处理搜索条件
                //查询关键字
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(a => a.Member.MemberName.Contains(request.Keyword));
                }

                //开始日期
                if (request.BeginDate.HasValue)
                {
                    query = query.Where(a => a.OPDDate >= request.BeginDate);
                }

                //结束日期
                if (request.EndDate.HasValue)
                {
                    request.EndDate = request.EndDate.Value.AddDays(1);
                    query = query.Where(a => a.OPDDate < request.EndDate);
                }


                //类型
                if (request.OPDType.HasValue)
                {
                    query = query.Where(a => a.OPDType == request.OPDType.Value);
                }
                //状态
                if (request.OrderState.HasValue)
                {
                    query = query.Where(a => a.Order.OrderState == request.OrderState.Value);
                }

                #endregion

                query = query.OrderByDescending(a => new { a.Order.OrderTime, a.Order.OrderState });
                result.Data = query.Pager<DTO.UserOPDRegisterDTO>(out Total, request.CurrentPage, request.PageSize);

                result.Total = Total;

            }

            return result;

        }


        /// <summary>
        /// 获取医生的语音/视频看诊
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public Response<List<ResponseOPDRegisterAudVidDTO>> GetDoctorAudVid(string DoctorID, int PageIndex = 1, int PageSize = int.MaxValue)
        {
            var result = new Response<List<ResponseOPDRegisterAudVidDTO>>();
            using (var db = new DBEntities())
            {
                var query = from opd in db.UserOpdRegisters
                            join member in db.UserMembers on opd.MemberID equals member.MemberID
                            join sch in db.DoctorSchedules on opd.ScheduleID equals sch.ScheduleID
                            join order in db.Orders on opd.OPDRegisterID equals order.OrderOutID into gleftjoinOrder
                            from orderStatus in gleftjoinOrder.DefaultIfEmpty()
                            join room in db.Set<Entity.ConversationRoom>().Where(a => a.IsDeleted == false) on opd.OPDRegisterID equals room.ServiceID into leftJoinRoom
                            from roomIfEmpty in leftJoinRoom.DefaultIfEmpty()
                            where opd.IsDeleted == false
                                  && opd.DoctorID == DoctorID
                                  && (opd.OPDType == EnumDoctorServiceType.AudServiceType || opd.OPDType == EnumDoctorServiceType.VidServiceType)
                            orderby opd.RegDate descending, opd.OPDRegisterID descending
                            select new ResponseOPDRegisterAudVidDTO()
                            {
                                OPDRegisterID = opd.OPDRegisterID,//预约编号
                                MemberID = opd.MemberID,
                                MemberName = member.MemberName,
                                Birthday = member.Birthday,
                                Gender = member.Gender,
                                ChannelID = roomIfEmpty == null ? 0 : roomIfEmpty.ChannelID,
                                OPDType = opd.OPDType,//预约类型
                                RegDate = opd.RegDate,//预约时间  
                                OPDDate = opd.OPDDate,
                                ConsultContent = opd.ConsultContent,
                                OrderState = orderStatus != null ? orderStatus.OrderState : EnumOrderState.Paid,
                                Price = orderStatus != null ? orderStatus.totalFee : 0,//价格
                                Schedule = new DoctorScheduleDto
                                {
                                    StartTime = sch.StartTime,
                                    EndTime = sch.EndTime,
                                    OPDate = sch.OPDate
                                }
                            };

                int Total = 0;
                result.Data = query.Pager<ResponseOPDRegisterAudVidDTO>(out Total, PageIndex, PageSize);
                result.Total = Total;
            }

            return result;

        }


        /// <summary>
        /// 创建聊天的房间
        
        /// 日期：2016年10月18日
        /// </summary>
        /// <param name="OPDRegisterID"></param>
        /// <returns></returns>
        public bool CreateIMRoom(string OPDRegisterID)
        {
            try
            {
                using (DAL.EF.DBEntities db = new DBEntities())
                {
                    var model = (from opd in db.UserOpdRegisters.Where(a => a.OPDRegisterID == OPDRegisterID)
                                 join user in db.Users on opd.UserID equals user.UserID
                                 join member in db.UserMembers on opd.MemberID equals member.MemberID
                                 select new
                                 {
                                     OPDType = opd.OPDType,
                                     DoctorID = opd.DoctorID,//opd.DoctorID,
                                     UserID = opd.UserID,
                                     MemberID = opd.MemberID,
                                     member.MemberName,
                                     user.PhotoUrl,
                                     user.UserCNName,
                                     user.UserENName
                                 }).FirstOrDefault();


                    if (model != null)
                    {

                        #region 创建IM群组
                        XuHos.Integration.QQCloudy.IMHelper imservice = new XuHos.Integration.QQCloudy.IMHelper();
                        ConversationIMUidService imUidService = new ConversationIMUidService(CurrentOperatorUserID);
                        var roomService = new BLL.Sys.Implements.ConversationRoomService();

                        BLL.Doctor.Implements.DoctorService doctorService = new Doctor.Implements.DoctorService();

                        //房间信息
                        var room = roomService.GetChannelInfo(OPDRegisterID);

                        if (room != null)
                        {
                            var GroupName = model.OPDType.GetEnumDescript();
                            var Introduction = "";
                            var Notification = "";
                            var groupMembers = new List<int>();
                            var channelMembers = new List<RequestChannelMemberDTO>();

                            //患者信息
                            var userIdentifier = imUidService.GetUserIMUid(model.UserID);
                            if (userIdentifier > 0)
                            {
                                groupMembers.Add(userIdentifier);
                                channelMembers.Add(new RequestChannelMemberDTO()
                                {
                                    Identifier = userIdentifier,
                                    UserType = EnumUserType.User,
                                    UserID = model.UserID,
                                    UserMemberID = model.MemberID,
                                    PhotoUrl = model.PhotoUrl,
                                    UserCNName = model.MemberName,
                                    UserENName = model.MemberName
                                });
                            }

                            //获取医生信息(如果走导诊系统，此时还未分配医生，分诊后才会把分诊医生加入到IM组)
                            if (!string.IsNullOrEmpty(model.DoctorID))
                            {
                                var doctorInfo = doctorService.GetDoctorInfo(model.DoctorID);
                                var doctorIdentifier = imUidService.GetDoctorIMUid(model.DoctorID);
                                if (doctorIdentifier > 0)
                                {
                                    groupMembers.Add(doctorIdentifier);
                                    channelMembers.Add(new RequestChannelMemberDTO()
                                    {
                                        Identifier = doctorIdentifier,
                                        UserID = doctorInfo.UserID,
                                        UserType = EnumUserType.Doctor,
                                        UserMemberID = doctorInfo.User.MemberID,
                                        PhotoUrl = doctorInfo.User._PhotoUrl,//DTO已经进行了路径转换，这里需要使用没有转换之前的数据
                                        UserENName = doctorInfo.DoctorEnName,
                                        UserCNName = doctorInfo.DoctorName
                                    });
                                }
                            }

                            if (room.Enable)
                            {
                                if (imservice.AddGroupMember(room.ChannelID, groupMembers))
                                {
                                    return roomService.InsertChannelMembers(room.ChannelID, channelMembers);
                                }
                            }
                            else
                            {
                                //创建裙子成功
                                if (imservice.CreateGroup(room.ChannelID, GroupName, model.OPDType, groupMembers, Introduction, Notification))
                                {
                                    using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                                    {
                                        if (mqChannel.Publish<EventBus.Events.ChannelCreatedEvent>(new EventBus.Events.ChannelCreatedEvent()
                                        {
                                            ChannelID = room.ChannelID,
                                            ServiceID = room.ServiceID,
                                            ServiceType = room.ServiceType

                                        }))
                                        {
                                            room.Enable = true;

                                            if (roomService.CompareAndSetChannelInfo(room))
                                            {

                                                return roomService.InsertChannelMembers(room.ChannelID, channelMembers);
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }


                        }
                        else
                        {
                            return true;

                        }

                        #endregion

                    }
                    else
                    {
                        return true;
                    }
                }
            }
            //出现数据库并发更新异常需要重试
            catch (DbUpdateConcurrencyException ex)
            {
                return CreateIMRoom(OPDRegisterID);
            }

            return false;
        }

        /// <summary>
        /// 获取排班预约数
        /// </summary>
        /// <param name="ScheduleID"></param>
        /// <returns></returns>
        public int GetRegisterCount(string ScheduleID)
        {
            int count = 0;
            using (DAL.EF.DBEntities db = new DBEntities())
            {

                var schInfo = db.UserOpdRegisters.Where(a => a.ScheduleID == ScheduleID && !a.IsDeleted);
                count = schInfo.Count();
            }
            return count;
        }
        public int RegisterNumByUserId(string userId)
        {
            using (var db = new DBEntities())
            {
                return db.UserOpdRegisters.Where(o => o.IsDeleted == false && o.UserID == userId).Count();
            }
        }

    }
}
