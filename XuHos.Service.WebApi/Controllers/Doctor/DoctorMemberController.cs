using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XuHos.Entity;
using XuHos.Extensions;

using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Service.Infrastructure.Filters;
using XuHos.BLL;
using XuHos.Common.Enum;
using XuHos.Common.Utility;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;

namespace XuHos.WebApi.Controllers
{

    /// <summary>
    /// 医生的患者
    /// </summary>
    public class DoctorMemberController : ApiBaseController
    {

        DoctorMemberService doctorMemberService;

        public DoctorMemberController()
        {
            doctorMemberService = new DoctorMemberService(CurrentOperatorUserID);
        }


        /**
              * @api {GET} /DoctorMembers?PageSize=10&CurrentPage=1 110001/我的患者列表
              * @apiGroup 110 Treatment
              * @apiVersion 4.0.0
              * @apiDescription 我的患者列表
              * @apiPermission 已登录（医生）
              * @apiHeader {String} apptoken appToken
              * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
              * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
              * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写
              * @apiParam {string} Keyword 患者姓名
              * @apiParam {string} CurrentPage 当前页码
              * @apiParam {string} PageSize 每页记录数
              * @apiParamExample {json} 请求样例：
              * ?PageSize=10&CurrentPage=1
              * @apiSuccess (Response) {String} Msg 提示信息 
              * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
              * @apiSuccess (Response) {int} Total 总记录数
              * @apiSuccess (Response) {Object} Data 处方列表数据
              * @apiSuccessExample {json} 返回样例:
              *{
                "Data": [
                    {
                        "DoctorMemberID": "365E5D455F2642A992448F354D7DC77F",
                        "DoctorID": "89F9E5907FD04DBF96A9867D1FA30396",
                        "MemberID": "77C5CF07923A4E3D8121F628336527B8",
                        "Gender": 1,
                        "GenderName": "女性",
                        "Birthday": "1989-02-02",
                        "Mobile": "18688941654",
                        "Age": 28
                    }
                ],
                "Total": 58,
                "Status": 0,
                "Msg": "操作成功"
            }
           **/
        /// <summary>
        /// 我的患者列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/DoctorMembers")]
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        public ApiResult GetList([FromUri]DoctorMemberCondition condition)
        {
            if (condition == null)
                condition = new DoctorMemberCondition();

            
            condition.UserID =CurrentOperatorUserID;

            var result = doctorMemberService.GetList(condition);
            return result.ToApiResultForList();
        }


        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetMemberInfo([FromUri]string MemberID, string UserID)
        {
            BLL.DoctorMemberService doctorPatientService = new DoctorMemberService(CurrentOperatorUserID);
            return doctorPatientService.GetMemberInfo(CurrentOperatorDoctorID, UserID, MemberID).ToApiResultForObject();
        }


        /**
              * @api {GET} /DoctorMembers?DoctorMemberID=XXX 110002/患者详细信息
              * @apiGroup 110 Treatment
              * @apiVersion 4.0.0
              * @apiDescription 患者详细信息
              * @apiPermission 已登录（医生）
              * @apiHeader {String} apptoken appToken
              * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
              * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
              * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写
              * @apiParam {string} DoctorMemberID 我的患者ID
              * @apiParamExample {json} 请求样例：
              * ?DoctorMemberID=XXX
              * @apiSuccess (Response) {String} Msg 提示信息 
              * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
              * @apiSuccess (Response) {int} Total 总记录数
              * @apiSuccess (Response) {Object} Data 处方列表数据
              * @apiSuccessExample {json} 返回样例:
              *{
                "Data": {
                    "MemberID": "77C5CF07923A4E3D8121F628336527B8",
                    "UserID": "9A4C83966C784DD5BEFA68766591A272",
                    "Relation": 1,
                    "Gender": 1,
                    "Marriage": 1,
                    "Birthday": "1989-02-02",
                    "Mobile": "18688941654",
                    "IDType": 1,
                    "IDNumber": "0",
                    "Address": "",
                    "Email": "18688941654@qq.com",
                    "PostCode": "0",
                    "GenderName": "女性",
                    "Age": 28,
                    "IsDefault": false
                },
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
            }
          **/
        /// <summary>
        /// 患者详细信息
        /// </summary>
        /// <param name="doctorMemberID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/DoctorMembers")]
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        public ApiResult GetMyMemberInfo(string doctorMemberID)
        {
            doctorMemberService = new DoctorMemberService(CurrentOperatorUserID);
            var model = doctorMemberService.GetMyMemberInfo(doctorMemberID);
            if (model != null)
                return model.ToApiResultForObject();
            else
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
        }

        /**
              * @api {GET} /DoctorMember/GetMyMemberVisitList?DoctorMemberID=XXXCurrentPage=1&PageSize=10 110003/就诊记录列表
              * @apiGroup 110 Treatment
              * @apiVersion 4.0.0
              * @apiDescription 患者就诊记录列表
              * @apiPermission 已登录（医生）
              * @apiHeader {String} apptoken appToken
              * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
              * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
              * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写
              * @apiParam {string} DoctorMemberID 我的患者ID
              * @apiParam {string} CurrentPage 当前页码
              * @apiParam {string} PageSize 每页记录数
              * @apiParamExample {json} 请求样例：
              * ?DoctorMemberID=XXX&PageIndex=1CurrentPage=10
              * @apiSuccess (Response) {String} Msg 提示信息 
              * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
              * @apiSuccess (Response) {int} Total 总记录数
              * @apiSuccess (Response) {Object} Data 处方列表数据
              * @apiSuccessExample {json} 返回样例:
              *{
                "Data": [
                    {
                        "OPDRegisterID": "d36e9a1ed6ce41bd8699a7d428a1c6f9",
                        "RegDate": "2016-12-13T15:13:30.84",
                        "OPDDate": "2016-12-13T00:00:00",
                        "OPDType": 0,
                        "Fee": 0,
                        "Member": {
                            "MemberID": "6836017638eb448ca96f7df75730700e",
                            "UserID": "54e169e1604943c991b1be48b5d5fa85",
                            "MemberName": "曾璐",
                            "Relation": 0,
                            "Gender": 0,
                            "Marriage": 0,
                            "Birthday": "1985-06-14",
                            "Mobile": "",
                            "IDType": 0,
                            "IDNumber": "xxxx",
                            "Address": "",
                            "Email": "",
                            "PostCode": "",
                            "GenderName": "男性",
                            "Age": 32,
                            "IsDefault": false
                        },
                        "Doctor": {
                            "DoctorID": "89F9E5907FD04DBF96A9867D1FA30396",
                            "DoctorName": "邱浩强",
                            "Gender": 0,
                            "Marriage": 0,
                            "IDType": 0,
                            "IsConsultation": false,
                            "IsExpert": false,
                            "IsFreeClinicr": false,
                            "HospitalID": "42FF1C61132E443F862510FF3BC3B03A",
                            "HospitalName": "康美医院",
                            "DepartmentID": "A8064D2DAE3542B18CBD64F467828F57",
                            "DepartmentName": "神经内分泌科"
                        },
                        "Schedule": {
                            "ScheduleID": "5a8322519d1849e1826e83e942c0960b",
                            "StartTime": "16:00",
                            "EndTime": "18:00"
                        },
                        "Room": {
                            "ServiceType": 0,
                            "ChannelID": 0,
                            "RoomState": 0,
                            "BeginTime": "0001-01-01T00:00:00",
                            "EndTime": "2016-12-14T17:39:00.4224242",
                            "TotalTime": 0
                        }
                    }
                ],
                "Total": 1,
                "Status": 0,
                "Msg": "操作成功"
            }
          **/
        /// <summary>
        /// 获取患者就诊记录
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpGet]
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        public ApiResult GetMyMemberVisitList([FromUri]DoctorMemberCondition condition)
        {
            doctorMemberService = new DoctorMemberService(CurrentOperatorUserID);
            condition.UserID = CurrentOperatorUserID;
            var model = doctorMemberService.GetMyMemberVisitList(condition);
            if (model != null)
                return model.ToApiResultForList();
            else
                return new ApiResult() { Data = null };
        }

        /// <summary>
        /// 110 Treatment：110004
        /// 获取主诊医生和患者信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpGet]
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        public ApiResult GetDoctorAndMemberInfo(string doctorMemberID)
        {
            var model = doctorMemberService.GetDoctorAndMember(doctorMemberID, CurrentOperatorUserID);
            if (model != null)
                return model.ToApiResultForObject();
            else
                return new ApiResult() { Data = null };
        }

        /**
          * @api {GET} /DoctorMember/GetDoctorMemberEMRs 113405/获取患者电子病历列表
          * @apiGroup 113 Examination
          * @apiVersion 4.0.0
          * @apiDescription 查询电子病历列表
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
          * @apiParam {string} [MemberID=null] 家庭成员ID
          * @apiParam {string} [DoctorMemberID=null] 医生患者ID （传了家庭成员ID，这个就不要传了）
          * @apiParam {string} [Keyword=null] 电子病历名称  
          * @apiParam {string} [CurrentPage=1]  页码
          * @apiParam {string} [PageSize=10] 分页大小
          * @apiParamExample {json} 请求样例：
          *                   ?MemberID=XXX
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 成员列表
          * @apiSuccessExample {json} 返回样例:
          * {
                "Data": [
                    {
                        "MemberID": "6836017638eb448ca96f7df75730700e",
                        "UserMemberEMRID": "54e169e1604943c991b1be48b5d5fa85",
                        "MemberName": "XXX",
                        "Date": "1985-01-01",
                        "EMRName": "",
                        "Remark": "详情",
                        "HospitalName": "医院",
                        "Files":[{},{}]
                    }
                ],
                "Total": 3,
                "Status": 0,
                "Msg": "操作成功"
            }
        **/
        /// <summary>
        /// 获取患者电子病历列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        [HttpGet]
        public ApiResult GetDoctorMemberEMRs([FromUri]UserMemeberQueryDTO dto)
        {
            doctorMemberService = new DoctorMemberService(CurrentOperatorUserID);
            var data = doctorMemberService.GetDoctorMemberEMRs(CurrentOperatorDoctorID, dto.MemberId, dto.DoctorMemberID, dto.CurrentPage, dto.PageSize, dto.Keyword);
            var ret = data.Data.ToApiResultForObject();
            ret.Total = data.Total;
            return ret;
        }

        /**
          * @api {GET} /DoctorMember/GetUserMemberEMR 113406/获取患者的电子病历详情
          * @apiGroup 113 Examination
          * @apiVersion 4.0.0
          * @apiDescription 电子病历详情
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
          * @apiParam {string} UserMemberEMRID  ID
          * @apiParamExample {json} 请求样例：
          *                   ?UserMemberEMRID=XXX
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 电子病历详情
          * @apiSuccessExample {json} 返回样例:
          * {
                "Data": {
                    "MemberID": "6836017638eb448ca96f7df75730700e",
                    "UserMemberEMRID": "54e169e1604943c991b1be48b5d5fa85",
                    "MemberName": "XXX",
                    "Date": "1985-01-01",
                    "EMRName": "",
                    "Remark": "详情",
                    "HospitalName": "医院",
                    "Files":[{},{}]
                },
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
            }
        **/
        /// <summary>
        /// 获取患者的电子病历详情
        /// </summary>
        /// <returns></returns>
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        [HttpGet]
        public ApiResult GetDoctorMemberEMR(string userMemberEMRID)
        {
            UserMemberEMRService service = new UserMemberEMRService(CurrentOperatorUserID);
            return service.GetUserMemberEMR(userMemberEMRID).ToApiResultForObject();
        }

        /// <summary>
        /// 110 Treatment：110008
        /// 我的患者(下拉框)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        public ApiResult GetMyMemberDDL([FromUri]DoctorMemberCondition condition)
        {
            doctorMemberService = new DoctorMemberService(CurrentOperatorUserID);
            condition.DoctorID = CurrentOperatorDoctorID;
            var result = doctorMemberService.GetMyMemberDDL(condition);
            return result.ToApiResultForList();
        }



    }
}