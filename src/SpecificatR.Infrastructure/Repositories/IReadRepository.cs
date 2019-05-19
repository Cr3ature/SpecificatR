using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
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
        /// <param name="id"></param>
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
