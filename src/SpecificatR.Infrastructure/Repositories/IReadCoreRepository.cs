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
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IReadCoreRepository{TEntity, TDbContext}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IReadCoreRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        /// <summary>
        /// Get all entities with optional tracked by EF Core. Default is set to AsNoTracking().
        /// </summary>
        /// <param name="asTracking"></param>
        /// <returns></returns>
        Task<TEntity[]> GetAll(bool asTracking = false);

        /// <summary>
        /// Get all entities based on specification (Query object).
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<TEntity[]> GetAll(ISpecification<TEntity> specification);

        /// <summary>
        /// Get entity based on specification (Query object).
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<TEntity> GetSingleWithSpecification(ISpecification<TEntity> specification);

        /// <summary>
        /// Get all entities as readonly objects. Using SqlQuery with Query params.
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TEntity[]> GetBySqlQuery(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Get single entity as readonly object. Using 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TEntity> GetSingleBySqlQuery(string sqlQuery, params object[] parameters);
    }
}
