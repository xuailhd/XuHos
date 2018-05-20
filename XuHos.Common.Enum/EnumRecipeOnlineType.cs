using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace SmartRecipeMP.Common.Enum
{
    /// <summary>
    /// 处方线上线下状态：0 离线处方  1 线下处方 2 在线处方 
    /// </summary>
    public enum EnumRecipeOnlineType
    {
        /// <summary>
        ///  0 离线处方
        /// </summary>
        [Description("离线处方")]
        Offline = 0,
        /// <summary>
        /// 1 线下处方
        /// </summary>
        [Description("线下处方")]
        NoOnline = 1,
        /// <summary>
        /// 2 在线处方
        /// </summary>
        [Description("在线处方")]
        Online = 2,
        
    }
}
