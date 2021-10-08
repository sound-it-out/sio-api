using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;
using System.Threading.Tasks;

namespace SIO.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var env = host.Services.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment())
                await host.RunProjectionMigrationsAsync();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
