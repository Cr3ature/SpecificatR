//-----------------------------------------------------------------------
// <copyright file="IReadBaseRepository.cs" company="David Vanderheyden">
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
    /// Defines the <see cref="IReadBaseRepository{TEntity, TIdentifier, TDbContext}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TIdentifier">Identifier type.</typeparam>
    /// <typeparam name="TDbContext">DbContext type.</typeparam>
    public interface IReadBaseRepository<TEntity, TIdentifier, TDbContext> : IReadCoreRepository<TEntity, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Get entity with optional tracked by EF Core by Id. Default is set to AsNoTracking().
        /// </summary>
        /// <param name="id">        Identifier.</param>
        /// <param name="asTracking">AsTracking bool.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetById(TIdentifier id, bool asTracking = false);
    }
}
