using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Superbid.Domain.ServicesInterfaces;
using Superbid.Repository;
using Superbid.Services.AccountServices;

namespace Superbid.WebAPI.Configurations
{
    public static class DiExtensions
    {
        public static IServiceCollection Register(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDbConnection");
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<AccountRepository, AccountRepository>();
            services.AddDbContext<SuperBidDbContext>(options => options.UseMongoDb(connectionString));
            // Add all other services here.
            return services;
        }
    }
}