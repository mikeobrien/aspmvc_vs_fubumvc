using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Infrastructure.Data
{
    public interface IRepository<TEntity> : IQueryable<TEntity> where TEntity : class, new()
    {
        TEntity Get(Guid id);
        TEntity Get(Guid id, bool lazy);
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        TEntity Add(TEntity entity);
        void Modify(TEntity entity);
        void Delete(TEntity entity);
        void Delete(Guid id);
        void Delete(Expression<Func<TEntity, bool>> filter);
        void DeleteMany(Expression<Func<TEntity, bool>> filter);
    }
}