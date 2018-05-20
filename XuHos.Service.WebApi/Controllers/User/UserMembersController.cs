using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using XuHos.Extensions;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Service.Infrastructure.Filters;
using XuHos.Common.Enum;
using XuHos.BLL.Sys.Implements;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 家庭成员
    /// </summary>
    [UserAuthenticate(IsValidUserType = false)]
    public class UserMembersController : ApiBaseController
    {
        BLL.User.Implements.UserMemberService bll;

        public UserMembersController()
        {
            bll = new BLL.User.Implements.UserMemberService();
        }
        /**
          * @api {POST} /UserMembers 102301/新增家庭成员
          * @apiGroup 102 Personal Info
          * @apiVersion 4.0.0
          * @apiDescription 添加家庭成员 
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写     
          * @apiParam {string} MemberName 姓名
          * @apiParam {string} Relation 成员关系 （0-自己、1-配偶、2-父亲、3-母亲、4-儿子、5女儿、6-其他）
          * @apiParam {string} Gender 性别（0-男、1-女、2-未知）
          * @apiParam {string} Marriage 婚姻状况(0-未婚、1-已婚、2-未知)
          * @apiParam {string} Birthday 生日
          * @apiParam {string} Mobile 手机号码
          * @apiParam {string} IDType 证件类型（0-身份证）
          * @apiParam {string} IDNumber 证件号码
          * @apiParam {string} [Address] 地址
          * @apiParam {string} [Email] 邮箱
          * @apiParam {string} [PostCode] 邮编
          * @apiParam {string} [SMSVerifyCode] 验证码
          * @apiParamExample {json} 请求样例：
          * {
              "MemberName": "sample string 3",
              "Relation": 4,
              "Gender": 0,
              "Marriage": 0,
              "Birthday": "sample string 5",
              "Mobile": "sample string 6",
              "IDType": 7,
              "IDNumber": "sample string 8",
              "Address": "sample string 9",
              "Email": "sample string 10",
              "PostCode": "sample string 11",
              "Age": 12,
              "SMSVerifyCode":"",
            "Province" : "省",
            "ProvinceRegionID" : "省ID",
            "City" : "市",
            "CityRegionID" : "市ID",
            "District" : "区",
            "DistrictRegionID" : "区ID",
            "Town" : "镇，街道",
            "TownRegionID" : "镇，街道ID"
            "Village" : "居委，村",
            "VillageRegionID" : "居委，村ID"
          * }
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {string} Data 0 最多只能添加5个就诊人,""或空添加失败，成员编号
          * @apiSuccessExample {json} 返回样例:
          *{
                "Data":"成员编号",
                "Total":0,
                "Status":0,
                "Msg":""
          *}
        **/

        /// <summary>
        /// 新增家庭成员
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="requst">实体</param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/UserMembers")]
        public ApiResult InsertMember([FromBody] BLL.User.DTOs.Request.RequestUserMemberDTO requst)
        {
            if (string.IsNullOrEmpty(requst.OrgID))
            {
                requst.OrgID = CurrentOperatorOrgID;
            }
            requst.UserID = CurrentOperatorUserID;

            if(!string.IsNullOrWhiteSpace(requst.SMSVerifyCode))//2017.08.03兼容健康服务站APP旧版本没有短信验证的功能
            {
                var result = VerifySMSCode(requst, true);
                if (result != null && result.Status != EnumApiStatus.BizOK)
                    return result;
            }

            return bll.InsertMemberInfo(requst);
        }


        /**
        * @api {DELETE} /UserMembers 102302/删除家庭成员
        * @apiGroup 102 Personal Info
        * @apiVersion 4.0.0
        * @apiDescription 通过成员编号删除 
        * @apiPermission 已登录（用户）
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
        * @apiParam {string} ID 用户编号
        * @apiParamExample 请求样例：
        * ?ID=XXXXX
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录数
        * @apiSuccess (Response) {bool} Data 是否成功 0删除失败，1成功，2有预约记录不能删除，3成员关系为本人的记录不能删除
        * @apiSuccessExample {json} 返回样例:
        *{
               "Data":true,
               "Total":0,
               "Status":0,
               "Msg":""
          }
       **/
        /// <summary>
        /// 删除家庭成员
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="ID">预约编号</param>
        /// <returns>0删除失败，1成功，2有预约记录不能删除</returns>
        [HttpDelete]
        [Route("~/UserMembers")]
        public ApiResult DeleteEntity(string ID)
        {
            var status = bll.DeleteMemberInfo(CurrentOperatorUserID, ID, CurrentOperatorOrgID);

            if (status == EnumApiStatus.BizUserMemberRejectDeleteMySelf)
            {
                return new ApiResult() { Status = status, Msg = status.GetEnumDescript(), Data = 3 };
            }
            else if (status == EnumApiStatus.BizUserMemberRejectDeleteHasRelationOrder)
            {
                return new ApiResult() { Status = status, Msg = status.GetEnumDescript(), Data = 2 };
            }
            else if (status == EnumApiStatus.BizOK)
            {
                return new ApiResult() { Status = status, Msg = status.GetEnumDescript(), Data = 1 };
            }
            else
            {
                return new ApiResult() { Status = status, Msg = status.GetEnumDescript(), Data = 0 };
            }
        }


        /**
         * @api {PUT} /UserMembers 102303/更新家庭成员
         * @apiGroup 102 Personal Info
         * @apiVersion 4.0.0
         * @apiDescription 更新家庭成员
         * @apiPermission 已登录（用户）
         * @apiHeader {String} apptoken appToken
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
         * @apiParamExample {json} 请求样例：
         * {
             "MemberID":"成员编号",
             "MemberName": "sample string 3",
             "Relation": 4, 成员关系 （0-自己、1-配偶、2-父亲、3-母亲、4-儿子、5女儿、6-其他）
             "Gender": 0, 性别（0-男、1-女、2-未知）
             "Marriage": 0, 婚姻状况(0-未婚、1-已婚、2-未知)
             "Birthday": "1898-03-45"
             "Mobile": "sample string 6",
             "IDType": 7,
             "IDNumber": "sample string 8",
             "Address": "sample string 9",
             "Email": "sample string 10",
             "PostCode": "sample string 11",
               "SMSVerifyCode" : "验证码"
             "Age": 12，
            "Province" : "省"，
            "ProvinceRegionID" : "省ID"，
            "City" : "市"，
            "CityRegionID" : "市ID"
            "District" : "区"，
            "DistrictRegionID" : "区ID"
            "Town" : "村，街道"，
            "TownRegionID" : "村，街道ID"
            "Village" : "居委，村",
            "VillageRegionID" : "居委，村ID"
         * }
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {bool} Data 是否成功 true=成功,false=失败
         * @apiSuccessExample {json} 返回样例:
         *{
                "Data":true,
                "Total":0,
                "Status":0,
                "Msg":""
           }
        **/
        /// <summary>
        /// 更新家庭成员
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="ID">预约编号</param>
        /// <returns></returns>
        [HttpPut]
        [Route("~/UserMembers")]
        public ApiResult UpdateEntity([FromBody] BLL.User.DTOs.Request.RequestUserMemberDTO requst)
        {
            if (string.IsNullOrEmpty(requst.OrgID))
            {
                requst.OrgID = CurrentOperatorOrgID;
            }

            requst.UserID = CurrentOperatorUserID;


            if (!string.IsNullOrWhiteSpace(requst.SMSVerifyCode))//2017.08.04兼容健康服务站APP旧版本没有短信验证的功能
            {
                var result = VerifySMSCode(requst, false);
                if (result != null && result.Status != EnumApiStatus.BizOK)
                    return result;
            }


            var ret = bll.UpdateMemberInfo(requst).ToApiResultForApiStatus().AsStatusToBoolean();
            //更新登录信息
            if (ret.Status == EnumApiStatus.BizOK && requst.Relation == EnumUserRelation.MySelf)
            {
                var curruser = Service.Infrastructure.SecurityHelper.LoginUser;
                if (curruser != null && curruser.UserType == EnumUserType.User)
                {
                    curruser.UserCNName = requst.MemberName ?? "";
                    curruser.UserENName = requst.MemberName ?? "";
                    curruser.IDNumber = requst.IDNumber ?? "";
                    curruser.Email = requst.Email ?? "";
                    curruser.Mobile = requst.Mobile ?? "";
                    Service.Infrastructure.SecurityHelper.SignIn(curruser);
                }
            }

            return ret;
        }

        /// <summary>
        /// 健康服务站，康美时代需要验证短信验证码
        /// </summary>
        /// <param name="requst"></param>
        /// <returns></returns>
        private ApiResult VerifySMSCode([FromBody] BLL.User.DTOs.Request.RequestUserMemberDTO requst, bool isInsert)
        {
            #region 校验身份证号码格式
            if (string.IsNullOrEmpty(requst.IDNumber))
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus("没有填写身份证号码");
            }
            #endregion

            #region  校验：短信验证码格式正确

            if (!string.IsNullOrEmpty(requst.Mobile) && !string.IsNullOrEmpty(requst.SMSVerifyCode))
            {
                if (requst.OrgID != null && (requst.OrgID.ToLower() == "jkfwz" || requst.OrgID.ToLower() == "kmsd")) //
                {
                    if (!new SysShortMessageService().CheckVerifyCode(requst.Mobile, "6", requst.SMSVerifyCode))
                    {
                        return EnumApiStatus.BizError.ToApiResultForApiStatus("对不起此短信验证码不存在或已经过期");
                    }
                }
            }
            #endregion

            #region 校验：身份证号码是否重复
            if (isInsert)
            {
                var members = bll.GetMemberList(CurrentOperatorUserID);

                if (members != null && members.Count > 0)
                {
                    var member = members.Where(t => t.IDNumber == requst.IDNumber).FirstOrDefault();
                    if (member != null)
                    {
                        return EnumApiStatus.BizError.ToApiResultForApiStatus("该身份证号已经绑定");
                    }
                }
            }
            #endregion

            return EnumApiStatus.BizOK.ToApiResultForApiStatus();
        }

        /**
          * @api {GET} /UserMembers 102304/获取家庭成员详情
          * @apiGroup 102 Personal Info
          * @apiVersion 4.0.0
          * @apiDescription 查询家庭成员列表
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
          * @apiParam {string} ID  家庭成员ID
          * @apiParamExample {json} 请求样例：
          *                   ?ID=XXX
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 家庭成员详情
          * @apiSuccessExample {json} 返回样例:
          * {
                "Data": {
                    "MemberID": "6836017638eb448ca96f7df75730700e",
                    "UserID": "54e169e1604943c991b1be48b5d5fa85",
                    "MemberName": "XXX",
                    "Relation": 0,
                    "Gender": 0,
                    "Marriage": 0,
                    "Birthday": "1985-01-01",
                    "Mobile": "",
                    "IDType": 0,
                    "IDNumber": "XXXX",
                    "Address": "",
                    "Email": "",
                    "PostCode": "",
                    "Age": 32,
                    "IsDefault": false
                },
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
            }
        **/
        /// <summary>
        /// 获取家庭成员详情
        /// 前置条件：已登录
        /// </summary>
        /// <param name="ID">成员编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/UserMembers")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetEntity([FromUri] string ID)
        {
            if (CurrentOperatorUserType == EnumUserType.User)
            {
                return bll.GetMemberInfo(CurrentOperatorUserID, ID).ToApiResultForObject();
            }
            else
            {
                return bll.GetMemberInfo(ID).ToApiResultForObject();
            }
        }


        /**
          * @api {GET} /UserMembers 102305/获取家庭成员列表
          * @apiGroup 102 Personal Info
          * @apiVersion 4.0.0
          * @apiDescription 查询家庭成员列表 
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
          * @apiParam {string} [CurrentPage=1]  页码
          * @apiParam {string} [PageSize=10] 分页大小
          * @apiParamExample {json} 请求样例：
          *                   ?CurrentPage=1PageSize=10
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 成员列表
          * @apiSuccessExample {json} 返回样例:
          * {
                "Data": [
                    {
                        "MemberID": "e868beff8c074a4b8364ab4760752570",
                        "UserID": "54e169e1604943c991b1be48b5d5fa85",
                        "MemberName": "ABC",
                        "Relation": 6,
                        "Gender": 0,
                        "Marriage": 0,
                        "Birthday": "1985-06-14",
                        "Mobile": "18028782898",
                        "IDType": 0,
                        "IDNumber": "43252419850614171X",
                        "Address": "",
                        "Email": "",
                        "PostCode": "",
                        "Age": 32,
                        "IsDefault": false
                    }
                ],
                "Total": 1,
                "Status": 0,
                "Msg": ""
            }
        **/
        /// <summary>
        /// 获取家庭成员列表
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月5日
        /// </summary>
        /// <param name="CurrentPage">分页索引</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/UserMembers")]
        public ApiResult GetEntitys(int CurrentPage = 1, int PageSize = int.MaxValue)
        {
            var list = bll.GetMemberList(CurrentOperatorUserID);
            if (PageSize < 1)
            {
                PageSize = int.MaxValue;
            }
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
            var result = list.Skip(CurrentPage * PageSize - PageSize).Take(PageSize).ToList().ToApiResultForObject();
            result.Total = list.Count;
            return result;
        }

        /**
          * @api {GET} UserMembers/GetDefaultMember 102306/获取默认家庭成员
          * @apiGroup 102 Personal Info
          * @apiVersion 4.0.0
          * @apiDescription 查询家庭成员列表 
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
          * @apiParamExample {json} 请求样例：
          *                   UserMembers/GetDefaultMember
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 成员列表
          * @apiSuccessExample {json} 返回样例:
          * {
                "Data": [
                    {
                        "MemberID": "77C5CF07923A4E3D8121F628336527B8",
                        "UserID": "9A4C83966C784DD5BEFA68766591A272",
                        "Relation": 1,
                        "Gender": 1,
                        "Marriage": 1,
                        "Birthday": "19890202",
                        "Mobile": "18688941654",
                        "IDType": 1,
                        "IDNumber": "0",
                        "Address": "",
                        "Email": "18688941654@qq.com",
                        "PostCode": "0",
                        "Age": 0,
                        "IsDefault":true
                    }
                ],
                "Total": 1,
                "Status": 0,
                "Msg": ""
            }
        **/
        /// <summary>
        /// 获取默认家庭成员
        /// </summary>
        /// <returns></returns>
        public ApiResult GetDefaultMember()
        {
            //兼容旧的返回模式
            var model = bll.GetDefaultMemberInfo(CurrentOperatorUserID);
            List<XuHos.BLL.User.DTOs.Response.ResponseUserMemberDTO> list = new List<XuHos.BLL.User.DTOs.Response.ResponseUserMemberDTO>();
            list.Add(model);
            return list.ToApiResultForObject();
        }

        /**
          * @api {GET} /UserMembers/SetDefault 102307/设置默认家庭成员
          * @apiGroup 102 Personal Info
          * @apiVersion 4.0.0
          * @apiDescription 设置默认成员  作者：郭超
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
          * @apiParam {string} [memberID]  成员ID
          * @apiParamExample {json} 请求样例：
          *                   ?memberID=77C5CF07923A4E3D8121F628336527B8
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 是否成功 true=成功,false=失败
          * @apiSuccessExample {json} 返回样例:
          * {"Data":true,"Total":1,"Status":0,"Msg":""}
        **/
        /// <summary>
        /// 设置默认成员
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/UserMembers/SetDefault")]
        public ApiResult SetDefaultMember(string memberID)
        {
            var res = bll.SetDefaultMember(memberID, CurrentOperatorUserID);
            return res.ToApiResultForBoolean();
        }
        
        /**
          * @api {GET} /UserMembers/GetExistUserMemberByMobile 102310/获取已存在的手机号账号的自已成员
          * @apiGroup 102 Personal Info
          * @apiVersion 4.0.0
          * @apiDescription 获取自己关系家庭成员
          * @apiPermission 已登录（用户）
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写    
          * @apiParam {string} [mobile]  手机号ID
          * @apiParamExample {json} 请求样例：
          *                   ?mobile=12899787788    
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 是否成功 true=成功,false=失败
          * @apiSuccessExample {json} 返回样例:
          * {
                "Data": {
                    "MemberID": "6836017638eb448ca96f7df75730700e",
                    "UserID": "54e169e1604943c991b1be48b5d5fa85",
                    "MemberName": "XXX",
                    "Relation": 0,
                    "Gender": 0,
                    "Marriage": 0,
                    "Birthday": "1985-01-01",
                    "Mobile": "",
                    "IDType": 0,
                    "IDNumber": "XXXX",
                    "Address": "",
                    "Email": "",
                    "PostCode": "",
                    "Age": 32,
                    "IsDefault": false
                },
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
            }
        **/
        /// <summary>
        /// 获取已存在的手机号账号的自已成员
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpGet]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [Route("~/UserMembers/GetExistUserMemberByMobile")]
        public ApiResult GetExistUserMemberByMobile(string mobile)
        {
            return bll.GetGetExistUserMemberByMoble(mobile);
        }
    }
}