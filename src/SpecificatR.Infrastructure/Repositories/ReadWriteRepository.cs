using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class ReadWriteRepository<TEntity, TIdentifier, TContext> : ReadRepository<TEntity, TIdentifier, TContext>, IReadWriteRepository<TEntity, TIdentifier, TContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TContext : DbContext
    {
        public ReadWriteRepository(DbContext context)
            : base(context)
        {
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return Task.FromResult(entity);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteByIdAsync(TIdentifier id)
        {
            TEntity entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                throw new NullReferenceException();

            _context.Set<TEntity>().Remove(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateFieldsAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
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

            return Task.CompletedTask;
        }
    }
}
