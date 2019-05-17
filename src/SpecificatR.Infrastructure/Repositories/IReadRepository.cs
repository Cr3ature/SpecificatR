using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    public interface IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        Task<TEntity[]> GetAllAsync();

        Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification);

        Task<TEntity> GetByIdAsync(TIdentifier id);

        Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification);
    }
}
