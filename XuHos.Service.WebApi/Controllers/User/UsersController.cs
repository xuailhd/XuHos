using XuHos.BLL;
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
using XuHos.Common.Cache;
using XuHos.Extensions;

using XuHos.BLL.Sys;
using System.Web.Security;
using XuHos.BLL.Common;
using XuHos.BLL.Common.DTOs;
using XuHos.BLL.Common.DTOs.Response;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.BLL.User.DTOs.Response;
using XuHos.BLL.User.DTOs;
using XuHos.BLL.User.DTOs.Request;
using XuHos.Common.Cache.Keys;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.User.Implements;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 用户访问权限：用户
    /// </summary>
    [UserAuthenticate(IsValidUserType = false)]
    public class UsersController : ApiBaseController
    {
        private BLL.User.Implements.UserService userService;
        private BLL.Sys.Implements.SysShortMessageService serviceMsgLog;

        public UsersController()
        {
            userService = new UserService();
            serviceMsgLog = new BLL.Sys.Implements.SysShortMessageService();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpPost]
        public ApiResult Login([FromBody]RequestUserLoginDTO model)
        {
            if (model != null)
            {
                var userService = new BLL.User.Implements.UserService();
                var appToken = CurrentOperatorApp;
                model.AppID = appToken.AppId;

                return userService.Login(model);
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResult UpdateUserInfo([FromBody]UserDTO model)
        {
            userService = new BLL.User.Implements.UserService();
            model.UserID = CurrentOperatorUserID;
            if (userService.UpdateUserInfo(model))
            {
                var GetUserIDByMobile_CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserDTO>(StringCacheKeyType.User, model.UserID);
                GetUserIDByMobile_CacheKey.RemoveCache();
                return true.ToApiResultForBoolean();
            }

            return EnumApiStatus.BizError.ToApiResultForApiStatus();
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>     
        public ApiResult Logout(RequestUserLogoutDTO request)
        {
            using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
            {
                channel.BeginTransaction();
                
                channel.Publish<XuHos.EventBus.Events.UserLogoutedEvent>(new EventBus.Events.UserLogoutedEvent()
                {
                    LogoutTime = DateTime.Now,
                    UserID = CurrentOperatorUserID,
                    ClientName=request!=null ?request.ClientName.IfNull(""):"",
                    UserType = CurrentOperatorUserType,
                    ClientSourceType= CurrentOperatorAppSourceType
                });
               

                channel.Publish<XuHos.EventBus.Events.UserOperatorLogEvent>(new EventBus.Events.UserOperatorLogEvent()
                {
                    UserID = CurrentOperatorUserID,
                    OperatorTime = DateTime.Now,
                    UserType = CurrentOperatorUserType,
                    OperatorType = EnumUserOperationType.Logout,
                    OperatorName = "",
                    OrgID = CurrentOperatorOrgID,
                    Remark = "",
                    ModuleName = ""
                });
                channel.Commit();
            }


            XuHos.Service.Infrastructure.SecurityHelper.SignOut();

            return EnumApiStatus.BizOK.ToApiResultForApiStatus();
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult ChangePassword([FromBody]RequestUserChangePasswordDTO request)
        {
            userService = new BLL.User.Implements.UserService();
            request.UserID = CurrentOperatorUserID;
            ApiResult msgResult = userService.ChangePassword(request);
            return msgResult;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpPost]
        public ApiResult UserRegister([FromBody]RequestUserRegisterDTO request)
        {
            #region 短信验证码是否正确
            if (!serviceMsgLog.CheckVerifyCode(request.Mobile, "1", request.MsgVerifyCode))
            {
                return new ApiResult() { Status = EnumApiStatus.BizError, Msg = "对不起此短信验证码不存在或已经过期" };
            }
            #endregion

            #region 参数校验
            if (string.IsNullOrEmpty(request.OrgID))
            {
                request.OrgID = CurrentOperatorOrgID;
            }
            #endregion

            #region 注册逻辑处理

            XuHos.Entity.User model = new XuHos.Entity.User()
            {
                UserID = System.Guid.NewGuid().ToString("N"),
                Mobile = request.Mobile,
                Email = request.Email,
                Password = request.Password,
                UserType = request.UserType == EnumUserType.Default ? EnumUserType.User : request.UserType,
            };
            var Reason = "";
            userService = new BLL.User.Implements.UserService();
            if (userService.RegisterUser(model, out Reason, request.OrgID))
            {
                return new ApiResult()
                {
                    Status = EnumApiStatus.BizOK,
                    Msg = Reason
                };
            }
            else
            {
                return new ApiResult()
                {
                    Status = EnumApiStatus.BizError,
                    Msg = Reason
                };
            }

            #endregion
        }


        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpPost]
        public ApiResult SendSmsCode([FromBody]RequestMsgCodeMobileDto request)
        {
            var appToken = CurrentOperatorApp;
            var userService = new BLL.User.Implements.UserService();
            var shortMessageService = new SysShortMessageService();
 
            #region 校验：图像验证码
            if (!string.IsNullOrEmpty(request.VerifyCode))
            {
                if (!userService.CheckVerifyCode(request.VerifyCode, appToken.Token))
                {   
                    return EnumApiStatus.BizError.ToApiResultForApiStatus("验证码过期或不正确");
                }
            }
            #endregion

            #region  校验：用户注册短信 对不起此手机号已经注册 
            if (request.TemplateID == "1")
            {
                var userInfo = userService.GetUserInfoByMobile(request.Mobile, request.userType);
                if (userInfo != null)
                {
                    return EnumApiStatus.BizError.ToApiResultForApiStatus("对不起此手机号已经注册");
                }            
            }
            #endregion

            #region 校验： 找回密码短信 对不起此手机号尚未注册
             if (request.TemplateID == "2")
            {
                var userInfo = userService.GetUserInfoByMobile(request.Mobile, request.userType);
                if (userInfo == null)
                {
                    return EnumApiStatus.BizError.ToApiResultForApiStatus("对不起此手机号尚未注册");
                }
            }
            #endregion

            #region 校验：绑定手机号码短信 对不起此手机号已经注册
            if (request.TemplateID == "4")
            {
                var res = userService.ExistsMobile(request.Mobile);
                if (res == true)
                {
                    return EnumApiStatus.BizError.ToApiResultForApiStatus("对不起此手机号已经注册");
                }
            }
            #endregion

            #region 默认值：机构编号
            if (string.IsNullOrEmpty(request.OrgID))
            {
                request.OrgID = appToken.OrgID;
            }
            #endregion


            //获取模板
            var template = shortMessageService.GetTemplate(request.TemplateID);
            if (template == null)
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus($"短信模板不存在 TemplateID={request.TemplateID}");
            }
            var outMinute = 30;
            var codeNum = new Random().Next(100000, 999999);
            var msgContent = string.Format(template.TemplateContent, codeNum, outMinute);
            //var msgParms = string.Format("{0},{1}", codeNum, outMinute);
            var msgParms = new List<string>();
            msgParms.Add(codeNum.ToString());
            msgParms.Add(outMinute.ToString());

            var evt = new RequestSendSMSDTO()
            {
                MsgParms = msgParms,
                Content = msgContent,
                TemplateID = template.TemplateID,
                Mobile = request.Mobile,
                Title = codeNum.ToString(),
                OutTime = DateTime.Now.AddMinutes(outMinute),
            };


            var result = shortMessageService.SendMsg(evt);
            if (result.Status != EnumApiStatus.BizOK)
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus(result.Msg);
            }

            return EnumApiStatus.BizOK.ToApiResultForApiStatus("验证码发送成功");
        }

        [HttpPost]
        public ApiResult SendSmsCodeLogin([FromBody]RequestMsgCodeMobileDto request)
        {
            #region 校验：是否是允许的短信类型
            if (request.TemplateID != "6")
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus("不支持的类型");
            }
            #endregion

            var shortMessageService = new SysShortMessageService();

            //获取模板
            var template = shortMessageService.GetTemplate(request.TemplateID);
            if(template == null)
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus($"短信模板不存在，TemplateID={request.TemplateID}");
            }
            int outMinute = 30;
            int codeNum = new Random().Next(100000, 999999);


            request.IDNumber = request.IDNumber.Substring(0, request.IDNumber.Length - 4) + "****";

            //短信内容
            string msgContent = string.Format(template.TemplateContent,
                codeNum.ToString(),
                outMinute.ToString(),
                "aaa",
                request.IDNumber);

            //短信参数
            //string msgParms = string.Format("{0},{1},{2},{3}",
            //    Service.Infrastructure.SecurityHelper.LoginUser.UserCNName,
            //    request.IDNumber, 
            //    codeNum,
            //    outMinute);

            var msgParms = new List<string>();
            msgParms.Add(codeNum.ToString());
            msgParms.Add(outMinute.ToString());
            msgParms.Add("aaa");
            msgParms.Add(request.IDNumber);

            //模板编号
            var TemplateID = template.TemplateID;

            var evt = new RequestSendSMSDTO()
            {
                MsgParms = msgParms,
                Content = msgContent,
                TemplateID = TemplateID,
                Title = codeNum.ToString(),
                Mobile = request.Mobile,
                OutTime = DateTime.Now.AddMinutes(outMinute),
            };
            var result = shortMessageService.SendMsg(evt);

            if (result.Status != EnumApiStatus.BizOK)
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus(result.Msg);
            }

            return EnumApiStatus.BizOK.ToApiResultForApiStatus("验证码发送成功");
        }

        /**
            * @api {Get} /users/isLogin  101004/是否已登录
            * @apiGroup 101 Login Register
            * @apiVersion 4.0.0
            * @apiDescription 是否已登录 
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
            *  {
            *     "Data":null
            *     "Total":0,
            *     "Status":0,
            *     "Msg":""
            *  }
          */
        /// <summary>
        /// 获取登录状态
        
        /// 日期：2016年8月5日
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [IgnoreUserAuthenticate]
        public ApiResult IsLogin(EnumUserType userType = EnumUserType.Default)
        {
            if (XuHos.Service.Infrastructure.SecurityHelper.IsLogin(userType))
            {
                return new ApiResult() { Data = true, Status = 0, Msg = "操作成功" };
            }
            else
            {
                return new ApiResult() { Data = false, Status = 0, Msg = "操作成功" };
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IgnoreUserAuthenticate]
        public ApiResult UserFindPwd([FromBody]UserFindPwdDto request)
        {
            ApiResult msgResult;

            #region 短信验证码和手机号相关验证

            var userService = new BLL.User.Implements.UserService();
            var userInfo = userService.GetUserInfoByMobile(request.Mobile, request.UserType);

            if (userInfo == null)
            {
                msgResult = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "对不起此手机号未注册" };
                return msgResult;
            }
            if (!serviceMsgLog.CheckVerifyCode(request.Mobile, request.MsgType.ToString(), request.MsgVerifyCode))
            {

                msgResult = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "对不起此短信验证码不存在或已经过期" };
                return msgResult;

            }
            #endregion

            #region 修改密码操作
            if (userService.ResetPassword(userInfo.UserID, request.Password))
            {
                msgResult = new ApiResult { Status = EnumApiStatus.BizOK, Msg = "密码设置成功" };
                return msgResult;
            }
            else
            {
                msgResult = new ApiResult { Status = EnumApiStatus.BizError, Msg = "密码设置失败" };
                return msgResult;
            }
            #endregion
        }

        /// <summary>
        /// 101 Login Register:101203
        /// 用户找加密码第一步
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreUserAuthenticate]
        public ApiResult UserFindPwdPre([FromBody]UserFindPwdPreDto request)
        {
            #region 短信验证码和手机号相关验证
            ApiResult msgResult;
            var userService = new BLL.User.Implements.UserService();
            var userInfo = userService.GetUserInfoByMobile(request.Mobile, request.UserType);
            if (userInfo == null)
            {
                msgResult = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "对不起此手机号未注册" };
                return msgResult;
            }
            if (!serviceMsgLog.CheckVerifyCode(request.Mobile, request.MsgType.ToString(), request.MsgVerifyCode))
            {
                msgResult = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "对不起此短信验证码不存在或已经过期" };
                return msgResult;
            }

            string uid = StringEncrypt.Encrypt(userInfo.UserID);
            uid = HttpUtility.UrlEncode(uid);
            msgResult = new ApiResult { Status = 0, Msg = "", Data = uid };
            return msgResult;
            #endregion
        }

        /// <summary>
        /// 101 Login Register:101204
        /// 用户找加密码第一步
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreUserAuthenticate]
        public ApiResult UserFindPwdNext([FromBody]UserFindPwdNextDto request)
        {
            #region 相关验证
            ApiResult msgResult;
            if (string.IsNullOrEmpty(request.Id))
            {
                msgResult = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "对不起此手机号未注册" };
                return msgResult;
            }
            else
            {
                userService = new BLL.User.Implements.UserService();
                request.Id = StringEncrypt.Decrypt(request.Id);
                var userInfo = userService.GetUserInfoByUserId(request.Id);
                if (userInfo != null)
                {
                    if (userService.ResetPassword(userInfo.UserID, request.Password))
                    {
                        msgResult = new ApiResult { Status = EnumApiStatus.BizOK, Msg = "密码设置成功" };
                        return msgResult;
                    }
                    else
                    {
                        msgResult = new ApiResult { Status = EnumApiStatus.BizError, Msg = "密码设置失败" };
                        return msgResult;
                    }
                }
                else
                {
                    msgResult = new ApiResult { Status = EnumApiStatus.BizError, Msg = "对不起此用户不存在" };
                    return msgResult;
                }
            }
            return msgResult;
            #endregion
        }

        /// <summary>
        /// 获取个人资料
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetUserInfo()
        {
            userService = new BLL.User.Implements.UserService();
            return userService.GetUserInfoByUserId(CurrentOperatorUserID).ToApiResultForObject();
        }


        /// <summary>
        /// 检查用户账号是否存在
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="AccountType">账号类型</param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpGet]
        public ApiResult Exist(string Account, string AccountType = "Mobile")
        {
            var bll = new BLL.User.Implements.UserService();
            Response<List<UserDTO>> list = null;
            if (AccountType == "Mobile")
            {
                list = bll.GetPageList<UserDTO>(1, 1, i => i.Mobile == Account && !i.IsDeleted);
            }
            if (AccountType == "UserAccount")
            {
                list = bll.GetPageList<UserDTO>(1, 1, i => i.UserAccount == Account && !i.IsDeleted);
            }

            UserDTO model = null;
            //敏感数据不返回
            if (list.Data != null && list.Data.Count() > 0)
            {
                model = list.Data[0];
                model.Password = null;
                model.PayPassword = null;
                return model.ToApiResultForObject();
            }

            return EnumApiStatus.BizError.ToApiResultForApiStatus("该用户不存在");
        }

        /// <summary>
        /// 添加用户(不验证手机验证码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [IgnoreUserAuthenticate]
        [HttpPost]
        public ApiResult AddUser([FromBody]RequestUserRegisterDTO model)
        {
            var userID = Guid.NewGuid().ToString("N");
            var memberID = Guid.NewGuid().ToString("N");
            var entity = new XuHos.Entity.User()
            {
                UserID = userID,
                Mobile = model.Mobile,
                Email = model.Email,
                Password = model.Password,
                UserType = model.UserType,
            };

            if (string.IsNullOrEmpty(model.OrgID))
            {
                model.OrgID = CurrentOperatorOrgID;
            }

            var Reason = "";
            userService = new BLL.User.Implements.UserService();
            var result = userService.RegisterUser(entity, out Reason, model.OrgID, memberID);

            //返回参数不要改
            if (result)
            {
                return (userID + memberID).ToApiResultForObject();
            }
            else
            {
                return "".ToApiResultForObject();
            }
        }

        /// <summary>
        /// app找回密码(不验证短信验证码)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IgnoreUserAuthenticate]
        public ApiResult UserFindPwdBAT([FromBody]UserFindPwdBATDto request)
        {
            ApiResult msgResult;
            #region 短信验证码和手机号相关验证
            var userService = new BLL.User.Implements.UserService();
            var userInfo = userService.GetUserInfoByMobile(request.Mobile, EnumUserType.User);
            if (userInfo == null)
            {
                msgResult = new ApiResult() { Status = EnumApiStatus.BizError, Msg = "对不起此手机号未注册" };
                return msgResult;
            }
            #endregion

            #region 修改密码操作
            if (userService.ResetPassword(userInfo.UserID, request.Password))
            {
                msgResult = new ApiResult { Status = EnumApiStatus.BizOK, Msg = "密码设置成功" };
                return msgResult;
            }
            else
            {
                msgResult = new ApiResult { Status = EnumApiStatus.BizError, Msg = "密码设置失败" };
                return msgResult;
            }

            #endregion
        }
    }
}