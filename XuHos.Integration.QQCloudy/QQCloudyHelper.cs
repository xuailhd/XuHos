using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using XuHos.Extensions;
using XuHos.Common.Utility;
using XuHos.Common.Cache;
using XuHos.Extensions;
using XuHos.Common;
using XuHos.DTO.Platform;
using XuHos.Common.Enum;
using XuHos.DTO;
using System.Data.Entity;
using System.Linq;
using XuHos.Common.Log.ApiTrack;
using XuHos.DTO.Common;
using XuHos.Integration.QQCloudy;


namespace XuHos.Integration.QQCloudy
{
    /// <summary>
    /// 云通信服务端请求返回结果
    /// </summary>
    public class ResponseIMResultDTO
    {
        public string ActionStatus
        { get; set; }


        public string ErrorInfo
        { get; set; }

        public int ErrorCode
        { get; set; }
    }

    public class ResponseIMQueryResultDTO
    {
        public string ActionStatus
        { get; set; }


        public string ErrorInfo
        { get; set; }

        public int ErrorCode
        { get; set; }

        public List<System.Collections.Generic.Dictionary<string, string>> QueryResult = new List<System.Collections.Generic.Dictionary<string, string>>();
    }

    /// <summary>
    /// 云通信配置
    /// </summary>
    public class ResponseIMConfigDTO
    {
        public uint sdkAppID { get; set; }
        public string userSig { get; set; }
        public string identifier { get; set; }
        public uint accountType { get; set; }

        /// <summary>
        /// 后台通过管理员发送消息
        /// </summary>
        public string manageSessId { get; set; }


    }


    /// <summary>
    /// 声网多媒体配置（语音、视频）
    /// </summary>
    public class ResponseIMMediaConfigDTO
    {
        public string AppID
        { get; set; }

        public string MediaChannelKey
        { get; set; }

        public string RecordingKey
        { get; set; }

        public string Secret { get; set; }

        public bool Audio { get; set; }

        public bool Video { get; set; }

        public bool Screen { get; set; }
        
        /// <summary>
        /// 总时长
        /// </summary>
        public int Duration { get; internal set; }


        /// <summary>
        /// 消耗
        /// </summary>
        public int TotalTime { get; internal set; }

        public bool DisableWebSdkInteroperability { get; set; }
    }

    public class IMHelper
    {  
        /// <summary>
        /// 获取全局随机数
        /// </summary>
        static int GlobalRandom
        {
            get
            {
                if (XuHos.Common.Cache.Manager.Instance != null)
                {
                    return (int)XuHos.Common.Cache.Manager.Instance.StringIncrement("IM.Random");
                }
                else
                {
                    return (int)DateTime.Now.Ticks;
                }
            }
        }


        /// <summary>
        /// 获取当前登录用户腾讯云通信配置
        /// 前置条件：已登录
        
        /// 日期：2016年8月6日
        /// </summary>
        /// <returns></returns>
        public ResponseIMConfigDTO Config(int identifier)
        {
            string userSig = Tencent.TSLHelper.GetSig(uint.Parse(Configuration.IMConfig.SDKAppID), identifier.ToString(), 
                uint.Parse(Configuration.IMConfig.AccountType));

            var result = new ResponseIMConfigDTO()
            {
                sdkAppID = uint.Parse(Configuration.IMConfig.SDKAppID),
                userSig = userSig,
                identifier = identifier.ToString(),
                accountType = uint.Parse(Configuration.IMConfig.AccountType),
                manageSessId = Configuration.IMConfig.AdminAccount
            };

            return result;

        }

        /// <summary>
        /// 获取多媒体配置（视频直播、视频录制）
        
        /// 日期：2016年8月20日
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="Identifier"></param>
        /// <returns></returns>
        public ResponseIMMediaConfigDTO MediaConfig(int ChannelID, int Identifier,int Duration,int TotalTime,bool DisableWebSdkInteroperability)
        {
                String vendor_key = "";
                String sign_key = "";
                //int expiredTs =DateTime.Now.AddSeconds(Duration-TotalTime).ToTimeStamp();//服务截止时间戳（2小时）
                int expiredTs = DateTime.Now.AddHours(2).ToTimeStamp();//服务截止时间戳（2小时）
                int unixTs = DateTime.Now.ToTimeStamp();//本次请求时间戳
                int randomInt = new Random().Next() * 100000000;

                // Generates Key for user to join Channel
                String media_channel_key = Agora.DynamicKey4.generateMediaChannelKey(vendor_key,
                    sign_key,
                    ChannelID.ToString(),
                    unixTs,
                    randomInt,
                    Identifier,
                    expiredTs);

                //Generates Key for Recording Server to join channel
                String recording_key = Agora.DynamicKey4.generateRecordingKey(vendor_key, sign_key,
                    ChannelID.ToString(),
                    unixTs,
                    randomInt,
                    Identifier,
                    expiredTs);

                var result = new ResponseIMMediaConfigDTO
                {
                    AppID = "",
                    MediaChannelKey = media_channel_key,
                    RecordingKey = recording_key,
                    Secret ="",
                    Video =true,
                    Audio = true,
                    Screen = true,                
                    Duration= Duration,
                    TotalTime=TotalTime
                };

                return result;
        }

        /// <summary>
        /// 发送群组文本消息
        
        /// 日期：2016年8月20日
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="From_Account"></param>
        /// <param name="MsgContent"></param>
        /// <returns></returns>
        public bool SendGroupTextMsg(int GroupId, int From_Account, string MsgContent)
        {

            dynamic obj = new
            {
                GroupId = GroupId.ToString(),
                From_Account = From_Account.ToString(), //指定消息发送者（选填）
                Random = GlobalRandom,//随机数（5分钟内不能重复）
                MsgBody = new List<dynamic>()
                {
                    new
                    {
                        MsgType="TIMTextElem",
                        MsgContent=new {Text=MsgContent }
                    }
                }
            };

            return Request(obj, "group_open_http_svc/send_group_msg");

        }


        /// <summary>
        /// 发送群组系统消息
        
        /// 日期：2016年8月20日
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="MsgContent"></param>
        /// <returns></returns>
        public bool SendGroupSystemMsg(int GroupId, string MsgContent)
        {
            dynamic obj = new
            {
                GroupId = GroupId.ToString(),
                Content = MsgContent
            };

            return Request(obj, "group_open_http_svc/send_group_system_notification");

        }

        /// <summary>
        /// 发送群组自定义消息
        
        /// 日期：2016年8月20日
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="From_Account"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public bool SendGroupCustomMsg<TMsgType>(int GroupId,
            int From_Account,
            IRequestIMCustomMsg<TMsgType> Msg
            )
        {

            string Data = XuHos.Common.JsonHelper.ToJson(Msg.Data);

            dynamic obj = new
            {
                GroupId = GroupId.ToString(),
                From_Account = From_Account.ToString(), //指定消息发送者（选填）
                Random = GlobalRandom,//随机数（5分钟内不能重复）
                MsgBody = new List<dynamic>()
                {
                     new
                    {
                        MsgType="TIMCustomElem",
                        MsgContent=new
                        {
                            Data =Data,
                            Desc =Msg.Desc,
                            Ext =Msg.Ext
                        }
                    },
                }
            };

            return Request(obj, "group_open_http_svc/send_group_msg");

        }

        public bool SendGroupImageMsg(int GroupId, int From_Account,string FileID,string FileUrl)
        {
            var MsgContent = new
            {
                UUID = FileID,
                ImageFormat = 255,
                ImageInfoArray = new List<dynamic>()
            };

            MsgContent.ImageInfoArray.Add(new
            {
                Height = 1993,
                Size = 877884,
                Type = 1,
                URL = FileUrl,
                Width = 1920
            });

            MsgContent.ImageInfoArray.Add(new
            {
                Height = 0,
                Size = 877884,
                Type = 2,
                URL = FileUrl,
                Width = 0
            });
            MsgContent.ImageInfoArray.Add(new
            {
                Height = 0,
                Size = 877884,
                Type = 3,
                URL = FileUrl,
                Width = 0
            });

            var obj = new
            {
                GroupId = GroupId.ToString(),
                From_Account = From_Account.ToString(), //指定消息发送者（选填）
                Random = GlobalRandom,//随机数（5分钟内不能重复）
                MsgBody = new List<dynamic>()
                {
                    new
                    {
                        MsgType = "TIMImageElem",
                        MsgContent = MsgContent,
                    },
                }
            };

            return Request(obj, "group_open_http_svc/send_group_msg");
        }



        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="From_Account"></param>
        /// <param name="FileID"></param>
        /// <param name="FileSize"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public bool SendGroupAudioMsg(int GroupId, int From_Account, string FileID, long FileSize,int Second)
        {

            var MsgContent = new
            {
                UUID = FileID,
                Size = FileSize,
                Second = Second,
            };

            var obj = new
            {
                GroupId = GroupId.ToString(),
                From_Account = From_Account.ToString(), //指定消息发送者（选填）
                Random = GlobalRandom,//随机数（5分钟内不能重复）
                MsgBody = new List<dynamic>()
                {
                    new
                    {
                        MsgType = "TIMSoundElem",
                        MsgContent = MsgContent,
                    },
                }
            };

            return Request(obj, "group_open_http_svc/send_group_msg");
        }

        /// <summary>
        /// 发送文件消息
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="From_Account"></param>
        /// <param name="FileID"></param>
        /// <param name="FileSize"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool SendGroupFileMsg(int GroupId, int From_Account, string FileID, long FileSize, string FileName)
        {
            var MsgContent = new
            {
                UUID = FileID,
                FileSize = FileSize,
                FileName = FileName
            };


            var obj = new
            {
                GroupId = GroupId.ToString(),
                From_Account = From_Account.ToString(), //指定消息发送者（选填）
                Random = GlobalRandom,//随机数（5分钟内不能重复）
                MsgBody = new List<dynamic>()
                {
                    new
                    {
                        MsgType = "TIMFileElem",
                        MsgContent = MsgContent,
                    },
                }
            };

            return Request(obj, "group_open_http_svc/send_group_msg");
        }


        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="GroupName"></param>
        /// <param name="ServiceType"></param>
        /// <param name="Introduction"></param>
        /// <returns></returns>
        public bool CreateGroup
            (int GroupId,
            string GroupName,
            XuHos.Common.Enum.EnumDoctorServiceType ServiceType,
            List<int> Members,
            string Introduction = "",
            string Notification = "",
            string FaceUrl = "")
        {

            //创建成员（如果不创建成员，如果此成员未登录过则腾讯云无法将此成员加入群组）
            if (CreateMembers(Members))
            {
                var MemberList = new List<dynamic>();

                foreach (int member in Members)
                {
                    MemberList.Add(new
                    {
                        Member_Account = member.ToString(), // 成员（必填）
                                                            //Role = "Admin" // 赋予该成员的身份，目前备选项只有Admin（选填）
                    });
                }

                dynamic obj = new
                {
                    GroupId = GroupId.ToString(),//自定义群组ID
                    Owner_Account = "", // 群主的UserId（选填）
                    Type = "ChatRoom", // 群组类型：Private/Public/ChatRoom/AVChatRoom（必填）
                    Name = GroupName, // 群名称（必填）
                    Introduction = Introduction, // 群简介（选填）
                    Notification = Notification, // 群公告（选填）
                    FaceUrl = FaceUrl, // 群头像URL（选填）
                    MaxMemberCount = 500, // 最大群成员数量（选填）
                    ApplyJoinOption = "FreeAccess",  // 申请加群处理方式（选填）
                    MemberList = MemberList,
                    AppDefinedData = new List<dynamic>
                    {
                        //new
                        //{
                        //    Key= "ServiceType", // APP自定义的字段Key
                        //    Value= (int)ServiceType// 自定义字段的值
                        //},
                        //new
                        //{
                        //    Key= "ServiceTypeDescript", // APP自定义的字段Key
                        //    Value= ServiceType.GetEnumDescript()// 自定义字段的值
                        //}

                    }
                };



                Func<int, bool> match = (ErrorCode) => ErrorCode == 10021;

                //10021,"ErrorInfo":"group id has be used
                if (Request(obj, "group_open_http_svc/create_group", match))
                {
                    return AddGroupMember(GroupId, Members);
                }
                else
                {
                    return AddGroupMember(GroupId, Members);
                }
            }
            else
            {
                return false;
            }
        }



                /// <summary>
        /// 添加群组成员
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="Members"></param>
        /// <returns></returns>
        public bool QueryState(List<string> To_Accounts)
        {

            dynamic obj = new
            {
                To_Account = To_Accounts,
            };


            /*{
                "ActionStatus": "OK",
                "ErrorInfo": "",
                "ErrorCode": 0,
                "QueryResult": [
                    {
                        "To_Account": "id1",
                        "State": "Offline"
                    },
                    {
                        "To_Account": "id2",
                        "State": "Online"
                    },
                    {
                        "To_Account": "id3",
                        "State": "Online"
                    }
                ]
            }*/
            var response= Post(obj, "/openim/querystate");

            var result = XuHos.Common.JsonHelper.FromJson<ResponseIMQueryResultDTO>(response);

            if (result != null && result.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 添加群组成员
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="Members"></param>
        /// <returns></returns>
        public bool AddGroupMember(int GroupId, List<int> Members)
        {

            var MemberList = new List<dynamic>();

            foreach (int member in Members)
            {
                MemberList.Add(new
                {
                    Member_Account = member.ToString(), // 成员（必填）
                });
            }

            dynamic obj = new
            {
                GroupId = GroupId.ToString(),//自定义群组ID
                MemberList = MemberList,

            };


            try
            {
                return Request(obj, "group_open_http_svc/add_group_member");
            }
            catch (InvalidToAccountException ex)
            {
                if (CreateMembers(Members))
                {
                    return Request(obj, "group_open_http_svc/add_group_member");
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }


        }


        /// <summary>
        /// 解散群组
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public bool DestroyGroup(int GroupId)
        {

            dynamic obj = new
            {
                GroupId = GroupId.ToString()
            };

            return Request(obj, "group_open_http_svc/destroy_group");

        }


        /// <summary>
        /// 解散群组
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public bool SendC2CTextMsg(string To_Account,string Text)
        {
            dynamic obj = new
            {
                SyncOtherMachine = 2,//消息不同步至发送方
                To_Account = To_Account,
                MsgRandom = GlobalRandom,
                MsgTimeStamp = DateTime.Now.ToTimeStamp(),
                MsgBody = new List<dynamic>
                {
                    new
                    {
                            MsgType= "TIMTextElem",
                            MsgContent= new {Text=Text}
                    }
                }
            };

         

            try
            {
                return Request(obj, "openim/sendmsg");
            }
            catch (InvalidToAccountException ex)
            {
                if (CreateMembers(new List<int>() { To_Account.ToInt(0) }))
                {
                    return Request(obj, "openim/sendmsg");
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }


        }


        /// <summary>
        /// 批量发送文本消息
        /// </summary>
        /// <param name="To_Accounts"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public bool SendC2CBatchTextMsg(string[] To_Accounts, string Text)
        {
            dynamic obj = new
            {
                SyncOtherMachine = 2,//消息不同步至发送方
                To_Account = To_Accounts,
                MsgRandom = GlobalRandom,
                MsgTimeStamp = DateTime.Now.ToTimeStamp(),
                MsgBody = new List<dynamic>
                {
                    new
                    {
                            MsgType= "TIMTextElem",
                            MsgContent= new {Text=Text}
                    }
                }
            };

            try
            {
                return Request(obj, "openim/batchsendmsg");
            }
            catch (InvalidToAccountException ex)
            {
                if (CreateMembers(To_Accounts.Select(a => int.Parse(a)).ToList()))
                {
                    return Request(obj, "openim/batchsendmsg");
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 发送自定义消息
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public bool SendC2CCustomMsg<TMsgType>(string To_Account, IRequestIMCustomMsg<TMsgType> Msg)
        {
            string Data = XuHos.Common.JsonHelper.ToJson(Msg.Data);


            dynamic obj = new
            {
                SyncOtherMachine = 2,//消息不同步至发送方
                To_Account = To_Account,
                MsgRandom = GlobalRandom,
                MsgTimeStamp = DateTime.Now.ToTimeStamp(),
                MsgBody = new List<dynamic>
                {
                     new
                    {
                        MsgType="TIMCustomElem",
                        MsgContent=new
                        {
                            Data =Data,
                            Desc =Msg.Desc,
                            Ext =Msg.Ext
                        }
                    }
                }
            };


            try
            {
                return Request(obj, "openim/sendmsg");
            }
            catch (InvalidToAccountException ex)
            {
                if (CreateMembers(new List<int>() { To_Account.ToInt(0) }))
                {
                    return Request(obj, "openim/sendmsg");

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// 批量发送自定义消息
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public bool SendC2CBatchCustomMsg<TMsgType>(string[] To_Accounts, IRequestIMCustomMsg<TMsgType> Msg)
        {
            string Data = XuHos.Common.JsonHelper.ToJson(Msg.Data);

            dynamic obj = new
            {
                SyncOtherMachine = 2,//消息不同步至发送方
                To_Account = To_Accounts,
                MsgRandom = GlobalRandom,
                MsgTimeStamp = DateTime.Now.ToTimeStamp(),
                MsgBody = new List<dynamic>
                {
                     new
                    {
                        MsgType="TIMCustomElem",
                        MsgContent=new
                        {
                            Data =Data,
                            Desc =Msg.Desc,
                            Ext =Msg.Ext
                        }
                    }
                }
            };

            try
            {
                return Request(obj, "openim/batchsendmsg");

            }
            catch (InvalidToAccountException ex)
            {
                if (CreateMembers(To_Accounts.Select(a => int.Parse(a)).ToList()))
                {
                    return Request(obj, "openim/batchsendmsg");
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="Members"></param>
        /// <returns></returns>
        public bool CreateMember(int Identifier, string Nick,string FaceUrl)
        {
            dynamic obj = new
            {
                Identifier = Identifier.ToString(),
                Nick = Nick,
                FaceUrl= FaceUrl

            };

            return Request(obj, "im_open_login_svc/account_import");

        }

        /// <summary>
        /// 导入成员信息
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="Members"></param>
        /// <returns></returns>
        public bool CreateMembers(List<int> Identifiers)
        {
            dynamic obj = new
            {
                Accounts = Identifiers.Select(a => a.ToString()).ToList()
            };

            return Request(obj, "im_open_login_svc/multiaccount_import");

        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="From_Account"></param>
        /// <param name="Accounts"></param>
        /// <returns></returns>
        public bool ApplyAddFriend(string From_Account,List<Models.AddFriendAccount> Accounts)
        {
            dynamic obj = new
            {
                From_Account= From_Account,//需要为该Identifier添加好友
                AddFriendItem = Accounts.ToList(),
                AddType = "Add_Type_Both",
                ForceAddFlags = 1
            };

            return Request(obj, "sns/friend_add");
        }


        /// <summary>
        /// 获取好友
        /// </summary>
        /// <param name="From_Account"></param>
        /// <param name="TimeStamp">上次拉取的时间戳，不填或为0时表示全量拉取</param>
        /// <param name="StartIndex">拉取的起始位置</param>
        /// <param name="LastStandardSequence">上次拉取标配关系链的Sequence，仅在只拉取标配关系链字段时有用</param>
        /// <param name="GetCount">每页需要拉取的数量，默认每页拉去100个好友</param>
        /// <returns></returns>
        public bool GetAllFriend(int From_Account,int TimeStamp=0,int StartIndex=0,int LastStandardSequence=0,int GetCount=100)
        {
            dynamic obj = new
            {
                From_Account=From_Account,
                TimeStamp= TimeStamp,
                StartIndex= StartIndex,
                TagList=new List<string>() {

                    "Tag_Profile_IM_Nick",
                    "Tag_SNS_IM_Group",
                    "Tag_SNS_IM_Remark"
                },
                LastStandardSequence= LastStandardSequence,
                GetCount= GetCount
            };

          

            return Request(obj, "sns/friend_get_all");
        }

        /// <summary>
        /// 请求云通信接口
        
        /// 日期：2016年8月20日
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        bool Request(dynamic obj,string method,Func<int,bool> match = null,string url= "https://console.tim.qq.com/v4/")
        {
            var requestParamters = XuHos.Common.JsonHelper.ToJson(obj);
            var requestBeginTime = DateTime.Now;
            var response = "";
            try
            {
                var userSig = Tencent.TSLHelper.GetSig(uint.Parse(Configuration.IMConfig.SDKAppID), 
                    Configuration.IMConfig.AdminAccount, uint.Parse(Configuration.IMConfig.AccountType));
                int random = GlobalRandom;
          
                url = string.Format(url + "/{4}?usersig={0}&identifier={1}&sdkappid={2}&random={3}&contenttype=json", 
                    userSig, Configuration.IMConfig.AdminAccount, Configuration.IMConfig.SDKAppID, random, method);

                //请求腾讯云接口
                response = XuHos.Common.Utility.WebAPIHelper.HttpPost(url,requestParamters);
                
                var result = XuHos.Common.JsonHelper.FromJson<ResponseIMResultDTO>(response);

                if (result != null && result.ErrorCode == 0)
                {
                    return true;
                }
                else
                {
                    XuHos.Common.LogHelper.WriteWarn($" Request {url},Params:{requestParamters} 失败,Response ErrorCode={result.ErrorCode},ErrorInfo={result.ErrorInfo}");

                    if (match != null)
                    {
                        if (match(result.ErrorCode))
                        {
                            return true;
                        }
                    }

                    if (result.ErrorCode == 10015)
                    {
                        return true;
                        //throw new InvalidGroupException();
                    }

                    //all identifiers you want to add are invalid
                    if (result.ErrorCode == 10019)
                    {
                        return true;
                        //throw new InvalidToAccountException();
                    }

                    if (result.ErrorCode == 90012)
                    {
                        return true;
                        //throw new InvalidToAccountException();
                    }
                }
            }
            catch(Exception E)
            {
                LogHelper.WriteError(E);
                throw E;
            }
            finally
            {
                WriteTrackLog(url, method, requestParamters, requestBeginTime, response);
            }

            return false;
        }


        /// <summary>
        /// 请求云通信接口
        
        /// 日期：2016年8月20日
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        string Post(dynamic obj, string method, Func<int, bool> match = null, string url = "https://console.tim.qq.com/v4/")
        {
            var requestParamters = XuHos.Common.JsonHelper.ToJson(obj);
            var requestBeginTime = DateTime.Now;
            var response = "";
            try
            {
                var userSig = Tencent.TSLHelper.GetSig(uint.Parse(Configuration.IMConfig.SDKAppID), 
                    Configuration.IMConfig.AdminAccount, uint.Parse(Configuration.IMConfig.AccountType));
                int random = GlobalRandom;

                url = string.Format(url + "/{4}?usersig={0}&identifier={1}&sdkappid={2}&random={3}&contenttype=json", userSig, 
                    Configuration.IMConfig.AdminAccount, Configuration.IMConfig.SDKAppID, random, method);

                //请求腾讯云接口
                return XuHos.Common.Utility.WebAPIHelper.HttpPost(url, requestParamters);

            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
                throw E;
            }
            finally
            {
                WriteTrackLog(url, method, requestParamters, requestBeginTime, response);
            }

            return "";
        }

        void WriteTrackLog(string requestUri, string comments, string RequestParamters, DateTime requestEnterTime, string Response)
        {
            XuHos.Common.LogHelper.WriteTrackLog("TrackQQCloudyApiOperatorLog",
             requestUri: requestUri,
             comments: comments,
             RequestParamters: RequestParamters,
             requestEnterTime: requestEnterTime,
             Response: Response);
        }
    }
}


/// <summary>
/// 腾讯云通信服务端集成

/// 日期：2016年8月16日
/// </summary>
namespace Tencent
{

    public static class TSLHelper
    {
        public static string GetPrivateKey(uint SdkAppid)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.KEY_IM_PriaveKey, SdkAppid.ToString());
            var key = cacheKey.FromCache<string>();

            if (string.IsNullOrEmpty(key))
            {
                var basedir = System.AppDomain.CurrentDomain.BaseDirectory;
                var pri_key_path = System.IO.Path.Combine(basedir, "App_Data/Key/IM", SdkAppid.ToString(), "private_key");
                FileStream f = new FileStream(pri_key_path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(f);
                byte[] b = new byte[f.Length];
                reader.Read(b, 0, b.Length);
                key=Encoding.Default.GetString(b);
                key.ToCache(cacheKey);
            }

            return key;
        }

        public static string GetPublicKey(uint SdkAppid)
        {
            var cacheKey = new XuHos.Common.Cache.Keys.StringCacheKey(XuHos.Common.Cache.Keys.StringCacheKeyType.KEY_IM_PublicKey, SdkAppid.ToString());
            var key = cacheKey.FromCache<string>();

            if (string.IsNullOrEmpty(key))
            {
                var basedir = System.AppDomain.CurrentDomain.BaseDirectory;
                var pri_key_path = System.IO.Path.Combine(basedir, "App_Data/Key/IM", SdkAppid.ToString(), "public_key");
                FileStream f = new FileStream(pri_key_path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(f);
                byte[] b = new byte[f.Length];
                reader.Read(b, 0, b.Length);
                key = Encoding.Default.GetString(b);
                key.ToCache(cacheKey);
            }

            return key;
        }

        static TSLHelper()
        {

        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="SdkAppid">SDK编号</param>
        /// <param name="identifier">用户唯一标识</param>
        /// <param name="accountType">用户类型</param>
        /// <returns></returns>
        public static string GetSig(uint SdkAppid, string identifier, uint accountType)
        {
            string pri_key = GetPrivateKey(SdkAppid);

            StringBuilder sig = new StringBuilder(4096);
            StringBuilder err_msg = new StringBuilder(4096);
            int ret = sigcheck.tls_gen_sig(10 * 24 * 3600,
                SdkAppid.ToString(),
                SdkAppid, identifier,
                accountType,
                sig,
                4096,
                pri_key,
                (UInt32)pri_key.Length, err_msg,
                4096);


            if (0 != ret)
            {
                return "";
            }
            else
            {
                return sig.ToString();
            }
        }


        public static bool VerifySig(string sig, uint SdkAppid, string identifier, uint accountType)
        {
            string pub_key = GetPublicKey(SdkAppid);

            StringBuilder err_msg = new StringBuilder(4096);

            UInt32 expire_time = 0;
            UInt32 init_time = 0;
            var ret = sigcheck.tls_vri_sig_ex(
                sig.ToString(),
                pub_key,
                (UInt32)pub_key.Length,
                SdkAppid,
                identifier,
                ref expire_time,
                ref init_time,
                err_msg,
                4096);

            if (0 != ret)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }

    class dllpath
    {
        // 开发者调用 dll 时请注意项目的平台属性，下面的路径是 demo 创建时使用的，请自己使用予以修改
        // 请使用适当的平台 dll
        //public const string DllPath = @"D:\src\oicq64\tinyid\tls_sig_api\windows\64\lib\libsigcheck\sigcheck.dll";       // 64 位
        // 如果选择 Any CPU 平台，默认加载 32 位 dll

        public const string DllPath = "sigcheck.dll";

    }

    class sigcheck
    {

        [DllImport(dllpath.DllPath, EntryPoint = "tls_gen_sig", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_gen_sig(
            UInt32 expire,
            string appid3rd,
            UInt32 sdkappid,
            string identifier,
            UInt32 acctype,
            StringBuilder sig,
            UInt32 sig_buff_len,
            string pri_key,
            UInt32 pri_key_len,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );

        [DllImport(dllpath.DllPath, EntryPoint = "tls_vri_sig", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_vri_sig(
            string sig,
            string pub_key,
            UInt32 pub_key_len,
            UInt32 acctype,
            string appid3rd,
            UInt32 sdkappid,
            string identifier,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );

        [DllImport(dllpath.DllPath, EntryPoint = "tls_gen_sig_ex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_gen_sig_ex(
            UInt32 sdkappid,
            string identifier,
            StringBuilder sig,
            UInt32 sig_buff_len,
            string pri_key,
            UInt32 pri_key_len,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );

        [DllImport(dllpath.DllPath, EntryPoint = "tls_vri_sig_ex", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public extern static int tls_vri_sig_ex(
            string sig,
            string pub_key,
            UInt32 pub_key_len,
            UInt32 sdkappid,
            string identifier,
            ref UInt32 expire_time,
            ref UInt32 init_time,
            StringBuilder err_msg,
            UInt32 err_msg_buff_len
        );
    }
}

/// <summary>
/// 声网服务端集成

/// 日期：2016年8月16日
/// </summary>
namespace Agora
{

    class DynamicKeyUtil
    {



        //数据签名
        public static string HMACSHA1Encrypt(string EncryptKey, string EncryptText)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(EncryptKey));
            byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(EncryptText));
            return bytesToHex(RstRes);
        }


        public static string bytesToHex(byte[] bytes)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2").PadLeft(2, '0'));
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// 动态秘钥
    /// </summary>
    public class DynamicKey4
    {

        private static string RECORDING_SERVICE = "ARS";
        private static string MEDIA_CHANNEL_SERVICE = "ACS";


        private static string generateSignature4(string vendorKey,
            string signKey,
            string channelName,
            string unixTsStr,
            string randomIntStr,
            string uidStr,
            string expiredTsStr,
            string serviceType)
        {

            StringBuilder builder = new StringBuilder();

            builder.Append(serviceType);
            builder.Append(vendorKey);
            builder.Append(unixTsStr);
            builder.Append(randomIntStr);
            builder.Append(channelName);
            builder.Append(uidStr);
            builder.Append(expiredTsStr);
            string sign = DynamicKeyUtil.HMACSHA1Encrypt(signKey, builder.ToString());
            return sign;
            //return DynamicKeyUtil.bytesToHex(sign.getBytes());
        }

        private static string doGenerate(String vendorKey,
            String signKey,
            String channelName,
            int unixTs,
            int randomInt,
            long uid,
            int expiredTs,
            String serviceType)
        {
            String version = "004";
            String unixTsStr = ("0000000000" + unixTs.ToString()).Substring(unixTs.ToString().Length);
            String randomIntStr = ("00000000" + randomInt.ToString().toHexString()).Substring(randomInt.ToString().toHexString().Length).ToLower();
            uid = uid & 0xFFFFFFFFL;
            String uidStr = ("0000000000" + uid.ToString()).Substring(uid.ToString().Length);
            String expiredTsStr = ("0000000000" + expiredTs.ToString()).Substring(expiredTs.ToString().Length);
            String signature = generateSignature4(vendorKey, signKey, channelName, unixTsStr, randomIntStr, uidStr, expiredTsStr, serviceType);



            if (version.Length != 3)
                throw new Exception("version 3");

            if (signature.Length != 40)
                throw new Exception("signature 长度不是40");

            if (vendorKey.Length != 32)
                throw new Exception("vendorKey 长度不是32");


            if (unixTsStr.Length != 10)
                throw new Exception("vendorKey 长度不是10");

            if (randomIntStr.Length != 8)
                throw new Exception("randomIntStr 长度不是8");

            if (expiredTsStr.Length != 10)
                throw new Exception("randomIntStr 长度不是10");



            string result = string.Format("{0}{1}{2}{3}{4}{5}", version, signature, vendorKey, unixTsStr, randomIntStr, expiredTsStr);

            if (result.Length == 103)
            {
                return result;
            }
            else
            {
                throw new Exception("Key无效，长度必须是103");
            }

        }


        /**
         * Generate Dynamic Key for recording service
         * @param vendorKey Vendor key assigned by Agora
         * @param signKey Sign key assigned by Agora
         * @param channelName name of channel to join, limited to 64 bytes and should be printable ASCII characters
         * @param unixTs unix timestamp in seconds when generating the Dynamic Key
         * @param randomInt salt for generating dynamic key
         * @param uid user id, range from 0 - max uint32
         * @param expiredTs should be 0
         * @return String representation of dynamic key
         * @throws Exception if any error occurs
         */
        public static String generateRecordingKey(String vendorKey,
            String signKey,
            String channelName,
            int unixTs,
            int randomInt,
            long uid,
            int expiredTs)
        {
            return doGenerate(vendorKey, signKey, channelName, unixTs, randomInt, uid, expiredTs, RECORDING_SERVICE);
        }

        /**
         * Generate Dynamic Key for media channel service
         * @param vendorKey Vendor key assigned by Agora
         * @param signKey Sign key assigned by Agora
         * @param channelName name of channel to join, limited to 64 bytes and should be printable ASCII characters
         * @param unixTs unix timestamp in seconds when generating the Dynamic Key
         * @param randomInt salt for generating dynamic key
         * @param uid user id, range from 0 - max uint32
         * @param expiredTs service expiring timestamp. After this timestamp, user will not be able to stay in the channel.
         * @return String representation of dynamic key
         * @throws Exception if any error occurs
         */
        public static String generateMediaChannelKey(String vendorKey,
            String signKey,
            String channelName,
            int unixTs,
            int randomInt,
            long uid,
            int expiredTs)
        {
            return doGenerate(vendorKey, signKey, channelName, unixTs, randomInt, uid, expiredTs, MEDIA_CHANNEL_SERVICE);
        }


    }

}