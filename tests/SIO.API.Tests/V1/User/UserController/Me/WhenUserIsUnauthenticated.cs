using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SIO.API.Tests.Abstractions;
using SIO.Testing.Attributes;

namespace SIO.API.Tests.V1.User.UserController.Me
{
    public class WhenUserIsUnauthenticated : UnauthenticatedServerSpecification<HttpResponseMessage>
    {
        public WhenUserIsUnauthenticated(ConfigurationFixture configurationFixture, EventSeederFixture eventSeederFixture, UnauthenticatedAPIWebApplicationFactory webApplicationFactory) : base(configurationFixture, eventSeederFixture, webApplicationFactory)
        {
        }

        protected override async Task<HttpResponseMessage> Given()
        {
            var client = _webApplicationFactory.CreateClient();
            return await client.GetAsync($"/v1/User/{nameof(SIO.API.V1.User.UserController.Me)}");
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
