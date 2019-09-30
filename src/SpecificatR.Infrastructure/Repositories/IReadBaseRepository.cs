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
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IReadBaseRepository{TEntity, TIdentifier, TDbContext}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IReadBaseRepository<TEntity, TIdentifier, TDbContext> : IReadCoreRepository<TEntity, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Get entity with optional tracked by EF Core by Id. Default is set to AsNoTracking().
        /// </summary>
        /// <param name="id">        </param>
        /// <param name="asTracking"></param>
        /// <returns></returns>
        Task<TEntity> GetById(TIdentifier id, bool asTracking = false);
    }
}
