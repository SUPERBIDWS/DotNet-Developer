using System;
using MongoDB.Driver;
using S4Pay.Factory.Repository;

namespace S4Pay.Infrastructure
{
    public abstract class RepositoryBase<TDocument> : IRepositoryBase<TDocument>
    {
        protected abstract string CollectionName { get; }

        protected abstract IMongoCollection<TDocument> Collection { get; }

        public virtual bool Save(TDocument document)
        {
            var insertOneAsync = Collection.InsertOneAsync(document);
            insertOneAsync.Wait();
            return insertOneAsync.IsCompleted;
        }

        public virtual bool Delete(Guid id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id.ToString());
            var deleteOneAsync = Collection.DeleteOneAsync(filter);
            deleteOneAsync.Wait();
            return deleteOneAsync.IsCompleted;
        }

        public virtual bool Set<TValue>(Guid id, string key, TValue value)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", id.ToString());
            var update = Builders<TDocument>.Update.Set(key, value);
            var  updateOneAsync = Collection.UpdateOneAsync(filter, update);
            updateOneAsync.Wait();
            return updateOneAsync.IsCompleted;
        }
    }
}
