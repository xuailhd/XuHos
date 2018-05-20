using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 100 以后 为特例扩展角色
    /// </summary>
    [Description("角色类型")]
    public enum EnumRoleType
    {
        #region 医生
        /// <summary>
        /// 看诊医生   
        /// </summary>
        [Description("看诊医生")]
        DocVist = 1,

        /// <summary>
        /// 会诊医生
        /// </summary>
        [Description("会诊医生")]
        DocConsultation = 2,

        /// <summary>
        /// 审方医生
        /// </summary>
        [Description("审方医生")]
        DocRecipe = 3,

        /// <summary>
        /// 家庭医生
        /// </summary>
        [Description("家庭医生")]
        DocFamily = 4,

        /// <summary>
        /// 诊所医生
        /// </summary>
        [Description("诊所医生")]
        DocClinic = 5,


        /// <summary>
        /// 药店坐诊开方医生
        /// </summary>
        [Description("药店坐诊开方医生")]
        DocDrugstore = 6,
        #endregion

        #region 机构
        /// <summary>
        /// 机构管理员
        /// </summary>
        [Description("机构管理员")]
        OrgManage = 11,

        /// <summary>
        /// 机构护士
        /// </summary>
        [Description("机构护士")]
        OrgNurse = 12,

        /// <summary>
        /// 机构客服
        /// </summary>
        [Description("机构客服")]
        OrgCustom = 13,

        /// <summary>
        /// 机构领导
        /// </summary>
        [Description("机构领导")]
        OrgLeader = 14,
        #endregion

        #region 药店

        /// <summary>
        /// 药店管理员
        /// </summary>
        [Description("药店管理员")]
        DrugManage = 20,

        /// <summary>
        /// 药店店员
        /// </summary>
        [Description("药店店员")]
        DrugOper = 21,

        /// <summary>
        /// 药店药师
        /// </summary>
        [Description("药店药师")]
        DrugApothecary = 22,

        /// <summary>
        /// 药店看诊账号
        /// </summary>
        [Description("药店看诊账号")]
        DrugTreatment = 23,
        #endregion

        #region 特殊
        /// <summary>
        /// 后台管理员角色类型
        /// </summary>
        [Description("后台管理员角色类型")]
        AdminManage = 30,

        /// <summary>
        /// 彼爱用户
        /// </summary>
        [Description("彼爱用户")]
        PierUser = 101,

        /// <summary>
        /// Vip6
        /// </summary>
        [Description("Vip6")]
        Vip6 = 102,

        /// <summary>
        /// 集团领导
        /// </summary>
        [Description("集团领导")]
        GroupLeader =103,

        /// <summary>
        /// 政府官员
        /// </summary>
        [Description("政府官员")]
        Official = 104,

        /// <summary>
        /// 经销商
        /// </summary>
        [Description("经销商")]
        Agency = 105,

        /// <summary>
        /// 体验店账号
        /// </summary>
        [Description("体验店账号")]
        TYD = 106,


        /// <summary>
        /// 线下家庭医生工作人员
        /// </summary>
        [Description("线下家庭医生工作人员")]
        OfflineOper = 107,

        /// <summary>
        /// 线下家庭医生领导
        /// </summary>
        [Description("线下家庭医生领导")]
        OfflineLead = 108,

        /// <summary>
        /// 线下家庭医生管理员
        /// </summary>
        [Description("线下家庭医生管理员")]
        OfflineAdmin = 109,

        /// <summary>
        /// 家庭套餐账号
        /// </summary>
        [Description("家庭套餐账号")]
        FamilyPackage = 110
        #endregion
    }
}
