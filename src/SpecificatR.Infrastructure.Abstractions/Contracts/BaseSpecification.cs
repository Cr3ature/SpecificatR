//-----------------------------------------------------------------------
// <copyright file="BaseSpecification.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:46</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="BaseSpecification{TEntity}" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity>
    {
        /// <summary>
        /// Defines the _includes
        /// </summary>
        private readonly HashSet<Expression<Func<TEntity, object>>> _includes = new HashSet<Expression<Func<TEntity, object>>>();

        /// <summary>
        /// Defines the _orderByExpressions
        /// </summary>
        private readonly HashSet<OrderByExpression<TEntity>> _orderByExpressions = new HashSet<OrderByExpression<TEntity>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecification{TEntity}"/> class.
        /// </summary>
        /// <param name="criteria">The criteria<see cref="Expression{Func{TEntity, bool}}"/></param>
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Gets a value indicating whether AsTracking
        /// </summary>
        public bool AsTracking { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether to only return different values
        /// </summary>
        public bool AsDistinct { get; private set; } = false;

        /// <summary>
        /// Gets the Criteria
        /// </summary>
        public Expression<Func<TEntity, bool>> Criteria { get; }

        /// <summary>
        /// Gets a value indicating whether IgnoreQueryFilters
        /// </summary>
        public bool IgnoreQueryFilters { get; private set; } = false;

        /// <summary>
        /// Gets the Includes
        /// </summary>
        public IReadOnlyCollection<Expression<Func<TEntity, object>>> Includes => _includes;

        /// <summary>
        /// Gets a value indicating whether IsPagingEnabled
        /// </summary>
        public bool IsPagingEnabled { get; private set; } = false;

        /// <summary>
        /// Gets the OrderByExpressions
        /// </summary>
        public IReadOnlyCollection<OrderByExpression<TEntity>> OrderByExpressions => _orderByExpressions;

        /// <summary>
        /// Gets the Skip
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        /// Gets the Take
        /// </summary>
        public int Take { get; private set; }

        /// <summary>
        /// Ignore query filters
        /// <para>If entities are set with additional QueryFilters (<see cref="https://docs.microsoft.com/en-us/ef/core/querying/filters"/>), <c>IgnoreQueryFilters</c> can be used to ignore these set QueryFilters</para>
        /// </summary>
        protected virtual void AddIgnoreQueryFilters()
            => IgnoreQueryFilters = true;

        /// <summary>
        /// To add Specific related entities to include in the query results.
        /// <para>Example: <code>AddInclude(customer => customer.Addresses.Select(address => address.Country);</code></para>
        /// </summary>
        /// <param name="includeExpression"></param>
        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
            => _includes.Add(includeExpression);

        /// <summary>
        /// Adding ordering to the query. First orderby is the first to be handled. Every additional <c>AddOrderBy()</c> will be executed in same order.
        /// <para>Example: <code>AddOrderBy(x => x.Id, OrderByDirection.Ascending);</code></para>
        /// </summary>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByDirection"></param>
        protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByDirection orderByDirection)
            => _orderByExpressions.Add(new OrderByExpression<TEntity>(orderByExpression, orderByDirection));

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
        /// Set retrieved query as tracking in EF Core. By default queries will not be tracked with specifications (AsNoTracking).
        /// </summary>
        protected virtual void ApplyTracking()
            => AsTracking = true;

        /// <summary>
        /// Set query as distinct in EF Core. By default queries will not be distinct with specifications (AsDistinct).
        /// </summary>
        protected virtual void ApplyDistinct()
            => AsDistinct = true;
    }
}
