using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SIO.API.Tests.Abstractions;
using SIO.Testing.Attributes;

namespace SIO.API.Tests.V1.Translation.TranslationController.ListOptions
{
    public class WhenUserIsUnauthenticated : UnauthenticatedServerSpecification<HttpResponseMessage>
    {
        public WhenUserIsUnauthenticated(ConfigurationFixture configurationFixture, UnauthenticatedAPIWebApplicationFactory webApplicationFactory) : base(configurationFixture, webApplicationFactory)
        {
        }

        protected override async Task<HttpResponseMessage> Given()
        {
            var client = _webApplicationFactory.CreateClient();
            return await client.GetAsync($"/v1/Translation/{nameof(SIO.API.V1.Translation.TranslationController.ListOptions)}");
        }

        protected override Task When()
        {
            return Task.CompletedTask;
        }

        [Integration]
        public void ThenResultShouldHaveUnauthorizedStatusCode()
        {
            Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
