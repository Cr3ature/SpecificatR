using System;
using System.Linq.Expressions;

namespace SpecificatR.Infrastructure.Abstractions
{
    public class OrderByExpression<T>
    {
        public OrderByExpression(Expression<Func<T, object>> expression, OrderByDirection orderByDirection)
        {
            Expression = expression;
            OrderByDirection = orderByDirection;
        }

        public Expression<Func<T, object>> Expression { get; set; }

        public OrderByDirection OrderByDirection { get; set; }
    }
}
