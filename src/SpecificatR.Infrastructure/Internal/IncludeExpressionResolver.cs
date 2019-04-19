using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpecificatR.Infrastructure.Internal
{
    internal static class IncludeExpressionResolver
    {
        public static string Resolve<TEntity>(Expression<Func<TEntity, object>> includeExpression)
        {
            IEnumerable<LambdaExpression> Lambdas(LambdaExpression lambda)
            {
                var method = lambda.Body as MethodCallExpression;
                while (method != null)
                {
                    yield return Expression.Lambda(method.Arguments[0], lambda.Parameters[0]);
                    lambda = (LambdaExpression)method.Arguments[1];
                    method = lambda.Body as MethodCallExpression;
                }

                yield return lambda;
            }

            IEnumerable<string> PropertyNames(IEnumerable<LambdaExpression> lambdas)
            {
                foreach (LambdaExpression lambda in lambdas)
                {
                    var member = (MemberExpression)lambda.Body;
                    Expression expression = member.Expression;
                    while (expression is MemberExpression childMember)
                    {
                        yield return childMember.Member.Name;
                        expression = childMember.Expression;
                    }

                    yield return member.Member.Name;
                }
            }

            return string.Join(".", PropertyNames(Lambdas(includeExpression)));
        }
    }
}
