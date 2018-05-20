using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    /// <summary>
    /// 订单日志
    /// </summary>

    public partial class ResponseOrderLogDTO
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public XuHos.Common.Enum.EnumEnumOrderLogOperationType OperationType { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OperationDesc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public System.DateTime OperationTime { get; set; }
    }
}
