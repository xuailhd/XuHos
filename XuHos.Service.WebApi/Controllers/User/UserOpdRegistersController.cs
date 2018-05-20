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
using XuHos.Entity;
using XuHos.DTO;
using XuHos.Extensions;
using XuHos.DTO.Platform;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.User.DTOs.Request;
using XuHos.BLL.User.Implements;
using UserOPDRegisterService = XuHos.BLL.UserOPDRegisterService;
using XuHos.BLL.Doctor.Implements;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 预约接口
    /// </summary>
    public class UserOPDRegistersController : ApiBaseController
    {

        /**
           * @api {POST} /UserOPDRegisters 110301//新增看诊预约
           * @apiGroup 110 Treatment
           * @apiVersion 4.0.0
           * @apiDescription 提交预约信息 
           * @apiPermission 已登录(用户)
           * @apiHeader {String} apptoken Users unique access-key.
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
           * @apiParam {string} MemberID 成员编号 
           * @apiParam {string} [DoctorID] 医生编号
           * @apiParam {string} ScheduleID 医生排版编号
           * @apiParam {string} OPDDate 预约日期(格式'yyyy-MM-dd')
           * @apiParam {int} OPDType=0,1,2,3  类型 0-挂号、1-图文咨询、2-语音咨询、3-视频咨询、4-家庭医生、5-远程会诊
           * @apiParam {string} [OrgnazitionID] 机构编号 (可空)
           * @apiParam {string} [DoctorGroupID] 医生分组编号(使用家庭医生咨询时提供)
           * @apiParam {string} [UserPackageID] 用户套餐ID (需要指定使用特定套餐的情况下使用)
           * @apiParam {string} RandomCode 验证码
           * @apiParamExample {json} 请求样例：
           *{
                "MemberID": "XXXX", //成员编号    
                "ScheduleID": "XXXX",//医生排版编号(一键呼叫不需要可控可难过)
                "OPDType": 0,//预约类型，
                "ConsultContent":"咨询内容"，  
                "OrgnazitionID":"机构编号",
                “Privilege”：0, //折扣类型(不使用特权=0,义诊=2,套餐=3,家庭医生=5,机构折扣=6)      
                 Files:[
                 {
                    FileUrl:"/xxx/xxx.jpg",
                    Remark:"这里是图片的说明"
                 }]
            *}
            * @apiSuccess (Response) {String} Msg 提示信息 
            * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
            * @apiSuccess (Response) {int} Total 总记录     
            * @apiSuccess (Response) {object} Data 业务信息
            * @apiSuccessExample {json} 返回样例:
            *{
                "Data":{
                        OPDRegisterID:"预约编号",
                        OrderNO:"订单编号",
                        OrderState:0, //订单状态（0-待支付、1-已支付、2-已完成、3-已取消）
                        ErrorInfo:"错误消息",
                        ActionStatus:"状态" //Repeat/Success/Fail/UnSupport
                },
                "Total":0,
                "Status":0,
                "Msg":""
            *}
        **/
        /// <summary>
        /// 新增看诊预约
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="requst">实体</param>
        /// <returns></returns>
        [Route("~/UserOPDRegisters")]
        [HttpPost]
        [UserAuthenticate(UserType = EnumUserType.User)]
        public ApiResult Insert([FromBody]RequestUserOPDRegisterSubmitDTO request)
        {
            BLL.UserOPDRegisterService bll = new UserOPDRegisterService(CurrentOperatorUserID);

            if(request.OPDType != EnumDoctorServiceType.VidServiceType && request.OPDType != EnumDoctorServiceType.AudServiceType)
            {
                return new ApiResult() { Status = EnumApiStatus.BizError, Msg = "此接口只支持视频和语音预约" };
            }

            //默认当前登录用户
            request.UserID = CurrentOperatorUserID;
            request.UserType = CurrentOperatorUserType;
            request.UserLevel = CurrentOperatorUserLevel;

            if (string.IsNullOrEmpty(request.OrgnazitionID))
            {
                if (CurrentOperatorUser != null && !string.IsNullOrEmpty(CurrentOperatorUser.OrgID))
                {
                    request.OrgnazitionID = CurrentOperatorUser.OrgID;
                }
            }

            var ret = bll.Submit(request, true);

            //成功
            if (ret.ActionStatus == "Success")
            {
                return ret.ToApiResultForObject(EnumApiStatus.BizOK);
            }
            //不支持
            else if (ret.ActionStatus == "UnSupport")
            {
                return ret.ToApiResultForObject(EnumApiStatus.BizOK);
            }
            //预约重复
            else if (ret.ActionStatus == "Repeat")
            {
                return ret.ToApiResultForObject(EnumApiStatus.BizOK);
            }
            else if (ret.ActionStatus == "DiagnoseOff")
            {
                return ret.ToApiResultForObject(EnumApiStatus.BizOK);
            }
            //预约失败
            else if (ret.ActionStatus == "Fail")
            {
                return ret.ToApiResultForObject(EnumApiStatus.BizError);
            }
            else
            {
                return new ApiResult() { Data = ret, Status = EnumApiStatus.BizError, Msg = ret.ErrorInfo };
            }
        }


        /**
           * @api {Post} /UserOPDRegisters/TodaySubmited 110302/查询是否已预约看诊
           * @apiGroup 110 Treatment
           * @apiVersion 4.0.0
           * @apiDescription 查询是否已预约看诊
           * @apiPermission 已登录(用户)
           * @apiHeader {String} apptoken Users unique access-key.
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@ appkey 串MD5加密后转成大写        
           * @apiParam {string} MemberID 成员编号 
           * @apiParam {string} DoctorID 医生编号
           * @apiParam {string} ScheduleID 医生排班编号
           * @apiParam {int} OPDType=2,3  类型 2-语音咨询、3-视频咨询
           * @apiParamExample {json} 请求样例：
           *{
                "MemberID": "XXXX", //成员编号    
                "DoctorID":"XXXX",
                "ScheduleID": "XXXX",//医生排版编号
                "OPDType": 2,//预约类型，        
            *}
            * @apiSuccess (Response) {String} Msg 提示信息 
            * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
            * @apiSuccess (Response) {int} Total 总记录     
            * @apiSuccess (Response) {object} Data 业务信息
            * @apiSuccessExample {json} 返回样例:
            *{
                "Data":"True",(已预约)
                "Total":0,
                "Status":0,
                "Msg":""
            *}
        **/
        /// <summary>
        /// 查询是否已预约看诊
        /// 前置条件：用户已登录
        /// </summary>
        /// <param name="requst">实体</param>
        /// <returns></returns>
        [HttpPost]
        [UserAuthenticate(UserType = EnumUserType.User)]
        public ApiResult TodaySubmited([FromUri]RequestUserOPDRegisterSubmitDTO request)
        {
            BLL.UserOPDRegisterService bll = new UserOPDRegisterService(CurrentOperatorUserID);

            //默认当前登录用户
            request.UserID = CurrentOperatorUserID;
            OrderRepeatReturnDTO existsOrder;
            return bll.ExistsWithSubmitRequest(request, out existsOrder).ToApiResultForBoolean();
        }


        /**
         * @api {GET} /UserOPDRegisters?OPDRegisterID=:OPDRegisterID 110305/获取预约详情
         * @apiGroup 110 Treatment
         * @apiVersion 4.0.0
         * @apiDescription 获取预约详情 
         * @apiPermission 已登录（用户/医生/分诊医生）
         * @apiHeader {String} apptoken appToken
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
         * @apiParam {int} OPDRegisterID 预约ID 
         * @apiParamExample {json} 请求样例：
         * ?OPDRegisterID=XXXX
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {int} Data 预约信息
         * @apiSuccessExample {json} 返回样例:
         *{
            "Data": {
                "OPDRegisterID": "e679e1e43cfe4c52a531657c6e5f1e49",
                "UserID": "717ddf61b8d14d23bc2967838b0a2690",
                "DoctorID": "da716d81d2b94c1baff784fa171dca9d",
                "ScheduleID": "",
                "RegDate": "2016-12-16T19:16:01.263",
                "OPDDate": "2016-12-16T00:00:00",
                "OPDType": 0,
                "Fee": 0,
                "MemberID": "a8d3baa4e91a45d8a46c49a2b35ec20c",
                "ConsultContent": "",
                "RecipeFileUrl": "",//处方文件URL,只有所有处方已经签名才有值
                "RecipeFiles": [],
                "Member": {
                    "MemberName": "19F9E5907FD04DBF96A9867D1FA30396",
                    "Gender": 0,//性别（0-男、1-女、2-未知）
                    "Birthday": "2015-01-01",//出生日期
                    "Age": 2//年龄
                },
                "Doctor": {
                    "DoctorID": "89F9E5907FD04DBF96A9867D1FA30396",
                    "DoctorName": "邱浩强",
                    "Title": "",//头衔
                    "User": {
                        "PhotoUrl": ""//医生头像
                    }
                },
                "Order": {
                    "OrderNo": "....",//订单编号
                    "TotalFee": 0.01,//订单金额
                    "OrderState": -1,//订单状态（state：-1=待确认、0=待支付、1=已支付、2=已完成、3=已取消）
                    "IsEvaluated": false//是否已评价
                },
                "Room": {
                    "ChannelID": 7875,//房间编号
                    "RoomState": 0,//房间状态 -1 其它，0 未就诊，1 候诊中，2 就诊中，3 已就诊，4 呼叫中，5 离开中，6 断开连接
                    "Secret": "",//房间密码
                },
                "UserMedicalRecord": {
                    "UserMedicalRecordID": "658cc672328a4f0cb1c28d825f2fd32d",
                    "OPDRegisterID": "e679e1e43cfe4c52a531657c6e5f1e49",
                    "UserID": "717ddf61b8d14d23bc2967838b0a2690",
                    "MemberID": "a8d3baa4e91a45d8a46c49a2b35ec20c",
                    "DoctorID": "da716d81d2b94c1baff784fa171dca9d",
                    "Sympton": "主诉",//主诉
                    "PastMedicalHistory": "既往病史",//既往病史
                    "PresentHistoryIllness": "现病史",//现病史
                    "PreliminaryDiagnosis": "中医诊断",//初步诊断
                    "AllergicHistory": "过敏史",//过敏史
                    "Advised": "治疗意见",//医嘱
                    "OrgnazitionID": ""
                }
            },
            "Total": 0,
            "Status": 0,
            "Msg": "操作成功"
        }
     **/
        /// <summary>
        /// 获取预约详情
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="OPDRegisterID">预约编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/UserOPDRegisters")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetEntity([FromUri]string OPDRegisterID)
        {
            BLL.UserOPDRegisterService bll = new UserOPDRegisterService(CurrentOperatorUserID);
            return bll.Single(OPDRegisterID, CurrentOperatorUserType == EnumUserType.User ? true : false).ToApiResultForObject();
        }

        /**
           * @api {GET} /UserOPDRegisters 110307/获取预约记录列表
           * @apiGroup 110 Treatment
           * @apiVersion 4.0.0
           * @apiDescription 查询用户预约的历史记录 
           * @apiPermission 已登录（用户）
           * @apiHeader {String} apptoken appToken
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
           * @apiParam {int} CurrentPage=1 页码 
           * @apiParam {int} PageSize=10 分页大小
            * @apiParam {int} [OPDType] 医生服务类型 1-图文咨询、2-语音咨询、3-视频咨询、4-家庭医生、5-远程会诊
           * @apiParam {string} [Keyword] 关键字
           * @apiParam {string} [BeginDate] 预约开始日期
           * @apiParam {string} [EndDate] 预约截止日期
           * @apiParam {string} [MemberID] 可空，传了则取这个Memberid下的预约记录和身份证号下预约记录
           * @apiParamExample {json} 请求样例：
           *                   ?CurrentPage=1&PageSize=1&OPDType=1&Keyword=&BeginDate=&EndDate=
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {Array} Data 业务数据
           * @apiSuccessExample {json} 返回样例:
           *{
                "Data": [
                    {
                        "OPDRegisterID": "d61bee05b454429c94cdf3ba237cfe43",
                        "UserID": "9A4C83966C784DD5BEFA68766591A272",
                        "RegDate": "2016-08-09T14:27:41.01",
                        "OPDDate": "20160809",
                        "VisitDate": "2016-08-09T14:27:41.01",
                        "OPDType": 1,//医生服务类型 1-图文咨询、2-语音咨询、3-视频咨询、4-家庭医生、5-远程会诊
                        "BeginTime": "0001-01-01T00:00:00",
                        "EndTime": "0001-01-01T00:00:00",
                        "TotalTime": 0,
                        "State": -1,
                        "ChannelId": "d61bee05b454429c94cdf3ba237cfe43",
                        "RandomCode": "09e6739e1b5d4ead983b06e716c2c9ca",
                        "Fee": 0,
                        "RecipeFileUrl": "http://commonapi.kmwlyy.com/File/Download/d61bee05b454429c94cdf3ba237cfe43/",// 处方文件路径
                        "Deletable": false, //是否可删除
                        "Cancelable": false, //是否可取消
                        "RecipeFiles":[
                         {
                            "RecipeFileID":"2020200",//处方编号
                            "RecipeDate":"2016-06-01",//处方日期
                            "RecipeFileStatus":"",//处方状态
                            "RecipeName":"处方名称",
                            "RecipeType":"1",//1=中药，2=西药
                            "Amount":"1.05"
                          }            
                        ],
                        "Order": {
                            "OrderNo": "TW2016080914274189636257",
                            "OrderTime": "2016-08-09T14:27:41.0794296",
                            "TradeNo": "",//交易编号
                            "LogisticNo": "",//物流编号
                            "PayType": -1,//支付类型（state：-1-免支付、0-康美支付、1-微信支付、2-支付宝、3-中国银联、4-MasterCard、5-PayPal、6-VISA,7-HIS,8=余额）
                            "OrderState": 2,//订单状态（state：0-待支付、1-已支付、2-已完成、3-已取消）
                            "RefundState": 0,//退款状态 1=申请退款,2=已退款,3=拒绝退款
                            "LogisticState": -1,//物流状态 -1=待审核,0=已审核,1=已备货,2=已发货,3=配送中,4=已送达                       
                            "TotalFee": 0,//订单金额
                            "IsEvaluated": false//是否已评价
                        },
                        "Member": {
                            "MemberID": "77C5CF07923A4E3D8121F628336527B8",
                            "UserID": "9A4C83966C784DD5BEFA68766591A272",
                            "Relation": 0,
                            "Gender": 1,
                            "Marriage": 0,
                            "IDType": 0,
                            "Age": 0
                        },
                        "Doctor": {
                            "DoctorID": "89F9E5907FD04DBF96A9867D1FA30396",
                            "DoctorName": "邱浩强",
                            "Gender": 0,
                            "Marriage": 0,
                            "IDType": 0,
                            "IsConsultation": false,
                            "IsExpert": false,
                            "HospitalID": "42FF1C61132E443F862510FF3BC3B03A",
                            "HospitalName": "康美医院",
                            "DepartmentID": "A8064D2DAE3542B18CBD64F467828F57",
                            "DepartmentName": "健康体检中心",
                            "CheckState": 0,
                            "Sort": 0,
                             Specialty:"",//擅长
                             Title:"",//头衔
                             Duties:""//职务,
                             User:{
                                PhotoUrl:""//医生头像
                            }
                        },
                        "Schedule": {
                            "ScheduleID": "de090d31290c49ac81da817444c8acbe",
                            "StartTime": "14:00",
                            "EndTime": "15:00",
                            "Disable": false,
                            "Checked": false
                        },
                        "Room": {
                            "ChannelID": 11288,
                            "Secret": "cbf569b4cede4bccbc13bfc403ac181d",
                            "RoomState": 3
                        }
                    }
                ],
                "Total": 2,
                "Status": 0,
                "Msg":""}
       **/
        /// <summary>
        /// 获取预约记录列表
        /// 前置条件：已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/UserOPDRegisters")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetEntitys([FromUri]DTO.RequestUserQueryOPDRegisterDTO request)
        {
            BLL.UserOPDRegisterService bll = new UserOPDRegisterService(CurrentOperatorUserID, CurrentOperatorOrgID);
            request.WithoutNotSigned = CurrentOperatorUserType == EnumUserType.User || CurrentOperatorUserType == EnumUserType.Drugstore;
            return bll.GetPageList(request).ToApiResultForList();
        }

        /**
        * @api {GET} /UserOPDRegisters/GetDoctorAudVid 110308/获取医生的语音/视频看诊
        * @apiGroup 110 Treatment
        * @apiVersion 4.0.0
        * @apiDescription 医生的语音/视频看诊订单
        * @apiPermission 已登录（医生）
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
        * @apiParam {int} CurrentPage 页码 
        * @apiParam {int} PageSize 分页大小
        * @apiParamExample {json} 请求样例：
        *?CurrentPage=1&PageSize=10
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录数
        * @apiSuccess (Response) {string} Data 数据
        * @apiSuccessExample {json} 返回样例:
        *{
           "Data":
               [{
                 "OPDRegisterID":"B04D4AE28F994AE2AACBB456D7E0647B",
                 "RegDate":"2016-8-25T00:00:00", //预约时间
                 "OPDType":1,  //预约类型（0挂号，1图文，2语音，3视频）
                 "OPDDate":"2016-8-26T00:00:00", //看诊时间
                 "OrderState":1, //订单状态（state：0-待支付、1-已支付、2-已完成、3-已取消）
                 "Price":0.01,
                 "MemberID":"B04D4AE28F994AE2AACBB456D7E0647B",
                 "ConsultContent":".....",//病情描述
                 "MemberName":"张三",
                 "Gender": 1,//性别
                 "GenderText": "女",//性别
                 "Age": 1,//年龄
                 "ChannelID": 0,//就诊的 ChannelID
                 "RecipeFileUrl": "http://commonapi.kmwlyy.com/File/Download/d61bee05b454429c94cdf3ba237cfe43/",// 处方文件路径
                 "RecipeFiles":[
                    {
                        "RecipeFileID":"2020200",//处方编号
                        "RecipeDate":"2016-06-01",//处方日期
                        "RecipeFileStatus":"",//处方状态
                        "RecipeName":"处方名称",
                        "RecipeType":"1",//1=中药，2=西药
                        "Amount":"1.05"
                    }            
                 ],
                 "Schedule": {
                    StartTime:"09:10",
                    EndTime="09:10",
                    OPDate="2016-09-08"
                 }
               }],
           "Total": 10,
           "Status": 0,
           "Msg": "操作成功"
         }
        **/
        /// <summary>
        /// 获取医生的语音/视频看诊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/UserOPDRegisters/GetDoctorAudVid")]
        [UserAuthenticate(UserType = EnumUserType.Doctor)]
        public ApiResult GetDoctorAudVid([FromUri]DTO.RequestUserQueryOPDRegisterDTO request)
        {
            var result = new UserOPDRegisterService(CurrentOperatorUserID).GetDoctorAudVid(CurrentOperatorDoctorID, request.CurrentPage, request.PageSize);
            return result.ToApiResultForList();
        }
        
        /**
        * @api {Delete} /UserOPDRegisters 110309/删除语音/视频看诊记录
        * @apiGroup 110 Treatment
        * @apiVersion 4.0.0
        * @apiDescription 删除语音/视频看诊记录
        * @apiPermission 已登录（用户）
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
        * @apiParam {string} ID 语音/视频记录ID  
        * @apiParamExample {json} 请求样例：
           /UserOPDRegisters?ID=xxx
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录数
        * @apiSuccess (Response) {Array} Data 业务数据
        * @apiSuccessExample {json} 返回样例:
        * {
            "Data": true
            "Total": 0,
            "Status": 0,
            "Msg": "操作成功"
        }
        **/
        /// <summary>
        /// 110 Treatment：110309
        /// 删除看诊预约
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("~/UserOPDRegisters")]
        public ApiResult DeleteEntity(string ID)
        {
            BLL.UserOPDRegisterService bll = new UserOPDRegisterService(CurrentOperatorUserID);
            return bll.Delete(ID).ToApiResultForBoolean();
        }

    }


}
