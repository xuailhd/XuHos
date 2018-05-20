using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity
{
    /// <summary>
    /// 会话房间
    /// </summary>
    public class ConversationRoomUid : AuditableEntity
    {
        public ConversationRoomUid()
        {
            PhotoUrl = "";
        }


        [Key, Required]
        /// <summary>
        /// 房间编号
        /// </summary>
        public string ConversationRoomID { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        public int  Identifier { get; set; }

        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 房间编号
        /// </summary>
        [Obsolete("已经废弃")]
        public int ChannelID { get; set; }

        /// <summary>
        /// 用户成员编号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserMemberID { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string UserID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string UserCNName { get; set; }


        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string UserENName { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(200)]
        public string PhotoUrl { get; set; }
    }
}
