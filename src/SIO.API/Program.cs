using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SIO.Domain.Projections;
using SIO.Migrations;

namespace SIO.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateWebHostBuilder(args).Build();

                await host.SeedDatabaseAsync();
                await host.RunAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(false)
                .UseSentry(options =>
                {
#if DEBUG
                    options.Debug = true;
#endif
                })
                .UseStartup<Startup>();
    }
}
