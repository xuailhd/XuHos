using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework;
using EntityFramework.Extensions;
using EntityFramework.Mapping;
using System.Data.Entity;

namespace XuHos.DAL.EF
{
    public static class Extensions_Query
    {
        /// <summary>
        /// 返回分页数据
        /// </summary>
        /// <typeparam name="TEntity">返回结果集中的类型</typeparam>
        /// <param name="query">查询结果集表达式</param>
        /// <param name="Total">返回的总记录数</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public static List<TEntity> Pager<TEntity>(
            this IQueryable<TEntity> query,
            out int Total,
            int? PageIndex = 1,
            int? PageSize = 10)
            where TEntity : class
        {

            if (PageIndex <= 0)
                PageIndex = 1;

            Total = 0;

            #region 设置分页
            if (PageSize.HasValue && PageIndex.HasValue)
            {
                Total = query.Count();
                return query.Skip((PageIndex.Value - 1) * PageSize.Value).Take(PageSize.Value).ToList();

            }
            else
            {
                Total = query.Count();
                return query.ToList();

            }
            #endregion

        }


        /// <summary>
        /// 通过直接拼接SQL语句获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static List<TEntity> Pager<TEntity>(this DbContext db, string strFields, string strFromWhere, string strSort, out int Total, int PageIndex = 1, int PageSize = 10)
        {
            string sqlCount = string.Format("select  count(1) FROM {0}", strFromWhere);
            Total = db.Database.SqlQuery<int>(sqlCount).FirstOrDefault();
            if (1 >= PageIndex)
            {
                string sql = string.Format("SELECT TOP  {0} {1} from {2} order by {3}", PageSize, strFields, strFromWhere, strSort);
                var result = db.Database.SqlQuery<TEntity>(sql).ToList();
                return result;
            }
            else
            {
                int startId = (PageIndex - 1) * PageSize + 1;
                int endId = PageIndex * PageSize;
                string sql = string.Format("SELECT * FROM(SELECT ROW_NUMBER() OVER(ORDER BY  {0} ) AS rownum,{1} FROM {2}) AS XX WHERE rownum BETWEEN {3} AND {4} ORDER BY XX.rownum ASC ",
                    strSort, strFields, strFromWhere, startId, endId);
                var result = db.Database.SqlQuery<TEntity>(sql).ToList();
                return result;
            }
        }
    }
}
