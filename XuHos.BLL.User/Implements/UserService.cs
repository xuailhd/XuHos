using EntityFramework.Extensions;
using XuHos.BLL.Common.DTOs.Response;
using XuHos.BLL.Doctor.DTOs.Response;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.User.DTOs;
using XuHos.BLL.User.DTOs.Request;
using XuHos.BLL.User.DTOs.Response;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;
using XuHos.Common.Config.Sections;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XuHos.BLL.Common;
using XuHos.BLL.Common.DTOs;

namespace XuHos.BLL.User.Implements
{
    public class UserService : UserBaseService<Entity.User>
    {
        public UserService() : base("")
        {

        }

        public void AppendOperateLog<T>(RequestUserOperateLogDTO<T> log)
        {
            XuHos.Common.LogHelper.WriteTrackLog("UserOperationLog", log);
        }

        #region 私有
        int _GetUserInfoExpireMinutes()
        {
            return 60 * 3;
        }

        private void AddUserLoginLog(UserLoginServerTicketDTO ticket,EnumLoginType loginType,string loginAccount)
        {
            var loginlog = new UserLoginLog();
            loginlog.UserID = ticket.UserID;
            loginlog.LoginType = loginType;
            loginlog.LoginTime = DateTime.Now;
            loginlog.LoginAccount = loginAccount;

            new UserLoginLogService(ticket.UserID).Insert(loginlog);
        }

        #endregion

        /// <summary>
        /// 更新个人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(UserDTO model)
        {
            using (var db = new DBEntities())
            {
                var user = db.Users.Where(t => t.UserID == model.UserID).FirstOrDefault();

                if (user != null)
                {
                    if (model.UserCNName != null)
                    {
                        user.UserCNName = model.UserCNName;
                        var member = (from m in db.UserMembers
                                      where m.UserID == user.UserID && m.Relation == EnumUserRelation.MySelf 
                                      && m.IsDeleted == false
                                      select m).FirstOrDefault();
                        if(member != null)
                        {
                            member.MemberName = model.UserCNName;
                        }
                    }

                    if (model.UserENName != null)
                    {
                        user.UserENName = model.UserENName;
                    }

                    if (model.Email != null)
                    {
                        user.Email = model.Email;
                    }

                    if (model._PhotoUrl != null)
                    {
                        user.PhotoUrl = model._PhotoUrl;
                    }

                    var CacheKey_User =
                        new XuHos.Common.Cache.Keys.EntityCacheKey<XuHos.Entity.User>(StringCacheKeyType.User,
                            user.UserID);
                    CacheKey_User.RemoveCache();

                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }

        /// <summary>
        /// 绑定手机号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApiResult BindMobile(RequestBingdMobileDTO model)
        {
            //并发检查  ，retrycount=0
            if (!$"BindMobile{model.Mobile}".Lock($"BindMobile{model.Mobile}", TimeSpan.FromSeconds(10), 200, 3))
            {
                throw new Exception("并发冲突");
            }

            var userid = GetUserIDByMobile(model.Mobile, EnumUserType.User);

            if (string.IsNullOrEmpty(userid))
            {
                var user = new Entity.User()
                {
                    Mobile = model.Mobile,
                    UserType = EnumUserType.User
                };
                RegisterUser(user, out string reason, model.OpenID, model.AppID);
                userid = user.UserID;
            }
            else
            {
                using (var db = new DBEntities())
                {
                    var map = db.UserWechatMaps.Where(t => t.OpenID == model.OpenID && t.AppID == model.AppID).FirstOrDefault();

                    if (map == null || map.IsDeleted)
                    {
                        if(map == null)
                        {
                            map = new UserWechatMap()
                            {
                                UserID = userid,
                                OpenID = model.OpenID,
                                AppID = model.AppID,
                            };
                            db.UserWechatMaps.Add(map);
                        }
                        else
                        {
                            map.IsDeleted = false;
                        }
                        db.SaveChanges();

                        UserLoginServerTicketDTO serverTicket = new UserLoginServerTicketDTO()
                        {
                            OpenID = model.OpenID,
                            UserID = userid,
                        };
                        ApiSecurityService.SetUserTicket(serverTicket, model.UserToken);
                    }
                }
            }

            return userid.ToApiResultForObject();
        }



        #region 用户登录

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        public ApiResult Login(RequestUserLoginDTO dto)
        {
            //检查账号和密码时长正确
            var user = CheckLogin(dto);
            //账号和密码正确
            if (user != null && !string.IsNullOrEmpty(user.UserID))
            {
                // 如果指定用户角色，则验证用户角色是否正确
                if (dto.UserRole.HasValue)
                {
                    UserService roleService = new UserService();
                    List<EnumRoleType> roles = roleService.GetUserRoles(user.UserID);
                    if (!roles.Exists(x => x == dto.UserRole.Value))
                        return EnumApiStatus.BizUserLoginAccountOrPwdFail.ToApiResultForApiStatus();
                }

                //获取服务端票据
                UserLoginServerTicketDTO serverTicket = new UserLoginServerTicketDTO()
                {
                    OpenID = dto.OpenID,
                    UserID = user.UserID,
                };

                if (serverTicket != null)
                {

                    var clientTicket = new ResponseUserTicketReturnDTO()
                    {
                        Identifier = user.Identifier,
                        Mobile = user.Mobile,
                        UserID = serverTicket.UserID,
                        UserToken = string.IsNullOrEmpty(dto.UserToken) ? Guid.NewGuid().ToString("N") : dto.UserToken,
                        UserCNName = user.UserCNName,
                        PhotoUrl = user.PhotoUrl,
                    };

                    ApiSecurityService.SetUserTicket(serverTicket, clientTicket.UserToken);

                    using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
                    {
                        channel.BeginTransaction();

                        channel.Publish<XuHos.EventBus.Events.UserLoginedEvent>(new EventBus.Events.UserLoginedEvent()
                        {
                            UserID = clientTicket.UserID,
                            LoginTime = DateTime.Now,
                            UserType = clientTicket.UserType,
                        });

                        channel.Publish<XuHos.EventBus.Events.UserOperatorLogEvent>(new EventBus.Events.UserOperatorLogEvent()
                        {
                            UserID = clientTicket.UserID,
                            OperatorTime = DateTime.Now,
                            UserType = clientTicket.UserType,
                            OperatorType = EnumUserOperationType.Login,
                            OperatorName = "",
                            OrgID = CurrentOperatorOrgID,
                            Remark = "",
                            ModuleName = ""
                        });

                        channel.Commit();
                    }


                    AddUserLoginLog(serverTicket, dto.UserLoginType, dto.Mobile);

                    return clientTicket.ToApiResultForObject();
                }
                else
                {
                    return EnumApiStatus.BizError.ToApiResultForApiStatus("LoginTicket Is Null");
                }
            }
            else
            {

                return EnumApiStatus.BizUserLoginAccountOrPwdFail.ToApiResultForApiStatus();
                #endregion
            }
        }



        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        private ResponseUserDTO CheckLogin(RequestUserLoginDTO dto)
        {
            ResponseUserDTO userModel = null;
            if (!string.IsNullOrEmpty(dto.OpenID))
            {
                userModel = GetUserInfoByOpenID(dto.OpenID);
                return userModel;
            }
            else
            {
                //获取手机号码
                userModel = GetUserInfoByMobile(dto.Mobile, dto.UserType);

                //用户存在，状态正常，未被删除
                if (userModel != null)
                {
                    string shapassword = StringEncrypt.EncryptWithSHA(dto.Password.Trim());
                    string md5password = StringEncrypt.EncryptWithMD5(dto.Password.Trim());

                    //登录密码正确
                    if (userModel.Password == shapassword || userModel.Password == md5password)
                    {
                        return userModel;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 登录失败此处递增
        /// </summary>
        /// <param name="account"></param>
        void _IncrementLoginFaildCount(string account)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.User_AccountLoginFail, account);
            CacheKey.Increment();
            CacheKey.ExpireEntryAt(DateTime.Now.AddHours(1) - DateTime.Now);
        }

        int? _GetLoginFaildCount(string account)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.User_AccountLoginFail, account);
            return CacheKey.FromCache<int?>();
        }

        void _ResetLoginFaildCount(string account)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.User_AccountLoginFail, account);
            CacheKey.RemoveCache();
        }

        /// <summary>
        /// 检查验证码是否正确
        /// </summary>
        /// <param name="VerifyCode"></param>
        /// <param name="appToken"></param>
        /// <returns></returns>
        public bool CheckVerifyCode(string VerifyCode, string VerifyCodeID)
        {
            var CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.VerifyCode, VerifyCodeID);
            var value = CacheKey.FromCache<string>();
            CacheKey.RemoveCache();
            if (value != VerifyCode.Trim())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region 用户注册
        /// <summary>
        /// 用户注册(新)
        /// </summary>
        /// <param name="model"></param>
        public bool RegisterUser(XuHos.Entity.User model,out string Reason,
            string openid = null,string appid = null)
        {
            Reason = "注册失败";
            var password = model.Password;
            var result = true;

            try
            {
                #region 检查是否支持此类型的用户注册
                if (model.UserType != EnumUserType.Doctor && model.UserType != EnumUserType.User 
                    && model.UserType != EnumUserType.SysAdmin )
                {
                    Reason = "不支持的用户类型";
                    return false;
                }
                #endregion


                using (DBEntities db = new DBEntities())
                {
                    #region 用户信息
                    if(string.IsNullOrEmpty(model.UserID))
                    {
                        model.UserID = Guid.NewGuid().ToString("N");
                    }

                    //密码
                    model.Password = StringEncrypt.EncryptWithMD5(model.Password);
                    //用户状态
                    model.UserState =EnumUserState.Disabled;

                    db.Users.Add(model);

                    UserExtend userExtend = new UserExtend();
                    userExtend.UserID = model.UserID;
                    userExtend.LastTime = DateTime.Now;

                    db.UserExtends.Add(userExtend);

                    if (!string.IsNullOrEmpty(openid))
                    {
                        UserWechatMap userWechatMap = new UserWechatMap();
                        userWechatMap.UserID = model.UserID;
                        userWechatMap.OpenID = openid;
                        userWechatMap.AppID = appid;
                        db.UserWechatMaps.Add(userWechatMap);
                    }
                    
                    #endregion

                    #region 家庭成员
                    UserMember userMember = null;
                    if (model.UserType == EnumUserType.User)
                    {
                        userMember = new UserMember();
                        userMember.MemberID = Guid.NewGuid().ToString("N");

                        if (!string.IsNullOrEmpty(model.Mobile))
                        {
                            userMember.Mobile = model.Mobile;
                        }
                        else
                        {
                            userMember.Mobile = "";
                        }

                        userMember.UserID = model.UserID;
                        userMember.IDType = EnumUserCardType.IDCard;
                        userMember.Address = "";
                        userMember.Birthday = "";
                        userMember.Email = "";
                        userMember.Gender = EnumUserGender.Other;
                        userMember.IsDefault = true;
                        userMember.Marriage = EnumUserMaritalStatus.Other;
                        userMember.MemberName = model.UserCNName;
                        userMember.PostCode = "";
                        userMember.Relation = EnumUserRelation.MySelf;
                        db.UserMembers.Add(userMember);
                    }
                    #endregion

                    #region 用户标识
                    db.ConversationIMUids.Add(new ConversationIMUid()
                    {
                        Identifier = Convert.ToInt32(WaterNoService.GetWaterNo(EnumWaterNoType.Identify)),
                        Enable = false,
                        IsDeleted = false,
                        CreateTime = DateTime.Now,
                        UserID = model.UserID
                    });
                    #endregion

                    if (db.SaveChanges() >= 0)
                    {
                        #region 将注册消息写入队列,队列中发送短信和注册云通信
                        using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                        {
                            mqChannel.Publish<EventBus.Events.UserRegisteredEvent>(new EventBus.Events.UserRegisteredEvent()
                            {
                                UserAccount = model.UserAccount,
                                UserID = model.UserID,
                                UserPassword = password,
                                UserType = model.UserType,
                                OrgCode = model.OrgCode,
                                Mobile = model.Mobile,
                            });
                        }
                        #endregion

                        result = true;
                        Reason = "注册成功";
                    }
                    else
                    {
                        Reason = "注册失败";
                    }
                }
            }
            catch (Exception ex)
            {
                Reason = "系统错误";
                result = false;
                throw ex;
            }
            return result;
        }

        #endregion

        #region 修改密码

        /// <summary>
        /// 重设用户密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ResetPassword(RequestResetPasswordDTO model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new Exception("确认密码错误！");
            }

            return ResetPassword(model.UserID, model.Password);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public bool ResetPassword(string userID, string userPwd)
        {
            if (base.Update(userID, i => new Entity.User { Password = StringEncrypt.EncryptWithMD5(userPwd) }))
            {
                //采用缓存淘汰策略
                var cacheKey_User = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserDTO>(StringCacheKeyType.User, userID);
                cacheKey_User.RemoveCache();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改密码信息
        /// </summary>
        /// <param name="userModel"></param>
        public ApiResult ChangePassword(RequestUserChangePasswordDTO passwordModel)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    if (passwordModel.NewPassword != passwordModel.ConfirmPassword.Trim())
                    {
                        return EnumApiStatus.BizChangePasswordConfirmPasswordError.ToApiResultForApiStatus();
                    }

                    if (passwordModel.NewPassword == passwordModel.OldPassword)
                    {
                        return EnumApiStatus.BizChangePasswordNewPasswordEqualOld.ToApiResultForApiStatus();
                    }

                    string oldMD5 = StringEncrypt.EncryptWithMD5(passwordModel.OldPassword);
                    string oldSHA = StringEncrypt.EncryptWithSHA(passwordModel.OldPassword);

                    var user = db.Users.Where(t => t.UserID == passwordModel.UserID && (t.Password == oldMD5 || t.Password == oldSHA)).FirstOrDefault();

                    if (user == null)
                    {
                        return EnumApiStatus.BizChangePasswordOldPasswordError.ToApiResultForApiStatus();
                    }
                    user.Password = StringEncrypt.EncryptWithMD5(passwordModel.NewPassword);
                    if (db.SaveChanges() > 0)
                    {
                        var GetUserIDByMobile_CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserDTO>(StringCacheKeyType.User, passwordModel.UserID);
                        GetUserIDByMobile_CacheKey.RemoveCache();

                        return EnumApiStatus.BizOK.ToApiResultForApiStatus("密码修改成功");
                    }
                    else
                    {
                        return EnumApiStatus.BizChangePasswordOldPasswordError.ToApiResultForApiStatus();
                    }
                }

            }
            catch (Exception ex)
            {

                LogHelper.WriteError(ex);
                return EnumApiStatus.BizError.ToApiResultForApiStatus("修改密码失败");
            }

        }
        #endregion

        #region 获取用户信息

        /// <summary>
        /// 获取用户的角色
        
        /// 日期：2017年8月18日
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<EnumRoleType> GetUserRoles(string userID)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<EnumRoleType>(StringCacheKeyType.User_RoleList, userID);
            var list = cacheKey.FromCache();
            if (list == null)
            {

                using (var db = new DBEntities())
                {
                    list = (from rolemap in db.UserRoleMaps
                            join role in db.UserRoles on rolemap.RoleID equals role.RoleID
                            where !role.IsDeleted && !rolemap.IsDeleted && rolemap.UserID == userID
                            select role.RoleType).ToList();

                    list.ToCache(cacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                }
            }

            return list;

        }


        /// <summary>
        /// 根据用户编号获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        public ResponseUserDTO GetUserInfoByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return default(ResponseUserDTO);

            var User_CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserDTO>(StringCacheKeyType.User, userId);
            var userModel = User_CacheKey.FromCache();

            if (userModel == null)
            {
                using (DBEntities db = new DBEntities())
                {
                    var query = from user in db.Users.Where(a => a.UserID == userId && a.IsDeleted == false)
                                join userUid in db.ConversationIMUids on user.UserID equals userUid.UserID
                                select new ResponseUserDTO()
                                {
                                    CancelTime = user.CancelTime,
                                    Email = user.Email,
                                    Identifier = userUid.Identifier,
                                    UserID = user.UserID,
                                    Mobile = user.Mobile,
                                    Password = user.Password,
                                    PhotoUrl = user.PhotoUrl,
                                    RegTime = user.RegTime,
                                    UserType = user.UserType,
                                    UserState = user.UserState,
                                    UserLevel = user.UserLevel,
                                    UserAccount = user.UserAccount,
                                    OrgID = user.OrgCode,
                                    UserCNName = user.UserCNName,
                                    UserENName = user.UserENName,
                                    CreateUserID = user.CreateUserID
                                };

                    userModel = query.FirstOrDefault();

                    if (userModel != null)
                    {
                        XuHos.BLL.User.Implements.UserMemberService service = new UserMemberService();
                        var MemberIsEmpty = service.GetMemberList(userModel.UserID).Find(a => a.Relation == EnumUserRelation.MySelf);

                        //取 usercnname, 先取自己关系membername, 为空则取users.usercname,还为空则取手机号
                        var UserCNName = userModel.UserCNName;
                        if (MemberIsEmpty != null && !string.IsNullOrWhiteSpace(MemberIsEmpty.MemberName))
                        {
                            UserCNName = MemberIsEmpty.MemberName;
                        }

                        if (string.IsNullOrEmpty(UserCNName))
                        {
                            UserCNName = userModel.Mobile;
                        }
                        userModel.UserCNName = UserCNName;
                        userModel.UserENName = MemberIsEmpty != null ? MemberIsEmpty.MemberName : userModel.UserENName;
                        userModel.MemberID = MemberIsEmpty != null ? MemberIsEmpty.MemberID : "";
                        userModel.IDType = MemberIsEmpty != null ? MemberIsEmpty.IDType : EnumUserCardType.IDCard;
                        userModel.IDNumber = MemberIsEmpty != null ? MemberIsEmpty.IDNumber : "";
                        userModel.Gender = MemberIsEmpty != null ? MemberIsEmpty.Gender : EnumUserGender.Other;
                    }

                    userModel.ToCache(User_CacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                }
            }

            return userModel;
        }


        /// <summary>
        /// 根据手机号码查询用户编号
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetUserIDByMobile(string Mobile, EnumUserType userType)
        {
            var GetUserIDByMobile_CacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.MAP_GetUserIDByMobile, $"{Mobile}:{userType}");

            var UserID = GetUserIDByMobile_CacheKey.FromCache<string>();

            //缓存没有命中，则查询数据库获取医生编号并设置缓存（缓存不过期）
            if (string.IsNullOrEmpty(UserID))
            {
                using (var db = new DBEntities())
                {
                    //TODO:这里需要优惠如果不是手机号码类型不需要这样查询
                    string currentUserID = "";

                    if (userType == EnumUserType.User)
                    {
                        currentUserID = db.Users.Where(p => p.IsDeleted == false && (p.Mobile == Mobile)
                            && (p.UserType == EnumUserType.User || p.UserType == EnumUserType.Drugstore)).Select(t => t.UserID).FirstOrDefault();

                        //避免使用Or条件而采用次查询
                        if (string.IsNullOrEmpty(currentUserID))
                        {
                            currentUserID = db.Users.Where(p => p.IsDeleted == false && (p.UserAccount == Mobile)
                             && (p.UserType == EnumUserType.User || p.UserType == EnumUserType.Drugstore)).Select(t => t.UserID).FirstOrDefault();
                        }
                    }
                    else
                    {
                        currentUserID = db.Users.Where(p => p.IsDeleted == false && (p.Mobile == Mobile) && p.UserType == userType).Select(t => t.UserID).FirstOrDefault();

                        //避免使用Or条件而采用次查询
                        if (string.IsNullOrEmpty(currentUserID))
                        {
                            currentUserID = db.Users.Where(p => p.IsDeleted == false && (p.UserAccount == Mobile) && p.UserType == userType).Select(t => t.UserID).FirstOrDefault();
                        }
                    }

                    if (!string.IsNullOrEmpty(currentUserID))
                    {
                        UserID = currentUserID;
                    }
                    else
                    {
                        //避免缓存穿透问题，有新用户注册时需要更新此缓存
                        UserID = "";
                    }

                    UserID.ToCache(GetUserIDByMobile_CacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                }
            }

            return UserID;
        }

        /// <summary>
        /// 根据手机号码查询用户编号
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetUserIDByOpenID(string openid)
        {
            var GetUserIDByOpenID_CacheKey = new StringCacheKey(StringCacheKeyType.MAP_GetUserIDByOpenID, $"{openid}");

            var UserID = GetUserIDByOpenID_CacheKey.FromCache<string>();

            //缓存没有命中，则查询数据库获取医生编号并设置缓存（缓存不过期）
            if (string.IsNullOrEmpty(UserID))
            {
                using (var db = new DBEntities())
                {
                    UserID = db.UserWechatMaps.Where(p => p.IsDeleted == false && p.OpenID == openid).
                        Select(t=>t.UserID).FirstOrDefault();

                    if (!string.IsNullOrEmpty(UserID))
                    {
                        UserID.ToCache(GetUserIDByOpenID_CacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                    }
                }
            }

            return UserID;
        }

        /// <summary>
        ///  根据用户手机号获取用户信息
        /// </summary>
        /// <param name="mobile"></param>
        public ResponseUserDTO GetUserInfoByMobile(string Mobile, EnumUserType userType = EnumUserType.Default)
        {
            var UserID = GetUserIDByMobile(Mobile, userType);
            return GetUserInfoByUserId(UserID);
        }

        /// <summary>
        ///  根据用户手机号获取用户信息
        /// </summary>
        /// <param name="mobile"></param>
        public ResponseUserDTO GetUserInfoByOpenID(string openID)
        {
            var UserID = GetUserIDByOpenID(openID);
            return GetUserInfoByUserId(UserID);
        }

        #endregion

        #region 检测数据

        /// <summary>
        /// 是否有角色权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roletype"></param>
        /// <returns></returns>
        public bool HasRolePermission(string userId, EnumRoleType roletype)
        {
            var roles = GetUserRoles(userId);
            return roles.Any(a => a == roletype);
        }

        public string GetUserID(EnumUserType UserType, string OrgID, string Mobile, string OuterID)
        {
            if (string.IsNullOrWhiteSpace(Mobile) && string.IsNullOrWhiteSpace(OuterID))
            {
                throw new Exception("Mobile,OuterID至少需指定一个");
            }
            if (!string.IsNullOrWhiteSpace(OuterID) && string.IsNullOrWhiteSpace(OrgID))
            {
                throw new Exception("OrgID不能为空");
            }
            string UserID = "";

            StringCacheKey GetUserIDByCreateUserID_CacheKey = null;
            GetUserIDByCreateUserID_CacheKey = new StringCacheKey(
            StringCacheKeyType.MAP_GetUserIDByCreateUserIDAndOrgIDAndMobile, $"{OrgID}&{OuterID}&{Mobile}");
            if (string.IsNullOrEmpty(UserID))
            {
                using (DBEntities db = new DBEntities())
                {
                    var q = from m in db.Users
                            where m.UserType == UserType && m.IsDeleted == false
                            select new { m.UserID, m.Mobile, m.OrgCode, m.CreateUserID };
                    if (!string.IsNullOrWhiteSpace(Mobile))
                    {
                        q = q.Where(w => w.Mobile == Mobile);
                    }
                    if (!string.IsNullOrWhiteSpace(OuterID))
                    {
                        q = q.Where(w => w.CreateUserID == OuterID);
                    }
                    if (!string.IsNullOrWhiteSpace(OrgID))
                    {
                        q = q.Where(w => w.OrgCode == OrgID);
                    }
                    var u = q.FirstOrDefault();
                    UserID = u?.UserID;
                    UserID.ToCache(GetUserIDByCreateUserID_CacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                }
            }
            return UserID;
        }
        public string GetKMSDUserID(EnumUserType UserType, string OrgID, string Mobile, string OuterID)
        {
            if (string.IsNullOrWhiteSpace(Mobile) && string.IsNullOrWhiteSpace(OuterID))
            {
                throw new Exception("Mobile,OuterID至少需指定一个");
            }
            if (!string.IsNullOrWhiteSpace(OuterID) && string.IsNullOrWhiteSpace(OrgID))
            {
                throw new Exception("OrgID不能为空");
            }
            string UserID = "";

            StringCacheKey GetUserIDByCreateUserID_CacheKey = null;
            if (!string.IsNullOrEmpty(OuterID))
            {
                GetUserIDByCreateUserID_CacheKey = new StringCacheKey(
                StringCacheKeyType.MAP_GetUserIDByCreateUserIDAndOrgID, $"{OrgID}&{OuterID}");
            }
            else
            {
                GetUserIDByCreateUserID_CacheKey = new StringCacheKey(
                StringCacheKeyType.MAP_GetUserIDByCreateUserIDAndOrgID, $"{Mobile}");
            }

            UserID = GetUserIDByCreateUserID_CacheKey.FromCache<string>();
            if (string.IsNullOrEmpty(UserID))
            {
                using (DBEntities db = new DBEntities())
                {
                    var q = from m in db.Users
                            where m.UserType == UserType && m.IsDeleted == false
                            select new { m.UserID, m.Mobile, m.OrgCode, m.CreateUserID };

                    if (!string.IsNullOrWhiteSpace(OuterID))// OuterID + OrgCode确定一个用户
                    {
                        q = q.Where(w => w.CreateUserID == OuterID);
                        if (!string.IsNullOrWhiteSpace(OrgID))
                        {
                            q = q.Where(w => w.OrgCode == OrgID);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(Mobile))//手机号确定一个帐号
                    {
                        q = q.Where(w => w.Mobile == Mobile && string.IsNullOrEmpty(w.CreateUserID));
                    }

                    var u = q.FirstOrDefault();
                    UserID = u?.UserID;
                    UserID.ToCache(GetUserIDByCreateUserID_CacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                }
            }
            return UserID;
        }

        /// <summary>
        /// 检查是否已存在  时代，蒙发利，掌上医院 用外部唯一ID；健康档案 一体机 用身份证号
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="outID"></param>
        /// <param name="orgID"></param>
        /// <param name="checkType">1-外部唯一ID;2-身份证号</param>
        /// <returns></returns>
        public string ExistUserByCreateID(string outID, string orgID, int checkType)
        {
            StringCacheKey GetUserIDByCreateUserID_CacheKey = null;
            string UserID = null;
            switch (checkType)
            {
                case 1:
                    GetUserIDByCreateUserID_CacheKey = new StringCacheKey(StringCacheKeyType.MAP_GetUserIDByCreateUserIDAndOrgID, $"{orgID}&{outID}");

                    UserID = GetUserIDByCreateUserID_CacheKey.FromCache<string>();

                    if (string.IsNullOrEmpty(UserID))
                    {
                        using (DBEntities db = new DBEntities())
                        {

                            //康美时代会员，蒙发利会员,掌上医院
                            //判断 outID  和orgID
                            UserID = (from aa in db.Users
                                      where aa.CreateUserID == outID && aa.OrgCode == orgID && !aa.IsDeleted
                                      select aa.UserID).FirstOrDefault();

                            if (!string.IsNullOrEmpty(UserID))
                            {
                                UserID.ToCache(GetUserIDByCreateUserID_CacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                            }
                        }
                    }
                    break;

                case 2:
                    GetUserIDByCreateUserID_CacheKey = new StringCacheKey(StringCacheKeyType.MAP_GetUserIDByCreateUserID, outID);
                    UserID = GetUserIDByCreateUserID_CacheKey.FromCache<string>();
                    if (string.IsNullOrEmpty(UserID))
                    {
                        using (DBEntities db = new DBEntities())
                        {
                            //双佳一体机,健康档案
                            //双佳一体机 调用，判断 身份证号
                            UserID = (from aa in db.Users
                                      where aa.CreateUserID == outID && !aa.IsDeleted
                                      select aa.UserID).FirstOrDefault();

                            UserID.ToCache(GetUserIDByCreateUserID_CacheKey, TimeSpan.FromMinutes(_GetUserInfoExpireMinutes()));
                        }
                    }
                    break;
            }
            return UserID;
        }

        /// <summary>
        /// 帐号唯一性检查,包含用户类型维度
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool ExistsMobile(string Mobile, EnumUserType userType)
        {
            bool result = false;

            using (DBEntities db = new DBEntities())
            {
                result = db.Users.Where(o => (o.Mobile == Mobile) && o.UserType == userType && o.IsDeleted == false).Count() > 0;
            }
            return result;
        }

        /// <summary>
        /// 帐号唯一性检查
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool ExistsUserAccount(string userAccount)
        {
            bool result = false;

            using (DBEntities db = new DBEntities())
            {
                var model = db.Users.FirstOrDefault(o => o.UserAccount == userAccount && o.IsDeleted == false);
                if (model != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 帐号唯一性检查
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool ExistsMobile(string Mobile)
        {
            bool result = false;

            using (DBEntities db = new DBEntities())
            {
                var model = db.Users.FirstOrDefault(o => o.Mobile == Mobile);

                if (model != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 帐号唯一性检查
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public bool ExistMobileExcludeMySelf(string Mobile, string userId, EnumUserType userType = EnumUserType.Doctor)
        {
            bool result = false;

            using (DBEntities db = new DBEntities())
            {
                result = db.Users.Where(o => o.Mobile == Mobile && o.UserID != userId && o.UserType == userType && o.IsDeleted == false).Count() > 0;
            }
            return result;
        }
        #endregion

        #region 更新用户信息

        /// <summary>
        /// 加入角色
        
        /// 日期：2017年8月19日
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roletype"></param>
        /// <returns></returns>
        public bool JoinRole(string userId, EnumRoleType roletype)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            using (var db = new DBEntities())
            {

                var ret = false;
                var cacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<EnumRoleType>(StringCacheKeyType.User_RoleList, userId);
                var rolem = (from roleMap in db.UserRoleMaps
                             join role in db.UserRoles on roleMap.RoleID equals role.RoleID
                             where roleMap.UserID == userId && role.RoleType == roletype
                             select roleMap).FirstOrDefault();

                //已经存在
                if (rolem != null)
                {
                    #region 修改
                    if (rolem.IsDeleted)
                    {
                        rolem.IsDeleted = false;
                        ret = db.SaveChanges() > 0;
                    }
                    else
                    {
                        ret = true;
                    }
                    #endregion
                }
                else
                {
                    #region 添加记录
                    var roleid = (from role in db.UserRoles
                                  where role.RoleType == roletype
                                  select role.RoleID).FirstOrDefault();

                    if (!string.IsNullOrEmpty(roleid))
                    {
                        rolem = new Entity.UserRoleMap();
                        rolem.UserRoleMapID = Guid.NewGuid().ToString("N");
                        rolem.RoleID = roleid;
                        rolem.UserID = userId;
                        db.UserRoleMaps.Add(rolem);
                        ret = db.SaveChanges() > 0;
                    }
                    #endregion
                }

                #region 删除缓存
                if (ret)
                {
                    cacheKey.RemoveCache();
                }
                #endregion

                return ret;
            }
        }

        /// <summary>
        /// 更新用户级别(0-普通用户、10 以上 Vip等级)
        /// </summary>
        public void UpdateUserLevel()
        {
            using (var db = new DBEntities())
            {
                var users = (from ua in db.Users
                             where !ua.IsDeleted && ua.UserLevel == -1
                             select ua.UserID).ToList();

                if (users != null && users.Count > 0)
                {
                    db.Database.ExecuteSqlCommand(@"update Users set UserLevel=isnull(aa.UserLevel,0)
                    from users left join (
                    select isnull(users.UserID,UserRoleMaps.UserID) UserID,max(UserLevelRules.UserLevel) UserLevel from UserLevelRules left join users on UserLevelRules.OrgCode=users.OrgCode
                    left join UserRoleMaps on UserLevelRules.RoleID=UserRoleMaps.RoleID
                    where UserLevelRules.IsDeleted=0 and ((UserLevelRules.RuleDimension=0 and users.UserID is not null)
                    or (UserLevelRules.RuleDimension=1 and UserRoleMaps.UserID is not null))
                    group by users.UserID,UserRoleMaps.UserID)aa on  users.UserID = aa.UserID
                    where Users.UserLevel=-1");

                    foreach (var item in users)
                    {
                        var User_CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserDTO>(StringCacheKeyType.User, item);
                        User_CacheKey.RemoveCache();
                    }
                }
            }
        }

        #endregion

    }
}
