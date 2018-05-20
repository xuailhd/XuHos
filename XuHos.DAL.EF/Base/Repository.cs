using EntityFramework.Caching;
using EntityFramework.Extensions;
using EntityFramework.Future;
using EntityFramework.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XuHos.Entity;

namespace XuHos.DAL.EF.Base
{
    public class Repository<TEntity>
          where TEntity :class,XuHos.Entity.IAuditableEntity

    {
        static List<string> _PK = new List<string>();

        static Repository()
        {
            Type EntityType = typeof(TEntity);

            var Properties = EntityType.GetProperties();

            foreach (var p in Properties)
            {
                //ID是主键
                if (p.Name.ToUpper() == "ID")
                {
                    _PK.Add(p.Name);
                }
                //实体名称+ID是主键
                else if (p.Name.ToLower() == (EntityType.Name + "ID").ToLower())
                {
                    _PK.Add(p.Name);
                }
                //设置了Key特性是主键
                else
                {
                    var KeyAttribute = p.GetCustomAttributes(typeof(KeyAttribute), false);

                    if (KeyAttribute != null && KeyAttribute.Length > 0)
                    {
                        _PK.Add(p.Name);
                    }
                }
            }

        }

        /// <summary>
        /// 新增一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Insert(params TEntity[] model)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                return this.PreInsert(db, model).SaveChanges() > 0 ? true : false;
            }
        
        }

        /// <summary>
        /// 新增（未提交保存）
        /// 作者：曾璐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual XuHos.DAL.EF.DBEntities PreInsert(XuHos.DAL.EF.DBEntities db, params TEntity[] model)
        {
            if (_PK.Count <= 0)
            {
                throw new Exception("没有设置主键");
            }

            if (_PK.Count > 1)
            {
                throw new Exception("不支持复合主键");
            }

            Type type = typeof(TEntity);
            var prop = type.GetProperty(_PK[0]);


            for (int i = 0; i < model.Length; i++)
            {
                string ID = prop.GetValue(model[i], null) as string;

                //设置主键
                if (String.IsNullOrWhiteSpace(ID))
                {
                    ID = Guid.NewGuid().ToString("N");
                    prop.SetValue(model[i], ID);
                }
                db.Set<TEntity>().Add(model[i]);
            }
            return db;
        }

        /// <summary>
        /// 更新一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(TEntity model)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                return this.PreUpdate(db, model).SaveChanges() > 0 ? true : false;                
            }
        }

        /// <summary>
        /// 更新（未提交保存）
        /// 作者：曾璐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual XuHos.DAL.EF.DBEntities PreUpdate(XuHos.DAL.EF.DBEntities db, TEntity model)
        {
            //if (db.Entry<TEntity>(model).State == EntityState.Detached)
            //{
            //    db.Set<TEntity>().Attach(model);
            //}
            var entry = db.Entry<TEntity>(model);
            if (entry.State != System.Data.Entity.EntityState.Modified)
            {
                entry.State = System.Data.Entity.EntityState.Modified;
            }
            return db;
        }

        /// <summary>
        /// 更新一条记录（部分更新）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="updateExpress">更新表达式）</param>
        /// <returns></returns>
        public virtual bool Update(string ID,Expression<Func<TEntity,TEntity>> updateExpress)
        {
            if (_PK.Count <= 0)
            {
                throw new Exception("没有设置主键");
            }

            if (_PK.Count > 1)
            {
                throw new Exception("不支持复合主键");
            }

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                IQueryable<TEntity> cities = db.Set<TEntity>();
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "TEntity");
                Expression left = Expression.Property(param, typeof(TEntity).GetProperty(_PK[0]));
                Expression right = Expression.Constant(ID);
                Expression filter = Expression.Equal(left, right);
                return db.Set<TEntity>().Where(Expression.Lambda<Func<TEntity, bool>>(filter, param)).Update(updateExpress) > 0 ? true : false;
            }
        }


        /// <summary>
        /// 更新一条记录（部分更新）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="updateExpress">更新表达式）</param>
        /// <returns></returns>
        public virtual bool Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> updateExpress)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                return db.Set<TEntity>().Where(where).Update(updateExpress) > 0 ? true : false;
            }
        }

        /// <summary>
        /// 通过主键删除记录(物理删除)
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="ID">主键值</param>
        /// <returns></returns>
        public virtual bool Delete(string ID)
        {
            if (_PK.Count <= 0)
            {
                throw new Exception("没有设置主键");
            }

            if (_PK.Count > 1)
            {
                throw new Exception("不支持复合主键");
            }

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                IQueryable<TEntity> cities = db.Set<TEntity>();
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "TEntity");
                Expression left = Expression.Property(param, typeof(TEntity).GetProperty(_PK[0]));
                Expression right = Expression.Constant(ID);
                Expression filter = Expression.Equal(left, right);
                
                return db.Set<TEntity>().Where(Expression.Lambda<Func<TEntity, bool>>(filter, param)).Delete() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 通过表达式删除记录（物理删除）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<TEntity, bool>> where)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                return db.Set<TEntity>().Where(where).Delete() > 0 ? true : false;
            }

        }


        /// <summary>
        /// 通过主键判断记录是否存在（不含已删除）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns></returns>
        public virtual bool Exists(string ID)
        {
            if (_PK.Count <= 0)
            {
                throw new Exception("没有设置主键");
            }

            if (_PK.Count > 1)
            {
                throw new Exception("不支持复合主键");
            }

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                IQueryable<TEntity> cities = db.Set<TEntity>();
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "TEntity");
                Expression left = Expression.Property(param, typeof(TEntity).GetProperty(_PK[0]));
                Expression right = Expression.Constant(ID);
                Expression filter = Expression.Equal(left, right);
                return db.Set<TEntity>().Where(Expression.Lambda<Func<TEntity, bool>>(filter, param)).Where(a=>a.IsDeleted==false).Count() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 通过表达式判断记录是否存在（排除已删除的）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return Count(where) > 0;
        }
        /// <summary>
        /// 获取记录数（不含已删除）
        /// 作者：曾璐
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> where)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                return db.Set<TEntity>().Where(where).Where(a => a.IsDeleted == false).Count();
            }
        }


        /// <summary>
        /// 通过主键获取一条记录（不含已删除）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns></returns>
        public virtual TEntity Single(string ID, List<Expression<Func<TEntity, object>>> includes = null)
        {
            if (_PK.Count <= 0)
            {
                throw new Exception("没有设置主键");
            }

            if (_PK.Count > 1)
            {
                throw new Exception("不支持复合主键");
            }

            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                IQueryable<TEntity> cities = db.Set<TEntity>();
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "TEntity");
                Expression left = Expression.Property(param, typeof(TEntity).GetProperty(_PK[0]));
                Expression right = Expression.Constant(ID);
                Expression filter = Expression.Equal(left, right);
                var query = db.Set<TEntity>().Where(Expression.Lambda<Func<TEntity, bool>>(filter, param)).Where(a=>a.IsDeleted==false);
               
                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                var result = query.SingleOrDefault();

                return result;
            }
        }


        /// <summary>
        /// 通过表达式获取一条记录（不含已删除）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public TEntity Single(Expression<Func<TEntity, bool>> where, List<Expression<Func<TEntity, object>>> includes = null)
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                var query = db.Set<TEntity>().Where(where).Where(a => a.IsDeleted == false);

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                var result = query.FirstOrDefault();

              
                return result;
            }

        }

        /// <summary>
        /// 获取分页（不含已删除）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="where">插叙条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="select">返回结果映射</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public virtual List<TResult> Select<TResult>(
            out int totalCount,
            int? PageIndex = 0,
            int? PageSize = 10,
            List<Expression<Func<TEntity, object>>> includes = null,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            Expression<Func<TEntity, TResult>> selector = null)
            where TResult : class
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                List<TEntity> result = new List<TEntity>();

                var query = db.Set<TEntity>().AsQueryable().Where(a => a.IsDeleted == false);

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                #region 增加查询条件
                if (where != null)
                {
                    query = db.Set<TEntity>().Where(where);
                }
                #endregion

                #region 设置排序
                if (orderBy != null)
                {
                    query = query.OrderByDescending(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(a => a.CreateTime);
                }

                #endregion

                #region 设置分页
                if (PageSize.HasValue && PageIndex.HasValue)
                {

                    var fTotal = query.FutureCount();
                    var fList = query.Skip((PageIndex.Value - 1) * PageSize.Value).Take(PageSize.Value).Select(selector).Future();
                    totalCount = fTotal.Value;
                    return fList.ToList();

                }
                else
                {
                    var fTotal = query.FutureCount();
                    var fList = query.Select(selector).Future();
                    totalCount = fTotal.Value;
                    return fList.ToList();
                }
                #endregion

            }

        }

        /// <summary>
        /// 获取分页
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="where">插叙条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="select">返回结果映射</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public virtual List<TEntity> Select(
            out int totalCount,
            int? PageIndex=0,
            int? PageSize=10,
            List<Expression<Func<TEntity, object>>> includes = null,
            Expression<Func<TEntity, bool>> where=null,
            Expression<Func<TEntity, object>> orderBy=null )
                 
        {
            using (XuHos.DAL.EF.DBEntities db = new DAL.EF.DBEntities())
            {
                List<TEntity> result = new List<TEntity>();

                var query = db.Set<TEntity>().AsQueryable().Where(a => a.IsDeleted == false);

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                #region 增加查询条件
                if (where != null)
                {
                    //许光丽 修改  (上面已经是 db.Set<TEntity>() 了)
                    //query = db.Set<TEntity>().Where(where);
                    query = query.Where(where); 
                }
                #endregion

                #region 设置排序
                if (orderBy != null)
                {   
                    query = query.OrderByDescending(orderBy);
                }
                else
                {
                    
                    

                    query = query.OrderByDescending(a => new { a.CreateTime });
                }

                #endregion

                #region 设置分页
                if (PageSize.HasValue && PageIndex.HasValue)
                {

                    var fTotal = query.FutureCount();
                    var fList = query.Skip((PageIndex.Value - 1) * PageSize.Value).Take(PageSize.Value).Future();
                    totalCount = fTotal.Value;
                    return fList.ToList();

                }
                else
                {
                    var fTotal = query.FutureCount();
                    var fList = query.Future();
                    totalCount = fTotal.Value;
                    return fList.ToList();
                }
                #endregion

            }

        }

    }
}
