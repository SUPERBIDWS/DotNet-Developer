namespace TesteSuperbid.WebAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TesteSuperbid.WebAPI.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TesteSuperbid.WebAPI.Models.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TesteSuperbid.WebAPI.Models.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            Account account1 = new Account()
            {
                Id = 1,
                Balance = 100000
            };
            Account account2 = new Account()
            {
                Id = 2,
                Balance = 10000
            };
            Account account3 = new Account()
            {
                Id = 3,
                Balance = 0
            };
            Account account4 = new Account()
            {
                Id = 4,
                Balance = 0
            };

            context.Accounts.AddOrUpdate(account1, account2, account3, account4);
        }
    }
}
