using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class ReadRepository<TEntity, TIdentifier> : IReadRepository<TEntity, TIdentifier>
       where TEntity : class, IBaseEntity<TIdentifier>
    {
        protected readonly DbContext _context;

        public ReadRepository(DbContext context)
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

        protected TEntity[] ApplyProjection(TEntity[] resultSet, ISpecification<TEntity> specification)
        {
            if (specification.Projection == null)
            {
                return resultSet;
            }

            return resultSet.AsQueryable().Select(specification.Projection).ToArray();
        }

        protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity, TIdentifier>.GetQuery(_context.Set<TEntity>().AsQueryable(), specification);
        }

        private async Task<TEntity[]> GetResultSetAsync(ISpecification<TEntity> specification)
        {
            TEntity[] resultSet = await ApplySpecification(specification).AsNoTracking().ToArrayAsync();

            resultSet = ApplyProjection(resultSet, specification);

            return resultSet;
        }
    }
}
