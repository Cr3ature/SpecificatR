//-----------------------------------------------------------------------
// <copyright file="IReadCoreRepository.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:44</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;

    /// <summary>
    /// Defines the <see cref="IReadCoreRepository{TEntity, TDbContext}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TDbContext">The <see cref="TDbContext"/>.</typeparam>
    public interface IReadCoreRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        /// <summary>
        /// Get all entities with optional tracked by EF Core. Default is set to AsNoTracking().
        /// </summary>
        /// <param name="asTracking">As tracking parameter.</param>
        /// <returns>The <see cref="TEntity[]"/>.</returns>
        Task<TEntity[]> GetAll(bool asTracking = false);

        /// <summary>
        /// Get all entities based on specification (Query object).
        /// </summary>
        /// <param name="specification">The <see cref="specification"/>.</param>
        /// <returns>The <see cref="TEntity[]"/>.</returns>
        Task<TEntity[]> GetAll(ISpecification<TEntity> specification);

        /// <summary>
        /// Get entity based on specification (Query object).
        /// </summary>
        /// <param name="specification">Specification parameter.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetSingleWithSpecification(ISpecification<TEntity> specification);

        /// <summary>
        /// Get all entities as readonly objects. Using SqlQuery with Query params.
        /// </summary>
        /// <param name="sqlQuery">Query string.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>The <see cref="TEntity[]"/>.</returns>
        Task<TEntity[]> GetByQueryFromDbSet(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Get single entity as readonly object. Using SqlQuery with Query params.
        /// </summary>
        /// <param name="sqlQuery">Query string.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetSingleByQueryFromDbSet(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Get all entities as readonly objects. Using SqlQuery with Query params.
        /// </summary>
        /// <param name="sqlQuery">Query string.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>The <see cref="TEntity[]"/>.</returns>
        Task<TEntity[]> GetByQueryFromQuerySet(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Get single entity as readonly object. Using SqlQuery with Query params.
        /// </summary>
        /// <param name="sqlQuery">Query string.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetSingleByQueryFromQuerySet(string sqlQuery, params object[] parameters);
    }
}
