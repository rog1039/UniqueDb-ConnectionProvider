using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogero.SqlKeyValue
{
    public interface IKeyValueStore
    {
        void Set(object key, object value);
        T Get<T>(object key);

        Task SetAsync(object key, object value);
        Task<T> GetAsync<T>(object key);
    }

    public interface ICollectionStore
    {
        void Set(object key, object value);
        T Get<T>(object key);

        Task SetAsync(object key, object value);
        Task<T> GetAsync<T>(object key);

        IList<T> GetAll<T>();
        Task<IList<T>> GetAllAsync<T>();
    }

    public class CollectionStore : ICollectionStore
    {
        private readonly string _collectionName;

        public CollectionStore(string collectionName)
        {
            _collectionName = collectionName;
        }

        public void Set(object key, object value)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(object key, object value)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(object key)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }
    }
}
