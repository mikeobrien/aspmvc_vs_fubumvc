using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Infrastructure.Data;

namespace Tests
{
    public class MemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private static readonly PropertyInfo KeyProperty = typeof(TEntity).GetProperties().First(x => x.Name == "Id");
        private static readonly Func<TEntity, Guid> GetKey = x => (Guid)KeyProperty.GetValue(x, new object[] { });
        private static readonly Action<TEntity, Guid> SetKey = (entity, key) => KeyProperty.SetValue(entity, key, null);

        private readonly IList<TEntity> _entities;

        public MemoryRepository()
        {
            _entities = new List<TEntity>();
        }

        public MemoryRepository(IEnumerable<TEntity> items)
        {
            _entities = new List<TEntity>(items);
        }

        public TEntity Get(Guid id)
        {
            return _entities.FirstOrDefault(x => GetKey(x).Equals(id));
        }

        public TEntity Get(Guid id, bool lazy)
        {
            return lazy ? Get(id) ?? GetLazy(id) : Get(id);
        }

        private static TEntity GetLazy(Guid id)
        {
            var entity = new TEntity();
            SetKey(entity, id);
            return entity;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _entities.FirstOrDefault(filter.Compile());
        }

        public void Add(TEntity entity)
        {
            SetKey(entity, Guid.NewGuid());
            _entities.Add(entity);
        }

        public void Modify(TEntity entity) { }

        public void Delete(Guid id)
        {
            var entity = _entities.FirstOrDefault(x => GetKey(x).Equals(id));
            if (entity != null) _entities.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> filter)
        {
            var entity = _entities.FirstOrDefault(filter.Compile());
            if (entity != null) _entities.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            var entities = _entities.Where(filter.Compile()).ToList();
            if (entities.Any()) entities.ToList().ForEach(x => _entities.Remove(x));
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        public Type ElementType { get { return _entities.AsQueryable().ElementType; } }
        public Expression Expression { get { return _entities.AsQueryable().Expression; } }
        public IQueryProvider Provider { get { return _entities.AsQueryable().Provider; } }
    }
}