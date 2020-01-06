//-----------------------------------------------------------------------
// <copyright file="IReadWriteCoreRepository.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:44</date>
//-----------------------------------------------------------------------

namespace SpecificatR
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="IReadWriteCoreRepository{TEntity, TIdentifier, TDbContext}" />.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TDbContext">The <see cref="TDbContext"/>.</typeparam>
    public interface IReadWriteCoreRepository<TEntity, TDbContext> : IReadCoreRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        /// <summary>
        /// Add entity to database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// Delete entity on database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete(TEntity entity);

        /// <summary>
        /// Update all properties of a entity in database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Update(TEntity entity);

        /// <summary>
        /// Update specific properties of a entity in database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>.</param>
        /// <param name="properties">The <see cref="Expression{Func{TEntity, object}}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateFields(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}
