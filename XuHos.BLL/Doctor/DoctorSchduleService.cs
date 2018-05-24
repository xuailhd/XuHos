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
            using (var db = new DBEntities())
            {
                var model = db.DoctorSchedules.Find(scheduleID);
                return model;
            }            
        }

        /// <summary>
        /// 查询医生排班列表设置
        /// </summary>
        /// <returns></returns>
        public List<DoctorScheduleDto> GetDoctorScheduleList(string doctorId, DateTime beginDate, DateTime endDate)
        {
            List<DoctorScheduleDto> response = new List<DoctorScheduleDto>();
            

            var hostimes = GetHosWorktime();
            List<DoctorScheduleDto> exists = null;
            using (var db = new DBEntities())
            {
                exists = (from m in db.DoctorSchedules
                              where m.IsDeleted == false && m.DoctorID == doctorId
                              && m.OPDate.CompareTo(beginDate) >= 0 && m.OPDate.CompareTo(endDate) <= 0
                              select new DoctorScheduleDto
                              {
                                  OPDate = m.OPDate,
                                  ScheduleID = m.ScheduleID,
                                  StartTime = m.StartTime,
                                  EndTime = m.EndTime,
                                  DoctorID = doctorId,
                                  AppointNumber = m.AppointNumber
                              }).ToList();
            }
                

            for (int i=0; beginDate.AddDays(i).CompareTo(endDate) < 0; i++)
            {
                var dt = beginDate.AddDays(i);
                if (exists == null || exists.Count <= 0)
                {
                    foreach (var item in hostimes)
                    {
                        item.OPDate = dt;
                        response.Add(item);
                    }
                }
                else
                {
                    var list = exists.Where(t => t.OPDate.CompareTo(dt) == 0).ToList();
                    //没有 直接新增 不查找
                    if (list == null || list.Count == 0)
                    {
                        foreach (var item in hostimes)
                        {
                            item.OPDate = dt;
                            response.Add(item);
                        }
                    }
                    else
                    {
                        //循环查找
                        foreach (var item in hostimes)
                        {
                            bool findflag = false;
                            foreach (var res in list)
                            {
                                if (item.StartTime.Equals(res.StartTime))
                                {
                                    findflag = true;
                                    break;
                                }
                            }
                            item.OPDate = dt;
                            item.Checked = findflag;
                            response.Add(item);
                        }
                    }
                }
            }

            //排序
            return response.OrderBy(t => t.OPDate).ThenBy(t => t.StartTime).ToList();
        }

        #endregion

        /// <summary>
        /// 获取医院公共排班
        /// </summary>
        /// <returns></returns>
        private List<DoctorScheduleDto> GetHosWorktime()
        {
            var cacheKey = new EntityListCacheKey<DoctorScheduleDto>(StringCacheKeyType.Hospital_Worktime,"99999");
            var hostimes = cacheKey.FromCache();
            if(hostimes == null || hostimes.Count<=0)
            {
                using (var db =new DBEntities())
                {
                    hostimes = (from h in db.HospitalWorkingTimes
                                    select new DoctorScheduleDto
                                    {
                                        StartTime = h.StartTime,
                                        EndTime = h.EndTime,
                                    }).ToList();
                    hostimes.ToCache(cacheKey);
                }
            }
            return hostimes;
        }

        #region Command
        /// <summary>
        /// 保存排班列表
        /// </summary>
        /// <param name="modelList"></param>
        public bool AddDoctorSchduleList(List<DoctorScheduleDto> request, string userId)
        {
            DateTime maxdt = request.Max(t => t.OPDate);
            DateTime mindt = request.Min(t => t.OPDate);

            request = request.OrderBy(t => t.OPDate).ThenBy(t => t.StartTime).ToList();

            var hostimes = GetHosWorktime();

            using (var db = new DBEntities())
            {
                var exist = db.DoctorSchedules.Where(t => t.DoctorID == userId
                    && t.OPDate.CompareTo(maxdt) <= 0 && t.OPDate.CompareTo(mindt) >= 0)
                    .OrderBy(t=>t.OPDate).ThenBy(t=>t.StartTime).ToList();

                int i =0, j = 0;
                foreach(var item in exist)
                {
                    item.IsDeleted = true;
                }

                for (; i < request.Count; i++)
                {
                    //未选中的不处理
                    if (!request[i].Checked)
                    {
                        continue;
                    }

                    //不符合医院时间的，排除掉
                    if (!hostimes.Where(t => t.StartTime == request[i].StartTime && t.EndTime == request[i].StartTime).Any())
                    {
                        request[i].Checked = false;
                        continue;
                    }

                    for (; j < exist.Count; j++)
                    {
                        if (request[i].StartTime == exist[j].StartTime)
                        {
                            //数据库的设置未 未删除
                            exist[j].IsDeleted = false;
                            //请求的标记未 不处理
                            request[j].Checked = false;
                        }
                        else if (request[i].StartTime.CompareTo(exist[i].StartTime) < 0)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                foreach(var item in request)
                {
                    if (item.Checked)
                    {
                        DoctorSchedule ds = new DoctorSchedule()
                        {
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Number = item.Number,
                            ScheduleID = Guid.NewGuid().ToString("N"),
                            OPDate = item.OPDate,
                            DoctorID = userId
                        };

                        db.DoctorSchedules.Add(ds);
                    }
                }
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除医生排班
        /// </summary>
        /// <param name="ScheduleID"></param>
        /// <returns></returns>
        public bool DeleteDoctorSchdule(string scheduleID)
        {
            bool result = true;
            using (var db = new DBEntities())
            {
                var model = db.DoctorSchedules.Find(scheduleID);
                if (model != null)
                {
                    model.IsDeleted = true;
                    result = db.SaveChanges() > 0;
                }
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
        /// 获取医生排版预约总数
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

        #endregion
    }
}
