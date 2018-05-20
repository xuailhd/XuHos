using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Extensions;
using XuHos.DAL.EF;
using XuHos.DTO.Common;
using XuHos.DTO;

using XuHos.Entity;

namespace XuHos.BLL
{
    /// <summary>
    /// 会员电子病历相关业务处理
    /// </summary>
    public class UserMemberEMRService : Common.CommonBaseService<XuHos.Entity.UserMemberEMR>
    {
        public UserMemberEMRService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }

        public UserMemberEMRDTO GetUserMemberEMR(string id)
        {
            UserMemberEMRDTO ret = null;
            using (var db = new DBEntities())
            {
                ret = (from m in db.UserMemberEMRs
                           join a in db.UserMembers on m.MemberID equals a.MemberID
                           join userfile in db.UserFiles.Where(a => a.IsDeleted == false).GroupBy(a => a.OutID) on m.UserMemberEMRID equals userfile.Key into leftJoinUserFiles
                           from userfileIfEmpty in leftJoinUserFiles.DefaultIfEmpty()
                           where m.UserMemberEMRID == id
                           select new UserMemberEMRDTO
                           {
                               MemberID = m.MemberID,
                               Date = m.Date,
                               EMRName = m.EMRName,
                               UserMemberEMRID = m.UserMemberEMRID,
                               HospitalName = m.HospitalName,
                               MemberName = a.MemberName,
                               Remark = m.Remark,
                               ModifyTime = m.ModifyTime ?? m.CreateTime,
                               Files = userfileIfEmpty.OrderBy(a => a.CreateTime).Select(a => new UserFileDTO
                               {
                                   FileName = a.FileName,
                                   FileUrl = a.FileUrl,
                               }).ToList()
                           }).FirstOrDefault();
            }
            return ret;
        }

        /// <summary>
        /// 获取会员电子病历列表
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public Response<List<UserMemberEMRDTO>> GetPageList(
            string userID,
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
                            join userfile in db.UserFiles.Where(a => a.IsDeleted == false).GroupBy(a => a.OutID) on m.UserMemberEMRID equals userfile.Key into leftJoinUserFiles
                            from userfileIfEmpty in leftJoinUserFiles.DefaultIfEmpty()
                            where a.UserID == userID && m.IsDeleted == false
                            orderby m.Date descending
                            select new UserMemberEMRDTO
                            {
                                UserMemberEMRID = m.UserMemberEMRID,
                                MemberID = m.MemberID,
                                MemberName = a.MemberName,
                                Date = m.Date,
                                EMRName = m.EMRName,
                                Remark = m.Remark,
                                HospitalName = m.HospitalName,
                                ModifyTime = m.ModifyTime ?? m.CreateTime,
                                Files = userfileIfEmpty.OrderBy(a => a.CreateTime).Select(a => new UserFileDTO
                                {
                                    FileName = a.FileName,
                                    FileUrl = a.FileUrl,
                                }).ToList()
                            };
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(m => m.EMRName.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(memberId))
                {
                    query = query.Where(m => m.MemberID == memberId);
                }

                Response<List<UserMemberEMRDTO>> result = new Response<List<UserMemberEMRDTO>>();
                int total = 0;
                result.Data = query.Pager(out total, pageIndex, pageSize);
                result.Total = total;
                return result;
            }
        }

        public ApiResult Save(UserMemberEMRDTO request)
        {
            ApiResult result = new ApiResult();

            if(string.IsNullOrEmpty(request.UserMemberEMRID))
            {
                var model = request.Map<UserMemberEMRDTO, XuHos.Entity.UserMemberEMR>();
                if(Insert(model) && request.Files != null)
                {
                    request.UserMemberEMRID = model.UserMemberEMRID;
                    XuHos.BLL.Sys.Implements.FileService fService = new XuHos.BLL.Sys.Implements.FileService();
                    foreach (var f in request.Files)
                    {
                        f.FileUrl = f.FileUrl;
                        f.Remark = "-";//必填项？
                        var file = f.Map<UserFileDTO, UserFile>();
                        file.OutID = model.UserMemberEMRID;
                        file.UserID = this.CurrentOperatorUserID;
                        file.AccessKey = file.ResourceID = string.Empty;
                        fService.Insert(file);
                    }
                }
            }
            else
            {
                var model = this.DBEntities.UserMemberEMRs.Where(m=> m.UserMemberEMRID == request.UserMemberEMRID).FirstOrDefault();
                if(model != null)
                {
                    model.EMRName = request.EMRName;
                    model.HospitalName = request.HospitalName;
                    model.MemberID = request.MemberID;
                    model.Remark = request.Remark;
                    model.Date = request.Date;

                    if (Update(model))
                    {
                        XuHos.BLL.Sys.Implements.FileService fService = new Sys.Implements.FileService();
                        var ids = request.Files.Select(m => m.FileID);
                        fService.Delete(m => ids.Contains(m.FileID) == false && m.OutID == model.UserMemberEMRID && m.IsDeleted == false);
                        if (request.Files != null)
                        {
                            foreach (var f in request.Files)
                            {
                                if(string.IsNullOrEmpty(f.FileID))
                                {
                                    f.FileUrl = f.FileUrl;
                                    f.Remark = "-";//必填项？
                                    var file = f.Map<UserFileDTO, UserFile>();
                                    file.OutID = model.UserMemberEMRID;
                                    file.UserID = this.CurrentOperatorUserID;
                                    file.AccessKey = file.ResourceID = string.Empty;
                                    fService.Insert(file);
                                }
                            }
                        }
                    }
                }
            }
            result.Data = GetUserMemberEMR(request.UserMemberEMRID);
            return result;
        }

        public override bool Delete(string ID)
        {
            XuHos.BLL.Sys.Implements.FileService fService = new Sys.Implements.FileService();
            fService.Delete(m => m.OutID == ID && m.IsDeleted == false);
            return base.Delete(ID);
        }
    }
}
