using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.BLL.Common;
using XuHos.BLL.User.DTOs;
using XuHos.BLL.User.DTOs.Response;
using XuHos.BLL.User.Implements;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.DTO.Common;
using XuHos.Entity;

namespace XuHos.BLL.User
{
    public class UserLoginLogService : CommonBaseService<UserLoginLog>
    {
        public UserLoginLogService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        {
            this.CurrentOperatorUserID = CurrentOperatorUserID;
        }

        /// <summary>
        /// 获取登录列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Response<List<ResponseUserLoginLogDTO>> GetUserLoginLogList(RequestUserLoginLogDTO request)
        {
            var response = new Response<List<ResponseUserLoginLogDTO>>();
            using (var db = new DBEntities())
            {
                var query = from l in db.UserLoginLogs
                            join u in db.Users on l.UserID equals u.UserID
                            join urm in db.UserRoleMaps on l.UserID equals urm.UserID into leftRolemaps
                            from rolemaps in leftRolemaps.DefaultIfEmpty()
                            join r  in db.UserRoles on rolemaps.RoleID equals r.RoleID into leftUserRoles
                            from userrole in leftUserRoles.DefaultIfEmpty()
                            where l.IsDeleted == false && u.IsDeleted == false
                    orderby l.LoginTime descending
                    select new ResponseUserLoginLogDTO
                    {
                        UserID=l.UserID,
                        LoginAccount = l.LoginAccount,
                        LoginType=l.LoginType,
                        LoginTime=l.LoginTime,
                        RoleType = userrole==null?0: (int)userrole.RoleType,                       
                    };

                if (!string.IsNullOrEmpty(request.TopPath))
                    query = query.Where(i => i.TopPath == request.TopPath);

                if (!string.IsNullOrEmpty(request.HospitalID))
                    query = query.Where(i => i.OrgID == request.HospitalID);

                if (request.BeginTime != default(DateTime))
                    query = query.Where(i => i.LoginTime >= request.BeginTime);
                if (request.EndTime != default(DateTime))
                    query = query.Where(i => i.LoginTime <= request.EndTime);

                int total = 0;
                var list = query.Pager(out total, request.PageIndex, request.PageSize);

                var result = new Response<List<ResponseUserLoginLogDTO>>()
                {
                    Total = total,
                    Data = list
                };

                return result;
            }
        }
    }
}
