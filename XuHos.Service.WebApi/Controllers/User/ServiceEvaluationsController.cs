using EmitMapper;
using XuHos.BLL;
using XuHos.BLL.Doctor.DTOs.Request;
using XuHos.Common;
using XuHos.Common.Config.Sections;
using XuHos.Common.Enum;
using XuHos.Common.Utility;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Entity;
using XuHos.Extensions;
using XuHos.Service.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace XuHos.WebApi.Controllers.User
{
    /// <summary>
    /// 表示用户登录后才能访问的接口
    /// </summary>
    [UserAuthenticate(IsValidUserType = false)]
    public class ServiceEvaluationsController : ApiBaseController
    {
        /**
        * @api {POST} /ServiceEvaluations/Add 109101/添加评价
        * @apiGroup 109 Attention Evaluation
        * @apiVersion 4.0.0
        * @apiDescription 添加评价
        * @apiPermission 已登录
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam  {String} OuterID 评价的服务订单ID
        * @apiParam  {int} Score 分数（1-5）
        * @apiParam  {String} EvaluationTags 评价标签，多个请用分号分割，如：医术高明;很有医德，标签来自（/ServiceEvaluations/GetAllTags）
        * @apiParam  {String} Content 评价内容(最大长度1024)
        * @apiParamExample {json} 请求样例：
        *    /ServiceEvaluations/Add
        *    {
                "OuterID": "XXX",
                "Score": 5,
                "EvaluationTags": "医术高明;很有医德",
                "Content": "好牛B的医生！！！"
            }
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
        *     "Data":true,
        *     "Total":0,
        *     "Status":0,
        *     "Msg":"保存成功"
        *  }
        */
        /// <summary>
        /// 添加评价
        /// </summary>
        /// <param name="transModel"></param>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult Add([FromBody] BLL.Doctor.DTOs.Request.RequestServiceEvaluationDTO request)
        {
            using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
            {
                return mqChannel.Publish(new EventBus.Events.OrderEvaluationEvent()
                {
                    Content = request.Content,
                    EvaluationTags = request.EvaluationTags,
                    OuterID = request.OuterID,
                    Score = request.Score,
                    CreateUserID = CurrentOperatorUserID
                }).ToApiResultForBoolean();
            }
        }

        /**
        * @api {GET} /ServiceEvaluations/GetAllTags 109102/获取评价标签列表
        * @apiGroup 109 Attention Evaluation
        * @apiVersion 4.0.0
        * @apiDescription 获取评价标签列表
        * @apiPermission 所有人
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        * {
            "Data": [
                {
                    "ServiceEvaluationTagID": "2",
                    "Score": 5,
                    "TagName": "很有医德"
                },
                {
                    "ServiceEvaluationTagID": "5",
                    "Score": 4,
                    "TagName": "耐心"
                },
                {
                    "ServiceEvaluationTagID": "6",
                    "Score": 1,
                    "TagName": "态度很差"
                },
                {
                    "ServiceEvaluationTagID": "3",
                    "Score": 4,
                    "TagName": "态度很好"
                },
                {
                    "ServiceEvaluationTagID": "1",
                    "Score": 5,
                    "TagName": "医术高明"
                },
                {
                    "ServiceEvaluationTagID": "7",
                    "Score": 1,
                    "TagName": "治疗没有效果"
                },
                {
                    "ServiceEvaluationTagID": "4",
                    "Score": 4,
                    "TagName": "治疗效果好"
                }
            ],
            "Total": 0,
            "Status": 0,
            "Msg": "操作成功"
        }
        */
        [HttpGet]
        [IgnoreAuthenticate, IgnoreUserAuthenticate]
        public ApiResult GetAllTags()
        {
            BLL.Doctor.Implements.DoctorService service = new BLL.Doctor.Implements.DoctorService();
            return service.GetServiceEvaluationTags().ToApiResultForObject();
        }

        /**
        * @api {GET} /ServiceEvaluations/Query 109103/获取评价列表
        * @apiGroup 109 Attention Evaluation
        * @apiVersion 4.0.0
        * @apiDescription 获取评价列表
        * @apiPermission 所有人
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写   
        * @apiParam  {String} ProviderID 服务提供者ID，如看诊的医生ID （不传或空串，则取当前医生doctorid）
        * @apiParam  {String} OuterID 评价的服务订单ID
        * @apiParam  {int} Score 分数（1-5）
        * @apiParam  {String} Keyword 评价内容关键字
        * @apiParam  {String} EvaluationTag 评价标签名称，如：医术高明
        * @apiParam {string} CurrentPage 当前页码
        * @apiParam {string} PageSize 每页记录数
        * @apiParamExample {json} 请求样例：
        *    ?ProviderID=89F9E5907FD04DBF96A9867D1FA30396
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
            "Data": [
                {
                    "ServiceEvaluationID": "38764db93e17417987d9c5eaa65dccbe",
                    "OuterID": "28436ba2f35445a7aea035cd409d597a",
                    "Score": 5,
                    "EvaluationTags": "医术高明;很有医德",
                    "Content": "好牛B的医生！！！",
                    "ProviderID": "89F9E5907FD04DBF96A9867D1FA30396",
                    "ServiceType": 3,//服务类型(0-挂号、1-图文咨询、2-语音问诊、3-视频问诊、4-处方支付、5-家庭医生、6-会员套餐、7-远程会诊、8-影像判读、100-其它)
                    "UserID": "07567DC13E2B45C68EB118904C6DFCAE",
                    "CreateTime": "2016-12-22T14:47:10.827",
                    "UserName": "曾璐",
                    "UserPhotoUrl": "http://127.0.0.1:52977/images/doctor/unknow.png"
                }
            ],
            "Total": 1,//总评价数
            "Status": 0,
            "Msg": "操作成功"
        }
        */
        [HttpGet]
        [IgnoreAuthenticate, IgnoreUserAuthenticate]
        public ApiResult Query(string ProviderID = null, string OuterID = null, string Keyword = null, string EvaluationTag = null, int? Score = null, int CurrentPage = 1, int PageSize = 10)
        {
            var condition = new RequestServiceEvaluationConditionDTO();
            var user = CurrentOperatorUser;

            //医生取自己的评价 不传ProviderID 则取doctorID
            if (!string.IsNullOrEmpty(ProviderID))
            {
                condition.ProviderID = ProviderID;
            }
            else if (user != null && !string.IsNullOrEmpty(user.DoctorID))
            {
                condition.ProviderID = user.DoctorID;
            }
            condition.CurrentPage = CurrentPage;
            condition.PageSize = PageSize;
            condition.OuterID = OuterID;
            condition.EvaluationTag = EvaluationTag;
            condition.Keyword = Keyword;
            condition.Score = Score;
            var service = new BLL.Doctor.Implements.DoctorService();
            var result = service.GetServiceEvaluationList(condition);
            if (result != null)
            {
                return new ApiResult
                {
                    Data = result.Data,
                    Total = result.Total,
                    Status = EnumApiStatus.BizOK
                };
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }

        /**
        * @api {GET} /ServiceEvaluations/GetEvaluationCount 109104/获取评价量
        * @apiGroup 109 Attention Evaluation
        * @apiVersion 4.0.0
        * @apiDescription 获取评价量
        * @apiPermission 所有人
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写   
        * @apiParam  {String} ProviderID 服务提供者ID，如看诊的医生DoctorID
        * @apiParamExample {json} 请求样例：
        *    ?ProviderID=XXX
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
                "Data": 10, //评价量
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
            }
        */
        [HttpGet]
        [IgnoreAuthenticate, IgnoreUserAuthenticate]
        /// <summary>
        /// 获取评价量
        /// </summary>
        /// <param name="ProviderID"></param>
        /// <returns></returns>
        public ApiResult GetEvaluationCount(string ProviderID)
        {
            var service = new BLL.Doctor.Implements.DoctorService();
            return service.GetEvaluationNum(ProviderID).ToApiResultForObject();
        }

        /**
        * @api {GET} /ServiceEvaluations/GetServiceProviderEvaluatedTags 109105/获取服务提供者获得的标签评价次数
        * @apiGroup 109 Attention Evaluation
        * @apiVersion 4.0.0
        * @apiDescription 获取服务提供者获得的标签评价次数
        * @apiPermission 所有人
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写   
        * @apiParam  {String} ProviderID 服务提供者ID，如看诊的医生ID
        * @apiParamExample {json} 请求样例：
        *    ?ProviderID=XXX
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
            "Data": [
                {
                    "ServiceEvaluationTagID": "1",
                    "TagName": "医术高明",
                    "EvaluatedCount": 5 //标签评价次数
                },
                {
                    "ServiceEvaluationTagID": "2",
                    "TagName": "很有医德",
                    "EvaluatedCount": 5
                },
                {
                    "ServiceEvaluationTagID": "3",
                    "TagName": "态度很好",
                    "EvaluatedCount": 0
                },
                {
                    "ServiceEvaluationTagID": "4",
                    "TagName": "治疗效果好",
                    "EvaluatedCount": 0
                },
                {
                    "ServiceEvaluationTagID": "5",
                    "TagName": "耐心",
                    "EvaluatedCount": 0
                },
                {
                    "ServiceEvaluationTagID": "6",
                    "TagName": "态度很差",
                    "EvaluatedCount": 0
                },
                {
                    "ServiceEvaluationTagID": "7",
                    "TagName": "治疗没有效果",
                    "EvaluatedCount": 0
                }
            ],
            "Total": 7,
            "Status": 0,
            "Msg": "操作成功"
        }
        */
        [HttpGet]
        [IgnoreAuthenticate, IgnoreUserAuthenticate]
        /// <summary>
        /// 获取服务提供者获得的标签评价次数
        /// </summary>
        /// <param name="ProviderID"></param>
        /// <returns></returns>
        public ApiResult GetServiceProviderEvaluatedTags(string ProviderID = null)
        {
            //医生取自己的评价 不传ProviderID 则取doctorID
            var user = CurrentOperatorUser;
            if (string.IsNullOrEmpty(ProviderID))
            {
                ProviderID = user.DoctorID;
            }

            var service = new BLL.Doctor.Implements.DoctorService();
            var result = service.GetServiceProviderEvaluatedTags(ProviderID);

            if (result != null)
            {
                return new ApiResult
                {
                    Data = result,
                    Total = result.Count,
                    Status = 0
                };
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }
    }
}