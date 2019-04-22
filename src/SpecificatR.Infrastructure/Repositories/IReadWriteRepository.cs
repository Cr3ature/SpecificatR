using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    public interface IReadWriteRepository<TEntity, TIdentifier, TContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TContext : DbContext
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<int> CommitAsync();

        Task DeleteAsync(TEntity entity);

        Task DeleteByIdAsync(TIdentifier id);

        Task<TEntity[]> GetAllAsync();

        Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification);

        Task<TEntity> GetByIdAsync(TIdentifier id);

        Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification);

        Task UpdateAsync(TEntity entity);

        Task UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}
