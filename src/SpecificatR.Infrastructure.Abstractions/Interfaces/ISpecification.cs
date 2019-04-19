using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpecificatR.Infrastructure.Abstractions
{
    public interface ISpecification<ClassType>
    {
        Expression<Func<ClassType, bool>> Criteria { get; }

        IReadOnlyCollection<Expression<Func<ClassType, object>>> Includes { get; }

        bool IsPagingEnabled { get; }

        bool IsReadOnly { get; }

        IReadOnlyCollection<OrderByExpression<ClassType>> OrderByExpressions { get; }

        int Skip { get; }

        int Take { get; }
    }
}
