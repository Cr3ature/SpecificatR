//-----------------------------------------------------------------------
// <copyright file="ReadWriteCoreRepository.cs" company="David Vanderheyden">
//     Copyright (c) 2019 All Rights Reserved
// </copyright>
// <licensed>Distributed under Apache-2.0 license</licensed>
// <author>David Vanderheyden</author>
// <date>25/05/2019 10:10:44</date>
//-----------------------------------------------------------------------

namespace SpecificatR.Infrastructure.Repositories
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="ReadWriteCoreRepository{TEntity, TDbContext}" />.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/>.</typeparam>
    /// <typeparam name="TDbContext">The <see cref="TDbContext"/>.</typeparam>
    internal class ReadWriteCoreRepository<TEntity, TDbContext> : ReadCoreRepository<TEntity, TDbContext>, IReadWriteCoreRepository<TEntity, TDbContext>
        where TEntity : class
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteCoreRepository{TEntity, TDbContext}"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="TDbContext"/>.</param>
        public ReadWriteCoreRepository(TDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// The AddAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        public async Task<TEntity> Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);

            await CommitAsync();

            return await Task.FromResult(entity);
        }

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);

            Context.SaveChanges();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task Update(TEntity entity)
        {
            Context.Update(entity);

            Context.SaveChanges();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The UpdateFieldsAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <param name="properties">The properties<see cref="Expression{Func{TEntity, object}}[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task UpdateFields(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            Context.Attach(entity);

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

                Context.Entry(entity).Property(propertyName).IsModified = true;
            }

            Context.SaveChanges();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The CommitAsync.
        /// </summary>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        internal async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
