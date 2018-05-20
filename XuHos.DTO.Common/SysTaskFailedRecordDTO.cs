using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.BLL.Sys
{

    public class SysTaskFailedRecordDTO
    {

        public string SysTaskFailedID { get; set; }

        /// <summary>
        /// 处理失败的模块
        /// </summary>
        public EnumTaskFailedModule Module { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 处理失败次数
        /// </summary>
        public int FailedCount { get; set; }

    }

  
}
