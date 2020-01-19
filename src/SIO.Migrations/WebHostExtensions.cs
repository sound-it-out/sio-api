using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;

namespace SIO.Migrations
{
    public static class WebHostExtensions
    {
        public static async Task<IWebHost> SeedDatabaseAsync(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<OpenEventSourcingDbContext>())
                    await context.Database.MigrateAsync();

                using (var context = scope.ServiceProvider.GetRequiredService<OpenEventSourcingProjectionDbContext>())
                    await context.Database.MigrateAsync();
            }

            return host;
        }
    }
}
