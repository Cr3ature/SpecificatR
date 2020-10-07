namespace SpecificatR.UnitTest.Abstractions
{
    using System.Linq;
    using System.Threading.Tasks;
    using SpecificatR.Abstractions;
    using SpecificatR.Infrastructure.Internal;

    /// <summary>
    /// Defines the <see cref="UnitTestSpecification"/>.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TIdentifier">The <see cref="TIdentifier"/>.</typeparam>
    public class SpecificationRepository<TEntity, TIdentifier>
        where TEntity : class, IBaseEntity<TIdentifier>
    {
        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <param name="queryable">    The queryable <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity[]}"/>.</returns>
        public async Task<TEntity[]> GetAll(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            TEntity[] result = SpecificationResolver<TEntity>.GetAllResult(inputQuery: queryable, specification: specification);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Get all with filteredcount by evaluated specification.
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="specification"></param>
        /// <returns>The <see cref="Task{(TEntity[], int)}"/>.</returns>
        public async Task<(TEntity[] entities, int filteredCount)> GetAllWithCount(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            (TEntity[] entities, int filteredCount) result = SpecificationResolver<TEntity>.GetAllResultsWithCount(inputQuery: queryable, specification: specification);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// The GetSingleWithSpecificationAsync.
        /// </summary>
        /// <param name="queryable">    The queryable <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> GetFirstOrDefault(IQueryable<TEntity> queryable, ISpecification<TEntity> specification)
        {
            TEntity result = SpecificationResolver<TEntity>.GetFirstOrDefaultResult(inputQuery: queryable, specification: specification);
            return await Task.FromResult(result);
        }
    }
}
