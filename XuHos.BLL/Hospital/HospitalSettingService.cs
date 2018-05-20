using KMEHosp.BLL.Common;
using KMEHosp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMEHosp.BLL
{
    public class HospitalSettingService : CommonBaseService<HospitalSetting>
    {
        public HospitalSettingService(string CurrentOperatorUserID) : base(CurrentOperatorUserID)
        { }
    }
}
