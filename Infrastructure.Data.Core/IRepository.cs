using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Seedwork;
using Domain.Seedwork.Specification;

namespace Infrastructure.Data.Core
{
    public interface IRepository<TEntity>:ISql
        where TEntity : Entity
    {
        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Add(TEntity item);

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Remove(TEntity item);

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        void Remove(Expression<Func<TEntity, bool>> predicate);
         
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="item"></param>
        void Modify(TEntity item);
         
        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">entity key values, the order the are same of order in mapping.</param>
        /// <returns></returns>
        TEntity Get(Guid id);

        /// <summary>
        /// 根据条件获取唯一的实体
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>实体</returns>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获取第一条实体
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>实体</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
         
        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns>总数</returns>
        int Count();

        /// <summary>
        /// 根据条件获取总数
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>总数</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get all elements of type {T} that matching a
        /// Specification <paramref name="specification"/>
        /// </summary>
        /// <param name="specification">Specification that result meet</param>
        /// <returns></returns>
        IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification);

        /// <summary>
        /// Get all elements of type {T} in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        /// <summary>
        /// Get  elements of type {T} in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
    }
}