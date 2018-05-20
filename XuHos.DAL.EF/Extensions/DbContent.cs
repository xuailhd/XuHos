using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;
namespace XuHos.DAL.EF
{
    public static class Extensions_DbContent
    {
        /// <summary>
        /// 更新一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TEntity Update<TEntity>(this DbContext db, TEntity model)
            where TEntity : class
        {
            //if (db.Entry<TEntity>(model).State == EntityState.Detached)
            //{
            //    db.Set<TEntity>().Attach(model);
            //}

            db.Entry<TEntity>(model).State = System.Data.Entity.EntityState.Modified;

            return model;
        }
    }
}
