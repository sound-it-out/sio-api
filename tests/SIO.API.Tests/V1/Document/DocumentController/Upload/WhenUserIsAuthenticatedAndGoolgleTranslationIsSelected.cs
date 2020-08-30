using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using SIO.API.Tests.Abstractions;
using SIO.API.Tests.Extensions;
using SIO.API.V1.Document.Requests;
using SIO.Infrastructure;
using SIO.Infrastructure.Translations;
using SIO.Testing.Attributes;

namespace SIO.API.Tests.V1.Document.DocumentController.Upload
{
    public class WhenUserIsAuthenticatedAndGoolgleTranslationIsSelected : AuthenticatedServerSpecification<HttpResponseMessage>
    {
        public WhenUserIsAuthenticatedAndGoolgleTranslationIsSelected(ConfigurationFixture configurationFixture, EventSeederFixture eventSeederFixture, AuthenticatedAPIWebApplicationFactory webApplicationFactory) : base(configurationFixture, eventSeederFixture, webApplicationFactory)
        {
        }

        protected override void BuildHost(IWebHostBuilder builder)
        {
            base.BuildHost(builder);
        }

        protected override async Task<HttpResponseMessage> Given()
        {
            using (var content = new MultipartFormDataContent())
            using (var ms = new MemoryStream())
            using (TextWriter tw = new StreamWriter(ms))
            {
                await tw.WriteAsync("This is a test.");
                await tw.FlushAsync();
                ms.Position = 0;
                var fileContent = new StreamContent(ms);
                var translationOption = new TranslationOption("test", TranslationType.Google);
                content.Add(fileContent, nameof(UploadRequest.File), "test.txt");
                content.Add(new StringContent(((int)translationOption.Type).ToString()), $"{nameof(UploadRequest.TranslationOption)}.{nameof(UploadRequest.TranslationOption.Type)}");
                content.Add(new StringContent(translationOption.Subject), $"{nameof(UploadRequest.TranslationOption)}.{nameof(UploadRequest.TranslationOption.Subject)}".ToLower());
                var client = _webApplicationFactory.CreateClientWithTestAuth();
                return await client.PostAsync($"/v1/Document/{nameof(SIO.API.V1.Document.DocumentController.Upload)}", content);
            }    
        }

        protected override Task When()
        {
            return Task.CompletedTask;
        }

        [Integration]
        public void ThenResultShouldHaveAcceptedStatusCode()
        {
            Result.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }
    }
}
