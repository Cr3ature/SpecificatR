//-----------------------------------------------------------------------
// <copyright file="ApplySpecification.cs">>
//     Copyright (c) 2019-2020 David Vanderheyden All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Internal
{
    using System.Linq;
    using System.Threading.Tasks;
    using SpecificatR.Abstractions;

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
        public static TEntity[] GetAllResult(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => ApplySpecification(inputQuery: inputQuery, specification: specification).ToArray();

        /// <summary>
        /// The GetSingleResultAsync.
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public static TEntity GetFirstOrDefaultResult(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
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
