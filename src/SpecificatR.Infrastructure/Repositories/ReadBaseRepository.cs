//-----------------------------------------------------------------------
// <copyright file="ReadBaseRepository.cs" company="David Vanderheyden">
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
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ReadBaseRepository{TEntity, TIdentifier, TDbContext}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    internal class ReadBaseRepository<TEntity, TIdentifier, TDbContext> : ReadCoreRepository<TEntity, TDbContext>, IReadBaseRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadBaseRepository{TEntity, TIdentifier, TDbContext}"/> class.
        /// </summary>
        /// <param name="context">The context <see cref="TDbContext"/></param>
        public ReadBaseRepository(TDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// The GetByIdAsync
        /// </summary>
        /// <param name="id">        The id <see cref="TIdentifier"/></param>
        /// <param name="asTracking">The asTracking <see cref="bool"/></param>
        /// <returns>The <see cref="Task{TEntity}"/></returns>
        public async Task<TEntity> GetById(TIdentifier id, bool asTracking = false)
        {
            if (asTracking)
            {
                return await _context.Set<TEntity>().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
            }

            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(fod => fod.Id.Equals(id));
        }
    }
}
