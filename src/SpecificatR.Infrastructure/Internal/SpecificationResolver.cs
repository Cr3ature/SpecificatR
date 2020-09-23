namespace SpecificatR.Infrastructure.Internal
{
    using System.Linq;
    using System.Threading.Tasks;
    using SpecificatR.Abstractions;

    /// <summary>
    /// Defines the <see cref="ApplySpecification"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type Entity.</typeparam>
    internal class SpecificationResolver<TEntity>
         where TEntity : class
    {
        /// <summary>
        /// The GetResultSetAsync.
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="Task{TEntity[]}"/>.</returns>
        public static TEntity[] GetAllResult(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => ApplySpecification(inputQuery: inputQuery, specification: specification).ToArray();

        /// <summary>
        /// GetAll with filteredCount by evaluated specification.
        /// </summary>
        /// <param name="inputQuery"></param>
        /// <param name="specification"></param>
        /// <returns>The <see cref="Task{(TEntity[] entities, int filteredCount)}"/>.</returns>
        public static (TEntity[] entities, int filteredCount) GetAllResultsWithCount(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            (IQueryable<TEntity> query, int filteredCount) = ApplySpecificationWithCount(inputQuery: inputQuery, specification: specification);

            return (query.ToArray(), filteredCount);
        }

        /// <summary>
        /// The GetSingleResultAsync.
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public static TEntity GetFirstOrDefaultResult(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => ApplySpecification(inputQuery: inputQuery, specification: specification).FirstOrDefault();

        /// <summary>
        /// The ApplySpecification.
        /// </summary>
        /// <param name="inputQuery">   The inputQuery <see cref="IQueryable{TEntity}"/>.</param>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/>.</param>
        /// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        protected static IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => SpecificationEvaluator<TEntity>.GetQuery(inputQuery: inputQuery, specification: specification);

        protected static (IQueryable<TEntity> query, int filteredCount) ApplySpecificationWithCount(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
            => SpecificationEvaluator<TEntity>.GetQueryWithCount(inputQuery: inputQuery, specification: specification);
    }
}
