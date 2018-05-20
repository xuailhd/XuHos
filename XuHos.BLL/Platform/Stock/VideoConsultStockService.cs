using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.DTO.Platform;
using XuHos.BLL.Sys;
using XuHos.Entity;
using XuHos.DAL.EF;
using EntityFramework.Extensions;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;

namespace XuHos.BLL.Platform.Order
{
    /// <summary>
    /// 语音咨询和视频咨询订单
    /// </summary>
    public class VideoConsultStockService : IStockService
    {
        public string CurrentOperatorUserID
        { get; private set; }

        public EnumStockRestoreResult Restore(OrderDTO order, string CancelReason)
        {
            using (DAL.EF.DBEntities db = new DBEntities())
            {
                db.ConversationRooms.Where(a =>
                a.ServiceID == order.OrderOutID && !a.IsDeleted &&
                (a.ServiceType == EnumDoctorServiceType.AudServiceType ||
                a.ServiceType == EnumDoctorServiceType.VidServiceType)).Update(a => new ConversationRoom()
                {
                    IsDeleted = true
                });

                return EnumStockRestoreResult.Success;
            }
        }

        /// <summary>
        /// 预扣库存
        /// </summary>
        /// <param name="ServiceType"></param>
        /// <param name="OutID"></param>
        /// <returns></returns>
        public bool PreDeduction(out string OutID)
        {
            OutID = "";
            return true;
        }


        /// <summary>
        /// 确认扣库存
        /// </summary>
        /// <param name="DeductionId"></param>
        /// <returns></returns>
        public bool ConfirmDeduction(string DeductionId)
        {
            return true;
        }

        public bool Delivery(string OrderOutID, out string LogisticNo, out EnumLogisticState LogisticState)
        {
            var Deliveryed = false;
            LogisticNo = "";
            LogisticState = EnumLogisticState.审核中;

            #region 发货->创建房间
            BLL.UserOPDRegisterService bll = new UserOPDRegisterService("");
            if (bll.CreateIMRoom(OrderOutID))
            {
                Deliveryed = true;
                LogisticNo = "-";
                LogisticState = EnumLogisticState.已备货;
            }
            #endregion

            return Deliveryed;
        }
    }
}
