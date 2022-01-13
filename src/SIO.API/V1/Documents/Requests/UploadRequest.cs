using Microsoft.AspNetCore.Http;
using SIO.IntegrationEvents.Documents;

namespace SIO.Api.V1.Documents.Requests
{
    public record UploadRequest(IFormFile File, TranslationType TranslationType, string TranslationSubject);
}
