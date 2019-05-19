using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    public interface IReadWriteRepository<TEntity, TIdentifier, TDbContext> : IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Add entity to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Delete entity on database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entity on database by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByIdAsync(TIdentifier id);

        /// <summary>
        /// Update all properties of a entity in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Update specific properties of a entity in database
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        Task UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}
