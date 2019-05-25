//-----------------------------------------------------------------------
// <copyright file="IReadRepository.cs" company="David Vanderheyden">
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
    /// Defines the <see cref="IReadRepository{TEntity, TIdentifier, TDbContext}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Get all entities with optional tracked by EF Core. Default is set to AsNoTracking().
        /// </summary>
        /// <param name="asTracking"></param>
        /// <returns></returns>
        Task<TEntity[]> GetAllAsync(bool asTracking = false);

        /// <summary>
        /// Get all entities based on specification (Query object).
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification);

        /// <summary>
        /// Get entity with optional tracked by EF Core by Id. Default is set to AsNoTracking().
        /// </summary>
        /// <param name="id">        </param>
        /// <param name="asTracking"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(TIdentifier id, bool asTracking = false);

        /// <summary>
        /// Get entity based on specification (Query object).
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification);
    }
}
