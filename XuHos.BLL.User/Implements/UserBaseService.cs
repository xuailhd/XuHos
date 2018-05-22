using XuHos.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Cache;

namespace XuHos.BLL.User.Implements
{
    public class UserBaseService<TEntity> :BLL.Common.CommonBaseService<TEntity>
        where TEntity:class, Entity.IUserBaseEntity, new()
    {

        public UserBaseService(string CurrentOperatorUserID):base(CurrentOperatorUserID)
        {
            this.CurrentOperatorUserID = CurrentOperatorUserID;
        }

        public override bool Insert(TEntity model)
        {
            if(string.IsNullOrEmpty(model.UserID))
            {   
                //添加数据是附加用户编号
                model.UserID = CurrentOperatorUserID;
            }

            return base.Insert(model);
        }

        public override bool Update(TEntity model)
        {
            if (string.IsNullOrEmpty(model.UserID))
            {   
                //添加数据是附加用户编号
                model.UserID = CurrentOperatorUserID;
            }

            return base.Update(model);
        }

    }
}
