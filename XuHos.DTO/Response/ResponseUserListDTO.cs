using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    public class ResponseUserListDTO : Common.ImageBaseDTO
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
                    return PaddingUrlPrefix("images/doctor/unknow.png");
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
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }
        int _Age;
        public int Age
        {
            get
            {
                if (_Age > 0)
                {
                    return _Age;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Birthday))
                    {
                        DateTime dt;

                        if (DateTime.TryParse(Birthday, out dt) == true)
                        {
                            _Age = DateTime.Now.Year - dt.Year;
                            if (DateTime.Now.Month >= dt.Month && DateTime.Now.Day >= dt.Day)
                            {
                                _Age += 1;
                            }
                            return _Age;
                        }
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            set
            {
                _Age = value;
            }
        }

    }
}
