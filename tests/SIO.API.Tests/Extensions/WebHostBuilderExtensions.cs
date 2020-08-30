using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SIO.API.Tests.Abstractions;

namespace SIO.API.Tests.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder WithAuthentication(this IWebHostBuilder builder, TestClaimsProvider claimsProvider)
        {
            return builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, AuthenticatedAuthHandler>("Test", op => { });

                services.AddScoped(_ => claimsProvider);
            });
        }

        public static IWebHostBuilder WithAuthentication(this IWebHostBuilder builder)
        {
            return builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, UnauthenticatedAuthHandler>("Test", op => { });
            });
        }

        public static HttpClient CreateClientWithTestAuth<T>(this WebApplicationFactory<T> factory) where T : class
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            return client;
        }
    }
}
