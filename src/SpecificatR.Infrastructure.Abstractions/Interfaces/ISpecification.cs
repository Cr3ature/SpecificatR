using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpecificatR.Infrastructure.Abstractions
{
    public interface ISpecification<ClassType>
    {
        bool AsTracking { get; }

        bool IgnoreQueryFilters { get; }

        Expression<Func<ClassType, bool>> Criteria { get; }

        IReadOnlyCollection<Expression<Func<ClassType, object>>> Includes { get; }

        IReadOnlyCollection<OrderByExpression<ClassType>> OrderByExpressions { get; }

        bool IsPagingEnabled { get; }

        int Skip { get; }

        int Take { get; }
    }
}
