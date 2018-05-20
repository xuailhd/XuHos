using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Cache.Keys
{
    /// <summary>
    /// 枚举出缓存Key的类型
    /// </summary>
    public enum StringCacheKeyType
    {
        #region 接口相关
        API_apptoken,
        API_usertoken,
        api_noncestr,
        #endregion

        #region 系统
        //系统访问账号
        SysAccessAccount,
        /// <summary>
        /// 系统文件索引
        /// </summary>
        SysFileIndex,
        /// <summary>
        /// 订单续费（去重复）
        /// </summary>
        SysDerep_OrderNewupgrade,
        /// <summary>
        /// 订单短信消息
        /// </summary>
        SysDerep_OrderSMS,
        /// <summary>
        /// 频道咨询消息（去重复）
        /// </summary>
        SysDerep_ChannelConsultContentMsg,
        /// <summary>
        /// 频道咨询消息（去重复）
        /// </summary>
        SysDerep_ChannelCallDrKangAnswerMsg,
        /// <summary>
        /// 处方购买消息
        /// </summary>
        SysDerep_RecipeBuyMsg,
        /// <summary>
        /// 用户等级规则
        /// </summary>
        Sys_UserLevelRules,

        /// <summary>
        /// 菜单功能权限
        /// </summary>
        Sys_SysModules,

        /// <summary>
        /// 家庭成员数量规则
        /// </summary>
        Sys_MemberCountRules,

        /// <summary>
        /// 有自己渠道 且 使用智慧药房药的 机构
        /// </summary>
        Sys_UseWisdomHos,
        /// <summary>
        /// 监控指标
        /// </summary>
        Sys_MonitorIndex,
        /// <summary>
        /// 药店机构
        /// </summary>
        Sys_DrugHos,

        Sys_TaskListResetTime,

        /// <summary>
        /// 系统消息通知扩展配置
        /// </summary>
        Sys_NoticeMessageExtrasConfig,

        /// <summary>
        /// 短信模板
        /// </summary>
        Sys_ShortMsgTemplates,

        /// <summary>
        /// 全国所有省，市，区等数据配制
        /// </summary>
        Sys_Regions,
        #endregion

        #region Order
        /// <summary>
        /// 订单信息
        /// </summary>
        Order,
        #endregion

        #region Channel

        /// <summary>
        /// 频道信息
        /// </summary>
        Channel,
        /// <summary>
        /// 频道成员信息
        /// </summary>
        Channel_Member,

        /// <summary>
        /// 康博士问诊状态
        /// </summary>
        Channel_DrKangState,

        /// <summary>
        /// 医生回复状态
        /// </summary>
        Channel_DoctorAnswerState,
        #endregion

        #region Hospital

        /// <summary>
        /// 医院信息
        /// </summary>
        Hospital,
        //医院工作时间
        Hospital_Worktime,
        #endregion

        #region 医生相关
        /// <summary>
        /// 医生信息
        /// </summary>
        Doctor,
        /// <summary>
        /// 医生免费义诊信息
        /// </summary>
        Doctor_FreeClinic,
        /// <summary>
        /// 医生服务价格
        /// </summary>
        Doctor_ServicePrice,
        /// <summary>
        /// 医生关注数量
        /// </summary>
        Doctor_FollowNum,
        /// <summary>
        /// 医生评价数量
        /// </summary>
        Doctor_EvaluationNum,
        /// <summary>
        /// 咨询数量
        /// </summary>
        Doctor_ConsultNum,
        /// <summary>
        /// 服务收入
        /// </summary>
        Doctor_ServiceIncome,
        /// <summary>
        /// 诊断数量
        /// </summary>
        Doctor_DiagnoseNum,
        /// <summary>
        /// 医生义诊断状态
        /// </summary>
        Doctor_FreeClinicState,
        /// <summary>
        /// 排班状态
        /// </summary>
        Doctor_ScheduleState,
        /// <summary>
        /// 服务总次数
        /// </summary>
        Doctor_ServiceNum,

        /// <summary>
        /// 医生排版的预约数量
        /// </summary>
        Doctor_ScheduleRegNum,

        /// <summary>
        /// 医生排版列表
        /// </summary>
        Doctor_ScheuleList,
        /// <summary>
        /// 医生配置
        /// </summary>
        Doctor_Configs,

        /// <summary>
        /// 医生的家庭医生签约信息
        /// </summary>
        Doctor_DoctorFamilyContract,

        /// <summary>
        /// 医生工作时间基础
        /// </summary>
        Doctor_WorkingTimeBase,

        /// <summary>
        /// 音视频看诊有效性时段
        /// </summary>
        Doctor_AudVidInquiryValidTime,

        /// <summary>
        /// 家庭医生服务包基础
        /// </summary>
        Doctor_FamilyDoctorPkgBase,

        /// <summary>
        /// 私人医生列表
        /// </summary>
        Doctor_PersonalList,

        /// <summary>
        /// 医生停诊取消排班
        /// </summary>
        Doctor_StopDiagnosisCancelSchedule,
        #endregion

        #region 用户相关
        /// <summary>
        /// 账号检查
        /// </summary>
        UserAccountCheck,
        /// <summary>
        /// 用户信息
        /// </summary>
        User,
        /// <summary>
        /// 注册所属机构编号
        /// </summary>
        User_OwnerOrgID,
        /// <summary>
        /// 用户应用机构编号
        /// </summary>
        User_AppOrgID,
        /// <summary>
        /// 用户角色列表
        /// </summary>
        User_RoleList,
        /// <summary>
        /// 用户信息
        /// </summary>
        User_Ticket,
        /// <summary>
        /// 用户成员信息
        /// </summary>
        User_Member,

        /// <summary>
        /// 账号登录失败次数
        /// </summary>
        User_AccountLoginFail,

        /// <summary>
        /// 用户
        /// </summary>
        User_Roles,

        /// <summary>
        /// 用户与OpenId
        /// </summary>
        User_OpenID,
        #endregion

        #region 短信相关
        VerifyCode,
        VerifySessionID,
        VerifyValue,
        SYS_SMSVerifyCode,
        #endregion

        BJCA_Token,

        #region  Heartbeat 心跳

        /// <summary>
        /// 心跳（WEB端，过期时间15秒）
        /// </summary>
        Heartbeat_Web,
        /// <summary>
        /// 在线状态(无过期时间下线后删除)
        /// </summary>
        Heartbeat_App,
        #endregion

        /// <summary>
        /// 抢单
        /// </summary>
        Grab,
        /// <summary>
        /// 短信去重复发送
        /// </summary>
        SMS_Duplicate,
        /// <summary>
        /// 160挂号
        /// </summary>
        Platform_JY160,

        /// <summary>
        /// km9000序列号
        /// </summary>
        HealthStation_KM9000s,

        /// <summary>
        /// 家庭医生平台 km9000序列号
        /// </summary>
        FamilyPlatform_KM9000s,

        KEY_IM_PriaveKey,
        KEY_IM_PublicKey,
        MAP_GetIMUidByDoctorID,
        MAP_GetIMUidByUserID,
        MAP_GetIMUidByMemberID,
        MAP_GetDoctorIDByUserID,
        MAP_GetUserIDByMobile,
        MAP_GetUserIDByCreateUserID,
        MAP_GetUserIDByCreateUserIDAndOrgID,
        MAP_GetUserIDByCreateUserIDAndOrgIDAndMobile,
        MAP_GetChannelIDByServiceID,
        MAP_GetOrderNoByOrderOutID,
        /// <summary>
        /// 区域
        /// </summary>
        Region,

        Drugstore_ImportDrug,
        /// <summary>
        /// 药店坐诊医生
        /// </summary>
        Doctor_DrugList,

        /// <summary>
        /// 医生工作时段
        /// </summary>
        Doctor_WorkingTime,

        /// <summary>
        /// 医生或患者的进入诊室状态
        /// </summary>
        Channel_EnteredState,
    }
}
