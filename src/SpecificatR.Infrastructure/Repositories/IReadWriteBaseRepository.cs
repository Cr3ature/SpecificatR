//-----------------------------------------------------------------------
// <copyright file="IReadWriteBaseRepository.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:44</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Repositories
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;

    /// <summary>
    /// Defines the <see cref="IReadWriteBaseRepository{TEntity, TIdentifier, TDbContext}" />.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TIdentifier">Identifier of entity type.</typeparam>
    /// <typeparam name="TDbContext">DbContext type.</typeparam>
    public interface IReadWriteBaseRepository<TEntity, TIdentifier, TDbContext> : IReadBaseRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Delete entity on database by Id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task DeleteById(TIdentifier id);

        /// <summary>
        /// Add entity to database.
        /// </summary>
        /// <param name="entity">The <see cref="TEntity"/>..</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
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
        /// <param name="properties">Expressions defining the property fields.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateFields(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}
