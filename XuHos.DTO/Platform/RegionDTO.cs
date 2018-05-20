using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class RegionDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public string RegionID { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        public string RegionCode { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public EnumRegionLevel RegionLevel { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int RegionOrder { get; set; }

        /// <summary>
        /// 区域英文名称
        /// </summary>
        public string RegionNameEN { get; set; }

        /// <summary>
        /// 英文简写
        /// </summary>
        public string RegionShortNameEN { get; set; }
    }
}
