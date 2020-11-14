using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using SIO.API.Tests.Fakes;

namespace SIO.API.Tests.Abstractions
{
    public class APIWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<FakeStartup>();
            builder.ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets(GetType().Assembly, optional: true)
                .AddEnvironmentVariables(prefix: "SIO_");
            });

            base.ConfigureWebHost(builder);
        }
    }
}
