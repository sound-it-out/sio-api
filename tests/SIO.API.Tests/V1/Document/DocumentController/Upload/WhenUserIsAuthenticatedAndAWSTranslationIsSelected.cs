using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using SIO.API.Tests.Abstractions;
using SIO.API.Tests.Extensions;
using SIO.API.V1.Document.Requests;
using SIO.Domain.Documents.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Files;
using SIO.Infrastructure.Translations;
using SIO.Testing.Attributes;

namespace SIO.API.Tests.V1.Document.DocumentController.Upload
{
    public class WhenUserIsAuthenticatedAndAWSTranslationIsSelected : AuthenticatedServerSpecification<HttpResponseMessage>
    {
        private readonly string _fileName = "test.txt";
        public WhenUserIsAuthenticatedAndAWSTranslationIsSelected(ConfigurationFixture configurationFixture, AuthenticatedAPIWebApplicationFactory webApplicationFactory) : base(configurationFixture, webApplicationFactory)
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
                var translationOption = new TranslationOption("test", TranslationType.AWS);
                content.Add(fileContent, nameof(UploadRequest.File), _fileName);
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

        [Integration]
        public async Task ThenFileShouldBeUploaded()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory>();

                using (var context = dbContextFactory.Create())
                {
                    var documentId = new Guid((await Result.Content.ReadAsStringAsync()).Replace('"', ' '));
                    var @event = await context.Events.FirstAsync(e => e.AggregateId == documentId);
                    var test = await context.Events.ToListAsync();
                    var fileClient = _webApplicationFactory.Services.GetRequiredService<IFileClient>();
                    var file = await fileClient.DownloadAsync($"{@event.AggregateId}.txt", TestClaimsProvider.UserId.ToString());
                    file.Should().NotBeNull();
                }
            }
        }
    }
}
