using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;

namespace SIO.Migrations
{
    class Program
    {
        public static void Main(string[] args)
        => CreateHostBuilder(args).Build().Run();

        // EF Core uses this method at design time to access the DbContext
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddTransient<IDesignTimeDbContextFactory<SIOProjectionDbContext>, SIOProjectionDbContextFactory>()
            );
    }

    internal class SIOProjectionDbContextFactory : IDesignTimeDbContextFactory<SIOProjectionDbContext>
    {
        public SIOProjectionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SIOProjectionDbContext>();
            optionsBuilder.UseSqlServer("Server=.,1450;Initial Catalog=sio-api-projections;User Id=sa;Password=1qaz-pl,", b => b.MigrationsAssembly("SIO.Migrations"));

            return new SIOProjectionDbContext(optionsBuilder.Options);
        }
    }
}
