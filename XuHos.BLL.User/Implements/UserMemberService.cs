using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO.Common;

using XuHos.Entity;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using XuHos.Common.Cache;
using EntityFramework.Extensions;
using XuHos.BLL.User.DTOs.Request;
using XuHos.BLL.User.DTOs.Response;
using XuHos.BLL.Sys.Implements;
using XuHos.Common.Cache.Keys;

namespace XuHos.BLL.User.Implements
{
    /// <summary>
    /// 用户家庭成员相关业务
    /// </summary>
    public class UserMemberService
    {
        /// <summary>
        /// 设置默认成员
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool SetDefaultMember(string memberID, string userID)
        {
            using (var db = new DBEntities())
            {
                using (db.BeginTransaction())
                {
                    db.UserMembers.Where(a => a.UserID == userID && a.MemberID != memberID).Update(a => new UserMember()
                    {
                        IsDefault = false,
                        ModifyTime = DateTime.Now,
                        ModifyUserID = userID
                    });

                    db.UserMembers.Where(a => a.UserID == userID && a.MemberID == memberID).Update(a => new UserMember()
                    {
                        IsDefault = true,
                        ModifyTime = DateTime.Now,
                        ModifyUserID = userID
                    });

                    db.Commit();
                    var userMemberMapCacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<UserMember>(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Member, memberID);
                    userMemberMapCacheKey.RemoveCache();
                }
                return true;
            }
        }

        /// <summary>
        /// 更新会员信息
        
        /// 日期：2017年4月15日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EnumApiStatus UpdateMemberInfo(BLL.User.DTOs.Request.RequestUserMemberDTO request)
        {
            using (DBEntities db = new DBEntities())
            {
                using (db.BeginTransaction())
                {
                    var model = db.UserMembers.Where(q => q.UserID == request.MemberID && q.MemberID == request.MemberID && q.IsDeleted == false).FirstOrDefault();
                    if (model == null) return EnumApiStatus.BizError;

                    // 当前用户不能有相同身份证的就诊人 add by lrj 2017-7-20
                    if (!string.IsNullOrEmpty(request.IDNumber))
                    {
                        var mid = db.UserMembers.Where(q => q.IDNumber == request.IDNumber && q.MemberID != model.MemberID
                            && q.IsDeleted == false).Select(q => q.MemberID).FirstOrDefault();
                    }

                    #region 必填参数
                    model.MemberName = request.MemberName;
                    // model.Relation = request.Relation;
                    model.Gender = request.Gender;
                    model.Marriage = request.Marriage;
                    model.IDType = request.IDType;
                    model.MemberName = request.MemberName;
                    #endregion

                    #region 可选参数(有值才修改)
                    if (request.Birthday != null)
                    {
                        model.Birthday = request.Birthday;
                    }

                    if (request.Mobile != null)
                    {
                        model.Mobile = request.Mobile;
                    }

                    if (request.IDNumber != null)
                    {
                        model.IDNumber = request.IDNumber;
                    }

                    if (request.Nationality != null)
                    {
                        model.Nationality = request.Nationality;
                    }
                    if (request.Province != null)
                    {
                        model.Province = request.Province;
                    }

                    if (request.ProvinceRegionID != null)
                    {
                        model.ProvinceRegionID = request.ProvinceRegionID;
                    }

                    if (request.City != null)
                    {
                        model.City = request.City;
                    }

                    if (request.CityRegionID != null)
                    {
                        model.CityRegionID = request.CityRegionID;
                    }

                    if (request.District != null)
                    {
                        model.District = request.District;
                    }

                    if (request.DistrictRegionID != null)
                    {
                        model.DistrictRegionID = request.DistrictRegionID;
                    }

                    if (request.Town != null)
                    {
                        model.Town = request.Town;
                    }

                    if (request.TownRegionID != null)
                    {
                        model.TownRegionID = request.TownRegionID;
                    }

                    if (request.Village != null)
                    {
                        model.Village = request.Village;
                    }

                    if (request.VillageRegionID != null)
                    {
                        model.VillageRegionID = request.VillageRegionID;
                    }

                    if (request.Address != null)
                    {
                        model.Address = request.Address;
                    }

                    if (request.Email != null)
                    {
                        model.Email = request.Email;
                    }

                    if (request.PostCode != null)
                    {
                        model.PostCode = request.PostCode;
                    }

                    #endregion

                    #region 是否已经存在本人关系的就诊人
                    if (request.Relation == EnumUserRelation.MySelf)
                    {
                        var members = GetMemberList(request.UserID);

                        //本人关系已经存在
                        if (members.Any(a => a.Relation == EnumUserRelation.MySelf && a.MemberID != model.MemberID))
                        {
                            return EnumApiStatus.BizUserMemberRejectUpdateMySelfExists;
                        }
                    }
                    #endregion

                    #region 通过身份证号码获取性别和身份证号码
                    if (model.IDType == EnumUserCardType.IDCard && !string.IsNullOrEmpty(model.IDNumber))
                    {
                        string birthday, sex;
                        var res = ToolHelper.GetBirthdaySexFromIdCard(model.IDNumber, out birthday, out sex);

                        //身份证号码正确
                        if (res)
                        {
                            model.Birthday = birthday;
                            model.Gender = sex == "0" ? EnumUserGender.Male : EnumUserGender.Female;
                        }
                        else
                        {
                            //身份证号码格式错误
                            return EnumApiStatus.BizUserMemberRejectInsertUpdateIDNumberFormatError;
                        }
                    }
                    #endregion

                    model.Relation = request.Relation;
                    if (request.IsDefault.HasValue && request.IsDefault == true)
                    {
                        model.IsDefault = true;
                    }
                    else if(request.IsDefault.HasValue && request.IsDefault == false)
                    {
                        model.IsDefault = false;
                    }

                    if (request.Relation == EnumUserRelation.MySelf)
                    {
                        var user = db.Users.Where(a => a.UserID == request.UserID).FirstOrDefault();
                        user.UserCNName = model.MemberName;
                        user.UserENName = model.MemberName;
                    }


                    if (db.SaveChanges() > 0)
                    {
                        db.Commit();
                        var userMemberCacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserMemberDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Member, model.MemberID);
                        userMemberCacheKey.RemoveCache();

                        var User_CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.User, request.UserID);
                        User_CacheKey.RemoveCache();
                    }
                }
                //设置默认
                if (request.IsDefault.HasValue && request.IsDefault.Value)
                {
                    SetDefaultMember(request.MemberID, request.UserID);
                }
                return EnumApiStatus.BizOK;
            }
        }

        /// <summary>
        /// 新增成员
        
        /// 日期：2017年4月15日
        /// </summary>
        /// <param name="userMemberEntity"></param>
        /// <returns></returns>
        public ApiResult InsertMemberInfo(BLL.User.DTOs.Request.RequestUserMemberDTO request)
        {

            ApiResult result = new ApiResult();

            string memberid = string.Empty;

            #region 身份证号码格式校验
            if (!string.IsNullOrEmpty(request.IDNumber) && request.IDType == EnumUserCardType.IDCard)
            {
                string birthday, sex;
                var res = ToolHelper.GetBirthdaySexFromIdCard(request.IDNumber, out birthday, out sex);

                //身份证号码正确
                if (res)
                {
                    request.Birthday = birthday;
                    request.Gender = sex == "0" ? EnumUserGender.Male : EnumUserGender.Female;
                }
                else
                {
                    result.Status = EnumApiStatus.BizUserMemberRejectInsertUpdateIDNumberFormatError;
                    result.Msg = result.Status.GetEnumDescript();
                    return result;
                }
            }
            #endregion
            
            using (DBEntities db = new DBEntities())
            {
                using (db.BeginTransaction())
                {
                    db.Configuration.AutoDetectChangesEnabled = true;
                    bool isAdd = false;
                    UserMember userMemberEntity = null;
                    if (!string.IsNullOrEmpty(request.IDNumber))
                    {
                        userMemberEntity = (from q in db.UserMembers
                                            where q.IDNumber == request.IDNumber && q.MemberName== request.MemberName && q.IsDeleted == false
                                            orderby q.ModifyTime descending, q.CreateTime descending, q.MemberID
                                            select q).FirstOrDefault();
                    }
                    
                    if (userMemberEntity == null)
                    {
                        isAdd = true;
                        #region 可选参数（默认值）
                        if (request.Birthday == null)
                        {
                            request.Birthday = "";
                        }

                        if (request.Mobile == null)
                        {
                            request.Mobile = "";
                        }

                        if (request.IDNumber == null)
                        {
                            request.IDNumber = "";
                        }

                        if (request.Nationality == null)
                        {
                            request.Nationality = "";
                        }

                        if (request.Province == null)
                        {
                            request.Province = "";
                        }
                        if (request.ProvinceRegionID == null)
                        {
                            request.ProvinceRegionID = "";
                        }

                        if (request.City == null)
                        {
                            request.City = "";
                        }
                        if (request.CityRegionID == null)
                        {
                            request.CityRegionID = "";
                        }

                        if (request.District == null)
                        {
                            request.District = "";
                        }
                        if (request.DistrictRegionID == null)
                        {
                            request.DistrictRegionID = "";
                        }

                        if (request.Town == null)
                        {
                            request.Town = "";
                        }
                        if (request.TownRegionID == null)
                        {
                            request.TownRegionID = "";
                        }

                        if (request.Village == null)
                        {
                            request.Village = "";
                        }
                        if (request.VillageRegionID == null)
                        {
                            request.VillageRegionID = "";
                        }

                        if (request.Address == null)
                        {
                            request.Address = "";
                        }

                        if (request.Email == null)
                        {
                            request.Email = "";
                        }

                        if (request.PostCode == null)
                        {
                            request.PostCode = "";
                        }

                        if (request.IsDefault == null || !request.IsDefault.HasValue)
                        {
                            request.IsDefault = false;
                        }
                        #endregion
                        userMemberEntity = request.Map<RequestUserMemberDTO, Entity.UserMember>();
                        userMemberEntity.MemberID = Guid.NewGuid().ToString("N");
                        userMemberEntity.CreateTime = DateTime.Now;
                        userMemberEntity.CreateUserID = request.UserID;
                    }

                   
                    //默认成员
                    var defaultMemberEntity = GetDefaultMemberInfo(request.UserID).Map<ResponseUserMemberDTO, Entity.UserMember>();

                    memberid = userMemberEntity.MemberID;

                    //默认成员不存在
                    if (defaultMemberEntity == null)
                    {
                        //当前成员为默认
                        userMemberEntity.IsDefault = true;
                    }

                    if (request.Relation == EnumUserRelation.MySelf)
                    {
                        var userEntity = db.Users.Where(a => a.UserID == request.UserID).FirstOrDefault();
                        userEntity.UserCNName = request.MemberName;
                        userEntity.UserENName = request.MemberName;
                        var mySelfMember = (from m in db.UserMembers
                                            where m.UserID == request.UserID && m.Relation == EnumUserRelation.MySelf 
                                            && m.IsDeleted == false && m.IsDeleted == false
                                            select m).FirstOrDefault();

                        //如果默认成员已经存在则更新默认成员                 
                        if (mySelfMember != null)
                        {
                            //兼容bat 服务站，新增自己关系是，要是原来的自己关系信息不完整（身份证号为空），则覆盖
                            //身份证不为空，且操作机构不为网络医院
                            var webapiConfig = SysConfigService.Get<XuHos.Common.Config.Sections.Api>();
                            if (string.IsNullOrEmpty(mySelfMember.IDNumber) && request.OrgID != webapiConfig.OrgID)
                            {
                                mySelfMember.Gender = request.Gender;
                                mySelfMember.Marriage = request.Marriage;
                                mySelfMember.MemberName = request.MemberName;

                                #region 可选参数(有值才修改)
                                if (request.Birthday != null)
                                {
                                    mySelfMember.Birthday = request.Birthday;
                                }

                                if (request.Mobile != null)
                                {
                                    mySelfMember.Mobile = request.Mobile;
                                }

                                if (request.IDNumber != null)
                                {
                                    mySelfMember.IDNumber = request.IDNumber;
                                }

                                if (request.Nationality != null)
                                {
                                    mySelfMember.Nationality = request.Nationality;
                                }
                                if (request.Province != null)
                                {
                                    mySelfMember.Province = request.Province;
                                }

                                if (request.ProvinceRegionID != null)
                                {
                                    mySelfMember.ProvinceRegionID = request.ProvinceRegionID;
                                }

                                if (request.City != null)
                                {
                                    mySelfMember.City = request.City;
                                }

                                if (request.CityRegionID != null)
                                {
                                    mySelfMember.CityRegionID = request.CityRegionID;
                                }

                                if (request.District != null)
                                {
                                    mySelfMember.District = request.District;
                                }

                                if (request.DistrictRegionID != null)
                                {
                                    mySelfMember.DistrictRegionID = request.DistrictRegionID;
                                }

                                if (request.Town != null)
                                {
                                    mySelfMember.Town = request.Town;
                                }

                                if (request.TownRegionID != null)
                                {
                                    mySelfMember.TownRegionID = request.TownRegionID;
                                }

                                if (request.Village != null)
                                {
                                    mySelfMember.Village = request.Village;
                                }

                                if (request.VillageRegionID != null)
                                {
                                    mySelfMember.VillageRegionID = request.VillageRegionID;
                                }

                                if (request.Address != null)
                                {
                                    mySelfMember.Address = request.Address;
                                }

                                if (request.Email != null)
                                {
                                    mySelfMember.Email = request.Email;
                                }

                                if (request.PostCode != null)
                                {
                                    mySelfMember.PostCode = request.PostCode;
                                }

                                #endregion
                                memberid = mySelfMember.MemberID;
                                db.Update(mySelfMember);
                            }
                            else
                            {
                                result.Status = EnumApiStatus.BizUserMemberRejectUpdateMySelfExists;
                                result.Msg = result.Status.GetEnumDescript();
                                return result;
                            }

                        }
                    }

                    if (isAdd)
                    {
                        db.UserMembers.Add(userMemberEntity);
                    }
                    //db.Update(userEntity);
                    var count = db.SaveChanges();
                    db.Commit();
                    if (count > 0)
                    {
                        var userMemberCacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserMemberDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Member, userMemberEntity.MemberID);
                        userMemberCacheKey.RemoveCache();

                        var User_CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.User, request.UserID);
                        User_CacheKey.RemoveCache();
                        result.Status = EnumApiStatus.BizOK;
                    }
                    else
                    {
                        result.Status = EnumApiStatus.BizError;
                    }
                }
                //设置默认
                if(request.IsDefault.HasValue && request.IsDefault.Value)
                {
                    SetDefaultMember(memberid, request.UserID);
                }
                result.Data = memberid;
                result.Msg = result.Status.GetEnumDescript();
                return result;
            }
        }

        /// <summary>
        /// 删除成员信息        
        /// </summary>
        ///<remarks>
        ///1.有预约记录不能删除
        ///2.本人不能删除
        ///</remarks>
        /// <param name="UserID"></param>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public EnumApiStatus DeleteMemberInfo(string UserID, string MemberID, string orgid = "kmwlyy")
        {
            using (DBEntities db = new DBEntities())
            {
                //var member = GetMemberInfo(UserID, MemberID);
                db.Configuration.AutoDetectChangesEnabled = true;
                var map = db.UserMembers.Where(q => q.UserID == UserID && q.MemberID == MemberID && q.IsDeleted == false).FirstOrDefault();
                if(map == null)
                {
                    throw new Exception($"没找到关联关系：（UserID:{UserID},MemberID:{MemberID}）");
                }
                //不允许删除本人
                if (map.Relation == EnumUserRelation.MySelf)
                    return EnumApiStatus.BizUserMemberRejectDeleteMySelf;

                if (orgid != null && orgid.ToLower() != "jkfwz" && orgid.ToLower() != "kmsd")
                {
                    var orderCount = db.UserOpdRegisters.Count(a => a.UserID == UserID && a.MemberID == MemberID && !a.IsDeleted);

                    if (orderCount > 0)
                    {
                        return EnumApiStatus.BizUserMemberRejectDeleteHasRelationOrder;
                    }

                }

                map.IsDeleted = true;
                map.DeleteTime = DateTime.Now;
                map.DeleteUserID = UserID;

                if (db.SaveChanges() > 0)
                {
                    var userMemberMapCacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<UserMember>(XuHos.Common.Cache.Keys.StringCacheKeyType.User_Member, MemberID);
                    userMemberMapCacheKey.RemoveCache();

                    return EnumApiStatus.BizOK;
                }
                else
                {
                    return EnumApiStatus.BizError;
                }
            }
        }
        /// <summary>
        /// 获取成员列表
        
        /// 日期：2017年4月15日
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<ResponseUserMemberDTO> GetMemberList(string UserID)
        {

            List<ResponseUserMemberDTO> members = new List<ResponseUserMemberDTO>();

            //缓存没有命中获取
            using (DBEntities db = new DBEntities())
            {

                var list = (from map in db.UserMembers
                            where map.UserID == UserID && !map.IsDeleted
                            select map).ToList();

                foreach (var model in list)
                {
                    members.Add(model.Map<UserMember, ResponseUserMemberDTO>());
                }
            }

            return members;
        }

        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public ResponseUserMemberDTO GetMemberInfo(string UserID, string MemberID)
        {
            var members = GetMemberList(UserID);

            if (members == null)
            {
                return null;
            }
            else
            {
                return members.Find(q => q.MemberID == MemberID);
            }
        }

        /// <summary>
        /// 医生 依据 MemberID查询 成员信息
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public ResponseUserMemberDTO GetMemberInfo(string MemberID)
        {
            var userMemberCacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<ResponseUserMemberDTO>
                (XuHos.Common.Cache.Keys.StringCacheKeyType.User_Member, MemberID);
            var member = userMemberCacheKey.FromCache();

            if (member == null)
            {
                using (DBEntities db = new DBEntities())
                {
                    member = (from members in db.UserMembers
                              where members.MemberID == MemberID && !members.IsDeleted
                              select members).FirstOrDefault().Map<Entity.UserMember, ResponseUserMemberDTO>();
                    member.ToCache(userMemberCacheKey);
                }
            }
            return member;
        }

        /// <summary>
        /// 获取默认家庭成员
        
        /// 日期：2016年11月25日
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetDefaultMemberID(string UserID)
        {
            var defaultMember = GetDefaultMemberInfo(UserID);

            if (defaultMember != null)
            {
                return defaultMember.MemberID;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取默认就诊人信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ResponseUserMemberDTO GetDefaultMemberInfo(string userID)
        {
            List<ResponseUserMemberDTO> members = GetMemberList(userID);

            //获取默认用户
            var defaultMember = members.Where(a => a.IsDefault).FirstOrDefault();

            if (defaultMember == null)
            {
                //如果没有默认用户则用户自己为默认用户
                defaultMember = members.Where(a => a.Relation == EnumUserRelation.MySelf).FirstOrDefault();
            }
            return defaultMember;
        }

        public List<ResponseUserMemberDTO> GetMemberListByIdNumber(string idNumber)
        {
            List<ResponseUserMemberDTO> members;
            using (var db = new DBEntities())
            {
                members = db.UserMembers
                        .Where(member => member.IDNumber == idNumber && !member.IsDeleted)
                        .ToList()
                        .Map<List<Entity.UserMember>, List<ResponseUserMemberDTO>>();
            }
            return members;
        }


        /// <summary>
        /// 检查看诊患者信息
        /// </summary>
        /// <returns></returns>
        public bool CheckMemberProfileWithSubmitRequest(string MemberID, out string Reason)
        {
            Reason = "";

            if (string.IsNullOrEmpty(MemberID))
            {
                Reason = "没有设置就诊人";
            }
            else
            {

                var usermember = GetMemberInfo(MemberID);
                if (usermember == null)
                {
                    Reason = "就诊人不存在";
                }
                else if (string.IsNullOrWhiteSpace(usermember.MemberName))
                {
                    Reason = "就诊人姓名不能为空，请编辑";
                }
                else if (usermember.Birthday != "" && !ToolHelper.CheckBirthDay(usermember.Birthday))
                {
                    Reason = "就诊人出生日期不合法";
                }
                //else if (usermember.Gender == EnumUserGender.Other)
                //{
                //    Reason = "请指定就诊人性别";
                //}
                else
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// 通过非id主键的形式匹配成员信息
        /// 沈腾飞
        /// </summary>
        /// <returns></returns>
        public List<ResponseUserMemberDTO> GetMemberInfo(string memberName, string mobile, EnumUserGender? gender, DateTime? birthday, string idNumber)
        {
            using (DBEntities db = new DBEntities())
            {
                var query = db.Set<UserMember>().Where(x => !x.IsDeleted);

                // 如果指定身份证号码，则直接使用身份证号匹配
                if (!string.IsNullOrEmpty(idNumber))
                {
                    var result = query.Where(x => x.IDNumber == idNumber).ToList().Map<List<Entity.UserMember>, List<ResponseUserMemberDTO>>();
                    if (result != null)
                        return result;
                }

                // 考虑有可能同一个成员使用了不同的手机号的或成员换了手机号的情况，姓名和手机号需一起匹配才能确认唯一身份
                if (!string.IsNullOrEmpty(memberName) && !string.IsNullOrEmpty(mobile))
                {
                    var result = query.Where(x => x.MemberName == memberName && x.Mobile == mobile).ToList().Map<List<Entity.UserMember>, List<ResponseUserMemberDTO>>();
                    if (result != null)
                        return result;
                }

                // 通过姓名出生年月和性确认唯一身份
                if (!string.IsNullOrEmpty(memberName) && gender.HasValue && birthday.HasValue)
                {
                    return query.Where(x => Convert.ToDateTime(x.Birthday).ToString("yyyy-MM-dd") == birthday.Value.ToString("yyyy-MM-dd") &&
                       x.Gender == gender.Value && x.MemberName == memberName).ToList().Map<List<Entity.UserMember>, List<ResponseUserMemberDTO>>();
                }

                return new List<ResponseUserMemberDTO>();
            }

        }

        /// <summary>
        /// 获取已存在的手机号账号的自已成员
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ApiResult GetGetExistUserMemberByMoble(string mobile)
        {
            ApiResult result = new ApiResult();
            using (DBEntities db = new DBEntities())
            {
                #region 新增是否存在手机号的账号判断，有的话需要返回该账号下 的自己关系（家庭成员）的信息

                if (!string.IsNullOrWhiteSpace(mobile))
                {
                    var isExistUser = (from u in db.Users
                                       join um in db.UserMembers on u.UserID equals um.UserID
                                       where u.Mobile == mobile && um.Relation == EnumUserRelation.MySelf 
                                       && u.UserType == EnumUserType.User &&
                                           u.IsDeleted == false && um.IsDeleted == false && um.IsDeleted == false
                                       select um).FirstOrDefault();

                    if (isExistUser != null)
                    {
                        result.Data = isExistUser.Map<Entity.UserMember, RequestUserMemberDTO>();
                    }



                }
                return result;

                #endregion
            }

        }
    }
}
