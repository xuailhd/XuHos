using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Integration.QQCloudy.Models
{
    public class AddFriendAccount
    {
        public AddFriendAccount()
        {
        }


        /// <summary>
        /// 好友的Identifier
        /// </summary>
        [Required]
        public string To_Account { get; set; }


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
        /// From_Account和To_Account形成好友关系时的附言信息
        /// </summary>
        public string AddWording
        { get; set; }

    
    }
}
