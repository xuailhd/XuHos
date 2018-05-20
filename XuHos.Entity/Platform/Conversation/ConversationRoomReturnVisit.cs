using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Entity.Platform.Conversation
{
    
    /// <summary>
    /// 回访
    /// </summary>
    public class ConversationRoomReturnVisit
    {
        /// <summary>
        /// 会话房间ID
        /// </summary>
        [Key, Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string ConversationRoomID { get; set; }


        /// <summary>
        /// 回访编号
        /// </summary>
        public string ReturnVisiitID { get; set; }


    }
}
