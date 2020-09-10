namespace SpecificatR
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Abstractions;

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
