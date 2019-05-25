//-----------------------------------------------------------------------
// <copyright file="ApplySpecification.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 18:51:47</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Internal
{
    using SpecificatR.Infrastructure.Abstractions;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ApplySpecification"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    internal class SpecificationResolver<TEntity, TIdentifier>
         where TEntity : class, IBaseEntity<TIdentifier>
    {
        /// <summary>
        /// The GetResultSetAsync
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/></param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/></param>
        /// <returns>The <see cref="Task{TEntity[]}"/></returns>
        public static TEntity[] GetResultSet(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var result = ApplySpecification(inputQuery: inputQuery, specification: specification).ToArray();
            return result;
        }

        /// <summary>
        /// The GetSingleResultAsync
        /// </summary>
        /// <param name="inputQuery">The inputQuery<see cref="IQueryable{TEntity}"/></param>
        /// <param name="specification">The specification<see cref="ISpecification{TEntity}"/></param>
        /// <returns>The <see cref="TEntity"/></returns>
        public static TEntity GetSingleResultAsync(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var result = ApplySpecification(inputQuery: inputQuery, specification: specification).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// The ApplySpecification
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/></param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/></param>
        /// <returns>The <see cref="IQueryable{TEntity}"/></returns>
        protected static IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TIdentifier>.GetQuery(inputQuery: inputQuery, specification: specification);
        }
    }
}
