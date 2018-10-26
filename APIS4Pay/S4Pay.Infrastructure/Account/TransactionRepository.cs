using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using S4Pay.Domain.Account;
using S4Pay.Factory.Repository;
using S4Pay.Infrastructure.DataContext;

namespace S4Pay.Infrastructure.Account
{
    public class TransactionRepository : RepositoryBase<BsonDocument>, ITransactionRepository
    {
        private readonly MongoDataContext _mongoDataContext;
        public TransactionRepository(MongoDataContext mongoDataContext)
        {
            _mongoDataContext = mongoDataContext;
        }
        protected override string CollectionName => "Transaction";
        protected override IMongoCollection<BsonDocument> Collection =>
            _mongoDataContext.MongoDatabase.GetCollection<BsonDocument>(CollectionName);


        public bool Save(Transaction transaction, bool pending = true)
        {
            var document = new BsonDocument
            {
                {"_id", Guid.NewGuid().ToString() },
                {"UserId", transaction.UserId.ToString() },
                {"AnotherUserId", transaction.AnotherUserId.ToString() },
                {"Value", transaction.Value },
                {"Pending", pending},
                {"TransactionDate", DateTime.Now.ToString("s") }
            };
            var isCompleted = Save(document);
            return isCompleted;
        }

        public bool Approve(Guid transactionId)
        {
            var isCompleted = Set(transactionId, "Pending", false);
            return isCompleted;
        }

        public bool Disapprove(Guid transactionId)
        {
            var isCompleted = Delete(transactionId);
            return isCompleted;
        }

        public ExpandoObject Get(Guid transactionId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", transactionId.ToString());
            var document = Collection.Find(filter).SingleOrDefault();
            var result = BsonSerializer.Deserialize<ExpandoObject>(document);
            return result;
        }

        public IEnumerable<ExpandoObject> GetTransactionPending(Guid userId)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("UserId", userId.ToString()),
                    Builders<BsonDocument>.Filter.Eq("Pending", true));
            var document = Collection.Find(filter).ToList();
            var result = document?.Select(x => BsonSerializer.Deserialize<ExpandoObject>(x));
            return result;
        }

        public IEnumerable<ExpandoObject> GetTransactionApproved(Guid userId)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UserId", userId.ToString()),
                Builders<BsonDocument>.Filter.Eq("Pending", false));
            var document = Collection.Find(filter).ToList();
            var result = document?.Select(x => BsonSerializer.Deserialize<ExpandoObject>(x));
            return result;
        }
    }
}