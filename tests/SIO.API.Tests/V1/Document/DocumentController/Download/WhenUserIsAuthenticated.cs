using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SIO.API.Tests.Abstractions;
using SIO.API.Tests.Extensions;
using SIO.API.V1.Document.Requests;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;
using SIO.Domain.User.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Files;
using SIO.Infrastructure.Translations;
using SIO.Testing.Attributes;
using Xunit;

namespace SIO.API.Tests.V1.Document.DocumentController.Download
{
    public class WhenUserIsAuthenticated : AuthenticatedServerSpecification<HttpResponseMessage>
    {
        private IFileClient FileClient => _webApplicationFactory.Services.GetRequiredService<IFileClient>();

        private HttpClient _client;
        private static Guid _userId = TestClaimsProvider.UserId;
        private static Guid _documentId = Guid.NewGuid();
        private static Guid _translationId = Guid.NewGuid();
        private static string _email = "test@test.test";
        private static string _firstname = "test";
        private static string _lastname = "test";
        private static string _documentFileName = "test.txt";
        private static string _translationFileName = "test.mp3";

        public WhenUserIsAuthenticated(ConfigurationFixture configurationFixture, AuthenticatedAPIWebApplicationFactory webApplicationFactory) : base(configurationFixture, webApplicationFactory)
        {
        }

        protected override void BuildHost(IWebHostBuilder builder)
        {
            base.BuildHost(builder);
        }

        protected override async Task<HttpResponseMessage> Given()
        {
            return await _client.GetAsync($"/v1/Document/{_documentId}/{nameof(SIO.API.V1.Document.DocumentController.Download)}");
        }

        protected override async Task When()
        {
            ExceptionMode = Testing.Abstractions.ExceptionMode.Record;
            _client = _webApplicationFactory.CreateClientWithTestAuth();

            var causationId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();

            await EventSeeder.SeedAsync(
                new UserRegistered(_userId, correlationId, causationId, _userId.ToString(), _email, _firstname, _lastname, "asdf"),
                new UserVerified(_userId, correlationId, causationId, _userId.ToString()),
                new DocumentUploaded(_documentId, _userId, causationId, correlationId, TranslationType.Google, "", _documentFileName),
                new TranslationQueued(_translationId, _documentId, causationId, _userId, 0)
            );

            using (var ms = new MemoryStream())
            using (TextWriter tw = new StreamWriter(ms))
            {
                await tw.WriteAsync("test text");
                await tw.FlushAsync();
                ms.Position = 0;
                await FileClient.UploadAsync(
                    fileName: $"{_translationId}.mp3",
                    userId: _userId.ToString(),
                    stream: ms
                );
            }
        }

        [Integration]
        public void ThenResultShouldHaveAcceptedStatusCode()
        {
            Result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Integration]
        public void ThenResultShouldHaveStreamContent()
        {
            Result.Content.Should().BeOfType<StreamContent>();
        }

        [Integration]
        public void ThenResultShouldHaveStreamContentWithCorrectFileName()
        {
            
            Result.Content.Headers.ContentDisposition.FileName.Should().Be(_translationFileName);
        }

        [Integration]
        public void ThenResultShouldHaveStreamContentWithCorrectContentType()
        {
            var test = Result.Content.ReadAsStringAsync().Result;
            Result.Content.Headers.ContentType.Should().Be(new MediaTypeHeaderValue("audio/mpeg"));

        }
    }
}
