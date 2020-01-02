using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SIO.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
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
