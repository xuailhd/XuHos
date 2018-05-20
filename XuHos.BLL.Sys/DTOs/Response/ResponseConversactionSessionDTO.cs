using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class ResponseConversactionSessionDTO
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set;}

        /// <summary>
        /// 消息时间
        /// </summary>
        public DateTime MessageTime { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
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
        /// 频道编号
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public XuHos.Common.Enum.EnumDoctorServiceType ServiceType { get; set; }
    }
}
