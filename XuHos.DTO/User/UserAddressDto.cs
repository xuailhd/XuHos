using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
   public class UserAddressDto
    {
        public UserAddressDto()
        {
            AddressID = string.Empty;
            IsDefault = false;
        }

        /// <summary>
        /// 地址ID
        /// </summary>
        public string AddressID { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [Required]
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [Required]
        public string DetailAddress { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// 是否默认地址(0-非默认、1-默认，一个用户下只有一个默认地址)
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
