using System;
using System.Web.Http;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using XuHos.Common;
using XuHos.Common.Utility;
using XuHos.Common.Enum;
using XuHos.Entity;
using XuHos.BLL;
using XuHos.BLL.Common;
using XuHos.BLL.Common.DTOs;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.DTO.Platform;
using XuHos.DTO;
using XuHos.Extensions;
using XuHos.Service.Infrastructure.Filters;
using XuHos.Common.Cache;
using XuHos.Extensions;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.Doctor.DTOs.Request;

namespace XuHos.WebApi.Controllers
{
    /// <summary>
    /// 腾讯云
    /// </summary>
    public class IMController : ApiBaseController
    {
        /**
         * @api {GET} /IM/Config 117001/获取云通信配置
         * @apiGroup obsolete
         * @apiVersion 4.0.0
         * @apiDescription 获取云通信独立认证配置 
         * @apiPermission 已登录
         * @apiHeader {String} apptoken appToken
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {Array} Data 配置信息
         * @apiSuccessExample {json} 返回样例:
         *{"Data":{
            "sdkAppID":1400009922,
            "userSig":"eJx1jktTgzAURvf9FQxbHQcCSYg7ykuqVdFW1A1DIUhaHjEElHH871qmM7Lxbs*Z*52vhaIo6ubm8SLNsrZvZCJHTlXlUlEh0IF6-sc5Z3mSysQQ*cR1U-s9QsDcop*cCZqkhaRisix01GYGy2kjWcFOnNimYxkEIQdbpuvCpefbyMIIQaLbAM9-d-khmSr*n*-Y2wTX3osTRq7hlbdtP5Bqt6rqYPuxK3j5ui39O9qvARmcbj8GeXxVCxGFpX0ftQPizfs1Jv5mhMi3xrA*LB8qR0YrMIg8jquns*cg2*PZpGQ1PQVhDQMLaXBGByo61jaTADQd6sA4Zmvq4nvxAyg3ZbU_",
            "identifier":"123",
            "accountType":5212
            },"Total":0,"Status":0,"Msg":""}             
         **/
        /// <summary>
        /// 获取当前登录用户腾讯云通信配置
        /// 前置条件：已登录
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UserAuthenticate(IsValidUserType = false)]
        [Obsolete("已移至共用接口基础层IM/Config")]
        public ApiResult Config()
        {


            var bll = new XuHos.Integration.QQCloudy.IMHelper();
            return bll.Config(CurrentOperatorUserIdentifier).ToApiResultForObject();


        }

        /**
        * @api {GET} /IM/MediaConfig 117002/获取多媒体配置
        * @apiGroup obsolete
        * @apiVersion 4.0.0
        * @apiDescription 获取多媒体配置（视频直播、录制需要） 
        * @apiPermission 已登录
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
        * @apiParam {String} ChannelID 房间编号 
        * @apiParam {int} Identifier 当前用户唯一标识（通过IM/Config接口获取）
        * @apiParamExample {json} 请求样例：
        *                   ?ChannelID=XXX&Identifier=1
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录数
        * @apiSuccess (Response) {Array} Data 配置信息
        * @apiSuccessExample {json} 返回样例:
        *{"Data":{
           "MediaChannelKey":"XXXX",//进入视频是使用的动态秘钥
           "RecordingKey":"XXXX",
           "Secret":"XXX", //进入视频房间时的动态密码
            "Duration":120,//服务时长
            "TotalTime":10,//已消耗
           },"Total":0,"Status":0,"Msg":""}             
        **/
        /// <summary>
        /// 获取多媒体配置（视频直播、视频录制）
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="Identifier"></param>
        /// <returns></returns>
        [HttpGet]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult MediaConfig(int ChannelID, int Identifier)
        {
            var bll = new XuHos.Integration.QQCloudy.IMHelper();
            BLL.Sys.Implements.ConversationRoomService roomService = new BLL.Sys.Implements.ConversationRoomService();
            var room = roomService.GetChannelInfo(ChannelID);
         
            //获取服务时长
            var Duration = room.Duration;
            var TotalTime = room.TotalTime;
           
            //如果没有服务时长，则默认30分钟
            if (Duration <=0)
            {
                Duration = 60 * 30; //30分钟
            }

            var config = bll.MediaConfig(ChannelID, Identifier, Duration, TotalTime,room.DisableWebSdkInteroperability);
            return config.ToApiResultForObject();
            
        }

     

        /// <summary>
        /// IM117003
        /// 腾讯云通信回调函数
        /// </summary>
        /// <param name="SdkAppid">APP在云通讯申请的Appid。</param>
        /// <param name="CallbackCommand"></param>
        /// <param name="contenttype">固定为：json。</param>
        /// <param name="ClientIP">客户端IP地址。</param>
        /// <param name="OptPlatform">RESTAPI（使用REST API发送请求）、Web（使用Web SDK发送请求）、Android、iOS、Windows、Mac、Unkown（使用未知类型的设备发送请求）。</param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreUserAuthenticate]
        [IgnoreAuthenticate]
        public dynamic CallBack(
            string SdkAppid,
            string CallbackCommand,
            string ClientIP,
            string OptPlatform,
            [FromBody]dynamic model)
        {

            try
            {
                //发单聊消息之前回调 https://www.qcloud.com/doc/product/269/1632
                if (CallbackCommand == "C2C.CallbackBeforeSendMsg")
                {
                    /*{
                        "CallbackCommand": "C2C.CallbackBeforeSendMsg",  // 回调命令
                        "From_Account": "jared",  // 发送者
                        "To_Account": "Jonh",  // 接收者
                        "MsgBody": [  // 消息体，参见TIMMessage消息对象
                            {
                                "MsgType": "TIMTextElem", // 文本
                                "MsgContent": {
                                    "Text": "red packet"
                                }
                            }
                        ]
                    }*/

                    //TODO:过滤垃圾消息，修改用户的消息，记录日志
                }
                //发单聊消息之后回调 https://www.qcloud.com/doc/product/269/2716
                else if (CallbackCommand == "C2C.CallbackAfterSendMsg")
                {

                    /*{
                        "CallbackCommand": "C2C.CallbackAfterSendMsg",  // 回调命令
                        "From_Account": "jared",  // 发送者
                        "To_Account": "Jonh",  // 接收者
                        "MsgBody": [  // 消息体，参见TIMMessage消息对象
                            {
                                "MsgType": "TIMTextElem", // 文本
                                "MsgContent": {
                                    "Text": "red packet"
                                }
                            }
                        ]
                    }*/

                }
                //群内发言之前回调 https://www.qcloud.com/doc/product/269/1619
                else if (CallbackCommand == "Group.CallbackBeforeSendMsg")
                {

                    #region 群内发言之前回调

                    /*{
                        "CallbackCommand": "Group.CallbackBeforeSendMsg",  // 回调命令
                        "GroupId": "@TGS#2J4SZEAEL",  // 群组ID
                        "Type": "Public",  // 群组类型
                        "From_Account": "jared",  // 发送者
                        "Operator_Account":"admin", //请求的发起者
                        "Random": 123456,  // 随机数
                        "MsgBody": [  // 消息体，参见TIMMessage消息对象
                            {
                                "MsgType": "TIMTextElem", // 文本
                                "MsgContent": {
                                    "Text": "red packet"
                                }
                            }
                        ]
                    }*/

                    //群组编号
                    int GroupId = model.GroupId;
                    //房间编号
                    int ChannelID = GroupId;
                    //发送者
                    string From_Account = model.From_Account;
                    //消息主题内容
                    dynamic MsgBody = model.MsgBody;

                    string MsgSeq = Guid.NewGuid().ToString("N");

                    if (From_Account != null)
                    {
                        ConversationMessage[] messages = new ConversationMessage[MsgBody.Count];
                        BLL.Sys.Implements.ConversationRoomService bllRoom = new BLL.Sys.Implements.ConversationRoomService();
                        //获取房间
                        var room = bllRoom.GetChannelInfo(ChannelID);

                        if (room != null)
                        {
                            
                          
                                #region 记录聊天日志

                                int i = 0;
                                foreach (var msg in MsgBody)
                                {
                                    var msgType = msg.MsgType;
                                    var content = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                                    messages[i] = new ConversationMessage();
                                    messages[i].ConversationMessageID = Guid.NewGuid().ToString("N");
                                    messages[i].ConversationRoomID = GroupId;//房间
                                    messages[i].MessageContent = content;//内容
                                    messages[i].MessageState = 0;//消息状态
                                    messages[i].MessageTime = DateTime.Now;//消息发送时间
                                    messages[i].MessageType = msgType;//消息发送类型 TIMTextElem/TIMImageElem/TIMCustomElem
                                    messages[i].ServiceID = room.ServiceID;
                                    messages[i].UserID = From_Account.ToString();
                                    messages[i].MessageSeq = MsgSeq;
                                    messages[i].MessageIndex = i;
                                    i++;
                                }

                                using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                                {

                                    if (mqChannel.Publish<EventBus.Events.ChannelNewMsgEvent>(new EventBus.Events.ChannelNewMsgEvent()
                                    {
                                        ChannelID = GroupId,
                                        ServiceID = room.ServiceID,
                                        ServiceType = room.ServiceType,
                                        FromAccount = From_Account,
                                        Messages = messages,
                                        OptPlatform = OptPlatform
                                    }))
                                    {
                                        // 0为允许发言
                                        return new { ActionStatus = "OK", ErrorInfo = "", ErrorCode = 0 };
                                    }
                                    else
                                    {
                                        // 0为允许发言
                                        return new { ActionStatus = "OK", ErrorInfo = "", ErrorCode = 1 };
                                    }

                                }

                                #endregion
                         
                        }
                    }

                    #endregion
                }
                //群内发言之后回调 https://www.qcloud.com/doc/product/269/2661
                else if (CallbackCommand == "Group.CallbackAfterSendMsg")
                {
                    /*{
                        "CallbackCommand": "Group.CallbackAfterSendMsg",  // 回调命令
                        "GroupId": "@TGS#2J4SZEAEL",  // 群组ID
                        "Type": "Public",  // 群组类型
                        "From_Account": "jared",  // 发送者
                        "MsgBody": [  // 消息体，参见TIMMessage消息对象
                            {
                                "MsgType": "TIMTextElem", // 文本
                                "MsgContent": {
                                    "Text": "red packet"
                                }
                            }
                        ]
                    }*/

                }
                //状态变更回调 https://www.qcloud.com/doc/product/269/2570
                else if (CallbackCommand == "State.StateChange")
                {
                    /*{
                        "CallbackCommand": "State.StateChange",
                            "Info": {
                                "Action": "Logout",
                                "To_Account": "testuser316",
                                "Reason": "Unregister"
                            }
                    }
                    */
                    var Action = model.Info.Action;
                    var To_Account = model.Info.To_Account;

                    using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                    {
                        if (mqChannel.Publish<EventBus.Events.UserOnlineStateChangedEvent>(new EventBus.Events.UserOnlineStateChangedEvent()
                        {
                            Action = Action, 
                            UserID= To_Account,

                        }))
                        {
                            // 0为允许发言
                            return new { ActionStatus = "OK", ErrorInfo = "", ErrorCode = 0 };
                        }
                        else
                        {
                            // 0为允许发言
                            return new { ActionStatus = "OK", ErrorInfo = "", ErrorCode = 1 };
                        }

                    }
                }

                //正常返回
                return new { ActionStatus = "OK", ErrorInfo = "", ErrorCode = 0 };
            }
            catch (Exception E)
            {
                LogHelper.WriteError(E.GetDetailException());

                return new { ActionStatus = "OK", ErrorInfo =E.GetDetailException().StackTrace, ErrorCode = 0 };
            }
        }


        /**
           * @api {Post} /IM/Users 117004/获取人员信息
           * @apiGroup obsolete
           * @apiVersion 4.0.0
           * @apiDescription 通过用户标识获取用户的信息 
           * @apiPermission 已登录（用户）
           * @apiHeader {String} apptoken appToken
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
           * @apiParam {Array} Identifiers 用户标识 
           * @apiParam {int} ChannelID 房间编号 
           * @apiParamExample {json} 请求样例：
           {"Identifiers":["45","110","1171"],"ChannelID":"10237"}
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {int} Data 返回人员信息
           * @apiSuccessExample {json} 返回样例:
           *{"Data":[{"UserID":"53C506B292B248FFB6EF1370FD6533D1","UserCNName":"陈松深","UserENName":"13","UserType":2,"_PhotoUrl":"images/cd9f0f48f9389f38337420396ad1b849","PhotoUrl":"http://121.15.153.63:8028/images/cd9f0f48f9389f38337420396ad1b849","Score":0,"Star":0,"Comment":0,"Good":0,"Fans":0,"Grade":0,"Checked":0,"RegTime":"0001-01-01T00:00:00","CancelTime":"0001-01-01T00:00:00","UserState":0,"UserLevel":0,"LastTime":"0001-01-01T00:00:00","identifier":45},{"UserID":"B04D4AE28F994AE2AACBB456D7E0647B","UserCNName":"邱浩强","UserENName":"3","UserType":2,"_PhotoUrl":"images/b427cae4799bf5387eadfc9d7e627e2e","PhotoUrl":"http://121.15.153.63:8028/images/b427cae4799bf5387eadfc9d7e627e2e","Score":0,"Star":0,"Comment":0,"Good":0,"Fans":0,"Grade":0,"Checked":0,"RegTime":"0001-01-01T00:00:00","CancelTime":"0001-01-01T00:00:00","UserState":0,"UserLevel":0,"LastTime":"0001-01-01T00:00:00","identifier":110},{"UserID":"165fe38c332f4dfea7e46d1fe0592e98","UserCNName":"tang","UserENName":"tang","UserType":1,"PhotoUrl":"http://121.15.153.63:8028/images/doctor/unknow.png","Score":0,"Star":0,"Comment":0,"Good":0,"Fans":0,"Grade":0,"Checked":0,"RegTime":"0001-01-01T00:00:00","CancelTime":"0001-01-01T00:00:00","UserState":0,"UserLevel":0,"LastTime":"0001-01-01T00:00:00","identifier":1171}],"Total":0,"Status":0,"Msg":"操作成功"}
       **/
        /// <summary>
        /// 获取多个用户的信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("~/IM/Users")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetUsersInfo(BLL.Sys.DTOs.Request.RequestGetMemberInfoDTO request)
        {

            var bll = new BLL.Sys.Implements.ConversationRoomService();
            return bll.GetChannelUsersInfo(request.ChannelID, request.Identifiers).ToApiResultForObject();

        }




        /**
           * @api {GET} /IM/Room/WaitingCount 117005/获取候诊人数
           * @apiGroup obsolete
           * @apiVersion 4.0.0
           * @apiDescription 获取候诊人数 
           * @apiPermission 已登录（用户）
           * @apiHeader {String} apptoken appToken
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
           * @apiParam {String} DoctorID 医生编号 
           * @apiParam {int} ChannelID 房间编号 
           * @apiParamExample {json} 请求样例：
           * ?DoctorID=XXX&OPDRegisterID=XXXX
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {int} Data 候诊人数
           * @apiSuccessExample {json} 返回样例:
           *{"Data":15,"Total":2,"Status":0,"Msg":""}
       **/
        /// <summary>
        /// 用户查询候诊队列
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <param name="DoctorID">医生编号</param>
        /// <param name="ChannelID">房间编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/IM/Room/WaitingCount")]
        [UserAuthenticate(UserType = EnumUserType.User)]
        public ApiResult GetRoomWaitingCount(string DoctorID, int ChannelID)
        {

           var bll = new BLL.Sys.Implements.ConversationRoomService();
            return bll.GetWaitingCount(DoctorID, ChannelID).ToApiResultForObject();


        }


        /**
           * @api {GET} /IM/Room/State 117006/获取房间状态
           * @apiGroup obsolete
           * @apiVersion 4.0.0
           * @apiDescription 获取房间状态 
           * @apiPermission 已登录(用户/主治医生/分诊医生)
           * @apiHeader {String} apptoken appToken
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
           * @apiParam {int} ChannelID 房间编号 
           * @apiParamExample {json} 请求样例：
           * {ChannelID:"XXX",State:2}
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {int} Data 预约状态  0=未就诊,1=候诊中,2=就诊中,3=已就诊,4=呼叫中,5=离开中
           * @apiSuccessExample {json} 返回样例:
           *{"Data":1,"Total":2,"Status":0,"Msg":""}
       **/
        /// <summary>
        /// 获取房间状态
        /// 前置条件：已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/IM/Room/State")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetRoomState(int ChannelID)
        {
            var bll = new BLL.Sys.Implements.ConversationRoomService();
            var entity = bll.GetChannelInfo(ChannelID);
            if (entity != null)
            {

                //前端用户只需要知道有候诊状态即可，不需要知道有重复候诊的状态
                if (entity.RoomState == EnumRoomState.WaitAgain)
                    entity.RoomState = EnumRoomState.Waiting;

                return entity.RoomState.ToApiResultForObject();
            }
            else
                return EnumApiStatus.BizError.ToApiResultForApiStatus("房间不存在");


        }

        /**
           * @api {GET} /IM/Room?ChannelID=:ChannelID 117007/获取房间信息
           * @apiGroup obsolete
           * @apiVersion 4.0.0
           * @apiDescription 获取预约详情 
           * @apiPermission 已登录（用户/医生/分诊医生）
           * @apiHeader {String} apptoken appToken
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
           * @apiParam {int} ChannelID 房间编号 
           * @apiParamExample {json} 请求样例：
           * ?ChannelID=XXXX
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {int} Data 预约信息
           * @apiSuccessExample {json} 返回样例:
           *{
                "Data": {
                    "ConversationRoomID": "650de17bbd184c1fae87b1bb5386ceb7",
                    "ServiceID": "8f8c435e09e24ce2af61d92fa2ff422e",
                    "ChannelID": 9,
                    "Secret": "593c3fe883234f9ea2e7d6b83ad0c7c8",
                    "RoomState": 2,
                    "BeginTime": "2016-08-18T13:42:35.8",
                    "EndTime": "2016-08-18T09:00:57.6001987",
                    "TotalTime": 0
                },
                "Total": 0,
                "Status": 0,
                "Msg": "操作成功"
            }
       **/
        /// <summary>
        /// 获取房间详情
        /// 前置条件：用户已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="ChannelID">房间编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/IM/Room")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetRoomInfo([FromUri]int ChannelID)
        {
            var bll = new BLL.Sys.Implements.ConversationRoomService();
            var room = bll.GetChannelInfo(ChannelID);

            //前端用户只需要知道有候诊状态即可，不需要知道有重复候诊的状态
            if (room.RoomState == EnumRoomState.WaitAgain)
                room.RoomState = EnumRoomState.Waiting;


            return room.ToApiResultForObject();

        }


        /**
           * @api {PUT} /IM/Room/State 117008/修改房间状态
           * @apiGroup obsolete
           * @apiVersion 4.0.0
           * @apiDescription 查询用户预约的历史记录
           * @apiPermission 已登录（用户/主治医生）
           * @apiHeader {String} apptoken appToken
           * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
           * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
           * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
           * @apiParam {int} ChannelID 房间编号 
           * @apiParam {int} State 新的状态  0=未就诊,1=候诊中,2=就诊中,3=已就诊,4=呼叫中,5=离开中
           * @apiParam {int} ExpectedState 当前状态 0=未就诊,1=候诊中,2=就诊中,3=已就诊,4=呼叫中,5=离开中
           * @apiParamExample {json} 请求样例：
           * {
           *    ChannelID:"XXX",
           *    State:2,
           *    ExpectedState:1,
           *    DisableWebSdkInteroperability:true //AgoraSDK1.12及以上版本设置True(以便于WEB端启用高版本)
           * }
           * @apiSuccess (Response) {String} Msg 提示信息 
           * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
           * @apiSuccess (Response) {int} Total 总记录数
           * @apiSuccess (Response) {bool} Data 是否成功
           * @apiSuccessExample {json} 返回样例:
           *{"Data":true,"Total":2,"Status":0,"Msg":""}
       **/
        /// <summary>
        /// 更新房间状态
        /// 前置条件：已登录
        
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("~/IM/Room/State")]
        [UserAuthenticate(IsValidUserType = false)]
        [Obsolete("已移至共用接口基础层IM/Room/State")]
        public ApiResult SetRoomState([FromBody]BLL.Sys.DTOs.Request.RequestConversationRoomSetStateDTO request)
        {
            try
            {
                var roomService = new BLL.Sys.Implements.ConversationRoomService();
                var room = roomService.GetChannelInfo(request.ChannelID);         
             
                #region 记录操作日志
                    EnumUserOperationType? type = null;
                    switch (room.ServiceType)
                    {
                        case EnumDoctorServiceType.VidServiceType:
                            type = EnumUserOperationType.EnterVidService;
                            break;
                        case EnumDoctorServiceType.AudServiceType:
                            type = EnumUserOperationType.EnterAudService;
                            break;
                        case EnumDoctorServiceType.PicServiceType:
                            type = EnumUserOperationType.EnterPicService;
                            break;
                    }
                    switch (request.State)
                    {
                        case EnumRoomState.InMedicalTreatment:
                            break;
                        case EnumRoomState.Disconnection:
                            if (type.HasValue)
                            {
                                type = type.Value + 1;
                            }
                            break;
                        default:
                            type = null;
                            break;
                    }
                    if (type.HasValue)
                    {
                        using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
                        {
                            channel.Publish<XuHos.EventBus.Events.UserOperatorLogEvent>(new EventBus.Events.UserOperatorLogEvent()
                            {
                                UserID = CurrentOperatorUserID,
                                OperatorTime = DateTime.Now,
                                UserType = CurrentOperatorUserType,
                                OperatorType = type.Value,
                                OperatorName = type.Value.GetEnumDescript(),
                                OrgID = CurrentOperatorOrgID,
                                Remark = "",
                                ModuleName = ""
                            });
                        }
                    }
                #endregion

                //期望的房间状态
                var ExpectedState = request.ExpectedState.HasValue ? request.ExpectedState.Value: room.RoomState ;//如果客户端没有指定期望的状态则系统获取（兼容之前版本）

                //并集并交换房间状态（如果设置失败则返回最新的状态）
                var ret = roomService.CompareAndSetChannelState(request.ChannelID,
                    CurrentOperatorUserID,
                    request.State,
                    request.DisableWebSdkInteroperability,
                    ref ExpectedState);

                return ret.ToApiResultForApiStatus(ExpectedState);
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return EnumApiStatus.BizError.ToApiResultForApiStatus(ex.Message);
            }
        }

        /**
         * @api {GET} /IM/Messages 117009/获取消息记录
         * @apiGroup obsolete
         * @apiVersion 4.0.0
         * @apiDescription 获取消息记录
         * @apiPermission 已登录（用户/主治医生）
         * @apiHeader {String} apptoken appToken
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写        
         * @apiParam {int} ChannelID 房间编号 
         * @apiParam {int} CurrentPage=1 当前页码 
         * @apiParam {int} PageSize=10 分页大小
         * @apiParamExample {json} 请求样例：
         * ?ChannelID=7&CurrentPage=1&PageSize=10
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {bool} Data 是否成功
         * @apiSuccessExample {json} 返回样例:
         *
         * {"Data":[{"MsgBody":["{\"MsgContent\":{\"Text\":\"111111111111111\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"2c5f2a8d267a4a428b49a3ac297c0ca9","MsgTime":"2016-10-10T10:34:54.75","FromAccount":"96","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Data\":\"{\\\"ChannelID\\\":2730,\\\"State\\\":2,\\\"ServiceID\\\":\\\"77e5761ed73a4745adf372a5ce795730\\\",\\\"ServiceType\\\":1}\",\"Desc\":\"\",\"Ext\":\"Room.StateChanged\"},\"MsgType\":\"TIMCustomElem\"}"],"MsgSeq":"e21b710d51ee4d8ca46c80e3d1ce44c1","MsgTime":"2016-10-10T10:35:04.63","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"1\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"c0c6cc0342f94812af74348a694bf515","MsgTime":"2016-10-10T10:54:48.97","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"2\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"436d930db5044afa8fad50365c95e40a","MsgTime":"2016-10-10T10:54:50.047","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"3\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"9f37846f36e645d6841f6116da98b9b5","MsgTime":"2016-10-10T10:54:50.36","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"3\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"a7dc34ee38b245c79707ea6b7133f9e9","MsgTime":"2016-10-10T10:54:50.75","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"3\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"cd9f624a268b4e17bd3ec0038df5f8ea","MsgTime":"2016-10-10T10:54:50.93","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"3\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"28c7f7e85c684a5baabd56c8f69b7fcb","MsgTime":"2016-10-10T10:54:51.163","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"3\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"9b6d592f69a64050a4e315c026d6fffe","MsgTime":"2016-10-10T10:54:51.45","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"3\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"9a8230366885475d913632e20bdb98a2","MsgTime":"2016-10-10T10:54:51.793","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"3\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"2ab8cd4c3a9647f9ad644a569d3a76d2","MsgTime":"2016-10-10T10:54:51.993","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"6\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"bdfe9d482502483ea332041fa58f2baf","MsgTime":"2016-10-10T10:54:53.283","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Data\":\"[撇嘴]\",\"Index\":2},\"MsgType\":\"TIMFaceElem\"}"],"MsgSeq":"63906a76488c4f16b43456f475ab7774","MsgTime":"2016-10-10T13:31:14.81","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"ImageFormat\":255,\"ImageInfoArray\":[{\"Height\":1993,\"Size\":877884,\"Type\":1,\"URL\":\"http://api.kmwlyy.com/store//images\\\\2016\\\\09\\\\29\\\\User\\\\9A4C83966C784DD5BEFA68766591A272\\\\5dfadebddf2e4802989526203bfd72e0.jpg\",\"Width\":1920},{\"Height\":0,\"Size\":0,\"Type\":2,\"URL\":\"http://api.kmwlyy.com/store//images\\\\2016\\\\09\\\\29\\\\User\\\\9A4C83966C784DD5BEFA68766591A272\\\\5dfadebddf2e4802989526203bfd72e0.jpg\",\"Width\":0},{\"Height\":0,\"Size\":877884,\"Type\":3,\"URL\":\"http://api.kmwlyy.com/store//images\\\\2016\\\\09\\\\29\\\\User\\\\9A4C83966C784DD5BEFA68766591A272\\\\5dfadebddf2e4802989526203bfd72e0.jpg\",\"Width\":0}],\"UUID\":\"426b02d0666abab72129539fad233b0a\"},\"MsgType\":\"TIMImageElem\"}"],"MsgSeq":"399ec0dfd2db4fa4bb686c6571d6b866","MsgTime":"2016-10-10T14:31:38.99","FromAccount":"110","ToGroupId":"2730"},{"MsgBody":["{\"MsgContent\":{\"Text\":\"1\"},\"MsgType\":\"TIMTextElem\"}"],"MsgSeq":"1c6099315ead489992b687566df945cd","MsgTime":"2016-10-10T14:39:40.827","FromAccount":"110","ToGroupId":"2730"}],"Total":24,"Status":0,"Msg":"操作成功"}
        **/
        /// <summary>
        /// 获取聊天记录
        /// 前置条件：无
                
        /// 日期：2016年8月4日
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/IM/Messages")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult GetMessages([FromUri]BLL.Sys.DTOs.Request.RequestGetConversationMessageDTO request)
        {
            ConversationMessageService bll = new ConversationMessageService(CurrentOperatorUserID);
            return bll.GetMessages(request.ChannelID,
                request.CurrentPage,
                request.PageSize
                ).ToApiResultForList();

        }

        /// <summary>
        /// IM:117010
        /// 录制完成
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [Route("~/IM/Recording/Done")]
        public ApiResult RecordingDone(BLL.Sys.DTOs.Request.RequestRecordingDoneDTO request)
        {
            XuHos.Common.LogHelper.WriteDebug("Recording/Done channelID=" + request.channelId + ",fileName=" + request.fileName);

            ConversationRecordingService bll = new ConversationRecordingService(CurrentOperatorUserID);

            return bll.Insert(new ConversationRecording()
            {
                ChannelID = request.channelId,
                FileID = Guid.NewGuid().ToString("N"),
                FileURL = request.fileName,
                CreateTime = DateTime.Now,
                IsDeleted = false

            }).ToApiResultForObject();

        }


        /**
        * @api {POST} /IM/SendFileMessage 117011/发送文件消息
        * @apiGroup obsolete
        * @apiVersion 4.0.0
        * @apiDescription 发送文件消息 
        * @apiPermission 已登录
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
         * @apiParamExample {json} 请求样例：
         * {ChanndlID:1,FileMD5:"xxxx"}
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {int} Total 总记录数
        * @apiSuccess (Response) {Array} Data 配置信息
        * @apiSuccessExample {json} 返回样例:
        *{"Data":true,"Total":0,"Status":0,"Msg":""}             
        **/
        /// <summary>
        /// 发送文件消息
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("~/IM/SendFileMessage")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult SendFileMessage(BLL.Sys.DTOs.Request.RequestSendFileDTO request)
        {
            var bll = new XuHos.Integration.QQCloudy.IMHelper();
            SysFileIndexService fileIndexService = new SysFileIndexService(CurrentOperatorUserID);
            var file = fileIndexService.Single<SysFileIndex>(a => a.MD5 == request.FileMD5);

            using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
            {
                return channel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>>(new EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>()
                {
                    ChannelID = request.ChannelID,
                    FromAccount = CurrentOperatorUserIdentifier,
                    Msg = new ResponseUserFileDTO()
                    {
                        FileUrl = $"{file.FileType}/{file.FileUrl}",
                        FileID = file.MD5,
                        FileName = file.Remark,
                        FileType = 1,
                        OutID = "",
                        FileSize = file.FileSize,
                        Remark = file.Remark
                    }

                }).ToApiResultForBoolean();
            }

        }

        /**
          * @api {POST} /IM/SendAudioMessage 117012/发送音频消息
          * @apiGroup obsolete
          * @apiVersion 4.0.0
          * @apiDescription 发送图片消息 
          * @apiPermission 已登录
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
           * @apiParamExample {json} 请求样例：
           * {ChanndlID:1,FileMD5:"xxxx"}
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {int} Total 总记录数
          * @apiSuccess (Response) {Array} Data 配置信息
          * @apiSuccessExample {json} 返回样例:
          *{"Data":true,"Total":0,"Status":0,"Msg":""}             
          **/
        [HttpPost]
        [Route("~/IM/SendAudioMessage")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult SendAudioMessage(BLL.Sys.DTOs.Request.RequestSendAudioDTO request)
        {
            var fileIndexService = new SysFileIndexService(CurrentOperatorUserID);
            var fileIndex = fileIndexService.Single<SysFileIndex>(a => a.MD5 == request.FileMD5);

            if (fileIndex != null)
            {
                using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
                {
                    return channel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>>(new EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>()
                    {
                        ChannelID = request.ChannelID,
                        FromAccount = CurrentOperatorUserIdentifier,
                        Msg = new ResponseUserFileDTO()
                        {
                            FileUrl = $"{fileIndex.FileType}/{fileIndex.FileUrl}",
                            FileID = fileIndex.MD5,
                            FileName = fileIndex.FileUrl,
                            FileType = 2,
                            OutID = "",
                            Remark = ""
                        }

                    }).ToApiResultForBoolean();
                }
            }
            else
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            }

        }

        /**
       * @api {POST} /IM/SendImageMessage 117013/发送图片消息
       * @apiGroup obsolete
       * @apiVersion 4.0.0
       * @apiDescription 发送图片消息 
       * @apiPermission 已登录
       * @apiHeader {String} apptoken appToken
       * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
       * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
       * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
        * @apiParamExample {json} 请求样例：
        * {ChanndlID:1,FileMD5:"xxxx"}
       * @apiSuccess (Response) {String} Msg 提示信息 
       * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
       * @apiSuccess (Response) {int} Total 总记录数
       * @apiSuccess (Response) {Array} Data 配置信息
       * @apiSuccessExample {json} 返回样例:
       *{"Data":true,"Total":0,"Status":0,"Msg":""}             
       **/
        /// <summary>
        /// 发送图片消息
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("~/IM/SendImageMessage")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult SendImageMessage(BLL.Sys.DTOs.Request.RequestSendImageDTO request)
        {
            XuHos.Integration.QQCloudy.IMHelper bll = new XuHos.Integration.QQCloudy.IMHelper();
            SysFileIndexService fileIndexService = new SysFileIndexService(CurrentOperatorUserID);
            var file = fileIndexService.Single<SysFileIndex>(a => a.MD5 == request.FileMD5);

            if (CurrentOperatorUserIdentifier <= 0)
            {
                throw new ArgumentException($"user {CurrentOperatorUserID} Identifier<=0");
            }

            using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
            {
                return channel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>>(new EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>()
                {
                    ChannelID = request.ChannelID,
                    FromAccount = CurrentOperatorUserIdentifier,
                    Msg = new ResponseUserFileDTO()
                    {
                        FileUrl =$"{file.FileType}/{file.FileUrl}",
                        FileID = file.MD5,
                        FileName = file.FileUrl,
                        FileType = 0,
                        OutID = "",
                        Remark = ""
                    }

                }).ToApiResultForBoolean();
            }
        }

        /**
         * @api {POST} /IM/SendTextMessage 117014/发送文本消息
         * @apiGroup obsolete
         * @apiVersion 4.0.0
         * @apiDescription 发送文本消息 
         * @apiPermission 已登录
         * @apiHeader {String} apptoken appToken
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
          * @apiParamExample {json} 请求样例：
          * {ChanndlID:1,Content:"消息内容"}
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {Array} Data 配置信息
         * @apiSuccessExample {json} 返回样例:
         *{"Data":true,"Total":0,"Status":0,"Msg":""}             
        **/
        /// <summary>
        /// 发送文本消息
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("~/IM/SendTextMessage")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult SendTextMessage(BLL.Sys.DTOs.Request.RequestSendTextDTO request)
        {
            using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
            {
                return channel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<string>>(new EventBus.Events.ChannelSendGroupMsgEvent<string>()
                {

                    ChannelID = request.ChanndlID,
                    FromAccount = CurrentOperatorUserIdentifier,
                    Msg = request.Content

                }).ToApiResultForBoolean();
            }
        }

        /**
         * @api {POST} /IM/SendEnterRoomNotify 117015 发送进入诊室通知
         * @apiGroup obsolete
         * @apiVersion 4.0.0
         * @apiDescription 发送进入诊室通知 作者：沈腾飞
         * @apiPermission 已登录
         * @apiHeader {String} apptoken appToken
         * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
         * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
         * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写   
         * @apiParamExample {json} 请求样例：
         * ?channelID=XXXX
         * @apiSuccess (Response) {String} Msg 提示信息 
         * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
         * @apiSuccess (Response) {int} Total 总记录数
         * @apiSuccess (Response) {Array} Data 配置信息
         * @apiSuccessExample {json} 返回样例:
         * { "Data": true, "Total": 0, "Status": 0, "Msg": "" }             
        **/
        /// <summary>
        /// 发送进入诊室通知
        /// </summary>
        [HttpPost]
        [Route("~/IM/SendEnterRoomNotify")]
        [UserAuthenticate(IsValidUserType = false)]
        public ApiResult SendEnterRoomNotify([FromBody]RequestSendEnterRoomNotifyDTO request)
        {
            using (XuHos.EventBus.MQChannel channel = new EventBus.MQChannel())
            {
                return channel.Publish<EventBus.Events.ChannelEnteredEvent>(new EventBus.Events.ChannelEnteredEvent()
                {
                    ChannelID = request.ChannelID,
                    UserID = CurrentOperatorUserID
                    
                }, 0, 600000).ToApiResultForBoolean();
            }
        }
    }
}

