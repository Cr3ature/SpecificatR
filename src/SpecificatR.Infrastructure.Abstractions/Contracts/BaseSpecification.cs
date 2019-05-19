using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpecificatR.Infrastructure.Abstractions
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity>
    {
        private readonly HashSet<Expression<Func<TEntity, object>>> _includes = new HashSet<Expression<Func<TEntity, object>>>();
        private readonly HashSet<OrderByExpression<TEntity>> _orderByExpressions = new HashSet<OrderByExpression<TEntity>>();

        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<TEntity, bool>> Criteria { get; }

        public IReadOnlyCollection<Expression<Func<TEntity, object>>> Includes => _includes;

        public bool IsPagingEnabled { get; private set; } = false;

        public bool AsTracking { get; private set; } = false;

        public IReadOnlyCollection<OrderByExpression<TEntity>> OrderByExpressions => _orderByExpressions;

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IgnoreQueryFilters { get; private set; } = false;

        /// <summary>
        /// To add Specific related entities to include in the query results.
        /// <para>Example: <code>AddInclude(customer => customer.Addresses.Select(address => address.Country);</code></para> 
        /// </summary>
        /// <param name="includeExpression"></param>
        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            _includes.Add(includeExpression);
        }

        /// <summary>
        /// Adding ordering to the query. First orderby is the first to be handled. Every additional <c>AddOrderBy()</c> will be executed in same order.
        /// <para>Example: <code>AddOrderBy(x => x.Id, OrderByDirection.Ascending);</code></para>
        /// </summary>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByDirection"></param>
        protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByDirection orderByDirection)
        {
            _orderByExpressions.Add(new OrderByExpression<TEntity>(orderByExpression, orderByDirection));
        }

        /// <summary>
        /// Ignore query filters
        /// <para>If entities are set with additional QueryFilters (<see cref="https://docs.microsoft.com/en-us/ef/core/querying/filters"/>), <c>IgnoreQueryFilters</c> can be used to ignore these set QueryFilters</para>
        /// </summary>
        protected virtual void AddIgnoreQueryFilters()
        {
            IgnoreQueryFilters = true;
        }

        /// <summary>
        /// Apply Paging to the query.
        /// <para>This will add a skip and take to the query based on the parameters (<c>pageIndex</c> and <c>pageSize</c></para>
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        protected virtual void ApplyPaging(int pageIndex, int pageSize)
        {
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
            IsPagingEnabled = true;
        }

        /// <summary>
        /// Set retrieved query as no tracking in EF Core. By default queries will not be tracked with specifications.
        /// </summary>
        protected virtual void ApplyTracking()
        {
            AsTracking = true;
        }
    }
}
