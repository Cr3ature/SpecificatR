namespace SpecificatR
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Abstractions;

    /// <summary>
    /// Defines the <see cref="ReadBaseRepository{TEntity, TIdentifier, TDbContext}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TIdentifier">The <see cref="TIdentifier"/>.</typeparam>
    /// <typeparam name="TDbContext">The <see cref="TDbContext"/>.</typeparam>
    internal class ReadBaseRepository<TEntity, TIdentifier, TDbContext> : ReadCoreRepository<TEntity, TDbContext>, IReadBaseRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadBaseRepository{TEntity, TIdentifier, TDbContext}"/> class.
        /// </summary>
        /// <param name="context">The context <see cref="TDbContext"/>.</param>
        public ReadBaseRepository(TDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// The GetByIdAsync.
        /// </summary>
        /// <param name="id">        The id <see cref="TIdentifier"/>.</param>
        /// <param name="asTracking">The asTracking <see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> GetById(TIdentifier id, bool asTracking = false)
        {
            if (asTracking)
                return await Context.Set<TEntity>().FirstOrDefaultAsync(fod => fod.Id.Equals(id));

            return await Context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
        }
    }
}
