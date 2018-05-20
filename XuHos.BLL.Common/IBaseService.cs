using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Common.Cache;
using XuHos.Common.Utility;
using XuHos.DTO.Common;

namespace XuHos.BLL.Common
{
    public interface IBaseService<TEntity>
        where TEntity : XuHos.Entity.IAuditableEntity
    {
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Insert(TEntity model);

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Insert(params TEntity[] model);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(TEntity model);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="updateExpress">更新表达式</param>
        /// <returns></returns>
        bool Update(string ID, Expression<Func<TEntity, TEntity>> updateExpress);

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool Delete(string ID);

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        bool Delete(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool Exists(string ID);


        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        TResult Single<TResult>(string ID, List<Expression<Func<TEntity, object>>> includes = null, params string[] MapIngoreMembers);

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        TResult Single<TResult>(Expression<Func<TEntity, bool>> where, List<Expression<Func<TEntity, object>>> includes = null, params string[] MapIngoreMembers);



        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Response<List<TResult>> GetPageList<TResult>(
            int PageIndex = 1,
            int PageSize = int.MaxValue,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null)
            where TResult : class;
    }
}
