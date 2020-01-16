using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SIO.Domain.Projections;

namespace SIO.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateWebHostBuilder(args).Build().RunAsync();
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
