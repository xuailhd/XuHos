using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class RespnoseConversactionFriendDTO
    {
        /// <summary>
        /// 好友的Identifier
        /// </summary>
        public int Identifier { get; set; }

        public int ChannelID { get; set; }

        public string UserID { get; set; }

        public string NickName { get; set; }

        string _Avatar { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string Avatar
        {
            get
            {
                return XuHos.DTO.Common.ImageBaseDTO.PaddingUrlPrefix(_Avatar);
            }
            set
            {
                _Avatar = value;
            }
        }

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

   
  

    }
}
