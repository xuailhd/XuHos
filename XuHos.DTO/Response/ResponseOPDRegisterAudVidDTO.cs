using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Extensions;

namespace XuHos.DTO
{



    public class ResponseOPDRegisterAudVidDTO
    {
        /// <summary>
        /// 预约登记ID
        /// </summary>
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime RegDate { get; set; }

        /// <summary>
        /// 预约类型（0-挂号、1-图文、2-语音、3-视频）
        /// </summary>
        public EnumDoctorServiceType OPDType { get; set; }

        public DateTime OPDDate { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// 订单状态（state：0-待支付、1-已支付、2-已完成、3-已取消）
        /// </summary>
        public EnumOrderState OrderState { get; set; }

        public string MemberID { get; set; }

        public string MemberName { get; set; }
        public string ConsultContent { get; set; }
        public string Birthday { get; set; }
        public EnumUserGender Gender { get; set; }
        public string GenderText {
            get
            {
                return this.Gender.GetEnumDescript();
            }
        }
        public int ChannelID { get; set; }

        public DTO.DoctorScheduleDto Schedule { get; set; }

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
