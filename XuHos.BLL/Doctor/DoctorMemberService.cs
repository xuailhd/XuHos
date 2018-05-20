using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.DAL.EF;
using XuHos.Entity;
using System.Linq.Expressions;
using System.Data.Entity;
using XuHos.BLL.Common;
using XuHos.Extensions;
using XuHos.Common.Enum;
using XuHos.BLL.Sys;
using XuHos.BLL.User.DTOs.Response;
using XuHos.BLL.Sys.Implements;

namespace XuHos.BLL
{
    /// <summary>
    /// 医生的患者业务处理
    /// </summary>
    public class DoctorMemberService : Doctor.Implements.DoctorBaseService<DoctorMember>
    {
        public DoctorMemberService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }

        /// <summary>
        /// 添加医生患者关系
        /// </summary>
        /// <param name="doctorID"></param>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public bool AddDoctorMember(string doctorID, string memberID)
        {
            if (string.IsNullOrEmpty(doctorID) || string.IsNullOrEmpty(memberID))
                return false;

            using (var db = new DBEntities())
            {
                // 如果已存在该医生和换照的关系，则直接返回
                if (db.DoctorMembers.FirstOrDefault(x => x.DoctorID == doctorID && x.MemberID == memberID && !x.IsDeleted) != null)
                    return true;

                var doctorMember = new DoctorMember()
                {
                    DoctorID = doctorID,
                    MemberID = memberID,
                    CreateTime = DateTime.Now,
                    CreateUserID = CurrentOperatorUserID,
                    DoctorMemberID = Guid.NewGuid().ToString("N"),
                };

                db.DoctorMembers.Add(doctorMember);
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 查询列表（带分页）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Response<List<DoctorMemberDTO>> GetList(DoctorMemberCondition condition)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.DoctorMembers
                            join doctor in db.Doctors on item.DoctorID equals doctor.DoctorID
                            join member in db.UserMembers on item.MemberID equals member.MemberID
                            //join map in db.UserMemberMaps on member.MemberID equals map.MemberID
                            //join user in db.Users on map.UserID equals user.UserID
                            orderby item.ModifyTime, item.CreateTime descending
                            where item.IsDeleted == false && member.IsDeleted == false
                            && doctor.UserID == condition.UserID //&& map.IsDeleted == false
                            select new DoctorMemberDTO
                            {
                                DoctorMemberID = item.DoctorMemberID,
                                DoctorID = item.DoctorID,
                                MemberID = item.MemberID,
                                MemberName = member.MemberName,
                                Birthday = member.Birthday,
                                Gender = member.Gender,
                                Mobile = member.Mobile
                            };
                //动态查询条件
                if (!string.IsNullOrEmpty(condition.Keyword))
                    query = query.Where(i => i.MemberName.Contains(condition.Keyword));
                //分页
                int total = 0;
                var page = query.Pager(out total, condition.CurrentPage, condition.PageSize);

                //从字典中取名称
                if (page != null && page.Count > 0)
                {
                    SysDictService dictService = new SysDictService();
                    page.ForEach(i =>
                    {
                        i.GenderName = dictService.GetDictName(EnumDictType.UserGender, ((int)i.Gender).ToString());
                    });
                }

                var result = new Response<List<DoctorMemberDTO>>()
                {
                    Data = page,
                    Total = total
                };
                return result;
            }
        }

        /// <summary>
        /// 获取会员电子病历列表
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public Response<List<UserMemberEMRDTO>> GetDoctorMemberEMRs(
            string doctorID,
            string memberID,
            string doctormemberID,
            int pageIndex = 1,
            int pageSize = int.MaxValue,
            string keyword = null
           )
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = from m in db.UserMemberEMRs
                            join a in db.UserMembers on m.MemberID equals a.MemberID
                            join b in db.DoctorMembers on m.MemberID equals b.MemberID
                            join userfile in db.UserFiles.Where(a => a.IsDeleted == false).GroupBy(a => a.OutID) on m.UserMemberEMRID equals userfile.Key into leftJoinUserFiles
                            from userfileIfEmpty in leftJoinUserFiles.DefaultIfEmpty()
                            where b.DoctorID == doctorID && m.IsDeleted == false
                            orderby m.Date descending
                            select new UserMemberEMRDTO
                            {
                                UserMemberEMRID = m.UserMemberEMRID,
                                MemberID = m.MemberID,
                                DoctorMemberID = b.DoctorMemberID,
                                MemberName = a.MemberName,
                                Date = m.Date,
                                EMRName = m.EMRName,
                                HospitalName = m.HospitalName,
                                Remark = m.Remark,
                                Files = userfileIfEmpty.OrderBy(a => a.CreateTime).Select(a => new UserFileDTO
                                {
                                    FileUrl = a.FileUrl,
                                }).ToList()
                            };

                if (!string.IsNullOrEmpty(doctormemberID))
                {
                    query = query.Where(m => m.DoctorMemberID == doctormemberID);
                }

                if (!string.IsNullOrEmpty(memberID))
                {
                    query = query.Where(m => m.MemberID == memberID);
                }

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

        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="PatientUserID"></param>
        /// <param name="PatientMemberID"></param>
        /// <returns></returns>
        public ResponseUserMemberDTO GetMemberInfo(string DoctorID, string UserID, string PatientMemberID)
        {
            if (Exists(a => a.DoctorID == DoctorID && a.MemberID == PatientMemberID))
            {
                BLL.User.Implements.UserMemberService service = new User.Implements.UserMemberService();

                var member = service.GetMemberInfo(PatientMemberID);

                if(member == null)
                {
                    //兼容药店端看诊，MemberID非当前用户下的Member
                    BLL.User.Implements.UserService ser = new User.Implements.UserService();
                    var user = ser.GetUserInfoByUserId(UserID);
                    if(user != null && user.UserType == EnumUserType.Drugstore)
                    {
                        return service.GetMemberInfo(UserID, PatientMemberID);
                    }
                }
                return member;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 患者个人信息
        /// </summary>
        /// <param name="doctorMemberID"></param>
        /// <returns></returns>
        public UserMemberDTO GetMyMemberInfo(string doctorMemberID)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.DoctorMembers
                            join doctor in db.Doctors on item.DoctorID equals doctor.DoctorID
                            join member in db.UserMembers on item.MemberID equals member.MemberID
                            where item.IsDeleted == false && member.IsDeleted == false && item.DoctorMemberID == doctorMemberID && doctor.UserID == CurrentOperatorUserID
                            select member;

                var entity = query.FirstOrDefault();
                if (entity != null)
                {
                    var model = entity.Map<UserMember, UserMemberDTO>();
                    //性别名称
                    model.GenderName = new SysDictService().GetDictName(EnumDictType.UserGender, ((int)model.Gender).ToString());
                    return model;
                }
                return null;
            }
        }

        /// <summary>
        /// 患者的就诊记录
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Response<List<DTO.UserOPDRegisterDTO>> GetMyMemberVisitList(DoctorMemberCondition condition)
        {
            using (var db = new DBEntities())
            {
                //检查是否有存在该患者
                var member = GetMyMemberInfo(condition.DoctorMemberID);
                if (member == null)
                    return new Response<List<UserOPDRegisterDTO>>();

                //患者的就诊记录
                var response = new DoctorPatentService(CurrentOperatorUserID).GetPatientVisitList("", member.MemberID, condition.CurrentPage, condition.PageSize);
                if (response.Data != null)
                    response.Data.ForEach(i => i.Member = member);

                return response;
            }
        }

        /// <summary>
        /// 获取主诊医生和患者信息
        /// </summary>
        /// <param name="doctorMemberID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DoctorMemberDTO GetDoctorAndMember(string doctorMemberID, string userID)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.DoctorMembers
                            join doctor in db.Doctors on item.DoctorID equals doctor.DoctorID
                            join member in db.UserMembers on item.MemberID equals member.MemberID
                            where item.IsDeleted == false && member.IsDeleted == false && item.DoctorMemberID == doctorMemberID && doctor.UserID == userID
                            select new DoctorMemberDTO
                            {
                                DoctorID = doctor.DoctorID,
                                DoctorName = doctor.DoctorName,
                                MemberID = member.MemberID,
                                MemberName = member.MemberName,
                                ConsulServicePrice = (from s in db.DoctorServices
                                                      where s.DoctorID == doctor.DoctorID
                                                      && s.ServiceType == EnumDoctorServiceType.Consultation
                                                      && s.ServiceSwitch == true
                                                      && s.IsDeleted == false
                                                      select s.ServicePrice).FirstOrDefault(),
                            };

                var model = query.FirstOrDefault();
                return model;
            }
        }
        
        /// <summary>
        /// 我的患者(下拉框)
        /// </summary>
        /// <returns></returns>
        public Response<List<DoctorMemberDTO>> GetMyMemberDDL(DoctorMemberCondition condition)
        {
          
            var result = new Response<List<DoctorMemberDTO>>();
            using (var db = new DBEntities())
            {
                var query = from item in db.DoctorMembers
                            join member in db.UserMembers on item.MemberID equals member.MemberID
                            //join map in db.UserMemberMaps on member.MemberID equals map.MemberID
                            //join user in db.Users on map.UserID equals user.UserID
                            orderby item.ModifyTime, item.CreateTime descending
                            where item.IsDeleted == false && member.IsDeleted == false
                            && item.DoctorID == condition.DoctorID //&& map.IsDeleted == false
                            select new DoctorMemberDTO
                            {
                                MemberID = item.MemberID,
                                MemberName = member.MemberName,
                                Gender = member.Gender,
                                Birthday = member.Birthday,
                                Mobile = member.Mobile
                            };
                if (!string.IsNullOrEmpty(condition.PatientName))
                    query = query.Where(i => i.MemberName.Contains(condition.PatientName));

                int total = 0;
                result.Data = query.Pager(out total, condition.CurrentPage, condition.PageSize);
                result.Total = total;
            }

            if (result.Data != null)
            {
                var dictService = new SysDictService();
                result.Data.ForEach(i =>
                {
                    i.GenderName = dictService.GetDictName(EnumDictType.UserGender, ((int)i.Gender).ToString());
                });
            }
            return result;
        }
    }
}
