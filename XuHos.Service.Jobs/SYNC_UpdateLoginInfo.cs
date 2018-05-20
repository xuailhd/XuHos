using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using KMEHosp.Common.Cache;
using KMEHosp.BLL.Sys.Implements;

namespace KMEHosp.Service.Jobs
{
    public class SYNC_UpdateLoginInfo : Quartz.IJob
    {
        
        public void Execute(IJobExecutionContext context)
        {
            if (typeof(SYNC_UpdateLoginInfo).Name.Lock(DateTime.Now.AddSeconds(5)))
            {

                try
                {
                    KMEHosp.BLL.User.Implements.UserService uaser = new BLL.User.Implements.UserService();

                    uaser.DoUpdateLoginInfo();

                }
                catch (Exception E)
                {
                    KMEHosp.Common.LogHelper.WriteError(E);
                }
                finally
                {
                    typeof(SYNC_UpdateLoginInfo).Name.UnLock();
                }
            }
            
        }
    }
}
