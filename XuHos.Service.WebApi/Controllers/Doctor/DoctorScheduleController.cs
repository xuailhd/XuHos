using EmitMapper;
using XuHos.BLL;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Common.Utility;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Entity;
using XuHos.Service.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 用户访问权限：用户
    /// </summary>
    [UserAuthenticate(IsValidUserType = false)]
    public class DoctorScheduleController : ApiBaseController
    {
        private DoctorSchduleService doctSchduleService;

        public DoctorScheduleController()
        {
            doctSchduleService = new DoctorSchduleService(CurrentOperatorUserID);
        }

        /// <summary>
        /// 查询医生单条排班
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpPost]
        public ApiResult GetDoctorSchedule(string ID)
        {
            var result = new ApiResult() { Status = 0, Msg = "暂无数据" };
            doctSchduleService = new DoctorSchduleService(CurrentOperatorUserID);
            DoctorSchedule data = doctSchduleService.GetDoctorSchedule(ID);
            if (data != null)
            {
                ObjectsMapper<DoctorSchedule, DoctorScheduleSingleDto> mapper = ObjectMapperManager.DefaultInstance.GetMapper<DoctorSchedule, DoctorScheduleSingleDto>();
                DoctorScheduleSingleDto model = mapper.Map(data);
                result.Status = 0;
                result.Msg = "成功";
                result.Data = data;
            }
            return result;
        }

        /// <summary>
        /// 删除医生单条排班
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.Doctor)]
        [HttpPost]
        public ApiResult DeleteDoctorSchdule(string ID)
        {
            doctSchduleService = new DoctorSchduleService(CurrentOperatorUserID);
            var result = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "删除失败" };
            if (doctSchduleService.DeleteDoctorSchdule(ID))
            {
                result.Status = 0;
                result.Msg = "删除成功";
            }
            return result;
        }

        /**
         * @api {GET} /DoctorSchedule/GetDoctorScheduleList 106101/获取医生排班设置
         * @apiGroup 106 Doctor Setting
         * @apiDescription 用于获取医生排班设置
         * @apiPermission 登录
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 校验小程序Code 返回的 session_key     
         * @apiParam  {Date} beginDate 查询起始日期
         * @apiParam  {Date} endDate 查询结束日期,如果超过起始日期3个月，只返回3个月
         * @apiParamExample {json} 请求样例:
         *    ?beginDate=2018-05-24&endDate=2018-06-24   //参数可能需要url编码
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录     
         * @apiSuccess (Response) {object} Data 业务数据，会按照 日期，StartTime排序
         * @apiSuccessExample {json} 返回样例:
         *  {
         *     "Data":[{
         *       "OPDate":"2016-08-15",
         *       "StartTime":"01:00",   
         *       "EndTime":"02:00",
         *      "Checked":true},
         *      {
         *       "OPDate":"2016-08-15",
         *       "StartTime":"02:00",
         *       "EndTime":"03:00",
         *      "Checked":true}
         *     ],
         *     "Total":0,
         *     "Status":0,
         *     "Msg":"保存成功"
         *  }
         */
        /// <summary>
        /// 获取医生排版设置
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.Doctor)]
        [HttpGet]
        public ApiResult GetDoctorScheduleList(DateTime beginDate, DateTime endDate)
        {
            beginDate = beginDate.Date;
            endDate = endDate.Date;
            if (endDate.Subtract(beginDate).Days>92)
            {
                endDate = beginDate.AddMonths(3);
            }
            return doctSchduleService.GetDoctorScheduleList(CurrentOperatorUserID, beginDate, endDate).ToApiResultForObject();
        }

        /**
         * @api {POST} /DoctorSchedule/AddDoctorSchduleList 106102/保存排班设置
         * @apiGroup 106 Doctor Setting
         * @apiDescription 用于批量保存医生排班设置
         * @apiPermission 登录
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 校验小程序Code 返回的 session_key     
         * @apiParam  {String} Data 批量保存排班数据
         * @apiParamExample {json} 请求样例:
         *    [{
         *       "OPDate":"2016-08-15",
         *       "StartTime":"01:00",   时间段范围必须是从系统获取的排版设置的时间段，不是自己去定义的。
         *       "EndTime":"02:00",
         *      "Checked":true},
         *      {
         *       "OPDate":"2016-08-15",
         *       "StartTime":"02:00",
         *       "EndTime":"03:00",
         *      "Checked":true}
         *     ]
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录     
         * @apiSuccess (Response) {object} Data 业务数据
         * @apiSuccessExample {json} 返回样例:
         *  {
         *     "Data":null,
         *     "Total":0,
         *     "Status":0,
         *     "Msg":"保存成功"
         *  }
         */
        /// <summary>
        /// 保存医生排班设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.Doctor)]
        [HttpPost]
        public ApiResult AddDoctorSchduleList([FromBody]List<DoctorScheduleDto> request)
        {
            var result = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "保存失败" };
            if (request == null || request.Count<=0)
            {
                return result;
            }

            if (doctSchduleService.AddDoctorSchduleList(request, CurrentOperatorUserID))
            {
                result.Status = 0;
                result.Msg = "保存成功";
            }
            return result;
        }

        /// <summary>
        /// 判断排班是否已预约，返回排班数
        /// </summary>
        /// <param name="ScheduleID"></param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [IgnoreAuthenticate]
        [HttpGet]
        public ApiResult ExistsOPDRegister(string ScheduleID)
        {
            if (string.IsNullOrWhiteSpace(ScheduleID))
            {
                return new ApiResult() { Data = false, Total = 0 };
            }

            UserOPDRegisterService userOPDRegisterService = new UserOPDRegisterService(CurrentOperatorUserID);
            int count = userOPDRegisterService.GetRegisterCount(ScheduleID);
            ApiResult api = new ApiResult() { Data = count > 0, Total = count };
            return new ApiResult() { Data = count > 0, Total = count };
        }
    }
}