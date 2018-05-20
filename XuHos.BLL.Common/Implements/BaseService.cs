using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using XuHos.Common.Utility;
using XuHos.Common.Cache;
using XuHos.Extensions;
using XuHos.DTO.Common;
using XuHos.DAL.EF;

namespace XuHos.BLL.Common
{
    /// <summary>
    /// 基础业务处理类
    
    /// 日期：2016年7月29日
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CommonBaseService<TEntity> : 
        IBaseService<TEntity>, IDisposable
        where TEntity : class, XuHos.Entity.IAuditableEntity, new()
    {
        DAL.EF.Base.Repository<TEntity> helper = new DAL.EF.Base.Repository<TEntity>();


        

        DBEntities _DBEntities = null;
        /// <summary>
        /// DBEntities 对象
        /// </summary>
        public DBEntities DBEntities
        {
            get
            {
                if (_DBEntities == null)
                    _DBEntities = new DBEntities();
                return _DBEntities;
            }
        }

        public string CurrentOperatorUserID { get; set; }

        public string CurrentOperatorOrgID { get; set; }

        /// <summary>
        /// 当前操作用户的唯一标识
        /// </summary>
        public int CurrentOperatorUserIdentifier
        {
            get
            {
                var UserIDList = new string[] { CurrentOperatorUserID };

                using (DBEntities db = new DBEntities())
                {
                    #region 根据医生编号查询医生的所有用户标识
                    var identifiers = (from user in db.Users.Where(a => !a.IsDeleted)
                                       join userId in UserIDList on user.UserID equals userId
                                       join uid in db.ConversationIMUids.Where(a => !a.IsDeleted) on user.UserID equals uid.UserID
                                       select uid.Identifier).FirstOrDefault();

                    return identifiers;
                    #endregion
                }
            }

        }

        /// <summary>
        /// 设置当前操作的用户编号
        /// </summary>
        /// <param name="CurrentOperatorUserID"></param>
        public CommonBaseService(string CurrentOperatorUserID)
        {
            this.CurrentOperatorUserID = CurrentOperatorUserID;
        }

        /// <summary>
        /// 新增记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Insert(TEntity model)
        {
            model.ModifyTime = model.CreateTime = DateTime.Now;
            model.ModifyUserID = model.CreateUserID = string.IsNullOrEmpty(model.CreateUserID) ? (CurrentOperatorUserID ?? "") : model.CreateUserID;
            return helper.Insert(model);
        }

        /// <summary>
        /// 新增（未提交保存）
        /// 作者：曾璐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual XuHos.DAL.EF.DBEntities PreInsert(XuHos.DAL.EF.DBEntities db, TEntity model)
        {
            model.ModifyTime = model.CreateTime = DateTime.Now;
            model.ModifyUserID = model.CreateUserID = string.IsNullOrEmpty(model.CreateUserID) ? (CurrentOperatorUserID??"") : model.CreateUserID;
            return helper.PreInsert(db, model);
        }


        /// <summary>
        /// 新增记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Insert(params TEntity[] model)
        {

            for (int i = 0; i < model.Length; i++)
            {   
                model[i].ModifyTime = DateTime.Now;
                model[i].ModifyUserID = CurrentOperatorUserID;
            }

            return helper.Insert(model);
        }

        /// <summary>
        /// 更新一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(TEntity model)
        {
            model.ModifyTime = DateTime.Now;
            model.ModifyUserID = string.IsNullOrEmpty(model.ModifyUserID) ? (CurrentOperatorUserID ?? "") : model.ModifyUserID;
            return helper.Update(model);
        }

        /// <summary>
        /// 更新（未提交保存）
        /// 作者：曾璐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual XuHos.DAL.EF.DBEntities PreUpdate(XuHos.DAL.EF.DBEntities db, TEntity model)
        {
            model.ModifyTime = DateTime.Now;
            model.ModifyUserID = string.IsNullOrEmpty(model.ModifyUserID) ? (CurrentOperatorUserID ?? "") : model.ModifyUserID;
            return helper.PreUpdate(db, model);
        }

      
        /// <summary>
        /// 更新一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(string ID, Expression<Func<TEntity, TEntity>> updateExpress)
        {
            return helper.Update(ID, updateExpress);
        }


        /// <summary>
        /// 更新一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="updateExpress">更新表达式</param>
        /// <returns></returns>
        public virtual bool Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> updateExpress)
        {

            return helper.Update(where, updateExpress);
        }


        /// <summary>
        /// 删除记录（逻辑删除）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="ID">主键值</param>
        /// <returns></returns>
        public virtual bool Delete(string ID)
        {
            Expression<Func<TEntity, TEntity>> updateExpress = a => new TEntity()
            {
                DeleteTime = DateTime.Now,
                IsDeleted = true,
            };

            //逻辑删除
            return helper.Update(ID, updateExpress);

        }

        /// <summary>
        /// 删除记录（逻辑删除, 未提交保存）
        /// 作者：曾璐
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool PreDelete(XuHos.DAL.EF.DBEntities db, TEntity model)
        {
            if(model != null)
            {
                model.DeleteTime = DateTime.Now;
                model.DeleteUserID = string.IsNullOrEmpty(model.CreateUserID) ? (CurrentOperatorUserID ?? "") : model.CreateUserID;
                model.IsDeleted = true;
                helper.PreUpdate(db, model);
            }
            return true;
        }

        /// <summary>
        /// 删除记录（逻辑删除）
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<TEntity, bool>> where)
        {
            Expression<Func<TEntity, TEntity>> updateExpress = a => new TEntity()
            {
                DeleteTime = DateTime.Now,
                IsDeleted = true
            };

            return helper.Update(where, updateExpress);
        }

        /// <summary>
        /// 删除记录（物理删除）
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool AbsDelete(Expression<Func<TEntity, bool>> where)
        {
            return helper.Delete(where);
        }


        /// <summary>
        /// 判断记录是否存在
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns></returns>
        public virtual bool Exists(string ID)
        {
            return helper.Exists(ID);
        }


        /// <summary>
        /// 记录是否存在
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            return helper.Exists(where);
        }

        /// <summary>
        /// 获取记录数（不含已删除）
        /// 作者：曾璐
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return helper.Count(where);
        }
        /// <summary>
        /// 获取一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns></returns>
        public virtual TResult Single<TResult>(string ID,
            List<Expression<Func<TEntity, object>>> includes = null,
            params string[] MapIngoreMembers)
        {
            return helper.Single(ID, includes).Map<TEntity, TResult>(MapIngoreMembers);
        }

        /// <summary>
        /// 返回一条记录
        
        /// 日期：2016年7月29日
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual TResult Single<TResult>(Expression<Func<TEntity, bool>> where,
            List<Expression<Func<TEntity, object>>> includes = null,
            params string[] MapIngoreMembers)
        {
            return helper.Single(where, includes).Map<TEntity, TResult>(MapIngoreMembers);
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
        public virtual Response<List<TResult>> GetPageList<TResult>(
            int PageIndex = 1,
            int PageSize = int.MaxValue,
            Expression<Func<TEntity, bool>> where = null,
            Expression<Func<TEntity, object>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null)
            where TResult : class
        {
            if (PageIndex <= 0)
            {
                throw new Exception("PageIndex：必须大于0");
            }

            if (PageSize <= 0)
            {
                throw new Exception("PageSize：必须大于0");
            }

            int totalCount = 0;

            Response<List<TResult>> result = new Response<List<TResult>>();
            var list = helper.Select(out totalCount, PageIndex, PageSize, includes, where, orderBy);
            result.Data = list.Map<List<TEntity>, List<TResult>>();
            result.Total = totalCount;
            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    if (_DBEntities != null)
                    {
                        _DBEntities.Database.Connection.Close();
                    }

                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Channel() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
