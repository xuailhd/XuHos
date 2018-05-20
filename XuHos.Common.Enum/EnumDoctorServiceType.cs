using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    #region 医生服务类型枚举信息
    /// <summary>
    /// 医生服务类型 0-挂号 1-图文咨询、2-语音咨询、3-视频咨询、4-家庭医生、5-远程会诊
    /// </summary>
    [Description("医生服务类型")]
    public enum EnumDoctorServiceType
    {
        /// <summary>
        /// 挂号
        /// </summary>
        [Description("医院挂号")]
        Registration = 0,
        /// <summary>
        /// 图文咨询
        /// </summary>
        [Description("图文咨询")]
        PicServiceType = 1,

        /// <summary>
        /// 语音问诊
        /// </summary>
        [Description("语音咨询")]
        AudServiceType = 2,

        /// <summary>
        /// 视频问诊
        /// </summary>
        [Description("视频咨询")]
        VidServiceType = 3,

        /// <summary>
        /// 家庭医生
        /// </summary>
        [Description("家庭医生")]
        FamilyDoctor = 4,

        /// <summary>
        /// 远程会诊
        /// </summary>
        [Description("远程会诊")]
        Consultation = 5,

        /// <summary>
        /// 处方药品
        /// </summary>
        [Description("处方药品")]
        Recipe = 6

    }
    #endregion
}
