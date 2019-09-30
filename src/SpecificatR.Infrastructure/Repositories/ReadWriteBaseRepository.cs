﻿//-----------------------------------------------------------------------
// <copyright file="ReadWriteBaseRepository.cs" company="David Vanderheyden">
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
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ReadWriteBaseRepository{TEntity, TIdentifier, TDbContext}" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    internal class ReadWriteBaseRepository<TEntity, TIdentifier, TDbContext> : ReadBaseRepository<TEntity, TIdentifier, TDbContext>, IReadWriteBaseRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteBaseRepository{TEntity, TIdentifier, TDbContext}"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="TDbContext"/></param>
        public ReadWriteBaseRepository(TDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// The DeleteByIdAsync
        /// </summary>
        /// <param name="id">The id<see cref="TIdentifier"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task DeleteById(TIdentifier id)
        {
            TEntity entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                throw new NullReferenceException();

            _context.Set<TEntity>().Remove(entity);

            await CommitAsync();
        }

        /// <summary>
        /// The AddAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/></param>
        /// <returns>The <see cref="Task{TEntity}"/></returns>
        public async Task<TEntity> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);

            await CommitAsync();

            return await Task.FromResult(entity);
        }

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public Task Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The UpdateAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public Task Update(TEntity entity)
        {
            _context.Update(entity);

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Update or Add entity accordingly if exists in db.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The <see cref="Task"/></returns>
        public Task AddOrUpdate(TEntity entity)
        {
            var record = GetById(entity.Id, true);
            if(record == null)
            {
                Update(entity);
            }
            else
            {
                _ = Add(entity);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// The UpdateFieldsAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/></param>
        /// <param name="properties">The properties<see cref="Expression{Func{TEntity, object}}[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public Task UpdateFields(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            _context.Attach(entity);

            foreach (Expression<Func<TEntity, object>> property in properties)
            {
                string propertyName = string.Empty;
                Expression bodyExpression = property.Body;
                if (bodyExpression.NodeType.Equals(ExpressionType.Convert) && bodyExpression is UnaryExpression)
                {
                    Expression operand = ((UnaryExpression)property.Body).Operand;
                    propertyName = ((MemberExpression)operand).Member.Name;
                }
                else if (bodyExpression.NodeType.Equals(ExpressionType.MemberAccess) && bodyExpression is MemberExpression)
                {
                    propertyName = ((MemberExpression)property.Body).Member.Name;
                }
                else
                {
                    throw new NotSupportedException();
                }

                _context.Entry(entity).Property(propertyName).IsModified = true;
            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The CommitAsync
        /// </summary>
        /// <returns>The <see cref="Task{int}"/></returns>
        internal async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}