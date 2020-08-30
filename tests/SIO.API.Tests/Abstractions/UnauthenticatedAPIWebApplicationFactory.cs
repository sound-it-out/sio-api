using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SIO.API.Tests.Extensions;

namespace SIO.API.Tests.Abstractions
{
    public class UnauthenticatedAPIWebApplicationFactory : APIWebApplicationFactory
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.WithAuthentication();
            base.ConfigureWebHost(builder);
        }
    }
}
