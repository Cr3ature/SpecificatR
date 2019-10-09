//-----------------------------------------------------------------------
// <copyright file="UnitTestSpecification.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 19:12:09</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.UnitTest.Abstractions
{
    using System.Linq;
    using System.Threading.Tasks;
    using SpecificatR.Infrastructure.Abstractions;
    using SpecificatR.Infrastructure.Internal;

    /// <summary>
    /// Defines the <see cref="UnitTestSpecification"/>.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TIdentifier">The <see cref="TIdentifier"/>.</typeparam>
    public class SpecificationRepository<TEntity, TIdentifier>
        where TEntity : class, IBaseEntity<TIdentifier>
    {
        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <param name="queryable">    The queryable <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity[]}"/>.</returns>
        public async Task<TEntity[]> GetAll(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            var result = SpecificationResolver<TEntity>.GetResultSet(inputQuery: queryable, specification: specification);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// The GetSingleWithSpecificationAsync.
        /// </summary>
        /// <param name="queryable">    The queryable <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> GetSingleWithSpecification(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            var result = SpecificationResolver<TEntity>.GetSingleResult(inputQuery: queryable, specification: specification);
            return await Task.FromResult(result);
        }
    }
}
