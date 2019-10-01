//-----------------------------------------------------------------------
// <copyright file="ReadCoreRepository.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:44</date>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using SpecificatR.Infrastructure.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class ReadCoreRepository<TEntity, TDbContext> : IReadCoreRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        internal readonly TDbContext _context;

        public ReadCoreRepository(TDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all from DbSet using FromSql
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity[]> GetByQueryFromDbSet(string sqlQuery, params object[] parameters)
            => await _context.Set<TEntity>().FromSql(sqlQuery, parameters).ToArrayAsync();

        /// <summary>
        /// Get single or default from DbSet using FromSql
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> GetSingleByQueryFromDbSet(string sqlQuery, params object[] parameters)
            => await _context.Set<TEntity>().FromSql(sqlQuery, parameters).SingleOrDefaultAsync();

        /// <summary>
        /// Get all from Query set using FromSql
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity[]> GetByQueryFromQuerySet(string sqlQuery, params object[] parameters)
            => await _context.Query<TEntity>().FromSql(sqlQuery, parameters).ToArrayAsync();

        /// <summary>
        /// Get single or default from Query set using FromSql
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<TEntity> GetSingleByQueryFromQuerySet(string sqlQuery, params object[] parameters)
            => await _context.Query<TEntity>().FromSql(sqlQuery, parameters).SingleOrDefaultAsync();

        /// <summary>
        /// The GetAllAsync
        /// </summary>
        /// <param name="asTracking">The asTracking <see cref="bool"/></param>
        /// <returns>The <see cref="Task{TEntity[]}"/></returns>
        public async Task<TEntity[]> GetAll(bool asTracking = false)
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
        public async Task<TEntity[]> GetAll(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity>.GetResultSet(_context.Set<TEntity>().AsQueryable(), specification));
        }

        /// <summary>
        /// The GetSingleWithSpecificationAsync
        /// </summary>
        /// <param name="specification">The specification <see cref="ISpecification{TEntity}"/></param>
        /// <returns>The <see cref="Task{TEntity}"/></returns>
        public async Task<TEntity> GetSingleWithSpecification(ISpecification<TEntity> specification)
        {
            return await Task.FromResult(SpecificationResolver<TEntity>.GetSingleResultAsync(_context.Set<TEntity>().AsQueryable(), specification));
        }
    }
}
