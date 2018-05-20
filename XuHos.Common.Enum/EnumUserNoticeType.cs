using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("用户通知类型")]
    public enum EnumUserNoticeType
    {
        /// <summary>
        /// 患者端
        /// </summary>
        [Description("患者端")]
        User = 1,
        /// <summary>
        /// 医生端
        /// </summary>
        [Description("医生端")]
        Doctor = 2,
        /// <summary>
        /// 远程会诊平台
        /// </summary>
        [Description("远程会诊平台")]
        RemoteConsultation = 3
    }

    [Description("一级消息分类")]
    public enum EnumNoticeFirstType
    {
        /// <summary>
        /// 所有用户-系统公告
        /// </summary>
        [Description("系统公告")]
        AllNotice = 0,
        /// <summary>
        /// 医生端-系统公告
        /// </summary>
        [Description("系统公告")]
        DoctorSystemNotice = 1,

        /// <summary>
        /// 医生端-订单消息
        /// </summary>
        [Description("订单消息")]
        DoctorOrderNotice = 2,

        /// <summary>
        /// 医生端-业务消息
        /// </summary>
        [Description("业务消息")]
        DoctorBusinessNotice = 3,

        /// <summary>
        /// 患者端-系统公告
        /// </summary>
        [Description("系统公告")]
        UserSystemNotice = 4,

        /// <summary>
        /// 患者端-服务消息
        /// </summary>
        [Description("服务消息")]
        UserServerNotice = 5,

        /// <summary>
        /// 远程会诊平台-业务消息
        /// </summary>
        [Description("远程会诊平台-业务消息")]
        RemoteConsultationBusinessNotice = 6
    }

    [Description("二级消息分类")]
    public enum EnumNoticeSecondType
    {
        /// <summary>
        /// 所有用户-系统公告-通知/公告
        /// </summary>
        [Description("所有用户-系统公告-通知/公告")]
        AllNotice = 0,

        /// <summary>
        /// 医生端-系统公告-通知/公告
        /// </summary>
        [Description("医生端-系统公告-通知/公告")]
        DoctorNotice = 1,

        /// <summary>
        /// 医生端-订单消息-图文咨询
        /// </summary>
        [Description("医生端-订单消息-图文咨询")]
        DoctorPicNotice = 2,

        /// <summary>
        /// 医生端-订单消息-音视频问诊
        /// </summary>
        [Description("医生端-订单消息-音视频问诊")]
        DoctorVidNotice = 3,


        /// <summary>
        /// 医生端-订单消息-家庭医生
        /// </summary>
        [Description("医生端-订单消息-家庭医生")]
        DoctorFamilyNotice = 4,

        /// <summary>
        /// 医生端-订单消息-远程会诊
        /// </summary>
        [Description("医生端-订单消息-远程会诊")]
        DoctorConsulNotice = 5,

        /// <summary>
        /// 医生端-业务消息-音视频问诊患者进入诊室
        /// </summary>
        [Description("医生端-业务消息-音视频问诊患者进入诊室")]
        DoctorVidUserEnterRoomNotice = 6,

        /// <summary>
        /// 医生端-业务消息-药店处方
        /// </summary>
        [Description("医生端-业务消息-药店处方")]
        DoctorPrescriptionNotice = 7,

        /// <summary>
        /// 医生端-业务消息-会诊正在进行
        /// </summary>
        [Description("医生端-业务消息-会诊正在进行")]
        DoctorConsulingNotice = 8,

        /// <summary>
        /// 患者端-系统公告-通知/公告
        /// </summary>
        [Description("患者端-系统公告-通知/公告")]
        UserNotice = 9,

        /// <summary>
        /// 患者端-服务消息-图文咨询医生回复
        /// </summary>
        [Description("患者端-服务消息-图文咨询医生回复")]
        UserPicDoctorReplyNotice = 10,

        /// <summary>
        /// 患者端-服务消息-处方已生成
        /// </summary>
        [Description("患者端-服务消息-处方已生成")]
        UserPrescriptionNotice = 11,

        /// <summary>
        /// 患者端-服务消息-音视频看诊医生呼叫
        /// </summary>
        [Description("患者端-服务消息-音视频看诊医生呼叫")]
        UserVidDoctorCallNotice = 12,

        /// <summary>
        /// 医生端-订单消息-报告解读
        /// </summary>
        [Description("医生端-订单消息-报告解读")]
        DoctorReportInterpretation = 13,

        /// <summary>
        /// 医生端-服务消息-统计数据
        /// </summary>
        [Description("医生端-服务消息-统计数据")]
        DoctorStatistics = 14,

        /// <summary>
        /// 医生端-BAT端生成的消息
        /// </summary>
        [Description("医生端-BAT端生成的消息")]
        DoctorBATMessage = 15,

        /// <summary>
        /// 远程会诊平台-订单消息
        /// </summary>
        [Description("远程会诊平台-订单消息")]
        RemoteConsultationOrderNotice = 16,

        /// <summary>
        /// 远程会诊平台-整理会诊意见消息
        /// </summary>
        [Description("远程会诊平台-整理会诊意见消息")]
        RemoteConsultationSummaryOpinionNotice = 17,

        /// <summary>
        /// 远程会诊平台-会诊开始消息
        /// </summary>
        [Description("远程会诊平台-会诊开始消息")]
        RemoteConsultationStartNotice = 18,

        /// <summary>
        /// 医生端-业务消息-一键呼叫申请的问诊无人领取
        /// </summary>
        [Description("医生端-业务消息-一键呼叫申请的问诊无人领取")]
        DoctorInquiriesUntakenNotice = 19,

        /// <summary>
        /// 医生端-业务消息-停诊取消订单通知患者
        /// </summary>
        [Description("医生端-业务消息-停诊取消订单通知患者")]
        DoctorStopDiagnosisCancelOrderNotice = 20,
        /// <summary>
        /// 健康教育
        /// </summary>
        [Description("用户端-系统公告-健康教育")]
        UserHealthEducationNotice = 0101021,

        /// <summary>
        /// 健康教育
        /// </summary>
        [Description("用户端-系统公告-健康随访")]
        UserHelathFollowupNotice = 0101022,

        /// <summary>
        /// 导诊平台，通知护士有新的订单
        /// </summary>
        [Description("导诊平台，通知护士有新的订单")]
        NurseGuidanceNewOrderNotice = 23,

        /// <summary>
        /// 用户端-服务消息-图文咨询未回复
        /// </summary>
        [Description("用户端-服务消息-图文咨询未回复")]
        UserPicUnrepliedNotice = 24,

        /// <summary>
        /// 用户端-服务消息-未候诊
        /// </summary>
        [Description("用户端-服务消息-未候诊")]
        UserRoomUnWaiting = 0105026,
    }


    [Description("发送消息的目标")]
    public enum EnumNoticeTarget
    {
        /// <summary>
        /// 所有用户
        /// </summary>
        [Description("所有用户")]
        All = 0,

        /// <summary>
        /// 发给指定用户(根据指定的用户ID)
        /// </summary>
        [Description("发给指定用户")]
        TargetUser = 3
    }

    /// <summary>
    /// 终端类型
    /// </summary>
    public enum EnumTerminalType
    {
        Web = 1,
        IOS = 2,
        Android = 3
    }

}
