using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using SpecificatR.Infrastructure.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class ReadRepository<TEntity, TIdentifier> : IReadRepository<TEntity, TIdentifier>
        where TEntity : class, IBaseEntity<TIdentifier>
    {
        protected readonly DbContextResolver ContextResolver;

        public ReadRepository(DbContextResolver contextResolver)
        {
            ContextResolver = contextResolver;
        }

        public async Task<TEntity[]> GetAllAsync()
        {
            return await ContextResolver().Set<TEntity>().AsNoTracking().ToArrayAsync();
        }

        public async Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification)
        {
            return await GetResultSetAsync(specification);
        }

        public async Task<TEntity> GetByIdAsync(TIdentifier id)
        {
            return await ContextResolver().Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
        }

        public async Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification)
        {
            TEntity[] resultSet = await GetResultSetAsync(specification);

            return resultSet.FirstOrDefault();
        }

        protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TIdentifier>.GetQuery(ContextResolver().Set<TEntity>().AsQueryable(), specification);
        }

        private async Task<TEntity[]> GetResultSetAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).AsNoTracking().ToArrayAsync();
        }
    }
}
