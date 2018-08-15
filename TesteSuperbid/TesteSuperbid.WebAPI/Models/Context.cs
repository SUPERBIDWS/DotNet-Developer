using System.Data.Entity;

namespace TesteSuperbid.WebAPI.Models
{
    public class Context : DbContext
    {
        public Context()
            : base("TesteSuperBidConnectionString")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Context>(null);
            base.OnModelCreating(modelBuilder);
        }

        private DbSet<Account> accounts;

        public DbSet<Account> Accounts
        {
            get { return accounts; }
            set { accounts = value; }
        }

        private DbSet<Transaction> transactions;

        public DbSet<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions = value; }
        }
    }
}