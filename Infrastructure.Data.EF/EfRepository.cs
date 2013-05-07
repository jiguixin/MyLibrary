using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Domain.Seedwork;
using Domain.Seedwork.Specification;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Data.Core;
using Infrastructure.Data.Ef.DbContextProvider;
using Infrastructure.Data.Ef.Resources;

namespace Infrastructure.Data.Ef
{
    public abstract class EfRepository<TEntity> :IRepository<TEntity> where TEntity : Entity
    {
        private readonly string _connectionStringName;
        private DbContext _dbContext;
        private IUnitOfWork _UnitOfWork;

        public EfRepository(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        private DbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    if (string.IsNullOrEmpty(_connectionStringName))
                        throw new Exception("_connectionStringName不能为空");
                    _dbContext = DbContextManager.CurrentByKey(_connectionStringName);
                }
                return _dbContext;
            }
        }
        #region Implementation of IRepository<TEntity>

        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _UnitOfWork ?? (_UnitOfWork = new EfUnitOfWork(this.DbContext)); }
        }

        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        public virtual void Add(TEntity item)
        { 
            if (item != (TEntity)null)
                GetSet().Add(item); // add new item in this set
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Messages.info_CannotAddNullEntity, typeof(TEntity).ToString());
            } 
        }

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        public virtual void Remove(TEntity item)
        {
            if (item != (TEntity)null)
            {  
                //set as "removed"
                GetSet().Remove(item);
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        public void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            var list = GetSet().Where(predicate);
            foreach (var entity in list)
            {
                GetSet().Remove(entity);
            }
        }
         
        public virtual void Modify(TEntity item)
        {
            if (item != (TEntity)null)
            {
                DbContext.Entry<TEntity>(item).State = System.Data.EntityState.Modified; 
            } 
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        } 

        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">entity key values, the order the are same of order in mapping.</param>
        /// <returns></returns>
        public virtual TEntity Get(Guid id)
        {
            if (id != Guid.Empty)
                return GetSet().Find(id);
            else
                return null;
        }

        /// <summary>
        /// 根据条件获取唯一的实体
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>实体</returns>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().SingleOrDefault(predicate);
        }

        /// <summary>
        /// 根据条件获取第一条实体
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>实体</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet().AsEnumerable();
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns>总数</returns>
        public int Count()
        {
            return GetSet().Count();
        }

        /// <summary>
        /// 根据条件获取总数
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>总数</returns>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().Count(predicate); 
        }

        /// <summary>
        /// Get all elements of type {T} that matching a
        /// Specification <paramref name="specification"/>
        /// </summary>
        /// <param name="specification">Specification that result meet</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> AllMatching(Domain.Seedwork.Specification.ISpecification<TEntity> specification)
        {
            return GetSet().Where(specification.SatisfiedBy())
                           .AsEnumerable();
        }
        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        public virtual IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsEnumerable();
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsEnumerable();
            }
        }

        /// <summary>
        /// Get  elements of type {T} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        public virtual IEnumerable<TEntity> GetFiltered(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter)
                           .AsEnumerable();
        } 

        #endregion

        private DbSet<TEntity> GetSet()
        {
            return DbContext.Set<TEntity>();
        }

        #region Implementation of ISql

        public IEnumerable<TEntity1> ExecuteQuery<TEntity1>(string sqlQuery, params object[] parameters)
        {
            return DbContext.Database.SqlQuery<TEntity1>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #endregion 
    }
}
