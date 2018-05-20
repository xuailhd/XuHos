using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    public enum EnumSyncCRMActionCode
    {
        [Description("无效参数")]
        InvalidParams = -1,
        [Description("医院不存在")]
        NotExistsHosp = -2,
        [Description("科室不存在")]
        NotExitsDept = -3,
        [Description("该数据已存在")]
        Exists = -4,
        [Description("手机号码已存在")]
        ExistsMobile = -5,
        [Description("修改失败")]
        UpdateFail = -10,
        [Description("新增失败")]
        InsertFail = -6,
        [Description("异常")]
        Error = 0,
        [Description("修改成功")]
        UpdateSuccess = 10,        
        [Description("注册成功")]
        InsertSuccess = 1,        

    }
}
