using XuHos.BLL.Common;
using XuHos.DAL.EF;
using XuHos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Trade.Implements
{
    public class OrderCallbackLogService : CommonBaseService<OrderCallbackLog>
    {
        public OrderCallbackLogService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        { }

        public int SaveAndGetTriedTimes(string OrgID, string OrderNo, int Status, string Message)
        {
            int triedTimes = 1;
            using (DBEntities db = new DBEntities())
            {
                var m = db.OrderCallbackLogs.Where(w => w.OrgID == OrgID && w.OrderNo == OrderNo && w.Status == 0 && w.IsDeleted == false).FirstOrDefault();//找出没有同步成功的记录
                if (m != null)
                {
                    triedTimes = m.TriedTimes = m.TriedTimes + 1;
                    m.Status = Status;
                    m.Message = Message;
                    m.ModifyTime = DateTime.Now;
                }
                else
                {
                    db.OrderCallbackLogs.Add(new OrderCallbackLog { CallbackLogID = Guid.NewGuid().ToString("N"), CreateTime = DateTime.Now, OrgID = OrgID, OrderNo = OrderNo, Status = Status, Message = Message, TriedTimes = triedTimes });
                }
                db.SaveChanges();
            }
            return triedTimes;
        }
    }
}
