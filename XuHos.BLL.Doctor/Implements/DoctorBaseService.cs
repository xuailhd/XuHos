using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Cache;
using XuHos.DAL.EF;
using System.Data.Entity;

namespace XuHos.BLL.Doctor.Implements
{
    public class DoctorBaseService<TEntity> :BLL.Common.CommonBaseService<TEntity>
        where TEntity:class, Entity.IAuditableEntity, new()
    {
        public DoctorBaseService(string CurrentOperatorUserID):base(CurrentOperatorUserID)
        {

        }

     


    }
}
