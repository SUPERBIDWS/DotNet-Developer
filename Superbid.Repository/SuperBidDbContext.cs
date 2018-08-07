using Blueshift.EntityFrameworkCore.MongoDB.Annotations;
using Microsoft.EntityFrameworkCore;
using Superbid.Domain.DomainModels;

namespace Superbid.Repository
{
    [MongoDatabase("superbid")]
    public class SuperBidDbContext : DbContext
    {
        public DbSet<Account> Account { get; set; }

        public SuperBidDbContext()
            : this(new DbContextOptions<SuperBidDbContext>())
        {
        }

        public SuperBidDbContext(DbContextOptions<SuperBidDbContext> superbiDbContextOptions)
            : base(superbiDbContextOptions)
        {
        }
    }
}