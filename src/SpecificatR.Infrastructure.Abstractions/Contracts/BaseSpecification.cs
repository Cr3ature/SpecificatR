using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpecificatR.Infrastructure.Abstractions.Contracts
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

        public bool IsReadOnly { get; private set; } = false;

        public IReadOnlyCollection<OrderByExpression<TEntity>> OrderByExpressions => _orderByExpressions;

        public int Skip { get; private set; }

        public int Take { get; private set; }

        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            _includes.Add(includeExpression);
        }

        protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, OrderByDirection orderByDirection)
        {
            _orderByExpressions.Add(new OrderByExpression<TEntity>(orderByExpression, orderByDirection));
        }

        protected virtual void ApplyPaging(int pageIndex, int pageSize)
        {
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
            IsPagingEnabled = true;
        }

        protected virtual void ApplyReadOnly()
        {
            IsReadOnly = true;
        }
    }
}
