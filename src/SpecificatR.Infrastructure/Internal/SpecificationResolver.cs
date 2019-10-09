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
    using System.Linq;
    using System.Threading.Tasks;
    using SpecificatR.Infrastructure.Abstractions;

    /// <summary>
    /// Defines the <see cref="ApplySpecification"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type Entity.</typeparam>
    internal class SpecificationResolver<TEntity>
         where TEntity : class
    {
        /// <summary>
        /// The GetResultSetAsync.
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity[]}"/>.</returns>
        public static TEntity[] GetResultSet(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => ApplySpecification(inputQuery: inputQuery, specification: specification).ToArray();

        /// <summary>
        /// The GetSingleResultAsync.
        /// </summary>
        /// <param name="inputQuery">The inputQuery<see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification<see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public static TEntity GetSingleResult(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => ApplySpecification(inputQuery: inputQuery, specification: specification).FirstOrDefault();

        /// <summary>
        /// The ApplySpecification.
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        protected static IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => SpecificationEvaluator<TEntity>.GetQuery(inputQuery: inputQuery, specification: specification);
    }
}
