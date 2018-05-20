using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    /// <summary>
    /// 关联外部业务数据
    /// </summary>
    public class ConversationExternalDTO
    {

        /// <summary>
        /// 关联ID
        /// </summary>
        [MaxLength(32)]
        public string ConversationExternalID { get; set; }

        /// <summary>
        /// 业务ID(内部业务ID)
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// 外部业务ID
        /// </summary>
        public string OutServiceID { get; set; }

        /// <summary>
        /// 外部业务父级ID
        /// </summary>
        public string OutParentServiceID { get; set; }
        
        /// <summary>
        /// 外部业务父级名称
        /// </summary>
        public string OutParentServiceName { get; set; }

        /// <summary>
        /// 关联的业务表(Hospitals，HospitalDepartments，Doctors等)
        /// </summary>
        public EnumExternalTable ExternalTable { get; set; }

        /// <summary>
        /// 关联的外部系统(His，BAT)
        /// </summary>
        public EnumExternalSystem ExternalSystem { get; set; }


    }
}
