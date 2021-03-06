namespace SpecificatR.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="ISpecification{ClassType}"/>.
    /// </summary>
    /// <typeparam name="TClass">The <see cref="TClass"/>.</typeparam>
    public interface ISpecification<TClass>
    {
        /// <summary>
        /// Gets a value indicating whether to only return different values.
        /// </summary>
        bool AsDistinct { get; }

        /// <summary>
        /// Gets a value indicating whether AsTracking.
        /// </summary>
        bool AsTracking { get; }

        /// <summary>
        /// Gets the Criteria.
        /// </summary>
        Expression<Func<TClass, bool>> Criteria { get; }

        /// <summary>
        /// Gets a comparer to set comparing rules to get a distinct result.
        /// </summary>
        IEqualityComparer<TClass> DistinctComparer { get; }

        /// <summary>
        /// Gets a value indicating whether IgnoreQueryFilters.
        /// </summary>
        bool IgnoreQueryFilters { get; }

        /// <summary>
        /// Gets the Includes.
        /// </summary>
        IReadOnlyCollection<Expression<Func<TClass, object>>> Includes { get; }

        /// <summary>
        /// Gets a value indicating whether IsPagingEnabled.
        /// </summary>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// Gets the OrderByExpressions.
        /// </summary>
        IReadOnlyCollection<OrderByExpression<TClass>> OrderByExpressions { get; }

        /// <summary>
        /// Gets the Skip.
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Gets the Take.
        /// </summary>
        int Take { get; }
    }
}
