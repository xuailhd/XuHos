using KMEHosp.BLL.Common;
using KMEHosp.DAL.EF;
using KMEHosp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMEHosp.BLL
{
    public class PaymentCallbackLogService : CommonBaseService<PaymentCallbackLog>
    {
        public PaymentCallbackLogService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        { }

        public int SaveAndGetTriedTimes(string HospitalID, string OrderNo, int Status, string Message)
        {
            int triedTimes = 1;
            using (DBEntities db = new DBEntities())
            {
                var m = db.PaymentCallbackLogs.Where(w => w.HospitalID == HospitalID && w.OrderNo == OrderNo && w.Status == 0 && w.IsDeleted == false).FirstOrDefault();//找出没有同步成功的记录
                if(m != null)
                {
                    triedTimes = m.TriedTimes = m.TriedTimes + 1;
                    m.Status = Status;
                    m.Message = Message;
                    m.ModifyTime = DateTime.Now;
                }
                else
                {
                    db.PaymentCallbackLogs.Add(new PaymentCallbackLog { CallbackLogID = Guid.NewGuid().ToString("N"), CreateTime = DateTime.Now, HospitalID = HospitalID, OrderNo = OrderNo, Status = Status, Message = Message, TriedTimes = triedTimes });
                }
                db.SaveChanges();
            }
            return triedTimes;
        }
    }
}
