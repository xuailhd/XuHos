using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuHos.DTO
{
    /// <summary>
    /// 会话房间
    /// </summary>
    public class ConversationRoomDTO
    {
        public ConversationRoomDTO()
        {
            this.DisableWebSdkInteroperability = false;
        }

        /// <summary>
        /// 会话房间ID
        /// </summary>
        [Required]
        public string ConversationRoomID { get; set; }

        /// <summary>
        /// 业务ID(关联外部业务ID)
        /// </summary>
        [Required]
        public string ServiceID { get; set; }


        /// <summary>
        /// 业务类型
        /// </summary>
        [Required]
        public XuHos.Common.Enum.EnumDoctorServiceType ServiceType { get; set; }

        /// <summary>
        /// 房间编号
        /// </summary>
        [Required]
        public int ChannelID { get; set; }

        /// <summary>
        /// 房间密码
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 房间状态
        /// </summary>
        [Required]
        public XuHos.Common.Enum.EnumRoomState RoomState { get; set; }

        public string RoomStateName { get; set; }


        /// <summary>
        /// 看诊开始时间
        /// </summary>
        [Required]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 看诊结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 看诊总时长
        /// </summary>
        [Required]
        public int TotalTime { get; set; }

        public bool Enable { get; set; }

        /// <summary>
        /// 房间是否已关闭（关闭状态无法设置房间状态）
        /// </summary>
        public bool Close { get; set; }

        public int Duration { get; set; }

        /// <summary>
        /// 计费状态
        /// </summary>
        public EnumRoomChargingState ChargingState { get; set; }

        /// <summary>
        /// 计费时钟序号
        /// </summary>
        public int ChargingSeq { get; set; }

        /// <summary>
        /// 计费时钟周期
        /// </summary>
        public int ChargingInterval { get; set; }

        /// <summary>
        /// 计费最后时间
        /// </summary>
        public DateTime ChargingTime { get; set; }

        public XuHos.Common.Enum.EnumRoomType RoomType { get; set; }

        public long TriageID { get; set; }

        public int Priority { get; set; }

        public DateTime ModifyTime { get; set; }

        public bool DisableWebSdkInteroperability { get; set;}

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
    
}
