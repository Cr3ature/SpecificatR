using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using SpecificatR.Infrastructure.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class ReadRepository<TEntity, TIdentifier, TDbContext> : IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;

        public ReadRepository(TDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity[]> GetAllAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToArrayAsync();
        }

        public async Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification)
        {
            return await GetResultSetAsync(specification);
        }

        public async Task<TEntity> GetByIdAsync(TIdentifier id)
        {
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
        }

        public async Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification)
        {
            TEntity[] resultSet = await GetResultSetAsync(specification);

            return resultSet.FirstOrDefault();
        }

        protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TIdentifier>.GetQuery(_context.Set<TEntity>().AsQueryable(), specification);
        }

        private async Task<TEntity[]> GetResultSetAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).AsNoTracking().ToArrayAsync();
        }
    }
}
