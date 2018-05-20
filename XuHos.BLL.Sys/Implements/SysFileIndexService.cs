using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common;
using XuHos.DAL.EF;
using XuHos.Entity;
using XuHos.Common.Enum;

namespace XuHos.BLL.Sys.Implements
{
    public class SysFileIndexService : Common.CommonBaseService<SysFileIndex>
    {
        public SysFileIndexService(string CurrentOperatorUserID) :base(CurrentOperatorUserID)
        {

        }

        public void Update(SysFileIndex model)
        {
            using (var db = new DBEntities())
            {
                var dbmodel = db.SysFileIndexs.Where(t => t.MD5 == model.MD5).FirstOrDefault();
                if (dbmodel != null)
                {
                    dbmodel.FileUrl = model.FileUrl;
                    dbmodel.AccessKey = model.AccessKey;
                    dbmodel.ModifyTime = DateTime.Now;
                    dbmodel.Remark = model.Remark;
                    dbmodel.FileSize = model.FileSize;
                    dbmodel.FileType = model.FileType;
                    db.SaveChanges();
                }
            }
        }
    }
}
