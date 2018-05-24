using XuHos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Cache;
using XuHos.DAL;
using XuHos.BLL.Doctor.DTOs.Response;
using XuHos.DTO.Common;
using XuHos.BLL.Doctor.DTOs.Request;
using System.Text.RegularExpressions;
using XuHos.DAL.EF;
using EntityFramework.Extensions;
using XuHos.Common.Enum;
using XuHos.Extensions;
using System.Data.Entity;
using EntityFramework.Caching;
using XuHos.BLL.Sys;
using static XuHos.BLL.Doctor.DTOs.Response.ResponseDoctorDTO;
using XuHos.Common;
using XuHos.Common.Cache.Keys;
using XuHos.BLL.Sys.Implements;
using XuHos.DTO;
using XuHos.DTO.Response;
using ResponseDoctorSerivceTypeIncomeDTO = XuHos.BLL.Doctor.DTOs.Response.ResponseDoctorSerivceTypeIncomeDTO;
using ResponseServiceEvaluationDTO = XuHos.BLL.Doctor.DTOs.Response.ResponseServiceEvaluationDTO;
using ResponseServiceProviderEvaluatedTagDTO = XuHos.BLL.Doctor.DTOs.Response.ResponseServiceProviderEvaluatedTagDTO;
using System.Linq.Dynamic;

namespace XuHos.BLL.Doctor.Implements
{
    public class DoctorService
    {
        #region Query

        #region 分页查询

        /// <summary>
        /// 医生列表(新，V4.5版)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorList(RequestDoctorSearchDTO condition)
        {
            var month = DateTime.Now.ToString("yyyyMM");
            var day = DateTime.Now.ToString("dd");

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join hosp in db.Hospitals on doctor.HospitalID equals hosp.HospitalID
                             join department in db.HospitalDepartments on doctor.DepartmentID equals department.DepartmentID
                             where doctor.IsDeleted == false && doctor.IsShow
                             select new ResponseDoctorDTO
                             {
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Title = doctor.Title,
                                 HospitalID = doctor.HospitalID,
                                 DepartmentID = doctor.DepartmentID,
                                 DepartmentName = department.DepartmentName,
                                 Specialty = doctor.Specialty,
                                 Duties = doctor.Duties,
                                 CAT_NO = department.PARENT_CAT_NO,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = doctor.DoctorID,
                                 },
                                 
                             });

                //过滤条件
                if (condition.CAT_NOs != null && condition.CAT_NOs.Count > 0)
                {
                    query = query.Where(i => condition.CAT_NOs.Contains(i.CAT_NO));
                }
                //过滤条件
                if (condition.DepartmentIDs != null && condition.DepartmentIDs.Count > 0)
                {
                    query = query.Where(i => condition.DepartmentIDs.Contains(i.DepartmentID));
                }
                if (condition.DoctorTitles != null && condition.DoctorTitles.Count > 0)
                {
                    query = query.Where(i => condition.DoctorTitles.Contains(i.Title));
                }
                if (condition.IsFreeClinicr == true)
                {
                    query = query.Where(i => i.IsFreeClinicr == true);
                }
                if (condition.IsFamilyDoctor == true)
                {
                    query =
                        query.Where(
                            i =>
                                i.DoctorServices.Where(
                                    j => j.ServiceType == EnumDoctorServiceType.FamilyDoctor && j.ServiceSwitch == 1)
                                    .FirstOrDefault() != null);
                }
                if (!string.IsNullOrEmpty(condition.Keyword))
                {
                    query =
                        query.Where(
                            i =>
                                i.DoctorName.Contains(condition.Keyword) || i.HospitalName.Contains(condition.Keyword) ||
                                i.DepartmentName.Contains(condition.Keyword));
                }

                //排序
                switch (condition.SortFiled)
                {
                    case EnumDoctorOrderBy.ServicePrice:
                        if (condition.IsFamilyDoctor == true)
                            query = condition.SortIsAsc
                                ? query.OrderBy(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.FamilyDoctor).ServicePrice)
                                : query.OrderByDescending(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.FamilyDoctor).ServicePrice);
                        else
                            query = condition.SortIsAsc
                                ? query.OrderBy(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.PicServiceType).ServicePrice)
                                : query.OrderByDescending(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.PicServiceType).ServicePrice);
                        break;
                    default:
                        break;
                }

                query = (query as IOrderedQueryable<ResponseDoctorDTO>).ThenBy(i => i.DoctorID);

                int Total = 0;
                var Data = query.Pager(out Total, condition.CurrentPage, condition.PageSize);

                Data.ForEach(o =>
                {
                    #region 医生职称

                    int intTitle;
                    if (int.TryParse(o.Title, out intTitle))
                    {
                        o.Title = ((EnumDoctorTitle)Convert.ToInt32(o.Title)).GetEnumDescript();
                    }

                    #endregion

                    #region 计算医生排班

                    var State = GetDoctorServiceScheduleState(o.DoctorID);
                    foreach (var item in o.DoctorServices)
                    {
                        if (item.ServiceSwitch == 1 &&
                            (item.ServiceType == EnumDoctorServiceType.VidServiceType ||
                             item.ServiceType == EnumDoctorServiceType.AudServiceType))
                        {
                            item.HasSchedule = State;
                        }
                        else
                        {
                            item.HasSchedule = false;
                        }
                    }

                    #endregion

                });

                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };
            }
        }

        /// <summary>
        /// 医生列表(新，V4.5版)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetHospitalDoctors(RequestHospitalDoctorSearchDTO condition)
        {
            var month = DateTime.Now.ToString("yyyyMM");
            var day = DateTime.Now.ToString("dd");

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                if (!string.IsNullOrWhiteSpace(condition.DepartmentID))
                {
                    var obj = db.HospitalDepartments.Where(w => w.DepartmentID == condition.DepartmentID).Select(m => new { m.CAT_NO, m.PARENT_CAT_NO, m.HospitalID }).FirstOrDefault();
                    if (obj == null)
                    {
                        throw new Exception("DepartmentID不存在");
                    }
                    condition.HospitalID = obj.HospitalID;
                    condition.PARENT_CAT_NO = obj.PARENT_CAT_NO;
                    condition.CAT_NO = obj.CAT_NO;
                }
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join hosp in db.Hospitals on doctor.HospitalID equals hosp.HospitalID
                             join department in db.HospitalDepartments on doctor.DepartmentID equals department.DepartmentID
                             where doctor.IsDeleted == false && doctor.IsShow
                             select new ResponseDoctorDTO
                             {
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Title = doctor.Title,
                                 HospitalID = doctor.HospitalID,
                                 DepartmentID = doctor.DepartmentID,
                                 DepartmentName = department.DepartmentName,
                                 Specialty = doctor.Specialty,
                                 Duties = doctor.Duties,
                                 CAT_NO = department.CAT_NO,
                                 PARENT_CAT_NO = department.PARENT_CAT_NO,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = doctor.DoctorID,
                                 },
                             });

                //过滤条件
                if (!string.IsNullOrWhiteSpace(condition.HospitalID))
                {
                    IQueryable<string> qMultiSpotDoctorsIds = from m in db.Doctors
                                                              where condition.HospitalID == m.HospitalID && m.IsDeleted == false
                                                              select m.DoctorID;
                    query = query.Where(i => condition.HospitalID == i.HospitalID || qMultiSpotDoctorsIds.Contains(i.DoctorID));
                }
                if (!string.IsNullOrWhiteSpace(condition.CAT_NO))
                {
                    query = query.Where(i => condition.CAT_NO == i.CAT_NO);
                }
                else if (!string.IsNullOrWhiteSpace(condition.PARENT_CAT_NO))
                {
                    query = query.Where(i => condition.PARENT_CAT_NO == i.PARENT_CAT_NO);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(condition.DepartmentID))
                    {
                        query = query.Where(i => condition.DepartmentID == i.DepartmentID);
                    }
                }
                if (condition.DoctorTitles != null && condition.DoctorTitles.Count > 0)
                {
                    query = query.Where(i => condition.DoctorTitles.Contains(i.Title));
                }
                if (condition.IsFreeClinicr == true)
                {
                    query = query.Where(i => i.IsFreeClinicr == true);
                }
                if (condition.IsFamilyDoctor == true)
                {
                    query =
                        query.Where(
                            i =>
                                i.DoctorServices.Where(
                                    j => j.ServiceType == EnumDoctorServiceType.FamilyDoctor && j.ServiceSwitch == 1)
                                    .FirstOrDefault() != null);
                }
                if (!string.IsNullOrEmpty(condition.Keyword))
                {
                    query =
                        query.Where(
                            i =>
                                i.DoctorName.Contains(condition.Keyword) || i.HospitalName.Contains(condition.Keyword) ||
                                i.DepartmentName.Contains(condition.Keyword));
                }

                //排序
                switch (condition.SortFiled)
                {
                    case EnumDoctorOrderBy.ServicePrice:
                        if (condition.IsFamilyDoctor == true)
                            query = condition.SortIsAsc
                                ? query.OrderBy(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.FamilyDoctor).ServicePrice)
                                : query.OrderByDescending(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.FamilyDoctor).ServicePrice);
                        else
                            query = condition.SortIsAsc
                                ? query.OrderBy(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.PicServiceType).ServicePrice)
                                : query.OrderByDescending(
                                    i =>
                                        i.DoctorServices.FirstOrDefault(
                                            j => j.ServiceType == EnumDoctorServiceType.PicServiceType).ServicePrice);
                        break;
                    default:
                        break;
                }

                query = (query as IOrderedQueryable<ResponseDoctorDTO>).ThenBy(i => i.DoctorID);

                int Total = 0;
                var Data = query.Pager(out Total, condition.CurrentPage, condition.PageSize);

                Data.ForEach(o =>
                {
                    #region 医生职称

                    int intTitle;
                    if (int.TryParse(o.Title, out intTitle))
                    {
                        o.Title = ((EnumDoctorTitle)Convert.ToInt32(o.Title)).GetEnumDescript();
                    }

                    #endregion

                    #region 计算医生排班

                    var State = GetDoctorServiceScheduleState(o.DoctorID);
                    foreach (var item in o.DoctorServices)
                    {
                        if (item.ServiceSwitch == 1 &&
                            (item.ServiceType == EnumDoctorServiceType.VidServiceType ||
                             item.ServiceType == EnumDoctorServiceType.AudServiceType))
                        {
                            item.HasSchedule = State;
                        }
                        else
                        {
                            item.HasSchedule = false;
                        }
                    }

                    #endregion

                });

                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };
            }
        }
        public ResponseMedicalLibraryDoctorPageDataDTO GetMedicalLibraryDoctorPageData(string DoctorID)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join hosp in db.Hospitals on doctor.HospitalID equals hosp.HospitalID
                             join department in db.HospitalDepartments on doctor.DepartmentID equals department.DepartmentID
                             //join data in db.DoctorStatisticalDatas on doctor.DoctorID equals data.DoctorID into dataLeftMid
                             //from dataLeft in dataLeftMid.DefaultIfEmpty()
                             where doctor.DoctorID == DoctorID && doctor.IsDeleted == false
                             select new ResponseMedicalLibraryDoctorPageDataDTO
                             {
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Title = doctor.Title,
                                 HospitalID = doctor.HospitalID,
                                 HospitalName = hosp.HospitalName,
                                 HomePageTheme = hosp.HomePageTheme,
                                 HospitalLogoUrl = hosp.LogoUrl,
                                 DepartmentID = doctor.DepartmentID,
                                 DepartmentName = department.DepartmentName,
                                 Specialty = doctor.Specialty,
                                 Intro = doctor.Intro,
                                 PhotoUrl = user.PhotoUrl,
                                 //FollowNum = dataLeft == null ? 0 : dataLeft.BaseFollowed + dataLeft.Followed,
                                 //DiagnoseNum = dataLeft == null ? 0 : dataLeft.BaseImageText + dataLeft.ImageText + dataLeft.BaseAudio + dataLeft.Audio + dataLeft.BaseVideo + dataLeft.Video
                             });

                var model = query.FirstOrDefault();
                return model;

            }
        }
        /// <summary>
        /// 医生详情(V4.5版)
        /// </summary>
        /// <param name="doctorID"></param>
        /// <returns></returns>
        public ResponseDoctorDTO GetDoctorDetail(string doctorID)
        {
            var month = DateTime.Now.ToString("yyyyMM");
            var day = DateTime.Now.ToString("dd");

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join hosp in db.Hospitals on doctor.HospitalID equals hosp.HospitalID
                             join department in db.HospitalDepartments on doctor.DepartmentID equals department.DepartmentID
                             where doctor.DoctorID == doctorID && doctor.IsDeleted == false
                             select new ResponseDoctorDTO
                             {
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Title = doctor.Title,
                                 HospitalID = doctor.HospitalID,
                                 DepartmentID = doctor.DepartmentID,
                                 DepartmentName = department.DepartmentName,
                                 Specialty = doctor.Specialty,
                                 Intro = doctor.Intro,
                                 Duties = doctor.Duties,
                                 CAT_NO = department.CAT_NO,
                                 DiseaseLabel = doctor.DiseaseLabel,
                                 CertificateNo = doctor.CertificateNo,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = doctor.DoctorID,
                                     Mobile = user.Mobile
                                 },
                             });

                var model = query.FirstOrDefault();

                if (model != null)
                {
                    #region 医生服务和音视频排班

                    if (model.DoctorServices == null || model.DoctorServices.Count == 0)
                    {
                        model.DoctorServices = new List<ResponseDoctorServiceDTO>();
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.AudServiceType
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.FamilyDoctor
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.PicServiceType
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.VidServiceType
                        });
                    }
                    //音视频排班
                    var State = GetDoctorServiceScheduleState(doctorID);
                    foreach (var item in model.DoctorServices)
                    {
                        if (item.ServiceSwitch == 1 &&
                            (item.ServiceType == EnumDoctorServiceType.VidServiceType ||
                             item.ServiceType == EnumDoctorServiceType.AudServiceType))
                        {
                            item.HasSchedule = State;
                        }
                        else
                        {
                            item.HasSchedule = false;
                        }
                    }

                    #endregion
                }

                return model;

            }
        }

        /// <summary>
        /// 推荐医生(V4.5版)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetRecommendDoctorList(RequestRecommendDoctorSearchDTO condition)
        {
            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             where doctor.IsDeleted == false && doctor.IsShow
                             orderby doctor.Sort descending, doctor.CreateTime, doctor.DoctorID
                             select new ResponseDoctorDTO
                             {
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 DepartmentID = doctor.DepartmentID,
                                 HospitalID = doctor.HospitalID,
                                 Title = doctor.Title,
                                 Specialty = doctor.Specialty,
                                 IsExpert = doctor.IsExpert,
                                 DoctorType = doctor.DoctorType,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = user.UserID,
                                 }
                             });

                //查询条件
                if (!string.IsNullOrEmpty(condition.HospitalID))
                    query = query.Where(i => i.HospitalID == condition.HospitalID);

                if (!string.IsNullOrEmpty(condition.DepartmentID))
                    query = query.Where(i => i.DepartmentID == condition.DepartmentID);

                if (condition.DoctorType.HasValue && condition.DoctorType.Value > 0)
                    query = query.Where(i => i.DoctorType == condition.DoctorType.Value);

                if (!string.IsNullOrEmpty(condition.CurrentDoctorID))
                    query = query.Where(i => i.DoctorID != condition.CurrentDoctorID); //排除当前医生

                //if (condition.IsExpert.HasValue)
                //    query = query.Where(i => i.IsExpert == condition.IsExpert.Value);

                //分页
                int Total = 0;
                var Data = query.Pager<ResponseDoctorDTO>(out Total, condition.CurrentPage, condition.PageSize);

                #region 医生职称

                Data.ForEach(o =>
                {
                    int intTitle;
                    if (int.TryParse(o.Title, out intTitle))
                    {
                        o.Title = ((EnumDoctorTitle)Convert.ToInt32(o.Title)).GetEnumDescript();
                    }
                });

                #endregion

                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };

            }

        }

        /// <summary>
        /// 获取所有开通 图文问诊，视频问诊，语音问诊的 医生 列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorPagerListForAllOnline(int page, int pagesize,
            string keyword = null)
        {
            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join docservice in db.DoctorServices on doctor.DoctorID equals docservice.DoctorID
                             where ((docservice.ServiceType == EnumDoctorServiceType.VidServiceType && docservice.ServiceSwitch)
                                    ||
                                    (docservice.ServiceType == EnumDoctorServiceType.AudServiceType && docservice.ServiceSwitch)
                                    ||
                                    (docservice.ServiceType == EnumDoctorServiceType.PicServiceType && docservice.ServiceSwitch))
                                   && !doctor.IsDeleted && !user.IsDeleted && doctor.IsShow
                             select new ResponseDoctorDTO
                             {
                                 areaCode = doctor.areaCode,
                                 Birthday = doctor.Birthday,
                                 Address = doctor.Address,
                                 CheckState = doctor.CheckState,
                                 DepartmentID = doctor.DepartmentID,
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Duties = doctor.Duties,
                                 HospitalID = doctor.HospitalID,
                                 Marriage = doctor.Marriage,
                                 Sort = doctor.Sort,
                                 Specialty = doctor.Specialty,
                                 Title = doctor.Title,
                                 DoctorType = doctor.DoctorType,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = user.UserID,
                                 }
                             });
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(m => m.DoctorName.Contains(keyword));
                }

                int Total = 0;
                var Data = query.Distinct()
                    .OrderByDescending(t => t.Sort)
                    .Pager<ResponseDoctorDTO>(out Total, page, pagesize);
                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };
            }
        }

        /// <summary>
        /// 后台获取医生列表
        /// 作者：杨必云
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorPagerListForExpertDoctor(int PageIndex = 1, int PageSize = int.MaxValue, string hospitalId = "")
        {
            hospitalId = hospitalId ?? "";
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             where
                                 doctor.IsExpert == true && doctor.IsDeleted == false && doctor.IsShow &&
                                 (hospitalId == "" || doctor.HospitalID == hospitalId)
                             orderby doctor.Sort descending

                             select new ResponseDoctorDTO
                             {
                                 areaCode = doctor.areaCode,
                                 Birthday = doctor.Birthday,
                                 Address = doctor.Address,
                                 CheckState = doctor.CheckState,
                                 DepartmentID = doctor.DepartmentID,
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Duties = doctor.Duties,
                                 HospitalID = doctor.HospitalID,
                                 Marriage = doctor.Marriage,
                                 Title = doctor.Title,
                                 Specialty = doctor.Specialty,
                                 DoctorType = doctor.DoctorType,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = doctor.DoctorID,
                                 },
                             });

                Response<List<ResponseDoctorDTO>> result = new Response<List<ResponseDoctorDTO>>();
                int Total = 0;
                var Data = query.Pager<ResponseDoctorDTO>(out Total, PageIndex, PageSize);
                Data.ForEach(o =>
                {
                    int intTitle;
                    if (int.TryParse(o.Title, out intTitle))
                    {
                        o.Title = ((EnumDoctorTitle)Convert.ToInt32(o.Title)).GetEnumDescript();
                    }

                });
                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };
            }
        }


        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorPageList(RequestDoctorSelectDTO condition, bool filterWeb = true, List<string> userPkgs = null)
        {
            condition.HospitalId = condition.HospitalId ?? "";
            condition.Keyword = condition.Keyword ?? "";
            condition.DepartmentName = condition.DepartmentName ?? "";
            condition.DepartmentKeyword = condition.DepartmentKeyword ?? "";
            condition.HospitalName = condition.HospitalName ?? "";
            condition.HospitalKeyword = condition.HospitalKeyword ?? "";
            string scheduleDate = condition.ScheduleDate.HasValue ? condition.ScheduleDate.Value.ToString("yyyyMMdd") : "";

            string queryPredicate = "IsDeleted = @0";
            List<object> paramValues = new List<object>() { false };
            if (condition.IncludePkgStatus && userPkgs != null)
            {
                paramValues.AddRange(userPkgs);
                int index = 1;
                foreach (var item in userPkgs)
                {
                    queryPredicate += $" OR UserPackageID = @{index}";
                    index++;
                };
            }

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join dept in db.HospitalDepartments on doctor.DepartmentID equals dept.DepartmentID
                             join hosp in db.Hospitals on doctor.HospitalID equals hosp.HospitalID                        
                                 // 不是所有的科室都有关联公共科室,用左关联
                             orderby doctor.DoctorType descending, doctor.Sort descending
                             where (condition.Keyword == "" || (doctor.DoctorName.Contains(condition.Keyword)))
                                   && (condition.HospitalId == "" || doctor.HospitalID == condition.HospitalId)
                                   && (condition.DepartmentName == "" )
                                   && (condition.DepartmentKeyword == "" || doctor.DoctorName.Contains(condition.DepartmentKeyword) || hosp.HospitalName.Contains(condition.DepartmentKeyword))
                                   && (condition.HospitalName == "" || hosp.HospitalName == condition.HospitalName)
                                   && (condition.HospitalKeyword == "" || hosp.HospitalName.Contains(condition.HospitalKeyword))
                                   && doctor.IsDeleted == false && (!filterWeb || doctor.IsShow)
                             select new ResponseDoctorDTO
                             {
                                 areaCode = doctor.areaCode,
                                 Birthday = doctor.Birthday,
                                 Address = doctor.Address,
                                 CheckState = doctor.CheckState,
                                 DepartmentID = dept.DepartmentID,
                                 DoctorID = doctor.DoctorID,
                                 DepartmentName = dept.DepartmentName,
                                 DoctorName = doctor.DoctorName,
                                 Duties = doctor.Duties,
                                 HospitalID = hosp.HospitalID,
                                 HospitalName = hosp.HospitalName,
                                 Marriage = doctor.Marriage,
                                 Title = doctor.Title,
                                 Education = doctor.Education,
                                 Specialty = doctor.Specialty,
                                 Intro = doctor.Intro,
                                 Grade = doctor.Grade,
                                 DoctorType = doctor.DoctorType,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = user.UserID,
                                 },
                             });


                // 排序规则
                if (condition.OrderBy != null)
                {
                    bool orderFlag = false;
                    for (int i = 0; i < condition.OrderBy.Count; i++)
                    {
                        switch (condition.OrderBy[i])
                        {
                            case EnumDoctorOrderBy.Together:
                                query = query.OrderByDescending(x => x.IsScheduleExist).ThenByDescending(x => x.IsPackageExist).ThenByDescending(x => x.EvaluationScore);
                                break;

                            case EnumDoctorOrderBy.ScheduleStatus:
                                if (orderFlag)
                                    query = (query as IOrderedQueryable<ResponseDoctorDTO>).ThenByDescending(x => x.IsScheduleExist);
                                else
                                    query = query.OrderByDescending(x => x.IsScheduleExist);
                                break;

                            case EnumDoctorOrderBy.PackageStatus:
                                if (orderFlag)
                                    query = (query as IOrderedQueryable<ResponseDoctorDTO>).ThenByDescending(x => x.IsPackageExist);
                                else
                                    query = query.OrderByDescending(x => x.IsPackageExist);
                                break;

                            case EnumDoctorOrderBy.EvaluationScore:
                                if (orderFlag)
                                    query = (query as IOrderedQueryable<ResponseDoctorDTO>).ThenByDescending(x => x.EvaluationScore);
                                else
                                    query = query.OrderByDescending(x => x.EvaluationScore);
                                break;

                            case EnumDoctorOrderBy.RepliedCount:
                                if (orderFlag)
                                    query = (query as IOrderedQueryable<ResponseDoctorDTO>).ThenByDescending(x => x.ReplyCount);
                                else
                                    query = query.OrderByDescending(x => x.ReplyCount);

                                break;
                        }
                    }
                }
                else
                {
                    query = query.OrderByDescending(x => x.IsScheduleExist).ThenByDescending(x => x.IsPackageExist).ThenByDescending(x => x.EvaluationScore);
                }               
                   

                Response<List<ResponseDoctorDTO>> result = new Response<List<ResponseDoctorDTO>>();
                int total = 0;
                result.Data = query.Pager(out total, condition.CurrentPage, condition.PageSize);
                var sdService = new SysDictService();
                result.Data.ForEach(model =>
                {
                    if (model.DoctorServices == null || model.DoctorServices.Count == 0)
                    {
                        model.DoctorServices = new List<ResponseDoctorServiceDTO>();
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.AudServiceType
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.FamilyDoctor
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.PicServiceType
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.VidServiceType
                        });
                    }
                    model.EvaluationScore = model.EvaluationScore.HasValue ? model.EvaluationScore.Value * 2 : 0;
                    if (!string.IsNullOrEmpty(model.Education))
                    {
                        model.Education = sdService.GetDictName(EnumDictType.Education, model.Education);
                    }

                });

                result.Total = total;
                return result;
            }
        }

        /// <summary>
        /// 获取 服务收入
        /// </summary>
        /// <param name="doctorid"></param>
        /// <returns></returns>
        public decimal GetServiceIncome(string doctorid)
        {
            var ServiceIncome_CacheKey =
                new XuHos.Common.Cache.Keys.StringCacheKey(
                    XuHos.Common.Cache.Keys.StringCacheKeyType.Doctor_ServiceIncome, doctorid);
            var ServiceIncome = ServiceIncome_CacheKey.FromCache<decimal?>();

            if (ServiceIncome == null)
            {
                var list = GetDoctorSerivceTypeIncomes(doctorid);
                ServiceIncome = list.Sum(m => m.Income);
                ServiceIncome.ToCache(ServiceIncome_CacheKey, TimeSpan.FromDays(1));
            }

            return ServiceIncome.Value;
        }


        /// <summary>
        /// 订单完成后刷新医生缓存&发送统计信息给医生
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool CleanDoctorCacheAndSendNoticeToDoctor(string orderNo)
        {
            using (var db = new DBEntities())
            {
                var ordermodel = db.Orders.Where(t => t.OrderNo == orderNo).FirstOrDefault();

                if (ordermodel != null)
                {
                    string noticeDoctorId = null;
                    //看诊 删除 诊断次数，收入，服务次数
                    if (ordermodel.OrderType == EnumProductType.Registration ||
                        ordermodel.OrderType == EnumProductType.Phone ||
                        ordermodel.OrderType == EnumProductType.video)
                    {
                        var opd = (from opdRegister in db.UserOpdRegisters.Where(t => t.OPDRegisterID == ordermodel.OrderOutID)
                                   select new { opdRegister.DoctorID }).FirstOrDefault();

                        if (opd != null && !string.IsNullOrEmpty(opd.DoctorID))
                        {
                            noticeDoctorId = opd.DoctorID;
                            var DiagnoseNum_CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(
                                XuHos.Common.Cache.Keys.StringCacheKeyType.Doctor_DiagnoseNum, opd.DoctorID);
                            DiagnoseNum_CacheKey.RemoveCache();

                            var ServiceNum_CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(
                                XuHos.Common.Cache.Keys.StringCacheKeyType.Doctor_ServiceNum, opd.DoctorID);
                            ServiceNum_CacheKey.RemoveCache();

                            var ServiceIncome_CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(
                                XuHos.Common.Cache.Keys.StringCacheKeyType.Doctor_ServiceIncome, opd.DoctorID);
                            ServiceIncome_CacheKey.RemoveCache();
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 后台获取医生列表
        /// 作者：杨必云
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorPagerListForHospital(string HospitalId,
            int PageIndex = 1,
            int PageSize = int.MaxValue

            )
        {

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join dept in db.HospitalDepartments on doctor.DepartmentID equals dept.DepartmentID
                             join hosp in db.Hospitals on doctor.HospitalID equals hosp.HospitalID
                             where doctor.IsDeleted == false && hosp.HospitalID == HospitalId
                             orderby doctor.Sort descending

                             select new ResponseDoctorDTO
                             {
                                 areaCode = doctor.areaCode,
                                 Birthday = doctor.Birthday,
                                 Address = doctor.Address,
                                 CheckState = doctor.CheckState,
                                 DepartmentID = doctor.DepartmentID,
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Duties = doctor.Duties,
                                 HospitalID = doctor.HospitalID,
                                 Marriage = doctor.Marriage,
                                 Title = doctor.Title,
                                 Specialty = doctor.Specialty,
                                 DoctorType = doctor.DoctorType,
                                 User = new ResopnseUserDTO
                                 {
                                     UserType = user.UserType,
                                     PhotoUrl = user.PhotoUrl,
                                     UserID = doctor.DoctorID,
                                 }
                             });

                Response<List<ResponseDoctorDTO>> result = new Response<List<ResponseDoctorDTO>>();
                int Total = 0;
                var Data = query.Pager<ResponseDoctorDTO>(out Total, PageIndex, PageSize);
                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };
            }
        }

        /// <summary>
        /// 获取处方医生列表
        /// 作者：许光丽
        /// 日期：2016年9月14日
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorPageListForDrugstore(
            int PageIndex = 1,
            int PageSize = int.MaxValue
            )
        {

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join user in db.Users on doctor.DoctorID equals user.UserID
                             join dept in db.HospitalDepartments on doctor.DepartmentID equals dept.DepartmentID
                             join hosp in db.Hospitals on doctor.HospitalID equals hosp.HospitalID
                             join rolemap in db.UserRoleMaps on user.UserID equals rolemap.UserID
                             join role in db.UserRoles on rolemap.RoleID equals role.RoleID
                             orderby doctor.Sort descending
                             where !doctor.IsDeleted && !rolemap.IsDeleted && role.RoleType == EnumRoleType.DocRecipe
                             select new ResponseDoctorDTO
                             {
                                 areaCode = doctor.areaCode,
                                 Birthday = doctor.Birthday,
                                 Address = doctor.Address,
                                 CheckState = doctor.CheckState,
                                 DepartmentID = dept.DepartmentID,
                                 DoctorID = doctor.DoctorID,
                                 DepartmentName = dept.DepartmentName,
                                 DoctorName = doctor.DoctorName,
                                 Duties = doctor.Duties,
                                 HospitalID = hosp.HospitalID,
                                 HospitalName = hosp.HospitalName,
                                 Marriage = doctor.Marriage,
                                 Title = doctor.Title,
                                 Specialty = doctor.Specialty,
                                 DoctorType = doctor.DoctorType,
                                 User = new ResopnseUserDTO
                                 {
                                     PhotoUrl = user.PhotoUrl
                                 }
                             });

                Response<List<ResponseDoctorDTO>> result = new Response<List<ResponseDoctorDTO>>();
                int total = 0;
                result.Data = query.Pager(out total, PageIndex, PageSize);
                result.Total = total;
                return result;
            }
        }


        /// <summary>
        /// 获取推荐医生
        /// </summary>
        /// <param name="Take"></param>
        /// <param name="CAT_NO"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorPageListForRecommend(int Take, string CAT_NO)
        {
            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = from doctor in db.Doctors
                            join department in db.HospitalDepartments on doctor.DepartmentID equals department.DepartmentID
                            join user in db.Users on doctor.DoctorID equals user.UserID
                            where doctor.IsDeleted == false && doctor.IsShow
                            orderby doctor.DoctorType descending, doctor.Sort descending
                            select new ResponseDoctorDTO
                            {
                                areaCode = doctor.areaCode,
                                Birthday = doctor.Birthday,
                                Address = doctor.Address,
                                CheckState = doctor.CheckState,
                                DepartmentID = doctor.DepartmentID,
                                DoctorID = doctor.DoctorID,
                                DoctorName = doctor.DoctorName,
                                Duties = doctor.Duties,
                                HospitalID = doctor.HospitalID,
                                Title = doctor.Title,
                                Marriage = doctor.Marriage,
                                Specialty = doctor.Specialty,
                                DoctorType = doctor.DoctorType,
                                CAT_NO = department.CAT_NO,
                                PhotoFullUrl = user.PhotoUrl,
                                User = new ResopnseUserDTO
                                {
                                    UserType = user.UserType,
                                    PhotoUrl = user.PhotoUrl,
                                    UserID = user.UserID,
                                }
                            };

                if (!string.IsNullOrEmpty(CAT_NO))
                    query = query.Where(i => i.CAT_NO == CAT_NO);

                query = query.Take(Take);

                int Total = 0;
                var Data = query.Pager<ResponseDoctorDTO>(out Total, 1, Take);
                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };
            }

        }

        /// <summary>
        /// 获取开通家庭医生服务的医生 列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public Response<List<ResponseDoctorDTO>> GetDoctorPageListForFamily(int page, int pagesize,
            string hospitalId = "")
        {
            hospitalId = hospitalId ?? "";
            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = (from doctor in db.Doctors
                             join docservice in db.DoctorServices on doctor.DoctorID equals docservice.DoctorID
                             orderby doctor.DoctorType descending, doctor.Sort descending
                             where
                                 docservice.ServiceType == EnumDoctorServiceType.FamilyDoctor && docservice.ServiceSwitch &&
                                 !doctor.IsDeleted && doctor.IsShow
                                 && (hospitalId == "" || hospitalId == doctor.HospitalID)
                             select new ResponseDoctorDTO
                             {
                                 areaCode = doctor.areaCode,
                                 Birthday = doctor.Birthday,
                                 Address = doctor.Address,
                                 CheckState = doctor.CheckState,
                                 DepartmentID = doctor.DepartmentID,
                                 DoctorID = doctor.DoctorID,
                                 DoctorName = doctor.DoctorName,
                                 Duties = doctor.Duties,
                                 HospitalID = doctor.HospitalID,
                                 Marriage = doctor.Marriage,
                                 Title = doctor.Title,
                                 Specialty = doctor.Specialty,
                                 DoctorType = doctor.DoctorType,
                             });

                int Total = 0;
                var Data = query.Pager<ResponseDoctorDTO>(out Total, page, pagesize);
                return new Response<List<ResponseDoctorDTO>>() { Data = Data, Total = Total };
            }

        }

        #endregion

        #region 统计

        /// <summary>
        /// 获取医生统计信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseDoctorStatisticsInfoDTO GetDoctorStatisticsInfo(string userId)
        {
            var doctor = GetDoctorInfoByUserID(userId);
            if (doctor != null)
            {
                var result = new ResponseDoctorStatisticsInfoDTO()
                {
                    DepartmentID = doctor.DepartmentID,
                    DepartmentName = doctor.DepartmentName,
                    DoctorID = doctor.DoctorID,
                    HospitalID = doctor.HospitalID,
                    HospitalName = doctor.HospitalName,
                    PhotoUrl = doctor.User.PhotoUrl,
                    ServiceTimes = doctor.ServiceNum,
                    Sort = doctor.Sort,
                    DiagnoseTimes = doctor.DiagnoseNum,
                    DoctorName = doctor.DoctorName,
                    EvaluatedCount = doctor.EvaluationNum,
                    FollowedCount = doctor.FollowNum,
                    Income = doctor.ServiceIncome, //新收入                    
                };

                return result;
            }
            else
            {
                return default(ResponseDoctorStatisticsInfoDTO);
            }


        }

        #endregion

        #region 医生信息
        /// <summary>
        /// 获取医生编号
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetDoctorIDByUserID(string UserID)
        {
            var MAP_GetDoctorIDByUserID_CacheKey =
                new XuHos.Common.Cache.Keys.StringCacheKey(
                    XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetDoctorIDByUserID, UserID);

            var DoctorID = MAP_GetDoctorIDByUserID_CacheKey.FromCache<string>();

            //缓存没有命中，则查询数据库获取医生编号并设置缓存（缓存不过期）
            if (DoctorID == null)
            {
                using (var db = new DBEntities())
                {
                    var doctor = db.Doctors.Where(p => p.IsDeleted == false && p.DoctorID == UserID).FirstOrDefault();
                    if (doctor != null)
                    {
                        DoctorID = doctor.DoctorID;
                        DoctorID.ToCache(MAP_GetDoctorIDByUserID_CacheKey);
                    }
                    else
                    {
                        //刚注册医生信息 不存在,缓存一条空的 doctor 记录
                        //不放到 GetDoctorInfo 里面处理的原因是因为，没有DoctorID,在这里把UserID当DoctorID
                        //认证通过的时候 创建的Doctor 信息里面 DoctorID 也是取值 UserID
                        ResponseDoctorDTO dto = new ResponseDoctorDTO();
                        var CacheKey =
                            new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseDoctorDTO>(StringCacheKeyType.Doctor,
                                DoctorID);
                        dto.ToCache(CacheKey);
                        DoctorID = UserID;
                    }
                }
            }
            return DoctorID;
        }


        /// <summary>
        /// 根据用户编号查询医生信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseDoctorDTO GetDoctorInfoByUserID(string userId)
        {
            //根据用户编号查询医生编号
            var DoctorID = GetDoctorIDByUserID(userId);
            return GetDoctorInfo(DoctorID);
        }

        /// <summary>
        /// 获取医生信息
        /// Log：0.1
        /// -增加缓存
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="ID"></param>
        /// <param name="includes"></param>
        /// <param name="MapIngoreMembers"></param>
        /// <returns></returns>
        public ResponseDoctorDTO GetDoctorInfo(string DoctorID)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseDoctorDTO>(StringCacheKeyType.Doctor,
                DoctorID);

            var model = default(ResponseDoctorDTO);
            model = CacheKey.FromCache();

            if (model == null)
            {
                var doctorHelper = new DAL.EF.Base.Repository<Entity.Doctor>();

                model =
                    doctorHelper.Single(DoctorID,
                        new List<System.Linq.Expressions.Expression<System.Func<Entity.Doctor, object>>>()
                        ).Map<Entity.Doctor, ResponseDoctorDTO>();



                if (model != null)
                {
                    if (model.DoctorServices == null || model.DoctorServices.Count == 0)
                    {
                        model.DoctorServices = new List<ResponseDoctorServiceDTO>();
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.AudServiceType
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.FamilyDoctor
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.PicServiceType
                        });
                        model.DoctorServices.Add(new ResponseDoctorServiceDTO()
                        {
                            ServiceType = EnumDoctorServiceType.VidServiceType
                        });
                    }

                    if (model.Department != null)
                    {
                        model.DepartmentName = model.Department.DepartmentName;
                        ;
                    }
                    if (model.Hospital != null)
                    {
                        model.HospitalName = model.Hospital.HospitalName;
                    }

                    //是否开通了会诊
                    model.IsConsultation = GetDoctorConsultationState(model.UserID);

                    model.PhotoUrl = model.User._PhotoUrl;

                    model.ToCache(CacheKey, TimeSpan.FromDays(1));
                }
            }

            if (model != null)
            {
                ///服务收入
                model.ServiceIncome = GetServiceIncome(DoctorID);

                //评价数量(缓存)
                model.EvaluationNum = GetEvaluationNum(DoctorID);


                #region 服务设置

                var services = GetDoctorServiceSettingList(DoctorID);

                model.DoctorServices = services.Select(a => new ResponseDoctorServiceDTO
                {
                    DoctorID = DoctorID,
                    HasSchedule = false,
                    ServiceID = a.ServiceID,
                    ServicePrice = a.ServicePrice,
                    ServiceSwitch = a.ServiceSwitch,
                    ServiceType = a.ServiceType
                }).ToList();

                #endregion


                if (model.DoctorServices != null && model.DoctorServices.Count > 0)
                {
                    var State = GetDoctorServiceScheduleState(DoctorID);

                    foreach (ResponseDoctorServiceDTO item in model.DoctorServices)
                    {
                        if (item.ServiceSwitch == 1 &&
                            (item.ServiceType == EnumDoctorServiceType.VidServiceType ||
                             item.ServiceType == EnumDoctorServiceType.AudServiceType))
                        {
                            item.HasSchedule = State;
                        }
                        else
                        {
                            item.HasSchedule = false;
                        }
                    }
                }
            }
            return model;
        }
      

        /// <summary>
        /// 获取不同服务的服务次数和收入数据
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        public List<ResponseDoctorSerivceTypeIncomeDTO> GetDoctorSerivceTypeIncomes(string doctorId,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            using (DBEntities db = new DBEntities())
            {
                //类型 0-挂号 1-图文咨询、2-语音咨询、3-视频咨询、4-家庭医生、5-远程会诊
                string dateCondition = "";
                if (startDate.HasValue || endDate.HasValue)
                {
                    if (startDate.HasValue)
                    {
                        dateCondition = dateCondition + string.Format(" AND FinishTime >= '{0}' ",
                        startDate.Value);
                    }

                    if (endDate.HasValue)
                    {
                        dateCondition = dateCondition + string.Format(" AND FinishTime <= '{0}' ",
                        endDate.Value.AddDays(1).Date);
                    }
                }


                var list = db.Database.SqlQuery<ResponseDoctorSerivceTypeIncomeDTO>(string.Format(@"
DECLARE @table TABLE( SerivceType int )
INSERT @table (SerivceType) Values (0)
INSERT @table (SerivceType) Values (1)
INSERT @table (SerivceType) Values (2)
INSERT @table (SerivceType) Values (3)
INSERT @table (SerivceType) Values (5)
INSERT @table (SerivceType) Values (7)
;
WITH O AS(
SELECT OrderNo, OrderOutID, totalFee FROM Orders WHERE OrderState = 2 AND OrderType <> 11 AND IsDeleted = 0 {0}
),
O1 AS (
 SELECT O.OrderNo, O.OrderOutID, A.totalFee FROM Orders A JOIN O ON A.OrderOutID = O.OrderNo AND A.OrderState = 2 AND  A.OrderType = 11 AND A.IsDeleted = 0
),
O2 AS (
SELECT OrderOutID, SUM(totalFee) totalFee FROM (
SELECT OrderNo, OrderOutID, totalFee FROM O
UNION ALL
SELECT OrderNo, OrderOutID, totalFee FROM O1) T GROUP BY OrderOutID)
SELECT M.SerivceType, ISNULL(TimesCount, 0) AS TimesCount, ISNULL(Income, 0.00) AS  Income FROM @table AS M LEFT JOIN 
(
--0挂号、3视频、2语音
SELECT A.OPDType AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM UserOPDRegisters AS A JOIN O2 AS B ON A.OPDRegisterID = B.OrderOutID WHERE A.DoctorID = @p0 AND A.IsDeleted = 0 AND A.OPDType IN (0, 2, 3)  GROUP BY OPDType
UNION ALL
--家庭医生
SELECT 5 AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM UserFamilyDoctors AS A JOIN O2 AS B ON A.FamilyDoctorID = B.OrderOutID WHERE A.DoctorID = @p0 AND A.IsDeleted = 0  
UNION ALL
--图文
SELECT 1 AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM UserConsults AS A JOIN O2 AS B ON A.UserConsultID = B.OrderOutID WHERE A.DoctorID = @p0 AND A.IsDeleted = 0 
UNION ALL
--远程会诊
SELECT 7 AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM DoctorConsultations AS A JOIN O2 AS B ON A.ConsultationID = B.OrderOutID WHERE A.DoctorID = @p0 AND A.IsDeleted = 0 
) AS N ON M.SerivceType = N.SerivceType", dateCondition), doctorId).ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取医生服务的排版状态
        /// </summary>
        /// <param name="DoctorID"></param>
        /// <param name="ServiceType"></param>
        /// <returns></returns>
        private bool GetDoctorServiceScheduleState(string DoctorID)
        {

            var ScheduleState_CacheKey =
                new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Doctor_ScheduleState, $"{DoctorID}");

            var state = ScheduleState_CacheKey.FromCache<bool?>();

            if (state == null)
            {
                var doctorScheduleHelper = new DAL.EF.Base.Repository<Entity.DoctorSchedule>();
                string beginDate = DateTime.Now.ToString("yyyyMMdd");
                string endDate = DateTime.Now.AddDays(4).ToString("yyyyMMdd");
                string begintime = DateTime.Now.ToString("HH:mm");

                state = doctorScheduleHelper.Exists(t =>
                    t.IsDeleted == false &&
                    t.DoctorID == DoctorID &&
                    (
                        t.OPDate.CompareTo(beginDate) > 0 ||
                        (
                            t.EndTime.CompareTo(begintime) > 0 &&
                            t.OPDate.CompareTo(beginDate) == 0
                            )
                        ) &&
                    t.OPDate.CompareTo(endDate) < 0);

                state.ToCache(ScheduleState_CacheKey, TimeSpan.FromDays(1));

            }

            return state.Value;
        }


        /// <summary>
        /// 获取医生会在状态
        
        /// 日期：2017年4月17日
        /// </summary>
        /// <param name="DoctorUserID"></param>
        /// <returns></returns>
        private bool GetDoctorConsultationState(string DoctorUserID)
        {
            //查询会诊资质
            using (DBEntities db = new DBEntities())
            {
                //获取会诊资质信息
                var consulAptitude = (from rolemap in db.UserRoleMaps
                                      join role in db.UserRoles on rolemap.RoleID equals role.RoleID
                                      where
                                          !role.IsDeleted && !rolemap.IsDeleted && role.RoleType == EnumRoleType.DocConsultation &&
                                          rolemap.UserID == DoctorUserID
                                      select rolemap.UserRoleMapID).FirstOrDefault();


                if (consulAptitude != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        #endregion


        #region 评价

        /// <summary>
        /// 获取评价量
        /// </summary>
        /// <param name="DoctorID"></param>
        /// <returns></returns>
        public int GetEvaluationNum(string DoctorID)
        {
            var EvaluationNum_CacheKey =
                new XuHos.Common.Cache.Keys.StringCacheKey(
                    XuHos.Common.Cache.Keys.StringCacheKeyType.Doctor_EvaluationNum, DoctorID);
            var EvaluationNum = EvaluationNum_CacheKey.FromCache<int?>();

            if (EvaluationNum == null)
            {
                using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
                {
                    EvaluationNum =
                        db.ServiceEvaluations.Where(w => w.ProviderID == DoctorID && w.IsDeleted == false).Count();
                    EvaluationNum.ToCache(EvaluationNum_CacheKey);
                }
            }

            return EvaluationNum.Value;
        }


        /// <summary>
        ///  获取服务评价列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Response<List<ResponseServiceEvaluationDTO>> GetServiceEvaluationList(
            RequestServiceEvaluationConditionDTO condition)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {

                var result = new Response<List<ResponseServiceEvaluationDTO>>();
                var query = from m in db.ServiceEvaluations
                            join a in db.Users on m.UserID equals a.UserID
                            join opd in db.UserOpdRegisters on m.OuterID equals opd.OPDRegisterID into opdLeftMid
                            from opdLeft in opdLeftMid.DefaultIfEmpty()
                            orderby m.CreateTime descending
                            where m.IsDeleted == false
                            select new ResponseServiceEvaluationDTO
                            {
                                ServiceEvaluationID = m.ServiceEvaluationID,
                                EvaluationTags = m.EvaluationTags,
                                Content = m.Content,
                                OuterID = m.OuterID,
                                ProviderID = m.ProviderID,
                                Score = m.Score,
                                ServiceType = m.ServiceType,
                                CreateTime = m.CreateTime,
                                UserID = m.UserID,
                                ConsultDisease = opdLeft == null ? "" : opdLeft.ConsultDisease,
                                UserName = string.IsNullOrEmpty(a.UserCNName) ? a.Mobile : a.UserCNName,
                                UserPhotoUrl = a.PhotoUrl
                            };
                if (!string.IsNullOrEmpty(condition.ProviderID))
                {
                    query = query.Where(w => w.ProviderID == condition.ProviderID);
                }
                if (!string.IsNullOrEmpty(condition.OuterID))
                {
                    query = query.Where(w => w.OuterID == condition.OuterID);
                }
                if (condition.Score.HasValue)
                {
                    query = query.Where(w => w.Score == condition.Score);
                }
                if (!string.IsNullOrEmpty(condition.Keyword))
                {
                    query = query.Where(w => w.Content.Contains(condition.Keyword));
                }
                if (!string.IsNullOrEmpty(condition.EvaluationTag))
                {
                    query = query.Where(w => w.EvaluationTags.Contains(condition.EvaluationTag));
                }
                int Total;
                result.Data = query.Pager<ResponseServiceEvaluationDTO>(out Total, condition.CurrentPage,
                    condition.PageSize);
                result.Total = Total;
                var reg = new Regex(@"^\d{11}$");
                result.Data.ForEach(i =>
                {
                    if (reg.IsMatch(i.UserName))
                    {
                        i.UserName = i.UserName.Remove(3, 4).Insert(3, "****");
                    }
                });
                return result;

            }

        }


        /// <summary>
        /// 获取服务提供者获得的标签评价次数
        /// </summary>
        /// <param name="ProviderID"></param>
        /// <returns></returns>
        public List<ResponseServiceProviderEvaluatedTagDTO> GetServiceProviderEvaluatedTags(string ProviderID)
        {
            using (DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {

                return db.Database.SqlQuery<ResponseServiceProviderEvaluatedTagDTO>(@"
DECLARE @S NVARCHAR(MAX), @L INT
SET @S = ''
SELECT @S = @S + ';' + EvaluationTags FROM ServiceEvaluations WHERE ProviderID = @p0 AND IsDeleted = 0
SET @S = @S + ';'
SET @L = LEN(@S)

SELECT ServiceEvaluationTagID, TagName, (@L - LEN(REPLACE(@S, ';' + TagName + ';', ';'))) / (LEN(TagName) + 1) AS EvaluatedCount FROM ServiceEvaluationTags WHERE IsDeleted = 0",
                    ProviderID).ToList();

            }
        }


        /// <summary>
        /// 获取服务评价标签
        /// </summary>
        /// <returns></returns>
        public List<ResponseServiceEvaluationTagDTO> GetServiceEvaluationTags()
        {
            using (DBEntities db = new DBEntities())
            {
                var q = from m in db.ServiceEvaluationTags.Where(w => w.IsDeleted == false)
                        orderby m.TagName
                        select new ResponseServiceEvaluationTagDTO
                        {
                            ServiceEvaluationTagID = m.ServiceEvaluationTagID,
                            Score = m.Score,
                            TagName = m.TagName
                        };
                return q.ToList();
            }
        }

        #endregion

        #region 医生服务设置

        /// <summary>
        /// 获取医生的服务设置列表
        
        /// 日期：2017年4月19日
        /// </summary>
        /// <param name="doctorID"></param>
        public List<ResponseDoctorServicePriceDTO> GetDoctorServiceSettingList(string doctorID)
        {
            var CacheKey =
                new XuHos.Common.Cache.Keys.EntityListCacheKey<ResponseDoctorServicePriceDTO>(
                    StringCacheKeyType.Doctor_ServicePrice, doctorID);
            List<ResponseDoctorServicePriceDTO> docServices = CacheKey.FromCache();

            if (docServices == null)
            {
                using (var db = new DBEntities())
                {
                    docServices = (from doc in db.DoctorServices.Where(p => p.IsDeleted == false && p.DoctorID == doctorID) 
                                   orderby doc.ServiceType
                                   select new ResponseDoctorServicePriceDTO
                                   {
                                       ServiceID = doc.ServiceID,
                                       ServiceType = doc.ServiceType,
                                       ServiceSwitch = doc.ServiceSwitch ? 1 : 0,
                                       ServicePrice = doc.ServicePrice,
                                   }).ToList();


                    docServices.ForEach(a =>
                    {
                        //a.ServiceTypeName = a.ServiceType.GetEnumDescript();

                        if (a.ServicePrice < a.ServicePriceDown)
                        {
                            a.ServicePrice = a.ServicePriceDown;
                        }

                        if (a.ServicePrice > a.ServicePriceUp)
                        {
                            a.ServicePrice = a.ServicePriceUp;
                        }
                    });
                    docServices.ToCache(CacheKey);


                }
            }

            return docServices;
        }


        /// <summary>
        /// 获取医生的服务设置
        
        /// 日期：2017年4月19日
        /// </summary>
        /// <param name="serviceId"></param>
        public ResponseDoctorServicePriceDTO GetDoctorServicePriceSetting(string DoctorID, string ServiceId)
        {
            var list = GetDoctorServiceSettingList(DoctorID);
            if (list.Count > 0)
            {
                return list.Find(a => a.ServiceID == ServiceId);
            }
            else
            {
                return default(ResponseDoctorServicePriceDTO);
            }
        }

        /// <summary>
        /// 获取服务设置
        
        /// 日期：2017年4月19日
        /// </summary>
        /// <param name="serviceId"></param>
        public ResponseDoctorServicePriceDTO GetDoctorServicePriceSetting(string DoctorID,
            EnumDoctorServiceType ServiceType)
        {
            var list = GetDoctorServiceSettingList(DoctorID);
            if (list.Count > 0)
            {
                return list.Find(a => a.ServiceType == ServiceType);
            }
            else
            {
                return default(ResponseDoctorServicePriceDTO);
            }
        }

        /// <summary>
        /// 获取医生服务价格
        
        /// 日期：2017年4月19日
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public decimal GetDoctorServicePrice(string doctorId, EnumDoctorServiceType serviceType)
        {
            var setting = GetDoctorServicePriceSetting(doctorId, serviceType);
            if (setting != null)
                return setting.ServicePrice;
            else
                return decimal.MinValue;
        }

        /// <summary>
        /// 更新服务设置
        
        /// 日期：2017年4月19日
        /// </summary>
        /// <param name="Settings"></param>
        /// <param name="DoctorID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [Obsolete("已在DoctorServiceService.DoctorServiceSettings重写优化")]
        public bool UpdateDoctorServiceSettings(
            List<RequestDoctorServiceSettingsSubmitDTO.RequestDoctorServiceSettingDTO> ServiceSetting,
            RequestDoctorServiceSettingsSubmitDTO.RequestDoctorClinicSettingDTO FreeClinicSetting, string DoctorID,
            string UserID)
        {
            var DoctorServiceSetting_CacheKey =
                new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseDoctorServicePriceDTO>(
                    StringCacheKeyType.Doctor_ServicePrice, DoctorID);

            var DoctorFreeClinicState_CacheKey =
                new XuHos.Common.Cache.Keys.StringCacheKey(
                    XuHos.Common.Cache.Keys.StringCacheKeyType.Doctor_FreeClinicState, DoctorID);

            var DoctorPrice_CacheKey =
             new XuHos.Common.Cache.Keys.EntityListCacheKey<ResponseDoctorServicePriceDTO>(
                 StringCacheKeyType.Doctor_ServicePrice, DoctorID);



            bool result = false;
            try
            {
                using (var db = new DBEntities())
                {

                    if (ServiceSetting != null && ServiceSetting.Count > 0)
                    {
                        //没有打开语音、图片、视频的时候家庭医生也不允许打开
                        var picTypeInfo =
                            ServiceSetting.FirstOrDefault(p => p.ServiceType == EnumDoctorServiceType.PicServiceType);
                        var audoTypeInfo =
                            ServiceSetting.FirstOrDefault(p => p.ServiceType == EnumDoctorServiceType.AudServiceType);
                        var VideoTypeInfo =
                            ServiceSetting.FirstOrDefault(p => p.ServiceType == EnumDoctorServiceType.VidServiceType);


                        #region  如果图文咨询，语音或视频咨询没有开启那么家庭医生需要关闭                        

                        if (picTypeInfo == null ||
                            audoTypeInfo == null ||
                            VideoTypeInfo == null ||
                            picTypeInfo.ServiceSwitch == 0 ||
                            audoTypeInfo.ServiceSwitch == 0 ||
                            VideoTypeInfo.ServiceSwitch == 0)
                        {
                            for (int i = 0; i < ServiceSetting.Count; i++)
                            {
                                if (ServiceSetting[i].ServiceType == EnumDoctorServiceType.FamilyDoctor)
                                {
                                    ServiceSetting[i].ServiceSwitch = 0;
                                }
                            }
                        }

                        #endregion


                        #region 更新服务价格

                        var oldServices =
                            db.DoctorServices.Where(w => w.DoctorID == DoctorID && w.IsDeleted == false).ToList();
                        oldServices.ForEach(i =>
                        {
                            i.IsDeleted = true;
                        });
                        foreach (var dtoModel in ServiceSetting)
                        {
                            Entity.DoctorService dbmodel =
                                oldServices.Where(w => w.ServiceType == dtoModel.ServiceType).FirstOrDefault();

                            if (dbmodel == null)
                            {
                                dbmodel = new Entity.DoctorService()
                                {
                                    ServicePrice = dtoModel.ServicePrice,
                                    ServiceSwitch = dtoModel.ServiceSwitch == 1 ? true : false,
                                    DoctorID = DoctorID,
                                    ServiceType = dtoModel.ServiceType,
                                    CreateTime = DateTime.Now,
                                    IsDeleted = false
                                };
                                dbmodel.ServiceID = System.Guid.NewGuid().ToString("N");
                                dbmodel.CreateTime = DateTime.Now;
                                dbmodel.CreateUserID = UserID;
                                db.DoctorServices.Add(dbmodel);
                            }
                            else
                            {
                                dbmodel.ServicePrice = dtoModel.ServicePrice;
                                dbmodel.ServiceSwitch = dtoModel.ServiceSwitch == 1 ? true : false;
                                dbmodel.ModifyTime = DateTime.Now;
                                dbmodel.ModifyUserID = UserID;
                                dbmodel.IsDeleted = false;
                            }
                        }
                        oldServices.ForEach(i =>
                        {
                            db.Update(i);
                        });

                        #endregion

                        result = db.SaveChanges() > 0 ? true : false;
                    }
                }

            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                throw ex;
            }

            DoctorServiceSetting_CacheKey.RemoveCache();
            DoctorFreeClinicState_CacheKey.RemoveCache();
            DoctorPrice_CacheKey.RemoveCache();
            return result;
        }


        #endregion

        #endregion

        #region Command

        public bool DeleteDoctor(string DoctorID)
        {
            var helper = new DAL.EF.Base.Repository<Entity.Doctor>();
            return helper.Update(DoctorID, a => new Entity.Doctor() { IsDeleted = true, DeleteTime = DateTime.Now });
        }

        /// <summary>
        /// 新增医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool InsertDoctor(RequestDoctorDTO request)
        {
            var helper = new DAL.EF.Base.Repository<Entity.Doctor>();
            var entity = request.Map<RequestDoctorDTO, Entity.Doctor>();
            var result = helper.Insert(entity);
            request.DoctorID = entity.DoctorID;
            return result;
        }

        /// <summary>
        /// 更新医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool UpdateDoctor(RequestDoctorDTO request)
        {
            var helper = new DAL.EF.Base.Repository<Entity.Doctor>();
            var entity = request.Map<RequestDoctorDTO, Entity.Doctor>();
            entity.ModifyTime = DateTime.Now;
            var result = helper.Update(entity);

            var CacheKey_User =
                new XuHos.Common.Cache.Keys.EntityCacheKey<XuHos.Entity.User>(StringCacheKeyType.User,
                    request.UserID);
            CacheKey_User.RemoveCache();

            var CacheKey_Doctor =
                new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseDoctorDTO>(StringCacheKeyType.Doctor,
                    request.DoctorID);
            CacheKey_Doctor.RemoveCache();

            return result;
        }

        /// <summary>
        /// 更新医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool UpdateDoctorInfo(RequestDoctorPersonalInfoDTO request, string UserID)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var u = db.Users.Where(w => w.UserID == UserID).FirstOrDefault();
                var d = db.Doctors.Where(w => w.DoctorID == UserID).FirstOrDefault();

                if (request.Intro != null)
                {
                    d.Intro = request.Intro;
                    d.ModifyTime = DateTime.Now;
                }
                if (request.Specialty != null)
                {
                    d.Specialty = request.Specialty;
                    d.ModifyTime = DateTime.Now;
                }
                if (request.DiseaseLabel != null)
                {
                    d.DiseaseLabel = request.DiseaseLabel;
                    d.ModifyTime = DateTime.Now;
                }
                if (request.PhotoUrl != null)
                {
                    u.PhotoUrl = ImageBaseDTO.RemoveUrlPrefix(request.PhotoUrl);
                    u.ModifyTime = DateTime.Now;
                }

                db.Update(d);
                db.Update(u);

                if (db.SaveChanges() > 0)
                {


                    var CacheKey_User =
                        new XuHos.Common.Cache.Keys.EntityCacheKey<XuHos.Entity.User>(StringCacheKeyType.User,
                            UserID);
                    CacheKey_User.RemoveCache();

                    var CacheKey_Doctor =
                        new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseDoctorDTO>(StringCacheKeyType.Doctor,
                            d.DoctorID);
                    CacheKey_Doctor.RemoveCache();

                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        /// <summary>
        /// 新增服务评价
        
        /// 日期：2017年4月17日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Evaluation(ServiceEvaluation model)
        {
            var helper = new DAL.EF.Base.Repository<ServiceEvaluation>();

            var Doctor_EvaluationNum_CacheKey =
                new XuHos.Common.Cache.Keys.StringCacheKey(
                    XuHos.Common.Cache.Keys.StringCacheKeyType.Doctor_EvaluationNum, model.ProviderID);

            //取一次次数，没有缓存时就可以重新加载
            GetEvaluationNum(model.ProviderID);

            model.CreateTime = DateTime.Now;
            model.CreateUserID = model.CreateUserID;
            model.ModifyTime = model.CreateTime;
            model.ModifyUserID = model.CreateUserID;
            model.ServiceEvaluationID = Guid.NewGuid().ToString("N");

            if (
                !helper.Exists(
                    a => a.ProviderID == model.ProviderID && a.OuterID == model.OuterID && a.UserID == model.UserID))
            {
                //服务次数增加                
                var ret = helper.Insert(model);

                if (ret)
                {
                    Doctor_EvaluationNum_CacheKey.Increment();
                    //服务次数一小时后过期
                    Doctor_EvaluationNum_CacheKey.ExpireEntryAt(TimeSpan.FromHours(1));
                }

                return ret;
            }
            else
            {
                //远程会诊可多次评价
                if (model.ServiceType == EnumProductType.Consultation)
                {
                    var ret = helper.Insert(model);
                    if (ret)
                    {
                        Doctor_EvaluationNum_CacheKey.Increment();
                        //服务次数一小时后过期
                        Doctor_EvaluationNum_CacheKey.ExpireEntryAt(TimeSpan.FromHours(1));
                    }
                    return ret;
                }

                return true;
            }
        }

        /// <summary>
        /// 通过医生类型获取医生信息
        /// </summary>
        /// <param name="doctorTypes"></param>
        /// <returns></returns>
        public List<ResponseDoctorDTO> GetDoctorListByDoctorType(List<int> doctorTypes)
        {
            using (DBEntities db = new DBEntities())
            {
                return
                    db.Doctors.Where(x => doctorTypes.Contains(x.DoctorType))
                        .ToList()
                        .Map<List<Entity.Doctor>, List<ResponseDoctorDTO>>();
            }
        }

        #endregion

        /// <summary>
        /// 获取医网签签名图片，base64
        /// </summary>
        /// <param name="certificateNo"></param>
        /// <returns></returns>
        public string GetBJCASignature(string certificateNo)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.Doctors.Where(x => x.CertificateNo == certificateNo).Select(x => x.BJCASignature).FirstOrDefault();
            }
        }

        /// <summary>
        /// 更新医网签签名图片，base64
        /// </summary>
        /// <param name="certificateNo"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool UpdateBJCASignature(string certificateNo, string signature)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.Doctors.Where(x => x.CertificateNo == certificateNo).Update(x => new Entity.Doctor()
                {
                    BJCASignature = signature
                }) == 1;
            }
        }

        /// <summary>
        /// 获取合作机构推荐医生
        /// </summary>
        /// <returns></returns>
        public List<DoctorBaseDTO> GetOrgCommandDoctors(string hospitalId)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var doctors =
                    from doctor in
                        db.Doctors.Where(
                            o => o.IsDeleted == false && o.IsShow && o.DoctorType == 3 && o.HospitalID == hospitalId)
                            .OrderByDescending(o => o.Sort)
                    select new DoctorBaseDTO()
                    {
                        DoctorID = doctor.DoctorID,
                        DoctorName = doctor.DoctorName,
                        Title = doctor.Title,
                    };

                return doctors.ToList();
            }
        }

        /// <summary>
        ///  统计指定时间段内的医生业务回复量（图文+音视频）
        /// </summary>
        /// <param name="doctorIDs"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        //public List<ResponseDoctorReplyDTO> GetReplayCount(List<string> doctorIDs, DateTime startTime, DateTime endTime)
        //{
        //    using (DBEntities db = new DBEntities())
        //    {
        //        return (from opd in db.Set<UserOPDRegister>().Where(x => !x.IsDeleted &&
        //                (x.OPDType == EnumDoctorServiceType.PicServiceType ||
        //                x.OPDType == EnumDoctorServiceType.AudServiceType ||
        //                x.OPDType == EnumDoctorServiceType.VidServiceType) &&
        //                x.OPDDate >= startTime && x.OPDDate < endTime && doctorIDs.Contains(x.DoctorID))
        //                join room in db.Set<ConversationRoom>().Where(x => !x.IsDeleted) on opd.OPDRegisterID equals room.ServiceID
        //                where room.RoomState == EnumRoomState.InMedicalTreatment ||
        //                    room.RoomState == EnumRoomState.AlreadyVisit ||
        //                    room.RoomState == EnumRoomState.WaitAgain
        //                group opd by opd.DoctorID into result
        //                select new
        //                {
        //                    DoctorID = result.Key,
        //                    ReplayCount = result.Count()
        //                }).ToList().Select(x => new ResponseDoctorReplyDTO { DoctorID = x.DoctorID, ReplayCount = x.ReplayCount }).ToList();
        //    }
        //}
    }
}
