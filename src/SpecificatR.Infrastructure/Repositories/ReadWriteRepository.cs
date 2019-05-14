using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using SpecificatR.Infrastructure.Internal;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class ReadWriteRepository<TEntity, TIdentifier> : ReadRepository<TEntity, TIdentifier>, IReadWriteRepository<TEntity, TIdentifier>
        where TEntity : class, IBaseEntity<TIdentifier>
    {
        public ReadWriteRepository(DbContextResolver context)
            : base(context)
        {
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            ContextResolver().Set<TEntity>().Add(entity);

            await CommitAsync();

            return await Task.FromResult(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            ContextResolver().Set<TEntity>().Remove(entity);

            ContextResolver().SaveChanges();

            return Task.CompletedTask;
        }

        public async Task DeleteByIdAsync(TIdentifier id)
        {
            TEntity entity = await ContextResolver().Set<TEntity>().FindAsync(id);

            if (entity == null)
                throw new NullReferenceException();

            ContextResolver().Set<TEntity>().Remove(entity);

            await CommitAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            ContextResolver().Update(entity);

            ContextResolver().SaveChanges();

            return Task.CompletedTask;
        }

        public Task UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            ContextResolver().Attach(entity);

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

                ContextResolver().Entry(entity).Property(propertyName).IsModified = true;
            }

            ContextResolver().SaveChanges();

            return Task.CompletedTask;
        }

        internal async Task<int> CommitAsync()
        {
            return await ContextResolver().SaveChangesAsync();
        }
    }
}
