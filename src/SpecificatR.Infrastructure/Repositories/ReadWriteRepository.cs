using Microsoft.EntityFrameworkCore;
using SpecificatR.Infrastructure.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpecificatR.Infrastructure.Repositories
{
    internal class ReadWriteRepository<TEntity, TIdentifier, TDbContext> : ReadRepository<TEntity, TIdentifier, TDbContext>, IReadWriteRepository<TEntity, TIdentifier, TDbContext>
        where TEntity : class, IBaseEntity<TIdentifier>
        where TDbContext : DbContext
    {
        public ReadWriteRepository(TDbContext context)
            : base(context)
        {
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);

            await CommitAsync();

            return await Task.FromResult(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task DeleteByIdAsync(TIdentifier id)
        {
            TEntity entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity == null)
                throw new NullReferenceException();

            _context.Set<TEntity>().Remove(entity);

            await CommitAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _context.Update(entity);

            _context.SaveChanges();

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

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        internal async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
