using XuHos.BLL.Sys.Implements;
using XuHos.Service.Infrastructure;
using System.Web.Http;

namespace XuHos.Service.WebStore.Controllers
{
    public class ApiBaseController : ApiController
    {


        /// <summary>
        /// 当前操作的用户编号
        /// </summary>
        public string CurrentOperatorUserID
        {

            get
            {
                var loginUser = SecurityHelper.LoginUser;
                return loginUser == null ? "" : loginUser.UserID;
            }
        }

        public XuHos.Common.Enum.EnumUserType CurrentOperatorUserType
        {

            get
            {
                return SecurityHelper.LoginUser.UserType;
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        public string CurrentOperatorAppId
        {

            get
            {
                var token = XuHos.Service.Infrastructure.SecurityHelper.GetCurrentAppToken();
                if (token != null)
                {
                    return token.AppId;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 当前操作Apptoken 的 OrgID
        /// </summary>
        public string CurrentOperatorOrgID
        {

            get
            {
                var token = XuHos.Service.Infrastructure.SecurityHelper.GetCurrentAppToken();
                if (token != null)
                {
                    return token.OrgID;
                }
                else
                {
                    return "";
                }
            }
        }

        public string CurrentOperatorAppSourceType
        {
            get
            {
                if (CurrentOperatorAppId != "")
                {
                    var account = new XuHos.BLL.Sys.Implements.SysAccessAccountService().GetById(CurrentOperatorAppId);

                    if (account != null)
                        return account.SourceType;
                }

                return "";
            }
        }

    }
}