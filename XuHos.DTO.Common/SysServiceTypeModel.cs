using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO.Common
{
    /// <summary>
    /// 系统服务类型
    /// </summary>
    public class SysServiceTypeModel
    {
        /// <summary>
        /// 服务类型ID
        /// </summary>
        public string ServiceTypeID { get; set; }

        /// <summary>
        /// 服务类型名称
        /// </summary>
        public string ServiceTypeName { get; set; }

    }
}
