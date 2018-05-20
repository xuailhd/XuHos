using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("WebAPI返回状态定义")]
    public enum EnumApiStatus
    {
        #region 默认业务状态 0~1

        [Description("操作成功")]
        BizOK = 0,

        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        BizError = 1,

        #endregion

        #region 系统接口状态 2~99

        /// <summary>
        /// 接口参数签名错误
        /// </summary>     
        [Description("接口签名参数错误")]
        ApiParamSignError = 2,

        /// <summary>
        /// 非法请求
        /// </summary>        
        [Description("接口用户令牌错误")]
        ApiParamTokenError = 3,

        /// <summary>
        /// 接口参数数据验证失败
        /// </summary>
        [Description("接口数据验证失败")]
        ApiParamModelValidateError = 4,

        /// <summary>
        /// 接口参数应用签名过期
        /// </summary>
        [Description("接口应用令牌过期")]
        ApiParamAppTokenExpire = 5,

        /// <summary>
        /// 接口时间戳参数错误
        /// </summary>
        [Description("接口时间戳参数错误")]
        ApiParamTimestampError = 9,

        /// <summary>
        /// 重复请求
        /// </summary>
        [Description("接口随机参数错误（重复请求)")]
        ApiRepeatedAccess = 8,

        /// <summary>
        /// 用户未登录
        /// </summary>
        [Description("用户未登录")]
        ApiUserNotLogin = 6,

        /// <summary>
        /// 用户无权限访问
        /// </summary>
        [Description("用户无权限访问")]
        ApiUserUnauthorized = 7,

        /// <summary>
        /// 操作成功
        /// </summary>      

        #endregion

        #region 账户：用户登录 100~109

        /// <summary>
        /// 用户登录账号或密码错误
        /// </summary>
        [Description("用户登录账号或密码错误")]
        BizUserLoginAccountOrPwdFail = 100,

        /// <summary>
        /// 用户登录验证码错误
        /// </summary>
        [Description("验证码错误")]
        BizUserLoginVerrifyCodeFail = 101,

        /// <summary>
        /// 用户帐号过期
        /// </summary>
        [Description("用户帐号过期")]
        BizUserLoginAccountExpiredFail = 102,

        #endregion

        #region 支付：余额支付 110~129

        /// <summary>
        /// 付款用户或收款用户不存在
        /// </summary>
        [Description("付款用户或收款用户不存在")]
        BizBalancePaymentAccountNotExists = 112,

        /// <summary>
        /// 支付业务不存在
        /// </summary>
        [Description("支付业务不存在")]
        BizBalancePaymentTransactionNotExists = 113,

        /// <summary>
        /// 余额不足
        /// </summary>
        [Description("余额不足")]
        BizBalancePaymentBalanceIsNotEnough = 117,

        /// <summary>
        /// 不支持余额支付
        /// </summary>
        [Description("不支持余额支付")]
        BizBalancePaymentNotSupport = 120,

        /// <summary>
        /// 用户支付密码错误
        /// </summary>
        [Description("用户支付密码错误")]
        BizBalancePaymentPayPasswordError = 121,

        #endregion

        #region 子系统：远程会诊 200~299

        /// <summary>
        /// 患者不存在
        /// </summary>
        [Description("患者不存在")]
        PatientNotExists = 200,

        /// <summary>
        /// 还有未完成的会诊单
        /// </summary>
        [Description("还有未完成的会诊单")]
        HaveConsulNotFinised = 201,

        /// <summary>
        /// 只能有一个主诊医生
        /// </summary>
        [Description("只能有一个主诊医生")]
        OnlyAttendingDoctorOne = 202,

        /// <summary>
        /// 会诊医生不能大于6个
        /// </summary>
        [Description("会诊医生不能大于6个")]
        DoctorCountGTSix = 203,

        /// <summary>
        /// 会诊目的和要求不能为空
        /// </summary>
        [Description("会诊目的和要求不能为空")]
        PurposeNotEmpty = 204,

        /// <summary>
        /// 患者主诉，病情描述，初步诊断不能为空
        /// </summary>
        [Description("患者主诉，病情描述，初步诊断不能为空")]
        SubjectNotEmpty = 205,

        /// <summary>
        /// 还未分配主诊医生
        /// </summary>
        [Description("还未分配主诊医生")]
        NoAssignedDoctor = 206,

        /// <summary>
        /// 还未分配主诊医生
        /// </summary>
        [Description("还未分配会诊专家")]
        NoAssignedSpecialty = 207,

        /// <summary>
        /// 会诊单不存在
        /// </summary>
        [Description("会诊单不存在")]
        ConsultationNotExists = 208,

        /// <summary>
        /// 该状态不能修改
        /// </summary>
        [Description("该状态不能修改")]
        CurrStatusNotModify = 209,

        /// <summary>
        /// 已取消，已付款，已完成的订单不能再修改
        /// </summary>
        [Description("已取消，已付款，已完成的订单不能再修改")]
        CurrOrderStatusNotModify = 210,

        /// <summary>
        /// 手机号已存在
        /// </summary>
        [Description("手机号已存在")]
        MobileIsExists = 211,

        /// <summary>
        /// 手机号和姓名不能为空
        /// </summary>
        [Description("手机号和姓名不能为空")]
        MobileAndNameNotEmpty = 212,

        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        OfflinePayed = 213,

        /// <summary>
        /// 订单不存在
        /// </summary>
        [Description("支付订单还未创建")]
        OrderNoExists = 214,

        /// <summary>
        /// 未上传支付附件
        /// </summary>
        [Description("请上传付款附件")]
        NoPayedFile = 215,

        /// <summary>
        /// 支付金额与订单金额不一致
        /// </summary>
        [Description("支付金额与订单金额不一致")]
        AmountInconsistent = 216,

        /// <summary>
        /// 不能更改会诊状态
        /// </summary>
        [Description("不能更改会诊状态")]
        ConsultationNotChangeProgress = 217,

        /// <summary>
        /// 订单已取消，不能支付
        /// </summary>
        [Description("订单已取消不能支付")]
        InvalidNoPay = 218,

        /// <summary>
        /// 已退款
        /// </summary>
        [Description("已退款，等待系统处理")]
        OfflineRefunded = 219,

        /// <summary>
        /// 不能线下退款
        /// </summary>
        [Description("该订单不能线下退款")]
        CannotOfflineRefunded = 220,

        /// <summary>
        /// 操作过快，请输后再试
        /// </summary>
        [Description("操作过快，请输后再试")]
        OperationTooFast = 221,

        #endregion

        #region 账户：修改密码 300~399

        /// <summary>
        /// 新密码与确认密码不一致
        /// </summary>
        [Description("新密码与确认密码不一致")]
        BizChangePasswordConfirmPasswordError = 300,

        /// <summary>
        /// 新密码与旧密码不能一致
        /// </summary>
        [Description("新密码与旧密码不能一致")]
        BizChangePasswordNewPasswordEqualOld = 301,

        /// <summary>
        /// 原始密码错误
        /// </summary>
        [Description("原始密码错误")]
        BizChangePasswordOldPasswordError = 302,

        /// <summary>
        /// 修改密码失败
        /// </summary>
        [Description("修改密码失败")]
        BizChangePasswordChangePasswordFail = 303,

        #endregion

        #region 账户：用户注册  400~499

        /// <summary>
        /// 手机号码已存在
        /// </summary>
        [Description("用户注册失败手机号码已存在")]
        BizUserRegisterMobileExists = 400,

        /// <summary>
        /// 手机号码格式错误
        /// </summary>
        [Description("手机号码格式错误")]
        BizUserRegisterMobileFormatError = 401,

        /// <summary>
        /// 身份证已经存在
        /// </summary>
        [Description("身份证已经存在")]
        BizUserRegisterIDNumberExists = 410,

        /// <summary>
        /// 身份证格式错误
        /// </summary>
        [Description("身份证格式错误")]
        BizUserRegisterIDNumberFormatError = 412,

        /// <summary>
        /// 医生已经存在机构内
        /// </summary>
        [Description("医生已经存在机构内")]
        BizDoctorExistsInOrg = 413,

        /// <summary>
        /// 用户注册失败
        /// </summary>
        [Description("注册失败")]
        BizUserRegisterFail = 499,

        #endregion

        #region 账户：就诊人管理 500~599

        /// <summary>
        /// 拒绝添加，超过最大人数限制
        /// </summary>
        [Description("拒绝添加，超过最大人数限制")]
        BizUserMemberRejectInsertOverLimit = 500,

        /// <summary>
        /// 身份证号码格式错误
        /// </summary>
        [Description("身份证号码格式错误")]
        BizUserMemberRejectInsertUpdateIDNumberFormatError = 501,

        /// <summary>
        /// 无就诊人姓名
        /// </summary>
        [Description("无就诊人姓名")]
        BizUserMemberInsertUpdateMemberNameIsNull = 502,

        /// <summary>
        /// 无诊疗卡发卡机构编号
        /// </summary>
        [Description("无诊疗卡发卡机构编号")]
        BizUserMemberInsertUpdateMedicalCardHospitalIDIsNull = 503,

        /// <summary>
        /// 无诊疗卡编号
        /// </summary>
        [Description("无诊疗卡编号")]
        BizUserMemberInsertUpdateMedicalCardNumberIsNull = 504,

        /// <summary>
        /// 拒绝删除，不能删除关系为“本人”的就诊人
        /// </summary>
        [Description("拒绝删除，不能删除关系为“本人”的就诊人")]
        BizUserMemberRejectDeleteMySelf = 510,

        /// <summary>
        /// 拒绝删除，存在关联的订单
        /// </summary>
        [Description("拒绝删除，存在关联的订单")]
        BizUserMemberRejectDeleteHasRelationOrder = 511,

        /// <summary>
        /// 拒绝修改，只能有一个关系为“本人”的就诊人
        /// </summary>
        [Description("拒绝修改，只能有一个关系为“本人”的就诊人")]
        BizUserMemberRejectUpdateMySelfExists = 520,

        /// <summary>
        /// 拒绝添加，身份证号已绑定
        /// </summary>
        [Description("拒绝添加，身份证号已绑定")]
        BizUserMemberRejectIDNumberExists = 521,

        /// <summary>
        /// 拒绝添加，身份证号没有绑定手机号或没建档
        /// </summary>
        [Description("拒绝添加，身份证号没有绑定手机号")]
        BizUserMemberRejectIDNumberNoPhone = 522,

        /// <summary>
        /// 拒绝添加，身份证号没有建档
        /// </summary>
        [Description("拒绝添加，身份证号没有建档")]
        BizUserMemberRejectIDNumberNoEMR = 523,

        #endregion

        #region 子系统：处方购买 600-699

        /// <summary>
        /// 拒绝购买处方，处方已过期
        /// </summary>
        [Description("拒绝购买处方，处方已过期")]
        BizRecipeOrderRejectBuyIfPrescriptionExpired = 600,

        /// <summary>
        /// 拒绝购买处方，重复购买
        /// </summary>
        [Description("拒绝购买处方，重复购买")]
        BizRecipeOrderRejectBuyIfRepeaterSubmit = 601,

        /// <summary>
        /// 没有处方记录
        /// </summary>
        [Description("拒绝购买处方，没有处方记录")]
        BizRecipeOrderRejectNotRecipeRecord = 602,

        #endregion

        #region 子系统：诊室 700-799

        /// <summary>
        /// 连接未就绪,请稍后重试
        /// </summary>
        [Description("连接未就绪,请稍后重试")]
        BizChannelNotReady = 700,

        /// <summary>
        /// 拒绝进入诊室，请在预约时间内或提前30分钟进入诊室
        /// </summary>
        [Description("拒绝进入诊室，请在预约时间内或提前30分钟进入诊室")]
        BizChannelRejectConnectIfNoReservationTime = 701,

        /// <summary>
        /// 拒绝设置状态，当前状态不是预期状态
        /// </summary>
        [Description("拒绝设置状态，当前状态不是预期状态")]
        BizChannelRejectSetStateIfNotExpectedState = 702,

        /// <summary>
        /// 拒绝设置状态，设置状态超时
        /// </summary>
        [Description("设置状态超时")]
        BizChanneSetStateIfTimeout = 703,

        /// <summary>
        /// 拒绝设置状态，当前诊室已失效
        /// </summary>
        [Description("拒绝设置状态，当前诊室已失效")]
        BizChannelSetStateIfClose = 704,


        /// <summary>
        /// 拒绝进入诊室，医生当前正在休诊
        /// </summary>
        [Description("拒绝进入诊室，医生当前正在休诊")]
        BizChannelRejectConnectIfDiagnoseOff = 705,
        #endregion

        #region 子系统：订单管理 800-899

        [Description("订单不存在")]
        BizOrderNotExists = 800,

        [Description("收货人无效")]
        BizOrderRejectConfirmIfConsigneeInvalid = 801,

        /// <summary>
        /// 折扣不可用
        /// </summary>
        [Description("折扣不可用")]
        BizOrderRejectConfirmIfPrivilegeUnavailable = 802,

        /// <summary>
        /// 已发货,配送中,已送达的订单不能取消
        /// </summary>
        [Description("已发货,配送中,已送达的订单不能取消")]
        BizOrderCannotCancel = 803,

        #endregion

        #region 子系统：医生服务 900-999

        /// <summary>
        /// 图文咨询、语音咨询和视频咨询同时开启状态，才能开启家族服务
        /// </summary>
        [Description("图文咨询、语音咨询和视频咨询同时开启状态，才能开启家族服务")]
        BizDoctorServiceNotOpenFamilyDoctorService = 900,

        /// <summary>
        /// 义诊月份不能为空
        /// </summary>
        [Description("义诊月份不能为空")]
        BizDoctorServiceClinicMonthNotEmpty = 901,

        /// <summary>
        /// 义诊日期不合法
        /// </summary>
        [Description("义诊日期不合法")]
        BizDoctorServiceClinicDayNotError = 902,

        /// <summary>
        /// 未开通图文咨询服务，不能设置义诊
        /// </summary>
        [Description("未开通图文咨询服务，不能设置义诊")]
        BizDoctorServicePicServiceIsClose = 903,

        /// <summary>
        /// 还在停诊中，不能再设置停诊
        /// </summary>
        [Description("还在停诊中，不能再设置停诊")]
        BizDoctorServiceCannotSetStopDiagnosis = 904,

        #endregion

        #region 子系统：医生任务 1100-1199

        /// <summary>
        /// 医生任务池是空的
        /// </summary>
        [Description("没有需要任务需要领取")]
        BizDoctorTaskPoolEmpty = 1100,

        /// <summary>
        /// 医生任务池是空的
        /// </summary>
        [Description("服务器忙，请稍后重试")]
        BizDoctorTaskAlreadyTaskUnhandledFinish = 1101,

        #endregion

        #region 私人医生 1200-1299

        /// <summary>
        /// 已签约私人医生
        /// </summary>
        [Description("已签约")]
        PersonalSigned = 1200,

        /// <summary>
        /// 未签约私人医生
        /// </summary>
        [Description("未签约")]
        UnPersonalSigned = 1201,

        /// <summary>
        /// 改签次数用完
        /// </summary>
        [Description("不能改签")]
        UnChangedSign = 1202,

        #endregion

        #region 药店账号相关 1300-1399

        /// <summary>
        /// 药店名称已存在
        /// </summary>
        [Description("药店名称已存在")]
        BizDrugstoreRegisterExistsDrugstore = 1300,

        /// <summary>
        /// 父级机构ID无效
        /// </summary>
        [Description("父级机构ID无效")]
        BizDrugstoreRegisterParentIdInvalid = 1301,

        /// <summary>
        /// 机构ID无效
        /// </summary>
        [Description("机构ID无效")]
        BizDrugstoreCreateAccountOrgIdInvalid = 1310,

        /// <summary>
        /// 账号已存在
        /// </summary>
        [Description("账号已存在")]
        BizDrugstoreCreateAccountExistsAccount = 1311,

        /// <summary>
        /// 药品已经被使用，不能删除
        /// </summary>
        [Description("药品已经被使用，不能删除")]
        BizDrugstoreDrugUsed = 1320,

        #endregion

        #region

        [Description("短信超频")]
        BizSMSOverclock = 1401,

        #endregion

        #region 医生排班 1500-1599

        /// <summary>
        /// 开始日期不能大于结束日期
        /// </summary>
        [Description("开始日期不能大于结束日期")]
        BizDoctorSchduleBeginDateGTEndDate = 1500,

        /// <summary>
        /// 开始日期不能小于当前日期
        /// </summary>
        [Description("开始日期不能小于当前日期")]
        BizDoctorSchduleBeginDateLTNowDate = 1501,

        /// <summary>
        /// 该号源表生效时间与其它号源表生效时间重叠
        /// </summary>
        [Description("该号源表生效时间与其它号源表生效时间重叠")]
        BizDoctorSchduleDateOverlay = 1502,

        /// <summary>
        /// 该名称已存在
        /// </summary>
        [Description("该名称已存在")]
        BizDoctorSchduleTemNameExists = 1503,

        /// <summary>
        /// 最多可设定5个模板
        /// </summary>
        [Description("最多新建5个排班模板")]
        BizDoctorSchduleTmpMaxFive = 1504,

        /// <summary>
        /// 排班最长时间不能大于一年
        /// </summary>
        [Description("排班最长时间不能大于一年")]
        BizDoctorSchduleTimeGTYear = 1505,

        /// <summary>
        /// 结束时间请选择在周日
        /// </summary>
        [Description("排班结束时间必须为周日")]
        BizEndDateNotOnSunday = 1506,

        #endregion

        #region
        /// <summary>
        /// 没有合适的排班
        /// </summary>
        [Description("没有合适的排班")]
        DoctorGuianceNoProperSchedule = 1600,
        #endregion

        //更多的状态自己可以添加（业务状态需大于100，例如10100，10200这样可保存一定的扩展性）
    }
}