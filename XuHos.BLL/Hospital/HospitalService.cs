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
using XuHos.Common.Enum;
using XuHos.BLL.Common;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;

namespace XuHos.BLL
{
    /// <summary>
    /// 医院相关业务处理
    /// </summary>
    public class HospitalService : Common.CommonBaseService<XuHos.Entity.Hospital>
    {
        public HospitalService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        {
        }

        public HospitalService(string CurrentOperatorUserID, string CurrentOperatorOrgID) : base(CurrentOperatorUserID)
        {
            base.CurrentOperatorOrgID = CurrentOperatorOrgID;
        }

        public Response<List<ResponseHospitalBaseDTO>> GetCooperativeHospitals(
            int PageIndex = 1,
            int PageSize = int.MaxValue,
            string Keyword = "")
        {
            Keyword = Keyword + "";

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = from hosp in db.Hospitals
                            orderby hosp.ModifyTime descending
                            where hosp.IsCooperation == true && hosp.IsShowInWeb == true && hosp.IsDeleted == false && (Keyword == "" || (Keyword != "" && (hosp.HospitalName.Contains(Keyword) || hosp.Intro.Contains(Keyword))))
                            select new DTO.ResponseHospitalBaseDTO
                            {
                                HospitalID = hosp.HospitalID,
                                HospitalName = hosp.HospitalName,
                                LogoUrl = hosp.LogoUrl,
                                ImageUrl = hosp.ImageUrl,
                                ListImageUrl = hosp.ListImageUrl,
                                Mp4Url = hosp.Mp4Url
                            };

                Response<List<ResponseHospitalBaseDTO>> result = new Response<List<ResponseHospitalBaseDTO>>();
                int total = 0;
                result.Data = query.Pager(out total, PageIndex, PageSize);
                result.Total = total;
                return result;
            }
        }

        /// <summary>
        /// 获取医生列表
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public Response<List<DTO.HospitalDto>> GetPageList(
            int PageIndex = 1,
            int PageSize = int.MaxValue,
            string Keyword = ""
        )
        {
            Keyword = Keyword + "";

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = from hosp in db.Hospitals
                            orderby hosp.ModifyTime descending
                            where Keyword == "" || (Keyword != "" && (hosp.HospitalName.Contains(Keyword) || hosp.Intro.Contains(Keyword)))
                            select new DTO.HospitalDto
                            {
                                Address = hosp.Address,
                                Email = hosp.Email,
                                HospitalID = hosp.HospitalID,
                                HospitalName = hosp.HospitalName,
                                ImageUrl = hosp.ImageUrl,
                                Intro = hosp.Intro,
                                License = hosp.License,
                                LogoUrl = hosp.LogoUrl,
                                PostCode = hosp.PostCode,
                                Telephone = hosp.Telephone
                            };

                Response<List<DTO.HospitalDto>> result = new Response<List<HospitalDto>>();
                int total = 0;
                result.Data = query.Pager(out total, PageIndex, PageSize);
                result.Total = total;
                return result;
            }
        }

        /// <summary>
        /// 医院选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetHospitalDropdownList(bool? IsStandalone = null)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Hospitals
                            orderby item.ModifyTime descending
                            where (IsStandalone == null || item.IsStandalone == IsStandalone) && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.HospitalName,
                                Value = item.HospitalID
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 总店选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetHeadStoreDropdownList()
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Hospitals
                            orderby item.ModifyTime descending
                            where item.OrgType == EnumOrgType.Drugstore && item.Level == 0 && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.HospitalName,
                                Value = item.Path
                            };
                return query.ToList();
            }
        }


        /// <summary>
        /// 总店选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetHeadStoreIDDropdownList()
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Hospitals
                            orderby item.ModifyTime descending
                            where item.OrgType == EnumOrgType.Drugstore && item.Level == 0 && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.HospitalName,
                                Value = item.HospitalID
                            };
                return query.ToList();
            }
        }
        /// <summary>
        /// 获取渠道列表
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetChannelDropdownList()
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Hospitals
                            orderby item.ModifyTime descending
                            where item.IsUseWisdom == true && (item.ParentID == null || item.ParentID == "") && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.HospitalName,
                                Value = item.HospitalID
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 分店选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetDrugStoreDropdownList(string TopPath)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Hospitals
                            orderby item.ModifyTime descending
                            where item.OrgType == EnumOrgType.Drugstore && item.Path.Substring(0, 4) == TopPath && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.HospitalName,
                                Value = item.HospitalID
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 分枝机构选择框
        /// </summary>
        /// <returns></returns>
        public List<ResponseTextAndValue> GetBranchOrgDropdownList(string TopPath)
        {
            using (var db = new DBEntities())
            {
                var query = from item in db.Hospitals
                            orderby item.ModifyTime descending
                            where item.OrgType == EnumOrgType.Hospital && item.Path.Substring(0, 4) == TopPath && item.IsDeleted == false
                            select new ResponseTextAndValue()
                            {
                                Text = item.HospitalName,
                                Value = item.HospitalID
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 医院名
        /// </summary>
        /// <param name="HospitalName"></param>
        /// <param name="hospitalid"></param>
        /// <returns></returns>
        public bool ExistHospitalName(string HospitalName, string hospitalid)
        {
            bool result = false;

            using (DBEntities db = new DBEntities())
            {
                var model = db.Hospitals.FirstOrDefault(o => o.HospitalName == HospitalName && o.HospitalID != hospitalid && o.IsDeleted == false);
                if (model != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 药店编号
        /// </summary>
        /// <param name="hospitalid"></param>
        /// <returns></returns>
        public bool ExistHospitalId(string hospitalId)
        {
            bool result = false;

            using (DBEntities db = new DBEntities())
            {
                var model = db.Hospitals.FirstOrDefault(o => o.HospitalID == hospitalId && o.IsDeleted == false);
                if (model != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }


        public List<HospitalWorkingTime> InitHospitalWorkSet(string hospitalId)
        {
            var list = new List<HospitalWorkingTime>();
            var dic = new Dictionary<string, string>()
            {
                { "08:00", "10:00" },
                { "10:00", "12:00" },
                { "14:00", "16:00" },
                { "16:00", "18:00" },
                { "18:00", "20:00" }
            };
            foreach (var kv in dic)
            {
                list.Add(new HospitalWorkingTime()
                {
                    WorkingTimeID = Guid.NewGuid().ToString("N"),
                    CreateTime = DateTime.Now,
                    HospitalID = hospitalId,
                    StartTime = kv.Key,
                    EndTime = kv.Value,
                    Sort = 0
                });
            }
            return list;
        }

        /// <summary>
        /// 根据机构编码获取机构整个路径（目前只实现两层路径，当前机构以及上一级机构）
        /// 沈腾飞
        /// 2017-08-01
        /// </summary>
        /// <param name="orgnazitionID"></param>
        /// <returns></returns>
        public List<string> GetOrgFullPathByOrgID(string orgnazitionID)
        {
            using (var db = new DBEntities())
            {
                //var query = from hosp in db.Hospitals.Select(x => new
                //{
                //    HospitalName = x.HospitalName,
                //    Path = x.Path,
                //    Level = x.Level,
                //    Link = 1,
                //})
                //            join org in db.Hospitals.Where(x => x.HospitalID == orgnazitionID).Select(x => new
                //            {
                //                HospitalName = x.HospitalName,
                //                Path = x.Path,
                //                Level = x.Level,
                //                Link = 1,
                //            }) on hosp.Link equals org.Link
                //            where org.Path.Contains(hosp.Path) && hosp.Level < org.Level
                //            select new List<string> { hosp.HospitalName, org.HospitalName };


                Hospital org = db.Hospitals.FirstOrDefault(x => x.HospitalID == orgnazitionID);
                if (org == null)
                    return null;

                if (org.Level == 0)
                    return new List<string> { org.HospitalName };

                var result = db.Hospitals.Where(x => org.Path.StartsWith(x.Path) && x.Level < org.Level).Select(x => new List<string>
                {
                    x.HospitalName,
                    org.HospitalName
                }).FirstOrDefault();

                if (result == null)
                    result = new List<string> { org.HospitalName };

                return result;

            }
        }



        public List<DruginfoDTO> GetDrugstoreInfo(string pId)
        {
            using (var db = new DBEntities())
            {
                var query = from druginfo in db.Hospitals
                            where !druginfo.IsDeleted && druginfo.IsShowInWeb && druginfo.OrgType == EnumOrgType.Drugstore
                            select druginfo;

                if (string.IsNullOrWhiteSpace(pId))
                {
                    query = query.Where(x => x.Level == 0);
                }
                else
                {
                    var hospital = db.Hospitals.FirstOrDefault(x => x.HospitalID == pId);
                    if (hospital == null)
                    {
                        return new List<DruginfoDTO>();
                    }
                    query = query.Where(x => x.Path.StartsWith(hospital.Path) && x.Level > hospital.Level);
                }
                return query.OrderBy(x => x.Path).Select(druginfo => new DruginfoDTO()
                {
                    Address = druginfo.Address,
                    AreaName = druginfo.AreaName,
                    DrugstoreManageName = druginfo.DrugstoreManageName,
                    HospitalID = druginfo.HospitalID,
                    HospitalName = druginfo.HospitalName,
                    Intro = druginfo.Intro,
                    Latitude = druginfo.Latitude,
                    Longitude = druginfo.Longitude,
                    Mobile = druginfo.Mobile,
                    Telephone = druginfo.Telephone,
                    LogoUrl = druginfo.LogoUrl,
                    CreateTime = druginfo.CreateTime
                }).ToList();
            }
        }

        public List<DruginfoDTO> GetDrugstoreInfo()
        {
            using (var db = new DBEntities())
            {
                var data = (from druginfo in db.Hospitals
                            where !druginfo.IsDeleted && druginfo.IsShowInWeb && druginfo.OrgType == EnumOrgType.Drugstore
                            select new DruginfoDTO()
                            {
                                Address = druginfo.Address,
                                AreaName = druginfo.AreaName,
                                DrugstoreManageName = druginfo.DrugstoreManageName,
                                HospitalID = druginfo.HospitalID,
                                HospitalName = druginfo.HospitalName,
                                Intro = druginfo.Intro,
                                Latitude = druginfo.Latitude,
                                Longitude = druginfo.Longitude,
                                Mobile = druginfo.Mobile,
                                Telephone = druginfo.Telephone,
                                CreateTime = druginfo.CreateTime,
                            }).ToList();

                return data;
            }
        }

        /// <summary>
        /// 获取药房 药店，用于同步药品
        /// 康美之恋，康美大药房
        /// </summary>
        /// <returns></returns>
        public List<HospitalDto> GetfyDrugstores(string toppath)
        {
            using (var db = new DBEntities())
            {
                var data = (from druginfo in db.Hospitals
                            where !druginfo.IsDeleted && druginfo.Path.Substring(0, 4) == toppath && druginfo.OrgType == EnumOrgType.Drugstore
                            select new HospitalDto()
                            {
                                HospitalID = druginfo.HospitalID,
                                HospitalName = druginfo.HospitalName,
                            }).ToList();

                return data;
            }
        }

        /// <summary>
        /// 获取不同服务的服务次数和收入数据
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        public List<DTO.ResponseDoctorSerivceTypeIncomeDTO> GetDoctorSerivceTypeIncomes(string HospitalID)
        {
            using (DBEntities db = new DBEntities())
            {
                //类型 0-挂号 1-图文咨询、2-语音咨询、3-视频咨询、4-家庭医生、5-远程会诊
                var list = db.Database.SqlQuery<DTO.ResponseDoctorSerivceTypeIncomeDTO>(@"
--0挂号、3视频、2语音
SELECT A.OPDType AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM UserOPDRegisters AS A JOIN Orders AS B ON A.OPDRegisterID = B.OrderOutID JOIN Doctors C ON A.DoctorID = C.DoctorID WHERE C.HospitalID = @p0 AND A.IsDeleted = 0 AND B.IsDeleted = 0 AND B.OrderState = 2 AND A.OPDType IN (0, 2, 3) GROUP BY OPDType
UNION
--家庭医生
SELECT 5 AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM UserFamilyDoctors AS A JOIN Orders AS B ON A.FamilyDoctorID = B.OrderOutID JOIN Doctors C ON A.DoctorID = C.DoctorID WHERE C.HospitalID = @p0 AND A.IsDeleted = 0 AND B.IsDeleted = 0 AND B.OrderState = 2
UNION
--图文
SELECT 1 AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM UserConsults AS A JOIN Orders AS B ON A.UserConsultID = B.OrderOutID JOIN Doctors C ON A.DoctorID = C.DoctorID WHERE C.HospitalID = @p0 AND A.IsDeleted = 0 AND B.IsDeleted = 0 AND B.OrderState = 2
UNION
--远程会诊
SELECT 7 AS SerivceType, COUNT(*) TimesCount, ISNULL(SUM(B.totalFee), 0) Income FROM DoctorConsultations AS A JOIN Orders AS B ON A.ConsultationID = B.OrderOutID JOIN Doctors C ON A.DoctorID = C.DoctorID WHERE C.HospitalID = @p0 AND A.IsDeleted = 0 AND B.IsDeleted = 0 AND B.OrderState = 2", HospitalID).ToList();
                return list;
            }
        }

        public override bool Update(Hospital model)
        {
            int count = 0;
            using (var db = new DBEntities())
            {
                using (db.BeginTransaction())
                {
                    //更新下级
                    if (string.IsNullOrEmpty(model.ParentID))
                    {
                        db.Database.ExecuteSqlCommand("update Hospitals set IsUseWisdom={0}, ChannelID={1} where ParentID={2}", new object[] { model.IsUseWisdom, (object)model.ChannelID ?? DBNull.Value, model.HospitalID });
                    }
                    else//获取父级设置
                    {
                        var parent = db.Hospitals.Where(q => q.HospitalID == model.ParentID).FirstOrDefault();
                        if (parent != null)
                        {
                            model.IsUseWisdom = parent.IsUseWisdom;
                            model.ChannelID = parent.ChannelID;
                        }
                    }

                    db.Entry<Hospital>(model).State = System.Data.Entity.EntityState.Modified;
                    count = db.SaveChanges();
                    db.Commit();
                }
            }

            return count > 0;
        }

        /// <summary>
        /// 根据orgId获取机构详情
        /// </summary>
        /// <returns></returns>
        public HospitalDto GetOrgInfo(string OrgId)
        {
            using (var db = new DBEntities())
            {
                var data = (from druginfo in db.Hospitals
                            where !druginfo.IsDeleted && druginfo.HospitalID == OrgId
                            select druginfo).FirstOrDefault();

                return data.Map<Hospital, HospitalDto>();
            }
        }

        public bool IsChildFor(string parentOrgID, string childOrgID)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var p = db.Hospitals.Where(hosp => !hosp.IsDeleted && hosp.HospitalID == parentOrgID).Select(hosp => hosp.Path).FirstOrDefault();
                var c = db.Hospitals.Where(hosp => !hosp.IsDeleted && hosp.HospitalID == childOrgID).Select(hosp => hosp.Path).FirstOrDefault();
                return c?.StartsWith(p ?? "--NOT_EXISTS--") ?? false;
            }
        }

        public string[] GetChildrenIDList(string orgId, EnumOrgType? type = null)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var root = db.Hospitals.Where(x => x.HospitalID == orgId).Select(x => x.Path).FirstOrDefault();
                var q = db.Hospitals.Where(x => x.Path.StartsWith(root));
                if (type.HasValue)
                {
                    q = q.Where(x => x.OrgType == type);
                }
                return q.Select(x => x.HospitalID).ToArray();
            }
        }
    }
}