using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SIO.API.Tests.Abstractions;
using SIO.API.Tests.Extensions;
using SIO.API.V1.Translation.Responses;
using SIO.Testing.Attributes;

namespace SIO.API.Tests.V1.Translation.TranslationController.ListOptions
{
    public class WhenUserIsAuthenticated : AuthenticatedServerSpecification<HttpResponseMessage>
    {
        public WhenUserIsAuthenticated(ConfigurationFixture configurationFixture, AuthenticatedAPIWebApplicationFactory webApplicationFactory) : base(configurationFixture, webApplicationFactory)
        {
        }

        protected override void BuildHost(IWebHostBuilder builder)
        {
            base.BuildHost(builder);
        }

        protected override async Task<HttpResponseMessage> Given()
        {
            var client = _webApplicationFactory.CreateClientWithTestAuth();
            return await client.GetAsync($"/v1/Translation/{nameof(SIO.API.V1.Translation.TranslationController.ListOptions)}");
        }

        protected override Task When()
        {
            return Task.CompletedTask;
        }

        [Integration]
        public async Task ThenResultShouldContainTranslationOptions()
        {
            var @json = await Result.Content.ReadAsStringAsync();
            var listOptionsResponse = JsonConvert.DeserializeObject<ListOptionsResponse>(json);
            listOptionsResponse.Options.Should().NotBeEmpty();
        }

        [Integration]
        public void ThenResultShouldHaveSuccessCode()
        {
            Result.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
