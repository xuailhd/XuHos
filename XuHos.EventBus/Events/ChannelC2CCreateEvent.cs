using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class ChannelC2CCreateEvent: BaseEvent,IEvent
    {
        public ChannelC2CCreateEvent()
        {
            AddFriendItem = new List<AddFriendAccount>();
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string FromUserID { get; set; }

        public List<AddFriendAccount> AddFriendItem { get; set; }

        public class AddFriendAccount
        {

            /// <summary>
            /// 好友的Identifier
            /// </summary>
            [Required]
            public string ToUserID { get; set; } 

            /// <summary>
            /// From_Account对To_Account的分组信息
            /// </summary>
            [Required]
            public string GroupName { get; set; }
      

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

    

        }
    }
}
