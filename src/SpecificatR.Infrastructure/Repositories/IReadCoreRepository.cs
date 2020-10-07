namespace SpecificatR
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Abstractions;

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
        /// Get all entities as readonly objects. Using SqlQuery with Query params.
        /// </summary>
        /// <param name="sqlQuery">  Query string.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>The <see cref="TEntity[]"/>.</returns>
        Task<TEntity[]> GetAll(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Get single entity as readonly object. Using SqlQuery with Query params.
        /// </summary>
        /// <param name="sqlQuery">  Query string.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetFirstOrDefault(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Get entity based on specification (Query object).
        /// </summary>
        /// <param name="specification">Specification parameter.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetFirstOrDefault(ISpecification<TEntity> specification);


        /// <summary>
        /// Get a clean DB Set to make a custom linq query without the specifications.
        /// </summary>
        /// <returns></returns>
        DbSet<TEntity> Query();
    }
}
