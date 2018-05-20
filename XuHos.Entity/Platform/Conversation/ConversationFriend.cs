using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    /// <summary>
    /// 好友
    /// </summary>
    public class ConversationFriend : AuditableEntity
    {
        public ConversationFriend()
        {
          
         
        }

        [Required]
        [Key]
        [MaxLength(32)]
        public string FriendID { get; set; }

        [Required]
        [MaxLength(32)]
        public string ConversationRoomID { get; set; }

        [Required]
        public int FromUserIdentifier { get; set; }

        [Required]
        public int ToUserIdentifier { get; set; }

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
        /// From_Account和To_Account形成好友关系时的附言信息
        /// </summary>
        public string AddWording
        { get; set; }

 

        [Required]
        public string ToUserID { get; set; }

        [Required]
        public string FromUserID { get; set; }
    }
}
