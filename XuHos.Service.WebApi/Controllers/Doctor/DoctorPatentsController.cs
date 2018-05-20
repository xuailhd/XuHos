using XuHos.BLL;
using XuHos.Common.Enum;
using XuHos.Common.Utility;
using XuHos.DTO;
using XuHos.Service.Infrastructure.Filters;
using System.Collections.Generic;
using System.Web.Http;
using XuHos.DTO.Common;
using System;
using XuHos.DTO.Request;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 医生患者
    /// 前置条件：医生已登录
    /// 日期：2017年6月16日
    /// </summary>
    [UserAuthenticate(UserType = EnumUserType.Doctor)]
    public class DoctorPatentsController : ApiBaseController
    {

        /// <summary>
        /// 110 Treatment：110010
        /// 获取患者就诊记录
        /// 前置条件：医生已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="OPDRegisterID">当前预约编号</param>
        /// <param name="MemberID">会员编号</param> 
        /// <param name="CurrentPage">分页索引</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/DoctorPatients/GetPatientVisitList")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetPatientVisitList(string MemberID, string OPDRegisterID = "", int CurrentPage=1,int PageSize=10)
        {
            BLL.DoctorPatentService bll = new DoctorPatentService(CurrentOperatorUserID);
            return bll.GetPatientVisitList(OPDRegisterID, MemberID, CurrentPage, PageSize).ToApiResultForList();
        }


                /// <summary>
        /// 113 Examination：113407
        /// 诊室/获取会员电子病历列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/DoctorPatients/GetPatientEMRPageList")]
        public ApiResult GetPatientEMRPageList([FromUri]RequestPatientEMRSearch request)
        {
            DoctorPatentService service = new DoctorPatentService(CurrentOperatorUserID);
            return service.GetPatientEMRPageList(request.MemberID, request.CurrentPage, request.PageSize, request.Keyword).ToApiResultForList();
        }

        /// <summary>
        /// 获取会员电子病历列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/DoctorPatients/GetPatientEMRRecord")]
        public ApiResult GetPatientEMRRecord(string ID)
        {
            UserMemberEMRService service = new UserMemberEMRService(CurrentOperatorUserID);
            return service.GetUserMemberEMR(ID).ToApiResultForObject();
        }
   
    }
}