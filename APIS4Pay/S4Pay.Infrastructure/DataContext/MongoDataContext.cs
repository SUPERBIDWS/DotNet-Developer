using System;
using MongoDB.Driver;
using S4Pay.Factory.Helper;

namespace S4Pay.Infrastructure.DataContext
{
    public class MongoDataContext
    {
        public MongoDataContext()
            : this("mongodb")
        {
        }

        public MongoDataContext(string connectionName)
        {
            try
            {
                var url = ConfigurationManagerHelper.GetConnectionString(connectionName);
                var mongoUrl = new MongoUrl(url);
                IMongoClient client = new MongoClient(mongoUrl);
                MongoDatabase = client.GetDatabase(mongoUrl.DatabaseName);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public IMongoDatabase MongoDatabase { get; }
    }
}