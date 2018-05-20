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
using System.Data.Entity;
using XuHos.Common.Enum;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;

namespace XuHos.BLL
{
    /// <summary>
    /// 医生排班相关业务
    /// </summary>
    public class DoctorSchduleService : Common.CommonBaseService<XuHos.Entity.DoctorSchedule>
    {
        public DoctorSchduleService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }

        #region Query

        /// <summary>
        /// 根据排班编号查询排班
        /// </summary>
        /// <param name="scheduleID"></param>
        public DoctorSchedule GetDoctorSchedule(string scheduleID)
        {
            DoctorSchedule model = null;
            try
            {
                var db = new DBEntities();
                model = db.DoctorSchedules.Find(scheduleID);
            }
            catch (Exception ex)
            {
                model = null;
                throw ex;
            }
            return model;
        }

        /// <summary>
        /// 查询医生排班列表
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="includeOneBtnCall">是否返回包含一键问诊的预约量</param>
        /// <returns></returns>
        public Response<List<DoctorScheduleDto>> GetDoctorScheduleList(string doctorId, DateTime? beginDate, DateTime? endDate, bool includeOneBtnCall = true)
        {
            Response<List<DoctorScheduleDto>> response = new Response<List<DoctorScheduleDto>>();
            var db = new DBEntities();
            var q = from m in db.DoctorSchedules
                    where m.IsDeleted == false && m.DoctorID == doctorId
                    select new DoctorScheduleDto
                    {
                        OPDate = m.OPDate,
                        ScheduleID = m.ScheduleID,
                        StartTime = m.StartTime,
                        EndTime = m.EndTime,
                        DoctorID = doctorId,
                        Number = m.Number,
                        Disable = false,
                        Checked = m.Number > 0 ? true : false
                    };
            if (beginDate.HasValue)
            {
                var _beginDate = beginDate.Value.ToString("yyyyMMdd");
                q = q.Where(m => m.OPDate.CompareTo(_beginDate) >= 0);
            }
            if (endDate.HasValue)
            {
                var _endDate = endDate.Value.ToString("yyyyMMdd");
                q = q.Where(m => m.OPDate.CompareTo(_endDate) < 0);
            }
            int pagesCount = q.Count();
            q = q.OrderBy(m => m.StartTime);
            var list = q.ToList();
            if (list != null)
            {
                var ids = list.Select(m => m.ScheduleID).ToList();
                var q1 = from m in db.UserOpdRegisters
                         join a in db.Orders on m.OPDRegisterID equals a.OrderOutID
                         where ids.Contains(m.ScheduleID) && m.IsDeleted == false && (a.OrderState == EnumOrderState.Finish || a.OrderState == EnumOrderState.Paid)
                         && !string.IsNullOrEmpty(m.ScheduleID)
                         select new DoctorScheduleTmpDto { ScheduleID = m.ScheduleID, OPDType = m.OPDType };
                var list1 = q1.ToList();

                #region 加上一键问诊的
                if (beginDate.HasValue && endDate.HasValue && includeOneBtnCall)
                {
                    DateTime _beginDate = beginDate.Value;
                    DateTime _endDate = endDate.Value;
                    var q2 = from m in db.UserOpdRegisters
                             join a in db.Orders on m.OPDRegisterID equals a.OrderOutID
                             where m.IsDeleted == false && (a.OrderState == EnumOrderState.Finish || a.OrderState == EnumOrderState.Paid)
                             && string.IsNullOrEmpty(m.ScheduleID)
                             && m.OPDDate.CompareTo(_beginDate) >= 0
                             && m.OPDDate.CompareTo(_endDate) <= 0
                             select new DoctorScheduleTmpDto { OPDType = m.OPDType, ScheduleID = m.ScheduleID, OPDDate = m.OPDDate };

                    var list2 = q2.ToList();

                    if (list2 != null && list2.Count > 0)
                    {
                        for (int i = 0; i < list2.Count; i++)
                        {
                            var item = list2[i];
                            var time = item.OPDDate.ToString("HH:mm");
                            var date = item.OPDDate.ToString("yyyyMMdd");
                            var Schedule = list.Where(t => t.OPDate == date && t.StartTime.CompareTo(time) <= 0 && t.EndTime.CompareTo(time) >= 0).FirstOrDefault();
                            if (Schedule != null)
                            {
                                item.ScheduleID = Schedule.ScheduleID;
                                list1.Add(item);
                            }
                        }
                    }
                }
                #endregion

                //分组求和
                var list3 = (from aa in list1
                             group new { aa.ScheduleID, aa.OPDType } by new { aa.ScheduleID, aa.OPDType } into g
                             select new { g.Key.ScheduleID, g.Key.OPDType, AppointmentCount = g.Count() }).ToList();

                list.ForEach(i =>
                {
                    i.AppointmentCounts = new Dictionary<int, int>();
                    var l = list3.FindAll(m => m.ScheduleID == i.ScheduleID);
                    l.ForEach(li =>
                    {
                        i.AppointmentCounts.Add((int)li.OPDType, li.AppointmentCount);
                    });
                });

            }

            response.Total = pagesCount;
            response.Data = list;
            return response;
        }

        #endregion

        #region Command
        /// <summary>
        /// 保存排班列表
        /// </summary>
        /// <param name="modelList"></param>
        public bool AddDoctorSchduleList(RequestDoctorSchedule<List<RowScheduleDto>> request, string userId)
        {
            XuHos.BLL.Doctor.Implements.DoctorService doctorService = new Doctor.Implements.DoctorService();
            var DoctorID = doctorService.GetDoctorIDByUserID(userId);
            var ScheduleState_CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Doctor_ScheduleState, $"{DoctorID}");

            bool result = true;
            try
            {
                var db = new DBEntities();
                var list = request.Data as List<RowScheduleDto>;
                foreach (var item in list)
                {
                    var l = item.DoctorSchedule as List<DoctorScheduleDto>;
                    foreach (var i in l)
                    {
                        #region
                        if (!i.Disable)
                        {
                            if (string.IsNullOrEmpty(i.ScheduleID))
                            {
                                var model = db.DoctorSchedules.FirstOrDefault(m => m.DoctorID == i.DoctorID && m.OPDate == i.OPDate && m.StartTime == i.StartTime && m.EndTime == i.EndTime);
                                if (model == null)
                                {
                                    if (i.Checked)
                                    {
                                        model = new DoctorSchedule();
                                        model.ScheduleID = System.Guid.NewGuid().ToString("N");
                                        model.OPDate = i.OPDate;
                                        model.IsDeleted = false;
                                        model.DoctorID = i.DoctorID;
                                        model.StartTime = i.StartTime;
                                        model.EndTime = i.EndTime;
                                        model.CreateUserID = userId;
                                        model.IsDeleted = false;
                                        db.DoctorSchedules.Add(model);
                                    }
                                }
                                else
                                {
                                    //无ID，需要恢复才生成更新操作；
                                    if (i.Checked)
                                    {
                                        model.IsDeleted = !i.Checked;
                                    }
                                }
                            }
                            else
                            {
                                var model = db.DoctorSchedules.FirstOrDefault(m => m.ScheduleID == i.ScheduleID);
                                if (!i.Checked)
                                {
                                    model.IsDeleted = !i.Checked;
                                }
                            }
                        }
                        #endregion
                    }
                }
                db.SaveChanges();
                ScheduleState_CacheKey.RemoveCache();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 删除医生排班
        /// </summary>
        /// <param name="ScheduleID"></param>
        /// <returns></returns>
        public bool DeleteDoctorSchdule(string scheduleID)
        {

            bool result = true;
            try
            {
                var db = new DBEntities();
                var model = db.DoctorSchedules.Find(scheduleID);

                var ScheduleState_CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(StringCacheKeyType.Doctor_ScheduleState, $"{model.DoctorID}");


                if (model != null)
                {
                    model.IsDeleted = true;
                    db.SaveChanges();
                    ScheduleState_CacheKey.RemoveCache();
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }
        #endregion

        #region 私有

        /// <summary>
        /// 获取每周第几天的名称
        /// </summary>
        /// <param name="dweek"></param>
        /// <returns></returns>
        private string getDayOfWeekName(DayOfWeek dweek)
        {
            string dtWeekStr = string.Empty;
            switch (dweek)
            {
                case DayOfWeek.Monday:
                    dtWeekStr = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    dtWeekStr = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    dtWeekStr = "周三";
                    break;
                case DayOfWeek.Thursday:
                    dtWeekStr = "周四";
                    break;
                case DayOfWeek.Friday:
                    dtWeekStr = "周五";
                    break;
                case DayOfWeek.Saturday:
                    dtWeekStr = "周六";
                    break;
                case DayOfWeek.Sunday:
                    dtWeekStr = "周日";
                    break;
            }
            return dtWeekStr;
        }

        /// <summary>
        /// 获取医院工作时间(缓存30分钟)
        
        /// 日期：2017年7月29日
        /// </summary>
        /// <param name="HospitalID"></param>
        List<HospitalWorkingTime> getHospWorkTimes(string HospitalID)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<HospitalWorkingTime>(StringCacheKeyType.Hospital_Worktime, HospitalID);
            var result = cacheKey.FromCache();
            if (result == null)
            {
                using (DBEntities db = new DBEntities())
                {
                    //查询医院工作时间
                    result = db.HospitalWorkingTimes
                        .Where(w =>
                                w.HospitalID.Equals(HospitalID) &&
                                w.IsDeleted == false)
                        .OrderBy(x => x.StartTime)
                        .OrderBy(x => x.EndTime).ToList();

                    result.ToCache(cacheKey, TimeSpan.FromMinutes(30));
                }
            }

            return result;


        }

        /// <summary>
        /// 获取医生排版预约总数
        
        /// 日期：2017年7月29日
        /// </summary>
        /// <param name="ScheduleID"></param>
        /// <returns></returns>
        int getDoctorScheduleRegNum(string ScheduleID)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<int?>(StringCacheKeyType.Doctor_ScheduleRegNum, ScheduleID);

            var count = cacheKey.FromCache();

            if (count == null)
            {
                using (DBEntities db = new DBEntities())
                {
                    //查询医生排版和对应预约数量
                    count = (from opd in db.Set<UserOPDRegister>().Where(c => c.IsDeleted == false && c.ScheduleID == ScheduleID)
                                 //目前没有库管管理（通过订单来判断）
                             join order in db.Orders.Where(a => a.OrderState != EnumOrderState.Canceled) on opd.OPDRegisterID equals order.OrderOutID
                             select opd.ScheduleID).Count();

                    //TODO:这里还没有做库存更新的处理
                    count.ToCache(cacheKey, TimeSpan.FromMinutes(5));
                }
            }

            return count.Value;
        }

        /// <summary>
        /// 获取医生排版                
        
        /// 日期：2017年7月29日
        /// </summary>
        /// <param name="DoctorID"></param>
        /// <param name="sDTBegin"></param>
        /// <param name="sDTEnd"></param>
        List<ResponseDoctorAvailableRegTimesDTO.RegNumOfSchedule> getDoctorScheduleList(string DoctorID, string sDTBegin, string sDTEnd)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<ResponseDoctorAvailableRegTimesDTO.RegNumOfSchedule>(StringCacheKeyType.Doctor_ScheuleList, $"{DoctorID}:{sDTBegin}-{sDTEnd}");

            var list = cacheKey.FromCache();

            if (list == null)
            {
                using (DBEntities db = new DBEntities())
                {
                    //查询医生排版和对应预约数量
                    list = (from ds in db.Set<DoctorSchedule>().Where(p =>
                                            p.IsDeleted == false
                                             && p.DoctorID == DoctorID
                                             && p.OPDate.CompareTo(sDTBegin) >= 0
                                             && p.OPDate.CompareTo(sDTEnd) <= 0)
                            select new ResponseDoctorAvailableRegTimesDTO.RegNumOfSchedule()
                            {
                                ScheduleId = ds.ScheduleID,
                                OPDDate = ds.OPDate,
                                StartTime = ds.StartTime,
                                EndTime = ds.EndTime,
                                Number = ds.Number,
                                Quantity = 0
                            }).ToList();

                    //修改排版之后一分钟生效
                    list.ToCache(cacheKey, TimeSpan.FromMinutes(1));
                }
            }

            return list;
        }
        #endregion
    }
}
