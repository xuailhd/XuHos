using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XuHos.Service.Infrastructure;
using XuHos.Common.Cache;
using XuHos.BLL.Sys.Implements;

namespace XuHos.WebApi.Controllers
{
    public class ApiBaseController : ApiController
    {

        #region 私有变量
        BLL.Common.DTOs.Response.UserLoginServerTicketDTO _CurrentOperatorUser = null;
        public BLL.Common.DTOs.Response.UserLoginServerTicketDTO CurrentOperatorUser
        {
            get
            {
                if (_CurrentOperatorUser == null)
                {
                    _CurrentOperatorUser = SecurityHelper.LoginUser;
                    return _CurrentOperatorUser;
                }

                return _CurrentOperatorUser;
            }
        }


        XuHos.BLL.Common.DTOs.Response.ResponseToken _CurrentOperatorApp = null;

        public XuHos.BLL.Common.DTOs.Response.ResponseToken CurrentOperatorApp
        {
            get
            {
                if (_CurrentOperatorApp == null)
                {
                    _CurrentOperatorApp = XuHos.Service.Infrastructure.SecurityHelper.GetCurrentAppToken();
                }

                return _CurrentOperatorApp;
            }
        }
        #endregion

        #region 当前操作用户
        /// <summary>
        /// 当前操作的用户编号
        /// </summary>
        public string CurrentOperatorUserID
        {
     
            get
            {
                return CurrentOperatorUser == null ? "" : CurrentOperatorUser.UserID;
            }
        }

        public XuHos.Common.Enum.EnumUserType CurrentOperatorUserType {

            get
            {
                return CurrentOperatorUser.UserType;
            }
        }

        /// <summary>
        /// 当前操作用户的唯一标识
        /// </summary>
        public int CurrentOperatorUserIdentifier
        {
            get
            {
                ConversationIMUidService ImUidService = new ConversationIMUidService(CurrentOperatorUserID);
                return ImUidService.GetUserIMUid(CurrentOperatorUserID);
            }

        }

        #endregion

        #region 当前操作医生
        /// <summary>
        /// 当前操作的医生编号
        /// </summary>
        public string CurrentOperatorDoctorID
        {
            get
            {
                var service = new XuHos.BLL.Doctor.Implements.DoctorService();
                return service.GetDoctorIDByUserID(CurrentOperatorUserID);
            }

        }

        #endregion

        #region 当前操作应用信息

        /// <summary>
        /// 
        /// </summary>
        public string CurrentOperatorAppId {

            get
            {
                return CurrentOperatorApp != null ? CurrentOperatorApp.AppId : "";
            }
        }

        /// <summary>
        /// 当前操作Apptoken 的 OrgID
        /// </summary>
        public string CurrentOperatorOrgID
        {

            get
            {
                return CurrentOperatorApp != null?CurrentOperatorApp.OrgID:"";
            }
        }

        public string CurrentOperatorAppSourceType
        {
            get
            {
                return CurrentOperatorApp!=null?CurrentOperatorApp.SourceType:"";
            }
        }

        #endregion

    }
}
