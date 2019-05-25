//-----------------------------------------------------------------------
// <copyright file="ISpecification.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:47</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="ISpecification{ClassType}" />
    /// </summary>
    /// <typeparam name="ClassType"></typeparam>
    public interface ISpecification<ClassType>
    {
        /// <summary>
        /// Gets a value indicating whether AsTracking
        /// </summary>
        bool AsTracking { get; }

        /// <summary>
        /// Gets the Criteria
        /// </summary>
        Expression<Func<ClassType, bool>> Criteria { get; }

        /// <summary>
        /// Gets a value indicating whether IgnoreQueryFilters
        /// </summary>
        bool IgnoreQueryFilters { get; }

        /// <summary>
        /// Gets the Includes
        /// </summary>
        IReadOnlyCollection<Expression<Func<ClassType, object>>> Includes { get; }

        /// <summary>
        /// Gets a value indicating whether IsPagingEnabled
        /// </summary>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// Gets the OrderByExpressions
        /// </summary>
        IReadOnlyCollection<OrderByExpression<ClassType>> OrderByExpressions { get; }

        /// <summary>
        /// Gets the Skip
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Gets the Take
        /// </summary>
        int Take { get; }
    }
}
