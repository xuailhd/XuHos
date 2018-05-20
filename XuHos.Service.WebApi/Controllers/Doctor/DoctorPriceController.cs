/********************************************************************************
** 作者：郭超
** 创始时间：2004-08-13
** 修改人：郭超
** 修改时间：2016-08-13
** 描述：
** 主要用于给web和app提供医生价格服务接口
*********************************************************************************/

using EmitMapper;
using XuHos.BLL;
using XuHos.BLL.Doctor.DTOs.Request;
using XuHos.BLL.Doctor.DTOs.Response;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Common.Utility;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Service.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 表示用户登录后才能访问的接口
    /// </summary>
    [UserAuthenticate(IsValidUserType = false)]
    public class DoctorPriceController : ApiBaseController
    {
        private BLL.Doctor.Implements.DoctorService docService;

        public DoctorPriceController()
        {
            docService = new BLL.Doctor.Implements.DoctorService();
        }

        /**
        * @api {GET} /doctorPrice/getDoctorPriceServiceList 106001/获取服务列表
        * @apiGroup 106 Service Setting
        * @apiVersion 4.0.0
        * @apiDescription 用于获取服务列表 作者：郭超
        * @apiPermission 已登录
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParamExample {json} 请求样例：
        *     /doctorPrice/getDoctorPriceServiceList
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
        *     "Data":
        *     [
        *       {"ServiceID":"7943c2cc368944c6af17c50bf4b0e291","ServiceType":1,"ServiceTypeName":"图文咨询","ServiceSwitch":0,"ServicePrice":1.00},
        *       {"ServiceID":"5fdd381b3d6c48b595d03d688e243a1a","ServiceType":2,"ServiceTypeName":"语音咨询","ServiceSwitch":0,"ServicePrice":5.00},
        *       {"ServiceID":"a4a1c98690c04ca78c5ca94177a3d2de","ServiceType":3,"ServiceTypeName":"视频咨询","ServiceSwitch":1,"ServicePrice":7.00},
        *       {"ServiceID":"a64cc0a0bdab43c2a117f6872cb34cc2","ServiceType":4,"ServiceTypeName":"家庭医生","ServiceSwitch":1,"ServicePrice":6.00},
        *       {"ServiceID":"a64cc0a0bdab43c2a117f6872cb34cc2","ServiceType":5,"ServiceTypeName":"远程会诊","ServiceSwitch":1,"ServicePrice":6.00}
        *      ]
        *     "Total":0,
        *     "Status":0,
        *     "Msg":"成功"
        *  }
        */
        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <returns></returns>
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        [HttpGet]
        public ApiResult GetDoctorPriceServiceList()
        {
            var result = new ApiResult();

            var docServiceList = docService.GetDoctorServiceSettingList(CurrentOperatorDoctorID);
            if (docServiceList != null && docServiceList.Count > 0)
            {
                result.Data = docServiceList;
                result.Status = 0;
            }
            return result;

        }

        /**
        * @api {GET} /doctorPrice/getDoctorPriceService 106002/获取医生服务详情
        * @apiGroup 106 Service Setting
        * @apiVersion 4.0.0
        * @apiDescription 获取服务详情 作者：郭超
        * @apiPermission 
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam  {String} ID 医生服务ID
        * @apiParamExample {json} 请求样例：
        *     /doctorPrice/getDoctorPriceService?ID=7943c2cc368944c6af17c50bf4b0e291
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
        *     "Data":{"ServiceID":"7943c2cc368944c6af17c50bf4b0e291","ServiceType":1,"ServiceTypeName":"图文咨询","ServiceSwitch":0,"ServicePrice":1.00},
        *     "Total":0,
        *     "Status":0,
        *     "Msg":"成功"
        *  }
        */
        /// <summary>
        /// 获取单个医生服务
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpGet]
        public ApiResult GetDoctorPriceService(string ID)
        {
            var result = new ApiResult();
            if (!string.IsNullOrEmpty(ID))
            {
                var data = docService.GetDoctorServicePriceSetting(CurrentOperatorDoctorID, ID);
                if (data != null)
                {
                    result.Status = 0;
                    result.Msg = "成功";
                    result.Data = data;
                }
            }
            return result;
        }

        /**
        * @api {POST} /doctorPrice/addOrEditeDoctorService 106003/服务设置保存
        * @apiGroup 106 Service Setting
        * @apiVersion 4.0.0
        * @apiDescription 用于保存医生服务 作者：郭超
        * @apiPermission 医生已登录
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam  {String} ServiceID 医生服务ID
        * @apiParam  {String} ServiceType 服务类型(1:图文咨询,2:语音咨询,3:视频咨询,4:家庭医生,5:远程会诊)，此字段值请不要修改
        * @apiParam  {String} ServiceTypeName 服务名称
        * @apiParam  {int} ServiceSwitch 开关(1:开，0：关)
        *@apiParam   {decimal} ServicePrice 服务价格
        * @apiParamExample {json} 请求样例：
        *    {
        *      "ServiceData":"
        *       [
        *        {"ServiceID":"7943c2cc368944c6af17c50bf4b0e291","ServiceType":1,"ServiceTypeName":"图文咨询","ServiceSwitch":0,"ServicePrice":1.00},
        *        {"ServiceID":"5fdd381b3d6c48b595d03d688e243a1a","ServiceType":2,"ServiceTypeName":"语音咨询","ServiceSwitch":0,"ServicePrice":5.00},
        *        {"ServiceID":"a4a1c98690c04ca78c5ca94177a3d2de","ServiceType":3,"ServiceTypeName":"视频咨询","ServiceSwitch":1,"ServicePrice":7.00},
        *        {"ServiceID":"a64cc0a0bdab43c2a117f6872cb34cc2","ServiceType":4,"ServiceTypeName":"家庭医生","ServiceSwitch":1,"ServicePrice":6.00},
        *        {"ServiceID":"a64cc0a0bdab43c2a117f6872cb34cc2","ServiceType":4,"ServiceTypeName":"远程会诊","ServiceSwitch":1,"ServicePrice":6.00}
        *        ]",
        *        "FreeClinicSetting"://义诊设置，可选
        *        {
        *           "AcceptCount" : 10,
        *           "ClinicMonth" : "201703",//义诊年月,默认为当前月份(请保证格式为yyyyMM)
        *           "State" : true //状态：true-开通，false-关闭
        *        }
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
        *     "Msg":"设置成功"
        *  }
        */
        /// <summary>
        /// 保存医生服务
        /// </summary>
        /// <returns></returns>
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        [HttpPost]
        public ApiResult AddOrEditeDoctorService([FromBody]RequestDoctorServiceSettingsSubmitDTO request)
        {
            #region 移动端 兼容移动端
            List<RequestDoctorServiceSettingsSubmitDTO.RequestDoctorServiceSettingDTO> list;
            if (!string.IsNullOrEmpty(request.ServiceData))
            {
                request.Data = JsonHelper.FromJson<List<RequestDoctorServiceSettingsSubmitDTO.RequestDoctorServiceSettingDTO>>(request.ServiceData);
            }
            #endregion
            return docService.UpdateDoctorServiceSettings(request.Data, request.FreeClinicSetting, CurrentOperatorDoctorID, CurrentOperatorUserID).ToApiResultForBoolean();
        }
    }
}