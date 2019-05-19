using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using SpecificatR.Infrastructure.Internal;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class SpecificationEvaluator<ClassType, IdType>
        where ClassType : class, IBaseEntity<IdType>
    {
        internal static IQueryable<ClassType> GetQuery(IQueryable<ClassType> inputQuery, ISpecification<ClassType> specification)
        {
            IQueryable<ClassType> outputQuery = inputQuery;

            outputQuery = SetCriteria(outputQuery, specification.Criteria);

            outputQuery = SetIncludes(outputQuery, specification);

            outputQuery = SetPaging(outputQuery, specification);

            outputQuery = SetOrderBy(outputQuery, specification);

            outputQuery = SetIgnoreQueryFilters(outputQuery, specification);

            outputQuery = SetTracking(outputQuery, specification);

            return outputQuery;
        }

        internal static (IQueryable<ClassType> query, int filteredCount) GetQueryWithCount(IQueryable<ClassType> inputQuery, ISpecification<ClassType> specification)
        {
            IQueryable<ClassType> outputQuery = inputQuery;

            outputQuery = SetCriteria(outputQuery, specification.Criteria);

            outputQuery = SetIncludes(outputQuery, specification);

            var filteredCount = outputQuery.Count();

            outputQuery = SetPaging(outputQuery, specification);

            outputQuery = SetOrderBy(outputQuery, specification);

            outputQuery = SetIgnoreQueryFilters(outputQuery, specification);

            outputQuery = SetTracking(outputQuery, specification);

            return (outputQuery, filteredCount);
        }

        private static IQueryable<ClassType> SetTracking(IQueryable<ClassType> outputQuery, ISpecification<ClassType> specification)
        {
            if (specification.AsTracking)
            {
                return outputQuery;
            }

            return outputQuery.AsNoTracking();
        }

        private static IQueryable<ClassType> SetIgnoreQueryFilters(IQueryable<ClassType> outputQuery, ISpecification<ClassType> specification)
        {
            if (specification.IgnoreQueryFilters)
            {
                return outputQuery.IgnoreQueryFilters();
            }

            return outputQuery;
        }

        private static IQueryable<ClassType> SetCriteria(IQueryable<ClassType> outputQuery, Expression<Func<ClassType, bool>> criteria)
        {
            if (criteria == null)
            {
                return outputQuery;
            }

            return outputQuery.Where(criteria);
        }

        private static IQueryable<ClassType> SetIncludes(IQueryable<ClassType> outputQuery, ISpecification<ClassType> specification)
        {
            if (specification.Includes == null || !specification.Includes.Any())
            {
                return outputQuery;
            }

            foreach (Expression<Func<ClassType, object>> argument in specification.Includes)
            {
                string include = IncludeExpressionResolver.Resolve(argument);
                outputQuery = outputQuery.Include(include);
            }

            return outputQuery;
        }

        private static IQueryable<ClassType> SetOrderBy(IQueryable<ClassType> outputQuery, ISpecification<ClassType> specification)
        {
            if (specification.OrderByExpressions == null || !specification.OrderByExpressions.Any())
            {
                return outputQuery;
            }

            OrderByExpression<ClassType> firstOrderByExpression = specification.OrderByExpressions.First();

            IOrderedQueryable<ClassType> orderedQuery = firstOrderByExpression.OrderByDirection.Equals(OrderByDirection.Ascending)
                ? outputQuery.OrderBy(firstOrderByExpression.Expression)
                : outputQuery.OrderByDescending(firstOrderByExpression.Expression);

            foreach (OrderByExpression<ClassType> orderByExpression in specification.OrderByExpressions)
            {
                if (orderByExpression.Equals(firstOrderByExpression))
                {
                    continue;
                }

                orderedQuery = orderByExpression.OrderByDirection.Equals(OrderByDirection.Ascending)
                    ? orderedQuery.ThenBy(orderByExpression.Expression)
                    : orderedQuery.ThenByDescending(orderByExpression.Expression);
            }

            return orderedQuery;
        }

        private static IQueryable<ClassType> SetPaging(IQueryable<ClassType> outputQuery, ISpecification<ClassType> specification)
        {
            if (!specification.IsPagingEnabled)
            {
                return outputQuery;
            }

            return outputQuery.Skip(specification.Skip).Take(specification.Take);
        }
    }
}
