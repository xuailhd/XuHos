using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    ///  商品类型
    /// 0-挂号、1-图文咨询、2-语音问诊、3-视频问诊、4-处方支付、5-家庭医生、6-会员套餐、7-远程会诊、8-影像判读、100-其它
    /// </summary>
    [Description("商品类型")]
    public enum EnumProductType
    {
        /// <summary>
        /// 挂号
        /// </summary>
        [Description("挂号")]
        Registration = 0,

        /// <summary>
        /// 图文咨询
        /// </summary>
        [Description("图文咨询")]
        ImageText = 1,

        /// <summary>
        /// 电话咨询
        /// </summary>
        [Description("语音问诊")]
        Phone = 2,

        /// <summary>
        /// 视频咨询
        /// </summary>
        [Description("视频问诊")]
        video = 3,

        /// <summary>
        /// 处方支付
        /// </summary>
        [Description("处方支付")]
        Recipe = 4,

        /// <summary>
        /// 家庭医生
        /// </summary>
        [Description("家庭医生")]
        FamilyDoctor = 5,

        /// <summary>
        /// 会员套餐
        /// </summary>
        [Description("会员套餐")]
        UserPackage = 6,

        /// <summary>
        /// 远程会诊
        /// </summary>
        [Description("远程会诊")]
        Consultation = 7,

        /// <summary>
        /// 影像判读
        /// </summary>
        [Description("影像判读")]
        ImageInterpretation = 8,

        /// <summary>
        /// 用户充值
        /// </summary>
        [Description("用户充值")]
        UserRecharge = 9,

        /// <summary>
        /// 大师手法（蒙发利）
        /// </summary>
        [Description("大师手法")]
        MasterTechnique = 10,


        /// <summary>
        /// 续费升级
        /// </summary>
        [Description("续费升级")]
        RenewUpgrade = 11,

        /// <summary>
        /// 私人医生
        /// </summary>
        [Description("私人医生")]
        PersonalDoctor = 12,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 100


    }
}
