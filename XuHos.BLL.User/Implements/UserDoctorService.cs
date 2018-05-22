using XuHos.Common;
using XuHos.DAL.EF;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Entity;
using XuHos.BLL.User.DTOs.Response;

namespace XuHos.BLL.User.Implements
{
    /// <summary>
    /// 用户收藏相关业务
    
    /// 日期：2016年8月11日
    /// </summary>
    public class UserDoctorService
    {

        /// <summary>
        /// 我的医生(与医生收藏合并)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="hosiptalId"></param>
        public Response<List<ResponseUserCollectedDoctorDTO>> GetMyVisitDoctors(string userID, int CurrentPage = 1, int PageSize = 10, string hospitalId = "")
        {
            hospitalId = hospitalId ?? "";
            var result = new Response<List<ResponseUserCollectedDoctorDTO>>();
            using (var db = new DBEntities())
            {


                var query = from doctorMember in (from doctorMember in db.DoctorMembers
                                                  group doctorMember by new { doctorMember.DoctorID } into gps
                                                  select new { gps.Key.DoctorID })
                            join doc in db.Doctors.Where(a => !a.IsDeleted) on doctorMember.DoctorID equals doc.DoctorID
                            join user in db.Users on doc.UserID equals user.UserID
                            join his in db.Hospitals.Where(a => !a.IsDeleted) on doc.HospitalID equals his.HospitalID
                            join depart in db.HospitalDepartments.Where(a => !a.IsDeleted) on doc.DepartmentID equals depart.DepartmentID
                            where hospitalId == "" || doc.HospitalID == hospitalId
                            orderby doc.Sort descending
                            select new ResponseUserCollectedDoctorDTO
                            {
                                DoctorID = doc.DoctorID,
                                DoctorName = doc.DoctorName,
                                HospitalID = his.HospitalID,
                                HospitalName = his.HospitalName,
                                DepartmentName = depart.DepartmentName,
                                DepartmentID = depart.DepartmentID,
                                Gender = doc.Gender.ToString(),
                                Portait = user.PhotoUrl,
                                Title = doc.Title,
                                Position = doc.Title,
                                IsExpert = doc.IsExpert,
                                Specialties = doc.Specialty
                            };

                int total = 0;
                result.Data = query.Pager(out total, CurrentPage, PageSize);
                result.Total = total;
                if (result.Data != null && result.Data.Count > 0)
                {
                    result.Data.ForEach(item =>
                    {
                        item.Gender = Sys.Implements.CodeService.GetGenders()[item.Gender];
                        item.Position = item.Title = Sys.Implements.CodeService.GetDoctorTitles()[item.Title];
                        item.Specialties = item.Specialties.Replace('\t', ',');
                    });
                }
            }

            return result;

        }
    }
}
