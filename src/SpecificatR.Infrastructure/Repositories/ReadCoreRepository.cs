using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpecificatR.Abstractions;
using SpecificatR.Infrastructure.Internal;

namespace SpecificatR
{
    internal class ReadCoreRepository<TEntity, TDbContext> : IReadCoreRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
#pragma warning disable SA1401 // Fields should be private
        internal readonly TDbContext Context;

#pragma warning restore SA1401 // Fields should be private

        public ReadCoreRepository(TDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all from DbSet using FromSql.
        /// </summary>
        /// <param name="sqlQuery">  Query string.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>The <see cref="TEntity[]"/>.</returns>
        public async Task<TEntity[]> GetAll(string sqlQuery, params object[] parameters)
            => await Context.Set<TEntity>().FromSqlRaw(sqlQuery, parameters).ToArrayAsync();

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <param name="asTracking">The asTracking <see cref="bool"/>.</param>
        /// <returns>The <see cref="TEntity[]"/>.</returns>
        public async Task<TEntity[]> GetAll(bool asTracking = false)
        {
            if (asTracking)
                return await Context.Set<TEntity>().ToArrayAsync();

            return await Context.Set<TEntity>().AsNoTracking().ToArrayAsync();
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity[]}"/>.</returns>
        public async Task<TEntity[]> GetAll(ISpecification<TEntity> specification)
            => await Task.FromResult(SpecificationResolver<TEntity>.GetAllResult(Context.Set<TEntity>().AsQueryable(), specification));

        /// <summary>
        /// Get all with count with optional tracking.
        /// </summary>
        /// <param name="asTracking"></param>
        /// <returns></returns>
        public async Task<(TEntity[] entities, int entitiesTotalCount)> GetAllWithCount(bool asTracking = false)
        {
            if (asTracking)
            {
                TEntity[] trackedEntities = await Context.Set<TEntity>().ToArrayAsync();
                return (trackedEntities, trackedEntities.Count());
            }

            TEntity[] untrackedEntities = await Context.Set<TEntity>().AsNoTracking().ToArrayAsync();
            return (untrackedEntities, untrackedEntities.Count());
        }

        /// <summary>
        /// Get all with count by specification.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns>The <see cref="Task{(TEntity[] entities, int entitiesTotalCount)}"/>.</returns>
        public async Task<(TEntity[] entities, int entitiesTotalCount)> GetAllWithCount(ISpecification<TEntity> specification)
            => await Task.FromResult(SpecificationResolver<TEntity>.GetAllResultsWithCount(Context.Set<TEntity>().AsQueryable(), specification));

        /// <summary>
        /// Get single or default from DbSet using FromSql.
        /// </summary>
        /// <param name="sqlQuery">  Query string.</param>
        /// <param name="parameters">Querye parameters.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public async Task<TEntity> GetFirstOrDefault(string sqlQuery, params object[] parameters)
            => await Context.Set<TEntity>().FromSqlRaw(sqlQuery, parameters).FirstOrDefaultAsync();

        /// <summary>
        /// The GetSingleWithSpecificationAsync.
        /// </summary>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> GetFirstOrDefault(ISpecification<TEntity> specification)
            => await Task.FromResult(SpecificationResolver<TEntity>.GetFirstOrDefaultResult(Context.Set<TEntity>().AsQueryable(), specification));


        /// <summary>
        /// Get a clean DB Set to make a custom linq query without the specifications.
        /// </summary>
        /// <returns></returns>
        public DbSet<TEntity> Query()
            => Context.Set<TEntity>();
    }
}
