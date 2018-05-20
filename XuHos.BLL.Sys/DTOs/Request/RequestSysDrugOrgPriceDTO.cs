using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestSysDrugOrgPriceDTO
    {
        /// <summary> 
        /// 渠道价格ID
        /// </summary>
        public string SysDrugOrgPriceID { get; set; }

        /// <summary>
        /// 药品ID
        /// </summary>
        public string DrugID { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int VersionNo { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime AvailableTime { get; set; } = DateTime.Now;
    }
}
