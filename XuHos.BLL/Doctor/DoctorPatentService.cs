using EntityFramework.Extensions;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO;
using XuHos.Entity;
using XuHos.Extensions;
using XuHos.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using XuHos.DTO.Common;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.User.DTOs.Response;

namespace XuHos.BLL
{
    /// <summary>
    /// 医生患者相关业务
    /// </summary>
    public class DoctorPatentService : BLL.Doctor.Implements.DoctorBaseService<Entity.DoctorMember>
    {
        public DoctorPatentService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }




        /// <summary>
        /// 获取患者历史就诊记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<List<DTO.UserOPDRegisterDTO>> GetPatientVisitList(string OPDRegisterID, string MemberID, int PageIndex, int PageSize)
        {
            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = from opd in db.Set<UserOPDRegister>().Where(a => a.IsDeleted == false && (a.OPDRegisterID != OPDRegisterID || string.IsNullOrEmpty(OPDRegisterID)) && a.MemberID == MemberID)
                            join room in db.Set<Entity.ConversationRoom>() on opd.OPDRegisterID equals room.ServiceID
                            join medi in db.Set<UserMedicalRecord>().Where(a => a.IsDeleted == false && a.MemberID == MemberID) on opd.OPDRegisterID equals medi.OPDRegisterID into medileftjoin
                            from mediifEmpty in medileftjoin.DefaultIfEmpty()
                            join doctor in db.Set<XuHos.Entity.Doctor>().Where(a => a.IsDeleted == false) on opd.DoctorID equals doctor.DoctorID
                            join dept in db.Set<HospitalDepartment>().Where(a => a.IsDeleted == false) on doctor.DepartmentID equals dept.DepartmentID
                            join hosp in db.Set<Hospital>().Where(a => a.IsDeleted == false) on doctor.HospitalID equals hosp.HospitalID
                            orderby room.BeginTime descending
                            select new DTO.UserOPDRegisterDTO
                            {
                                OPDRegisterID = opd.OPDRegisterID,
                                RegDate = opd.RegDate,
                                OPDDate = opd.OPDDate,
                                OPDType = opd.OPDType,
                                OPDBeginTime = opd.OPDBeginTime,
                                OPDEndTime = opd.OPDEndTime,
                                Room = new DTO.ConversationRoomDTO()
                                {
                                    TotalTime = room.TotalTime,
                                    Enable = room.Enable,
                                    Duration = room.Duration,
                                    ChargingState = room.ChargingState,
                                    RoomState = room.RoomState,//状态
                                    ChannelID = room.ChannelID,//房间号码
                                    EndTime = room.EndTime, //结束时间
                                    Priority = room.Priority
                                },
                                UserMedicalRecord = new UserMedicalRecordDTO()
                                {
                                    PastMedicalHistory = mediifEmpty.PastMedicalHistory,
                                    PreliminaryDiagnosis = mediifEmpty.PreliminaryDiagnosis,
                                    PresentHistoryIllness = mediifEmpty.PresentHistoryIllness,
                                    Sympton = mediifEmpty.Sympton,
                                    Advised = mediifEmpty.Advised,
                                    AllergicHistory = mediifEmpty.AllergicHistory
                                },
                                Doctor = new DTO.DoctorDto()
                                {
                                    DoctorName = doctor.DoctorName,
                                    DepartmentName = doctor.DepartmentName,
                                    DoctorID = doctor.DoctorID,
                                    DepartmentID = doctor.DepartmentID,
                                    HospitalID = hosp.HospitalID,
                                    HospitalName = hosp.HospitalName
                                }
                            };

                query = query.OrderByDescending(t => t.OPDDate);

                Response<List<DTO.UserOPDRegisterDTO>> result = new Response<List<DTO.UserOPDRegisterDTO>>();
                int Total = 0;
                result.Data = query.Pager<DTO.UserOPDRegisterDTO>(out Total, PageIndex, PageSize);
                result.Total = Total;
                return result;
            }
        }


        /// <summary>
        /// 获取会员电子病历列表
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public Response<List<UserMemberEMRDTO>> GetPatientEMRPageList(
            string memberId,
            int pageIndex = 1,
            int pageSize = int.MaxValue,
            string keyword = null
           )
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = from m in db.UserMemberEMRs
                            join a in db.UserMembers on m.MemberID equals a.MemberID
                            where m.MemberID == memberId && m.IsDeleted == false
                            orderby m.Date descending
                            select new UserMemberEMRDTO
                            {
                                UserMemberEMRID = m.UserMemberEMRID,
                                MemberID = m.MemberID,
                                MemberName = a.MemberName,
                                Date = m.Date,
                                EMRName = m.EMRName,
                                HospitalName = m.HospitalName
                            };
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(m => m.EMRName.Contains(keyword));
                }
                Response<List<UserMemberEMRDTO>> result = new Response<List<UserMemberEMRDTO>>();
                int total = 0;
                result.Data = query.Pager(out total, pageIndex, pageSize);
                result.Total = total;
                return result;
            }
        }


        #region 患者诊断

   

        
        /// <summary>
        /// 获取患者最近的一次病历记录
        /// </summary>
        /// <param name="opdRegisterID"></param>
        /// <returns></returns>
        public UserMedicalRecordDTO GetPatientLatestMedicalRecord(string opdRegisterID)
        {
            using (DAL.EF.DBEntities db = new DBEntities())
            {
                UserMedicalRecordDTO result = db.Set<UserMedicalRecord>().Where(x => x.OPDRegisterID == opdRegisterID).OrderByDescending(x => x.CreateTime).Select(x => new UserMedicalRecordDTO
                {
                    Advised = x.Advised,
                    AllergicHistory = x.AllergicHistory,
                    ConsultationSubject = x.ConsultationSubject,
                    Description = x.Description,
                    FamilyMedicalHistory = x.FamilyMedicalHistory,
                    PastMedicalHistory = x.PastMedicalHistory,
                    PastOperatedHistory = x.PastOperatedHistory,
                    PreliminaryDiagnosis = x.PreliminaryDiagnosis,
                    PresentHistoryIllness = x.PresentHistoryIllness,
                    Sympton = x.Sympton,
                    UserMedicalRecordID = x.UserMedicalRecordID,
                }).FirstOrDefault();

                return result;
            }
        }
        #endregion

    }
}