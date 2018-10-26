using S4Pay.Factory.Repository;
using S4Pay.Factory.Service;
using S4Pay.Infrastructure.Account;
using S4Pay.Infrastructure.DataContext;
using S4Pay.Service.Account;
using SimpleInjector;

namespace S4Pay.CrossCutting
{
    public static class DependencyInjection
    {
        public static void Configure(Container container)
        {
            container.Register(() => new MongoDataContext(), Lifestyle.Scoped);
            container.Register<ITransactionService, TransactionService>();
            container.Register<ITransactionRepository, TransactionRepository>();
        }
    }
}