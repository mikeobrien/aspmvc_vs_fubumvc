using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Core.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private static readonly PropertyInfo KeyProperty = typeof(TEntity).GetProperties().First(x => x.Name == "Id");
        private static readonly Func<TEntity, Guid> GetKey = x => (Guid)KeyProperty.GetValue(x, new object[] { });
        private static readonly Action<TEntity, Guid> SetKey = (entity, key) => KeyProperty.SetValue(entity, key, null);
        private static readonly Lazy<IEnumerable<TEntity>> Data = new Lazy<IEnumerable<TEntity>>(LoadDataFromEmbeddedResource); 

        private readonly IList<TEntity> _entities;

        public Repository()
        {
            _entities = new List<TEntity>(Data.Value);
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

        public TEntity Add(TEntity entity)
        {
            SetKey(entity, Guid.NewGuid());
            _entities.Add(entity);
            return entity;
        }

        public void Modify(TEntity entity)
        {
            Delete(GetKey(entity));
            Add(entity);
        }

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

        private static IEnumerable<TEntity> LoadDataFromEmbeddedResource()
        {
            var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().
                FirstOrDefault(x => x.EndsWith(typeof(TEntity).Name + ".xml", true, CultureInfo.InvariantCulture));
            if (resourceName == null) yield break;
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {   
                var data = XDocument.Load(resource);
                if (data.Root == null) yield break;
                var entityProperties = typeof(TEntity).GetProperties();
                foreach (var element in data.Root.Elements(typeof(TEntity).Name))
                {
                    var entity = new TEntity();
                    foreach (var attribute in element.Attributes())
                    {
                        var property = entityProperties.FirstOrDefault(x => x.Name == attribute.Name);
                        var value = property.PropertyType == typeof(Guid) ? 
                            Guid.Parse(attribute.Value) : Convert.ChangeType(attribute.Value, property.PropertyType);
                        if (property != null) property.SetValue(entity, value, new object[] { });
                    }
                    yield return entity;
                }
            }
        }
    }
}