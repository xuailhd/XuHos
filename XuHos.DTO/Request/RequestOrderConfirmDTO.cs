using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    /// <summary>
    /// 支付业务类
    /// </summary>
    public class RequestOrderConfirmDTO
    {
        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 收货人详细信息
        /// </summary>
        public XuHos.DTO.Platform.OrderConsigneeDTO Consignee { get; set; }

        public EnumPayPrivilege Privilege { get; set; }

        /// <summary>
        ///  套餐ID
        /// </summary>
        public string UserPackageID { get; set; }
    }
}
