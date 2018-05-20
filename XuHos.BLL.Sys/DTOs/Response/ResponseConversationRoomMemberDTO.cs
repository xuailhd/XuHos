using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class ResponseConversationRoomMemberDTO : ImageBaseDTO
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 自己关系的 memberID
        /// </summary>
        public string MemberID { get; set; }

     

        /// <summary>
        /// 用户中文名
        /// </summary>
        public string UserCNName { get; set; }

        /// <summary>
        /// 用户英文名
        /// </summary>
        public string UserENName { get; set; }

        /// <summary>
        /// 用户类型(0-管理员、1-患者、2-医生)
        /// </summary>
        public EnumUserType UserType { get; set; }


        string _PhotoUrl { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PhotoUrl))
                {
                    if (UserType == EnumUserType.Doctor)
                    {
                        return PaddingUrlPrefix("images/doctor/default.jpg");
                    }
                    else
                    {
                        return PaddingUrlPrefix("images/doctor/unknow.png");
                    }

                }
                else
                {
                    return PaddingUrlPrefix(_PhotoUrl);
                }
            }
            set
            {
                _PhotoUrl = value;
            }
        }


        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public int identifier { get; set; }

    }
}
