using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SIO.API.Tests.Abstractions;
using SIO.API.Tests.Extensions;
using SIO.API.V1.User.Responses;
using SIO.Domain.Users.Events;
using SIO.Testing.Attributes;

namespace SIO.API.Tests.V1.User.UserController.Me
{
    public class WhenUserIsAuthenticated : AuthenticatedServerSpecification<HttpResponseMessage>
    {
        private HttpClient _client;
        private Guid _userId;
        private string _email;
        private string _firstname;
        private string _lastname;

        public WhenUserIsAuthenticated(ConfigurationFixture configurationFixture, AuthenticatedAPIWebApplicationFactory webApplicationFactory) : base(configurationFixture, webApplicationFactory)
        {
        }

        protected override void BuildHost(IWebHostBuilder builder)
        {
            base.BuildHost(builder);
        }

        protected override async Task<HttpResponseMessage> Given()
        {            
            return await _client.GetAsync($"/v1/User/{nameof(SIO.API.V1.User.UserController.Me)}");
        }

        protected override async Task When()
        {
            _userId = TestClaimsProvider.UserId;
            _email = "test@test.test";
            _firstname = "test";
            _lastname = "test";

            _client = _webApplicationFactory.CreateClientWithTestAuth();

            var causationId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();

            await EventSeeder.SeedAsync(
                new UserRegistered(_userId, correlationId, causationId, _userId.ToString(), _email, _firstname, _lastname, "asdf"),
                new UserVerified(_userId, correlationId, causationId, _userId.ToString())
            );
        }

        [Integration]
        public async Task ThenResultShouldContainUserResponse()
        {
            var @json = await Result.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(json);
            userResponse.Should().NotBeNull();
        }

        [Integration]
        public async Task ThenResultShouldContainUserResponseWithExpectedId()
        {
            var @json = await Result.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(json);
            userResponse.Id.Should().Be(_userId);
        }

        [Integration]
        public async Task ThenResultShouldContainUserResponseWithExpectedEmail()
        {
            var @json = await Result.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(json);
            userResponse.Email.Should().Be(_email);
        }

        [Integration]
        public async Task ThenResultShouldContainUserResponseWithExpectedFirstName()
        {
            var @json = await Result.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(json);
            userResponse.FirstName.Should().Be(_firstname);
        }

        [Integration]
        public async Task ThenResultShouldContainUserResponseWithExpectedLastName()
        {
            var @json = await Result.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(json);
            userResponse.LastName.Should().Be(_lastname);
        }

        [Integration]
        public void ThenResultShouldHaveSuccessCode()
        {
            Result.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
