using XuHos.BLL.User.DTOs.Request;
using XuHos.BLL.User.DTOs.Response;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.Implements
{
    public class UserOPDRegisterService : UserBaseService<Entity.UserOPDRegister>
    {
        public UserOPDRegisterService(string currentOperatorUserID)
            : base(currentOperatorUserID)
        {

        }

        public Response<List<ResponseUserOPDRegisterDTO>> GetPageList(RequestUserQueryOPDRegisterDTO request)
        {
            Response<List<ResponseUserOPDRegisterDTO>> result = new Response<List<ResponseUserOPDRegisterDTO>>();

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

                var query = from opd in db.UserOpdRegisters.Where(queryPredicate, paramValues.ToArray())
                            join doctor in db.Doctors.Where(a => !a.IsDeleted) on opd.DoctorID equals doctor.DoctorID
                            join order in db.Orders.Where(a => !a.IsDeleted) on opd.OPDRegisterID equals order.OrderOutID
                            join room in db.ConversationRooms.Where(a => !a.IsDeleted) on opd.OPDRegisterID equals room.ServiceID into leftJoinRoom
                            from roomIfEmpty in leftJoinRoom.DefaultIfEmpty()
                            select new ResponseUserOPDRegisterDTO()
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
                                    IDNumber = opd.IDNumber
                                },
                                Doctor = new DTO.DoctorDto()
                                {

                                    DoctorID = opd.DoctorID,//医生编号
                                    DoctorName = doctor.DoctorName,
                                    HospitalID = doctor.HospitalID,
                                    HospitalName = doctor.HospitalName,
                                    DepartmentID = doctor.DepartmentID,
                                    DepartmentName = doctor.DepartmentName,
                                    Specialty = doctor.Specialty,
                                    Title = doctor.Title,
                                    Duties = doctor.Duties,
                                }
                            };


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

                //传了MemberID，查memberid 的身份号的所有记录
                //if (!string.IsNullOrEmpty(request.MemberID))
                //{
                //    BLL.User.Implements.UserMemberService memser = new UserMemberService();
                //    var member = memser.GetMemberInfo(request.MemberID);
                //    if (!string.IsNullOrEmpty(member.IDNumber))
                //        query = query.Where(t => t.IDNumber == member.IDNumber);
                //    else
                //        query = query.Where(t => t.MemberID == member.MemberID);
                //}
                //else
                //{
                //    query = query.Where(t => t.UserID == CurrentOperatorUserID);
                //}

                #endregion

                query = query.OrderByDescending(a => new { a.Order.OrderTime, a.Order.OrderState });
                result.Data = query.Pager<ResponseUserOPDRegisterDTO>(out Total, request.CurrentPage, request.PageSize);
                result.Total = Total;
            }

            return result;
        }
    }
}
