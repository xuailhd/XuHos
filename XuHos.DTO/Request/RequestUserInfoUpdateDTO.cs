using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Request
{
    public class RequestUserInfoUpdateDTO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }


        /// <summary>
        /// 用户中文名
        /// </summary>
        public string UserCNName { get; set; }

        /// <summary>
        /// 用户英文名
        /// </summary>
        public string UserENName { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// 头像路径
        /// </summary>
        public string PhotoUrl
        {
            get; set;
        }


        /// <summary>
        /// 找回密码问题
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
    }
}
