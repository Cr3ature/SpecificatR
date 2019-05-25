//-----------------------------------------------------------------------
// <copyright file="ReadRepository.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:44</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SpecificatR.Infrastructure.Abstractions;
    using SpecificatR.Infrastructure.Internal;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ReadRepository{TEntity, TIdentifier, TDbContext}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    internal class ReadRepository<TEntity, TIdentifier, TDbContext> : IReadRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        protected readonly TDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadRepository{TEntity, TIdentifier, TDbContext}"/> class.
        /// </summary>
        /// <param name="context">The context <see cref="TDbContext"/></param>
        public ReadRepository(TDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The GetAllAsync
        /// </summary>
        /// <param name="asTracking">The asTracking <see cref="bool"/></param>
        /// <returns>The <see cref="Task{TEntity[]}"/></returns>
        public async Task<TEntity[]> GetAllAsync(bool asTracking = false)
        {
            if (asTracking)
            {
                return await _context.Set<TEntity>().ToArrayAsync();
            }

            return await _context.Set<TEntity>().AsNoTracking().ToArrayAsync();
        }

        /// <summary>
        /// The GetAllAsync
        /// </summary>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/></param>
        /// <returns>The <see cref="Task{TEntity[]}"/></returns>
        public async Task<TEntity[]> GetAllAsync(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetResultSet(_context.Set<TEntity>().AsQueryable(), specification));
        }

        /// <summary>
        /// The GetByIdAsync
        /// </summary>
        /// <param name="id">        The id <see cref="TIdentifier"/></param>
        /// <param name="asTracking">The asTracking <see cref="bool"/></param>
        /// <returns>The <see cref="Task{TEntity}"/></returns>
        public async Task<TEntity> GetByIdAsync(TIdentifier id, bool asTracking = false)
        {
            if (asTracking)
            {
                return await _context.Set<TEntity>().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
            }

            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
        }

        /// <summary>
        /// The GetSingleWithSpecificationAsync
        /// </summary>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/></param>
        /// <returns>The <see cref="Task{TEntity}"/></returns>
        public async Task<TEntity> GetSingleWithSpecificationAsync(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity, TIdentifier>.GetSingleResultAsync(_context.Set<TEntity>().AsQueryable(), specification));
        }
    }
}
