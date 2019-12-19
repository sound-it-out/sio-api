using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SIO.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSentry(options =>
                {
#if DEBUG
                    options.Debug = true;
#endif
                })
                .UseDefaultServiceProvider((context) =>
                {
                    context.ValidateScopes = false;
                })
                .UseStartup<Startup>();
    }
}
