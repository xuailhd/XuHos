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
        }


        /**
        * @api {GET} /DoctorSchedule/getDoctorSchedule 106101/查询单条排班
        * @apiGroup 106 Service Setting
        * @apiVersion 4.0.0
        * @apiDescription 查询医生单条排班
        * @apiPermission none
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam  {String} ID 排班ID
        * @apiParamExample {json} 请求样例：
        *    /DoctorSchedule/getDoctorSchedule?ID=89F9E5907FD04DBF96A9867D1FA30396
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
        *     "Data":{"ScheduleID":"","DoctorID": "","OPDate": "","StartTime":"","EndTime":"","AreaName":""}
        *     "Total":0,
        *     "Status":0,
        *     "Msg":"成功"
        *  }
        */
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


        /**
        * @api {POST} /DoctorSchedule/deleteDoctorSchdule 106103/删除单条排班
        * @apiGroup 106 Service Setting
        * @apiVersion 4.0.0
        * @apiDescription 用于删除单条医生排班
        * @apiPermission 医生
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam  {String} ID  医生排班ID
        * @apiParamExample {json} 请求样例：
        *      /DoctorSchedule/deleteDoctorSchdule
        *      {ID:'89F9E5907FD04DBF96A9867D1FA30396'}
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
         * @api {POST} /DoctorSchedule/AddDoctorSchduleList 106104/保存排班设置
         * @apiGroup 106 Service Setting
         * @apiVersion 4.0.0
         * @apiDescription 用于批量保存医生排班设置
         * @apiPermission 登录
         * @apiHeader {String}  apptoken Users unique access-key.
         * @apiHeader {String}  noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String}  usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String}  sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
         * @apiParam  {String} Data 批量保存排班数据
         * @apiParamExample {json} 请求样例：
         *    /DoctorSchedule/AddDoctorSchduleList
         *    {
         *     "BeginDate":"2016/08/28"
         *     "ScheduleData":"[{"StartTime":"01:00","EndTime":"06:00","DoctorSchedule":[{"ScheduleID":"2a430949a05e430b9b9a3a8ef3beee1e","OPDate":"20160815","StartTime":"01:00","EndTime":"06:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":true},{"ScheduleID":"9e70538614914ed799207bbe55055f2f","OPDate":"20160816","StartTime":"01:00","EndTime":"06:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":true},{"ScheduleID":"","OPDate":null,"StartTime":"01:00","EndTime":"06:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"01:00","EndTime":"06:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"01:00","EndTime":"06:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"01:00","EndTime":"06:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"01:00","EndTime":"06:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false}]},{"StartTime":"08:00","EndTime":"10:00","DoctorSchedule":[{"ScheduleID":"","OPDate":null,"StartTime":"08:00","EndTime":"10:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":true},{"ScheduleID":"","OPDate":null,"StartTime":"08:00","EndTime":"10:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"08:00","EndTime":"10:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"08:00","EndTime":"10:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"08:00","EndTime":"10:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"08:00","EndTime":"10:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"08:00","EndTime":"10:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false}]},{"StartTime":"10:00","EndTime":"12:00","DoctorSchedule":[{"ScheduleID":"","OPDate":null,"StartTime":"10:00","EndTime":"12:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"10:00","EndTime":"12:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"10:00","EndTime":"12:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"10:00","EndTime":"12:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"10:00","EndTime":"12:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"10:00","EndTime":"12:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"10:00","EndTime":"12:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false}]},{"StartTime":"14:00","EndTime":"15:00","DoctorSchedule":[{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"15:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"15:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"15:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"15:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"15:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"15:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"15:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false}]},{"StartTime":"14:00","EndTime":"18:00","DoctorSchedule":[{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"18:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"18:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"18:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"18:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"18:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"18:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"14:00","EndTime":"18:00","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false}]},{"StartTime":"19:00","EndTime":"23:59","DoctorSchedule":[{"ScheduleID":"","OPDate":null,"StartTime":"19:00","EndTime":"23:59","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"19:00","EndTime":"23:59","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"19:00","EndTime":"23:59","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"19:00","EndTime":"23:59","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"19:00","EndTime":"23:59","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"19:00","EndTime":"23:59","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false},{"ScheduleID":"","OPDate":null,"StartTime":"19:00","EndTime":"23:59","DoctorID":"89F9E5907FD04DBF96A9867D1FA30396","Disable":false,"Checked":false}]}]"
         *     }
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
        public ApiResult AddDoctorSchduleList([FromBody]XuHos.DTO.RequestDoctorSchedule<List<RowScheduleDto>> request)
        {
            string str = JsonHelper.ToJson(request.Data);
            var result = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "保存失败" };
            DateTime searchDate = DateTime.Today;
            if (!string.IsNullOrEmpty(request.BeginDate))
            {
                searchDate = DateTime.Parse(request.BeginDate);
            }
            DateTime searchFirstWeekDay = searchDate.AddDays(1 - Convert.ToInt32(searchDate.DayOfWeek.ToString("d")));
            request.BeginDate = searchFirstWeekDay.ToString("yyyy/MM/dd");
            if (!string.IsNullOrEmpty(request.ScheduleData))
            {
                var dataList = JsonHelper.FromJson<List<RowScheduleDto>>(request.ScheduleData);
                request.Data = dataList;
            }
            foreach (var item in request.Data)
            {
                for (int i = 0; i < 7; i++)
                {
                    item.DoctorSchedule[i].OPDate = DateTime.Parse(request.BeginDate).AddDays(i).ToString("yyyyMMdd");
                }
            }
            doctSchduleService = new DoctorSchduleService(CurrentOperatorUserID);
            if (doctSchduleService.AddDoctorSchduleList(request, CurrentOperatorUserID))
            {
                result.Status = 0;
                result.Msg = "保存成功";
            }
            return result;
        }

        /**
           * @api {GET} /DoctorSchedule/ExistsOPDRegister 106105/判断排班是否已预约
           * @apiGroup 106 Service Setting
           * @apiVersion 4.0.0
           * @apiDescription 用户查询医生可预约的排班 
           * @apiPermission 所有人
           * @apiHeader {String} apptoken appToken
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
           * @apiParam {string} ScheduleID 排班ID 
           * @apiParamExample {json} 请求样例：
           *                   ?ScheduleID=89F9E5907FD04DBF96A9867D1FA30396
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {Array} Data 业务数据
           * @apiSuccessExample {json} 返回样例:
           *{"Data":true,"Total":1,"Status":0,"Msg":"操作成功"}
       **/
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