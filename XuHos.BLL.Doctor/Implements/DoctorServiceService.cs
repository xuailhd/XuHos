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
using XuHos.BLL.Sys;
using static XuHos.BLL.Doctor.DTOs.Response.ResponseDoctorDTO;
using XuHos.Common;
using XuHos.Common.Cache.Keys;
using XuHos.BLL.Sys.Implements;

namespace XuHos.BLL.Doctor.Implements
{
    /// <summary>
    /// 医生服模块务业务处理
    /// </summary>
    public class DoctorServiceService : Common.CommonBaseService<Entity.DoctorService>
    {
        public DoctorServiceService(string CurrentOperatorUserID) : base(CurrentOperatorUserID) { }

        #region 医生服务设置

        /// <summary>
        /// 开通或设置医生服务（支持单项服务设置）
        /// </summary>
        /// <param name="list">服务列表</param>
        /// <param name="doctorID">医生</param>
        /// <returns></returns>
        public EnumApiStatus DoctorServiceSettings(List<RequestDoctorServiceSettingDTO> list, string doctorID)
        {
            var result = EnumApiStatus.BizError;

            using (var db = new DBEntities())
            {
                var serviceList = db.DoctorServices.Where(i => i.DoctorID == doctorID).ToList();
                list.ForEach(model =>
                {
                    #region 添加或修改服务
                    var entity = serviceList.Where(i => i.ServiceType == model.ServiceType).FirstOrDefault();
                    if (entity == null)
                    {
                        entity = new Entity.DoctorService()
                        {
                            DoctorID = doctorID,
                            ServiceType = model.ServiceType,
                            ServicePrice = model.ServicePrice,
                            ServiceSwitch = model.ServiceSwitch == 1 ? true : false,
                        };
                        PreInsert(db, entity);

                        serviceList.Add(entity);
                    }
                    else
                    {
                        entity.IsDeleted = false;
                        entity.ServicePrice = model.ServicePrice;
                        entity.ServiceSwitch = model.ServiceSwitch == 1 ? true : false;
                    }
                    #endregion

                });

                #region 图文咨询、语音咨询和视频咨询都是开启状态，才能手动开启家族服务
                var familyModel = list.Where(i => i.ServiceType == EnumDoctorServiceType.FamilyDoctor && i.ServiceSwitch == 1).FirstOrDefault();
                if (familyModel != null)
                {
                    if (serviceList.Count(i =>
                          (i.ServiceType == EnumDoctorServiceType.PicServiceType ||
                           i.ServiceType == EnumDoctorServiceType.AudServiceType ||
                           i.ServiceType == EnumDoctorServiceType.VidServiceType) &&
                           i.ServiceSwitch == true && i.IsDeleted == false) < 3)
                    {
                        return EnumApiStatus.BizDoctorServiceNotOpenFamilyDoctorService;
                    }
                }
                #endregion

                #region  如果图文咨询、语音、视频咨询没有开启，那么家庭医生需要关闭     
                var isClose = serviceList.Where(i =>
                         (i.ServiceType == EnumDoctorServiceType.PicServiceType ||
                          i.ServiceType == EnumDoctorServiceType.AudServiceType ||
                          i.ServiceType == EnumDoctorServiceType.VidServiceType) &&
                          i.ServiceSwitch == false && i.IsDeleted == false).FirstOrDefault() != null ? true : false;
                if (isClose == true)
                {
                    var familyEntity = serviceList.Where(i => i.ServiceType == EnumDoctorServiceType.FamilyDoctor && i.IsDeleted == false && i.ServiceSwitch == true).FirstOrDefault();
                    if (familyEntity != null)
                    {
                        familyEntity.ServiceSwitch = false;
                    }
                }
                #endregion

                //result = db.SaveChanges() > 0 ? EnumApiStatus.BizOK : EnumApiStatus.BizError;

                var res = db.SaveChanges();
                result = EnumApiStatus.BizOK;

                //清缓存
                if (result == EnumApiStatus.BizOK)
                {
                    var CacheKey = new EntityListCacheKey<ResponseDoctorServicePriceDTO>(StringCacheKeyType.Doctor_ServicePrice, doctorID);
                    CacheKey.RemoveCache();

                    var DoctorServiceSetting_CacheKey = new EntityCacheKey<ResponseDoctorServicePriceDTO>(StringCacheKeyType.Doctor_ServicePrice, doctorID);
                    DoctorServiceSetting_CacheKey.RemoveCache();
                }
            }

            return result;
        }
        #endregion


        #region 服务业务订单

        /// <summary>
        /// 所有未回复的预约图文量、今天的音视频数量(app调用)
        /// </summary>
        /// <param name="doctorID"></param>
        /// <returns></returns>
        public ResponseTodayApptCountDTO GetTodayApptCountAPP(string doctorID)
        {
            var result = new ResponseTodayApptCountDTO();
            var startTime = DateTime.Now.Date;
            var endTime = startTime.AddDays(1);

            using (DBEntities db = new DBEntities())
            {
                //当天已支付的音视频数
                var query = from item in db.UserOpdRegisters
                            join order in db.Orders on item.OPDRegisterID equals order.OrderOutID
                            where item.DoctorID == doctorID &&
                            order.OrderState == EnumOrderState.Paid &&
                            item.OPDDate >= startTime &&
                            item.OPDDate < endTime &&
                            item.IsDeleted == false &&
                            (item.OPDType == EnumDoctorServiceType.AudServiceType || item.OPDType == EnumDoctorServiceType.VidServiceType)
                            && item.ScheduleID != "" && item.ScheduleID != null //忽略一键呼叫的订单
                            group item by item.OPDType into g
                            select new { OPDType = g.Key, Count = g.Count() };

                query.ToList().ForEach(i =>
                {
                    if (i.OPDType == EnumDoctorServiceType.AudServiceType)
                        result.AudCount = i.Count;
                    if (i.OPDType == EnumDoctorServiceType.VidServiceType)
                        result.VidCount = i.Count;
                });

                return result;
            }
        }
        #endregion

    }
}