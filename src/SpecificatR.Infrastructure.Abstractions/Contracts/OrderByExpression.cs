//-----------------------------------------------------------------------
// <copyright file="OrderByExpression.cs" >
//     Copyright (c) 2019-2020 David Vanderheyden All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
//-----------------------------------------------------------------------

namespace SpecificatR.Abstractions
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="OrderByExpression{T}"/>.
    /// </summary>
    /// <typeparam name="T">Is the class type of <see cref="T"/>.</typeparam>
    public class OrderByExpression<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByExpression{T}"/> class.
        /// </summary>
        /// <param name="expression">      The expression <see cref="Expression{Func{T, object}}"/>.</param>
        /// <param name="orderByDirection">The orderByDirection <see cref="OrderByDirection"/>.</param>
        public OrderByExpression(Expression<Func<T, object>> expression, OrderByDirection orderByDirection)
        {
            Expression = expression;
            OrderByDirection = orderByDirection;
        }

        /// <summary>
        /// Gets or sets the Expression.
        /// </summary>
        public Expression<Func<T, object>> Expression { get; set; }

        /// <summary>
        /// Gets or sets the OrderByDirection.
        /// </summary>
        public OrderByDirection OrderByDirection { get; set; }
    }
}
