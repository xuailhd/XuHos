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
using XuHos.Common.Utility;
using XuHos.Common.Enum;
using XuHos.BLL.User.DTOs.Response;
using XuHos.BLL.Doctor.DTOs.Request;
using XuHos.BLL.Doctor.DTOs.Response;
using XuHos.DTO.Request;

namespace XuHos.WebApi.Controllers
{

    /// <summary>
    /// 医生
    /// </summary>
    public class DoctorsController : ApiBaseController
    {
        XuHos.BLL.Doctor.Implements.DoctorService doctorBll;
        BLL.User.Implements.UserDoctorService userDoctor;
        public DoctorsController()
        {
            doctorBll = new XuHos.BLL.Doctor.Implements.DoctorService();
            userDoctor = new BLL.User.Implements.UserDoctorService(); ;
        }

        /// <summary>
        /// 新增医生
        /// 前置条件：管理员登录
        /// </summary>
        /// <param name="requst">实体</param>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        [Route("~/Doctors")]
        public ApiResult InsertEntity([FromBody]RequestDoctorDTO requst)
        {

            if (doctorBll.InsertDoctor(requst))
            {
                return requst.DoctorID.ToApiResultForObject();
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }

        /// <summary>
        /// 更新医生
        /// 前置条件：管理员登录
        /// </summary>
        /// <param name="requst">实体</param>
        /// <returns></returns>
        [HttpPut]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        [Route("~/Doctors")]
        public ApiResult UpdateEntity([FromBody]RequestDoctorDTO requst)
        {
            return doctorBll.UpdateDoctor(requst).ToApiResultForBoolean();
        }

        /**
           * @api {Post} /Doctors/UpdateDoctorInfo 102102/更新医生信息
           * @apiGroup 102 Personal Info
           * @apiDescription 更新医生信息
           * @apiPermission 医生登陆  
           
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken userToken，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写    
           * @apiParam {String} Intro 个人介绍
           * @apiParam {String} Specialty 擅长领域
           * @apiParam {String} PhotoUrl 头像
            * @apiSuccess (Response) {String} Msg 提示信息 
            * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
            * @apiSuccess (Response) {int} Total 总记录     
            * @apiSuccess (Response) {object[]} Data 业务数据
           * @apiSuccessExample {json} 返回样例:
           *                {"Data":True,"Total":0,"Status":0,"Msg":"操作成功"}
       **/
        /// <summary>
        /// 更新医生信息
        /// 前置条件：医生登陆
        /// </summary>
        /// <param name="request">实体</param>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.Doctor)]
        public ApiResult UpdateDoctorInfo([FromBody]RequestDoctorPersonalInfoDTO request)
        {
            var userId = CurrentOperatorUserID;
            var doctorBll = new BLL.Doctor.Implements.DoctorService();
            if (doctorBll.UpdateDoctorInfo(request, userId))
            {
                return true.ToApiResultForBoolean();
            }
            return EnumApiStatus.BizError.ToApiResultForApiStatus();
        }


        /**
        * @api {GET} /Doctors/GetDoctorInfo 102101/获取医生个人信息
        * @apiGroup 102 Personal Info
        * @apiVersion 4.0.0
        * @apiDescription 获取个人资料 作者：郭超
        * @apiPermission 用户登录
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        *  {
        *     "Data":{"Intro":"...","PhotoUrl":"...","PhotoUrl":""},
        *     "Total":0,
        *     "Status":0,
        *     "Msg":"获取信息成功"
        *  }
        */
        /// <summary>
        /// 获取医生个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetDoctorInfo()
        {
            var userId = CurrentOperatorUserID;
            var userBll = new BLL.User.Implements.UserService();
            var doctorBll = new BLL.Doctor.Implements.DoctorService();

            var u = userBll.GetUserInfoByUserId(userId);
            var d = doctorBll.GetDoctorInfoByUserID(userId);
            if (u != null && d != null)
            {
                var model = new DTO.ResponseDoctorPersonalInfoDTO
                {
                    Intro = d.Intro,
                    PhotoUrl = u.PhotoUrl,
                    Specialty = d.Specialty,
                    DiseaseLabel = d.DiseaseLabel
                };
                return model.ToApiResultForObject();
            }
            return EnumApiStatus.BizError.ToApiResultForApiStatus();
        }

        /// <summary>
        /// 获取医生统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [IgnoreAuthenticate, IgnoreUserAuthenticate]
        public ApiResult GetDoctorStatisticsInfo(string UserID)
        {
            return doctorBll.GetDoctorStatisticsInfo(UserID).ToApiResultForObject();
        }
        /// <summary>
        /// 删除医生
        /// 前置条件：管理员登录
        /// </summary>
        /// <param name="ID">预约编号</param>
        /// <returns></returns>
        [HttpDelete]
        [UserAuthenticate(UserType = XuHos.Common.Enum.EnumUserType.SysAdmin)]
        [Route("~/Doctors")]
        public ApiResult DeleteEntity(string ID)
        {
            return doctorBll.DeleteDoctor(ID).ToApiResultForBoolean();
        }


        /**
           * @api {GET} /Doctors/?ID=:ID 102104/获取医生详情
           * @apiGroup 102 Personal Info
           * @apiVersion 4.0.0
           * @apiDescription 获取医生详情 
           * @apiPermission 所有人
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken userToken，用户未登录时传空       
           * @apiParam {String} ID 医生编号 
           * @apiParamExample {json} 请求样例：
           *                   ?ID=89F9E5907FD04DBF96A9867D1FA30396
            * @apiSuccess (Response) {String} Msg 提示信息 
            * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
            * @apiSuccess (Response) {int} Total 总记录     
            * @apiSuccess (Response) {object} Data 业务数据
           * @apiSuccessExample {json} 返回样例:
           * {
                "Data": {
                    "DoctorID": "89F9E5907FD04DBF96A9867D1FA30396",
                    "DoctorName": "邱浩强",
                    "IsFollowed":false,//是否已关注
                    "UserID": "B04D4AE28F994AE2AACBB456D7E0647B",
                    "Gender": 1,
                    "Marriage": 0,
                    "Birthday": "19850808",
                    "IDType": 4,
                    "IDNumber": "123",
                    "CertificateNo": "89F9E5907FD04DBF96A9867D1FA30396",
                    "Address": "2131",
                    "PostCode": "231",
                    "Intro": "... ...",
                    "IsConsultation": false,
                    "IsExpert": false,
                    "IsFreeClinicr": false,
                    "Specialty": "高血压 糖尿病 恶性肿瘤 其他",
                    "areaCode": "",
                    "HospitalID": "42FF1C61132E443F862510FF3BC3B03A",
                    "HospitalName": "康美医院",
                    "Grade": "0",
                    "DepartmentID": "A8064D2DAE3542B18CBD64F467828F57",
                    "DepartmentName": "健康体检中心",
                    "Education": "",
                    "Title": "4",
                    "Duties": "",
                    "CheckState": 1,
                    "SignatureURL": "",
                    "Sort": 0,
                    "FollowNum": 4,//关注量
                    "DiagnoseNum": 3,
                    "ConsultNum": 1,
                    "ConsulServicePrice": 0,
                    "Department": {
                        "DepartmentID": "A8064D2DAE3542B18CBD64F467828F57",
                        "HospitalID": "42FF1C61132E443F862510FF3BC3B03A",
                        "DepartmentName": "健康体检中心",
                        "Intro": "..."
                    },
                    "DoctorServices": [
                        {
                            "ServiceID": "a55291ac95b4472ba1c966953fc17b49",
                            "DoctorID": "89F9E5907FD04DBF96A9867D1FA30396",
                            "ServiceType": 3,
                            "ServiceSwitch": 1,
                            "ServicePrice": 0.01,
                            "HasSchedule": true
                        }
                    ],
                    "Hospital": {
                        "HospitalID": "42FF1C61132E443F862510FF3BC3B03A",
                        "HospitalName": "康美医院",
                        "Intro": "...",
                        "License": "YYZZ000001",
                        "LogoUrl": "http://121.15.153.63:8028///Uploads/hospital/201509/151815495260.png",
                        "Address": "广东省普宁市流沙新河西路38号",
                        "PostCode": "515300",
                        "Telephone": "(0663)2229222",
                        "Email": "km@kmlove.com.cn",
                        "ImageUrl": "http://121.15.153.63:8028///Uploads/hospital/201512/1.jpg"
                    },
                    "User": {
                        "UserID": "B04D4AE28F994AE2AACBB456D7E0647B",
                        "UserAccount": "jack",
                        "UserCNName": "邱浩强",
                        "UserENName": "3",
                        "UserType": 2,
                        "Mobile": "13692248249",
                        "Email": "zenglu@km.com",
                        "PayPassword": "",
                        "PhotoUrl": "http://121.15.153.63:8028/images/b427cae4799bf5387eadfc9d7e627e2e",
                        "Score": 0,
                        "Star": 0,
                        "Comment": 0,
                        "Good": 0,
                        "Fans": 0,
                        "Grade": 0,
                        "Checked": 0,
                        "RegTime": "2016-08-01T17:40:07.92",
                        "CancelTime": "2016-08-01T17:40:07.92",
                        "UserState": 0,
                        "UserLevel": 3,
                        "Terminal": "0",
                        "LastTime": "2016-12-26T13:57:08.013",
                        "identifier": 110
                    }
                },
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
            }
           * 
       **/
        /// <summary>
        /// 获取医生详情
        /// 前置条件：无
        /// </summary>
        /// <param name="ID">医生编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Doctors")]
        [IgnoreAuthenticate]
        public ApiResult GetEntity([FromUri]string ID)
        {
            var model = doctorBll.GetDoctorDetail(ID);
            return model.ToApiResultForObject();
        }

        /// <summary>
        /// 药店/获取有排班的处方医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [IgnoreAuthenticate]
        public ApiResult GetAvailableDoctors([FromUri]DTO.RequestDoctorSelectDTO request)
        {
            if (request == null)
            {
                request = new RequestDoctorSelectDTO();
            }           

            var result = doctorBll.GetDoctorPageList(request, false);
            if (result.Data.Count > 0)
            {
                result.Data = result.Data.Where(a => a.IsScheduleExist.HasValue && a.IsScheduleExist.Value).ToList();
            }
            return result.ToApiResultForList();
        }

        /**
            * @api {GET} /Doctors 102103/查询医生列表
            * @apiGroup 102 Personal Info
            * @apiDescription 通过关键字查询医生列表 
            * @apiPermission 所有人
            * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
            * @apiHeader {String} usertoken 登录用户token，用户未登录时传空     
            * @apiParam {int} CurrentPage=1 页码 
            * @apiParam {int} PageSize=10 分页大小
            * @apiParam {string} Keyword='' 关键字
            * @apiParam {string} OrderBy= 排序规则，可叠加（4：按照有无排班排序；5：按照有无套餐排序；6：按照评分降序排序；7：按问诊回复量降序排序）
            * @apiParamExample {json} 请求样例：
            *                   ?CurrentPage=1&PageSize=10&Keyword=&OrderBy=4&OrderBy=6
            * @apiSuccess (Response) {String} Msg 提示信息 
            * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
            * @apiSuccess (Response) {int} Total 总记录数
            * @apiSuccess (Response) {Array} Data 业务数据
            * @apiSuccessExample {json} 返回样例:
            *{
                "Data": [
                    {
                        "DoctorID": "4FDADA2DD7E3450CAEC78E9CA407BF06",
                        "DoctorName": "向金龙",
                        "Gender": 0,
                        "Marriage": 0,
                        "Birthday": "19850808",
                        "IDType": 0,
                        "Address": "深圳市福田区国际创新中心A座8楼",
                        "IsConsultation": false,
                        "IsExpert": false,
                        "areaCode": "",
                        "HospitalID": "42FF1C61132E443F862510FF3BC3B03A",
                        "HospitalName": "康美医院",
                        "DepartmentID": "BCE87580389041A0A70F9465F305BBC2",
                        "DepartmentName": "全科",
                        "Duties": "",
                        "CheckState": 0,
                        "Sort": 0,
                        "DoctorType" : 1, //医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生
                        "IsScheduleExist": true, //是否有排班
                        "IsPackageExist": true, //是否可使用套餐
                        "ReplyCount" : 99, //最近一周的回复数（图文以及音视频）
                        "EvaluationScore": 9.9, // 服务综合评分
                        "DoctorServices": [
                            {
                                "ServiceType": 3,//视频咨询
                                "ServiceSwitch": 1,//开启
                                "ServicePrice": 8//价格（单位：元）
                            },
                            {
                                "ServiceType": 2,//语音咨询
                                "ServiceSwitch": 1,
                                "ServicePrice": 5
                            },
                            {
                                "ServiceType": 4,//家庭医生
                                "ServiceSwitch": 1,
                                "ServicePrice": 7
                            },
                            {
                                "ServiceType": 1,//图文咨询
                                "ServiceSwitch": 1,
                                "ServicePrice": 1
                            }
                        ],
                        "User": {
                            "UserID": "5E5E4318744248E99C18A71B8774E2E9",
                            "UserType": 0,
                            "PhotoUrl": "http://www.kmwlyy.com/Uploads/doctor/xiangjinlong.jpg",
                        }
                    }
                ],
                "Total": 75,
                "Status": 0,
                "Msg": "操作成功"
            }
        **/
        /// <summary>
        /// 查询医生记录
        /// 前置条件：无
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Doctors")]
        [IgnoreAuthenticate]
        public ApiResult GetEntitys([FromUri]DTO.RequestDoctorSelectDTO request)
        {
            if (request == null)
            {
                request = new RequestDoctorSelectDTO();
            }

            if (!request.ScheduleDate.HasValue)
                request.ScheduleDate = DateTime.Now;

            var response = doctorBll.GetDoctorPageList(request, true);
            return response.ToApiResultForList();
        }


        /**
       * @api {Get} /Doctors/GetMyVisitDoctors 104001/已就诊的医生
       * @apiGroup 104 Treatment
       * @apiDescription 查询已就诊过的医生
       * @apiPermission 已登录(用户)
       * @apiHeader {String} apptoken Users unique access-key.
       * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
       * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
       * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写           
       * @apiParam {int} CurrentPage=1 页码 
       * @apiParam {int} PageSize=10 分页大小   
       * @apiParamExample {json} 请求样例：
       * ?CurrentPage=1&PageSize=10
       * @apiSuccess (Response) {String} Msg 提示信息 
       * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
       * @apiSuccess (Response) {int} Total 总记录     
       * @apiSuccess (Response) {object} Data 业务数据
       * @apiSuccessExample {json} 返回样例:
       *  {
            "Data": [
                {
                    "OPDRegisterID": "90ecac5ebf2d4587876733155af46c6d",
                    "DoctorID": "2bc4be23a01f4c7c865671918721df1d",
                    "DoctorName": "吴大大",
                    "HospitalID": "c4cd1db578d84b94bd091f95495f256a",
                    "HospitalName": "His对接医院(勿删)",
                    "DepartmentName": "测试-外科",
                    "DepartmentID": "c3d5f41a930b4412ba57cc1ad11e871e",
                    "Gender": "",
                    "Portait": "http://121.15.153.63:8028/images/doctor/default.jpg",
                    "Position": "",
                    "IsExpert": false,
                    "Specialties": "",
                    "Title": "",
                    "IsFollowed": false,
                    "DoctorType" : 1, //医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生
                }
            ],
            "Total": 1,
            "Status": 0,
            "Msg": "操作成功"
        }
       */
        /// <summary>
        /// 我的医生
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("~/Doctors/GetMyVisitDoctors")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetMyVisitDoctors(int CurrentPage = 1, int PageSize = 10, string hospitalId = null)
        {
            var list = userDoctor.GetMyVisitDoctors(CurrentOperatorUserID, CurrentPage, PageSize, hospitalId);
            return list.ToApiResultForList();
        }


        /// <summary>
        /// 102 Personal Info:102107
        /// 获的某医院医生（pier88）
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        [Route("~/Doctors/GetDoctorsByHospitalId")]
        [HttpGet]
        [IgnoreAuthenticate]
        public ApiResult GetHospitalDoctorPagerList(string hospitalId)
        {
            return doctorBll.GetDoctorPagerListForHospital(hospitalId).ToApiResultForList();
        }


        /**
         * @apiIgnore Not finished Method
       * @api {Get} /Doctors/GetExpertDoctors 102108/获的推荐专家
       * @apiGroup 102 Personal Info
       * @apiVersion 4.0.0
       * @apiDescription 获的推荐专家
       * @apiPermission 所有人
       * @apiHeader {String} apptoken Users unique access-key.
       * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
       * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
       * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
       * @apiParam {int} pageIndex=1 页码 
       * @apiParam {int} pageSize=10 分页大小   
       * @apiParamExample {json} 请求样例：
       * ?pageIndex=1&pageSize=10
       * @apiSuccess (Response) {String} Msg 提示信息 
       * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
       * @apiSuccess (Response) {int} Total 总记录     
       * @apiSuccess (Response) {object} Data 业务数据
       * @apiSuccessExample {json} 返回样例:
            *{
                "Data": [
                    {
                        "DoctorID": "4FDADA2DD7E3450CAEC78E9CA407BF06",
                        "DoctorName": "向金龙",
                        "Gender": 0,
                        "Marriage": 0,
                        "Birthday": "1985-08-08",
                        "IDType": 0,
                        "Address": "深圳市福田区国际创新中心A座8楼",
                        "IsConsultation": false,
                        "IsExpert": false,
                        "areaCode": "",
                        "HospitalID": "42FF1C61132E443F862510FF3BC3B03A",
                        "HospitalName": "康美医院",
                        "DepartmentID": "BCE87580389041A0A70F9465F305BBC2",
                        "DepartmentName": "全科",
                        "Duties": "",
                        "CheckState": 0,
                        "Sort": 0,
                        "DoctorType" : 1, //医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生
                        "DoctorServices": [
                            {
                                "ServiceType": 3,//视频咨询
                                "ServiceSwitch": 1,//开启
                                "ServicePrice": 8//价格（单位：元）
                            },
                            {
                                "ServiceType": 2,//语音咨询
                                "ServiceSwitch": 1,
                                "ServicePrice": 5
                            },
                            {
                                "ServiceType": 4,//家庭医生
                                "ServiceSwitch": 1,
                                "ServicePrice": 7
                            },
                            {
                                "ServiceType": 1,//图文咨询
                                "ServiceSwitch": 1,
                                "ServicePrice": 1
                            }
                        ],
                        "User": {
                            "UserID": "5E5E4318744248E99C18A71B8774E2E9",
                            "UserType": 0,
                            "PhotoUrl": "http://www.kmwlyy.com/Uploads/doctor/xiangjinlong.jpg",
                        }
                    }
                ],
                "Total": 75,
                "Status": 0,
                "Msg": "操作成功"
            }
       */
        /// <summary>
        /// 102 Personal Info:102108
        /// 获的推荐专家
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("~/Doctors/GetExpertDoctors")]
        [HttpGet]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        public ApiResult GetExpertDoctors(int pageIndex, int pageSize, string hospitalId = null)
        {
            return doctorBll.GetDoctorPagerListForExpertDoctor(pageIndex, pageSize, hospitalId).ToApiResultForList();
        }


        /**
         * @apiIgnore Not finished Method
       * @api {Get} /Doctors/GetRecommendDoctors 获的推荐专家
       * @apiGroup 102 Personal Info
       * @apiVersion 4.0.0
       * @apiDescription 获的推荐专家
       * @apiPermission 所有人
       * @apiHeader {String} apptoken Users unique access-key.
       * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
       * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
       * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
       * @apiParam {int} pageIndex=1 页码 
       * @apiParam {int} pageSize=10 分页大小
       * @apiParam {int} DoctorType  医生类型（0互联网医生，1多点执业医生，2执业医生(在康美医院工作的)，3自聘医生）
       * @apiParamExample {json} 请求样例：
       * Doctors/GetRecommendDoctors?CurrentPage=1&PageSize=10
       * @apiSuccess (Response) {String} Msg 提示信息 
       * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
       * @apiSuccess (Response) {int} Total 总记录     
       * @apiSuccess (Response) {object} Data 业务数据
       * @apiSuccessExample {json} 返回样例:
            *{
                "Data": [ {
                "DoctorID": "791ba9df4cc549979a306924fdf8d7a6",
                "DoctorName": "康医生",
                "Specialty": "能吹能抗能打",
                "HospitalID": "2fde5c6b640c42318929b17e8633d6bc",
                "HospitalName": "深圳康美网络在线门诊急诊综合大型国际化医院",
                "DepartmentID": "394603afb812482da766f935dfd83fc4",
                "DepartmentName": "精致内科",
                "Title": "住院医师",
                "TitleName": "",
                "User": {
                    "UserID": "e179019e46bf47a7804e028ccce2c84f",
                    "_PhotoUrl": "images/71a7eb532a4035edda430ddb258b90a5.jpg",
                    "PhotoUrl": "https://tstore.kmwlyy.com:8027/images/71a7eb532a4035edda430ddb258b90a5.jpg"
                }
             }],
                "Total": 75,
                "Status": 0,
                "Msg": "操作成功"
            }
       */
        /// <summary>
        /// 获的推荐专家(4.5版)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [Route("~/Doctors/GetRecommendDoctors")]
        [HttpGet]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [ApiOutputCacheFilter(60 * 60 * 2, 60 * 60 * 1, false)]
        public ApiResult GetRecommendDoctors([FromUri]RequestRecommendDoctorSearchDTO condition)
        {
            condition = condition ?? new RequestRecommendDoctorSearchDTO();

            var pageList = doctorBll.GetRecommendDoctorList(condition);
            return pageList.ToApiResultForList();

        }


        /**
         * @apiIgnore Not finished Method
       * @api {Get} /Doctors/GetSerivceTypeIncomes 107001/获取所有服务的服务次数和收入数据
       * @apiGroup 107 UserAccount
       * @apiVersion 4.0.0
       * @apiDescription 获取所有服务的服务次数和收入数据
       * @apiPermission 已登录(用户)
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
                    "SerivceType": 1,   //服务类型：0-挂号、1-图文咨询、2-语音问诊、3-视频问诊、4-处方支付、5-家庭医生、6-会员套餐、7-远程会诊、8-影像判读、100-其它
                    "TimesCount": 0,    //服务次数
                    "Income": 0         //收入
                },
                {
                    "SerivceType": 5,
                    "TimesCount": 0,
                    "Income": 0
                },
                {
                    "SerivceType": 7,
                    "TimesCount": 0,
                    "Income": 0
                }
            ],
            "Total": 0,
            "Status": 0,
            "Msg": "操作成功"
        }
       */
        /// <summary>
        /// 获取所有服务的服务次数和收入数据
        /// </summary>
        /// <returns></returns>
        [Route("~/Doctors/GetSerivceTypeIncomes")]
        [HttpGet]
        public ApiResult GetSerivceTypeIncomes(DateTime? startDate = null, DateTime? endDate = null)
        {
            string doctorId = doctorBll.GetDoctorIDByUserID(CurrentOperatorUserID);
            return doctorBll.GetDoctorSerivceTypeIncomes(doctorId, startDate, endDate).ToApiResultForObject();
        }
        
        /// <summary>
        /// 109 Attention Evaluation:109002
        /// 获取评价量
        /// </summary>
        /// <returns></returns>
        [Route("~/Doctors/GetEvaluationCount")]
        [HttpGet]
        public ApiResult GetEvaluationCount()
        {
            string doctorId = CurrentOperatorDoctorID;
            var bll = new BLL.Doctor.Implements.DoctorService();
            return bll.GetEvaluationNum(doctorId).ToApiResultForObject();
        }


        /// <summary>
        /// 102 Consultation：104019
        /// 医生获取自己远程会诊价格
        /// </summary>
        /// <returns></returns>
        [Route("~/Doctors/GetMyConsulServicePrice")]
        [HttpGet]
        public ApiResult GetMyConsulServicePrice()
        {
            var bll = new BLL.Doctor.Implements.DoctorService();
            var price = bll.GetDoctorServicePriceSetting(CurrentOperatorDoctorID, EnumDoctorServiceType.Consultation);
            return price.ToApiResultForObject();
        }

        /**
         * @apiIgnore Not finished Method
        * @api {POST} /Doctors/GetDoctorList 获取医生列表
        * @apiGroup 102 Personal Info
        * @apiVersion 4.0.0
        * @apiDescription 获取医生列表
        * @apiPermission 所有人
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam {int} CurrentPage=1 页码 
        * @apiParam {int} PageSize=10 分页大小
        * @apiParam {[]string} DepartmentIDs 科室ID（数组类型）
        * @apiParam {[]string} CAT_NOs 公共科室ID（数组类型）
        * @apiParam {[]string} DoctorTitles 医生职称（数组类型）
        * @apiParam {bool} IsFreeClinicr 是否义诊
        * @apiParam {int} SortFiled 排序字段(0综合，1好评量，2问诊量，3价格)
        * @apiParam {bool} SortIsAsc 是否升序排序
        * @apiParam {bool} IsFamilyDoctor 是否家庭医生
        * @apiParamExample {json} 请求样例：
        * Doctors/GetDoctorList?CurrentPage=1&PageSize=10
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
            *{
                "Data": [{
                "DoctorID": "791ba9df4cc549979a306924fdf8d7a6",
                "DoctorName": "康医生",
                "IsFreeClinicr": false,  //是否义诊
                "Specialty": "能吹能抗能打",
                "HospitalID": "2fde5c6b640c42318929b17e8633d6bc",
                "HospitalName": "深圳康美网络在线门诊急诊综合大型国际化医院",
                "DepartmentID": "394603afb812482da766f935dfd83fc4",
                "DepartmentName": "精致内科",
                "Title": "住院医师",
                "TitleName": "",
                "Duties": "联盟会成员"
                "User": {
                    "UserID": "e179019e46bf47a7804e028ccce2c84f",
                    "PhotoUrl": "https://tstore.kmwlyy.com:8027/images/71a7eb532a4035edda430ddb258b90a5.jpg"
                },
                "StatisticalData": {
                    "Sort": 9656,  //排名
                    "Followed": 5,  //关注量
                    "Commented": 0,  //评论数
                    "ImageText": 0,  //图文咨询量
                    "Audio": 0,  //语音问诊量
                    "Video": 0,  //视频问诊量
                    "FamilyDoctor": 0,  //家庭医生数
                    "RemoteConsultation": 0,  //远程会诊数
                    "AvgScore": 0,  //评价平均分
                    "Diagnose": 0  //问诊量(图文+视频+语音)
                }  
            }],
                "Total": 75,
                "Status": 0,
                "Msg": "操作成功"
            }
        */
        /// <summary>
        /// 获取医生列表(4.5版)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [Route("~/Doctors/GetDoctorList")]
        [HttpPost]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        public ApiResult GetDoctorList([FromBody]RequestDoctorSearchDTO condition)
        {
            condition = condition ?? new RequestDoctorSearchDTO();

            var pageList = doctorBll.GetDoctorList(condition);
            return pageList.ToApiResultForList();
        }


        /**
         * @apiIgnore Not finished Method
        * @api {POST} /Doctors/GetHospitalDoctors 获取机构医生列表（包括其他机构在该机构执业的医生）
        * @apiGroup 102 Personal Info
        * @apiVersion 4.0.0
        * @apiDescription 获取医生列表
        * @apiPermission 所有人
        * @apiHeader {String} apptoken Users unique access-key.
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
        * @apiParam {int} CurrentPage=1 页码 
        * @apiParam {int} PageSize=10 分页大小
        * @apiParam {string} DepartmentID 科室ID
        * @apiParam {[]string} DoctorTitles 医生职称（数组类型）
        * @apiParam {bool} IsFreeClinicr 是否义诊
        * @apiParam {int} SortFiled 排序字段(0综合，1好评量，2问诊量，3价格)
        * @apiParam {bool} SortIsAsc 是否升序排序
        * @apiParam {bool} IsFamilyDoctor 是否家庭医生
        * @apiParamExample {json} 请求样例：
        * Doctors/GetHospitalDoctors?CurrentPage=1&PageSize=10
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录     
        * @apiSuccess (Response) {object} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
            *{
                "Data": [{
                "DoctorID": "791ba9df4cc549979a306924fdf8d7a6",
                "DoctorName": "康医生",
                "IsFreeClinicr": false,  //是否义诊
                "Specialty": "能吹能抗能打",
                "HospitalID": "2fde5c6b640c42318929b17e8633d6bc",
                "HospitalName": "深圳康美网络在线门诊急诊综合大型国际化医院",
                "DepartmentID": "394603afb812482da766f935dfd83fc4",
                "DepartmentName": "精致内科",
                "Title": "住院医师",
                "TitleName": "",
                "Duties": "联盟会成员"
                "User": {
                    "UserID": "e179019e46bf47a7804e028ccce2c84f",
                    "PhotoUrl": "https://tstore.kmwlyy.com:8027/images/71a7eb532a4035edda430ddb258b90a5.jpg"
                },
                "StatisticalData": {
                    "Sort": 9656,  //排名
                    "Followed": 5,  //关注量
                    "Commented": 0,  //评论数
                    "ImageText": 0,  //图文咨询量
                    "Audio": 0,  //语音问诊量
                    "Video": 0,  //视频问诊量
                    "FamilyDoctor": 0,  //家庭医生数
                    "RemoteConsultation": 0,  //远程会诊数
                    "AvgScore": 0,  //评价平均分
                    "Diagnose": 0  //问诊量(图文+视频+语音)
                }  
            }],
                "Total": 75,
                "Status": 0,
                "Msg": "操作成功"
            }
        */
        /// <summary>
        /// 获取医生列表(4.5版)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [Route("~/Doctors/GetHospitalDoctors")]
        [HttpPost]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        public ApiResult GetHospitalDoctors([FromBody]RequestHospitalDoctorSearchDTO condition)
        {
            condition = condition ?? new RequestHospitalDoctorSearchDTO();

            var pageList = doctorBll.GetHospitalDoctors(condition);
            return pageList.ToApiResultForList();
        }
    }
}