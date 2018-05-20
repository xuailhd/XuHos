using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestConversactionApplyAddFriendDTO
    {
        public RequestConversactionApplyAddFriendDTO()
        {
            AddFriendItem = new List<AddFriendAccount>();
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public int FromUserIdentifier { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public XuHos.Common.Enum.EnumUserType FromUserType { get; set; }

        /// <summary>
        /// 成员编号
        /// </summary>
        public string FromUserMemberID { get; set; }

        public string FromUserID { get; set; }

        public string FromUserName { get; set; }

        public List<AddFriendAccount> AddFriendItem { get; set; }

        public class AddFriendAccount
        {
            public AddFriendAccount()
            {
                ForceAddFlags = 1;
                AddSource = "";
          
            }


            /// <summary>
            /// 好友的Identifier
            /// </summary>
            [Required]
            public int ToUserIdentifier { get; set; }

            /// <summary>
            /// 用户类型
            /// </summary>
            public XuHos.Common.Enum.EnumUserType ToUserType { get; set; }

            /// <summary>
            ///成员编号
            /// </summary>
            public string ToUserMemberID { get; set; }

            /// <summary>
            /// From_Account对To_Account的分组信息
            /// </summary>
            [Required]
            public string GroupName { get; set; }

            /// <summary>
            /// 加好友来源字段
            /// </summary>
            [Required]
            public string AddSource { get; set; }

            /// <summary>
            /// From_Account对To_Account的好友备注
            /// </summary>
            public string Remark { get; set; }


            /// <summary>
            /// 加好友方式（默认双向加好友方式）："Add_Type_Single"表示单向加好友；"Add_Type_Both"表示双向加好友。
            /// </summary>
            public string AddType { get; set; }


            /// <summary>
            /// From_Account和To_Account形成好友关系时的附言信息
            /// </summary>
            public string AddWording
            { get; set; }

            /// <summary>
            /// 管理员强制加好友标记：1表示强制加好友；0表示常规加好友方式
            /// </summary>
            public int ForceAddFlags { get; set; }
            public string ToUserID { get; set; }
            public string ToUserName { get; set; }
        }
    }
}
