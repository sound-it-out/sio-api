using Microsoft.AspNetCore.Http;
using SIO.Domain.Documents.Events;

namespace SIO.Api.V1.Documents.Requests
{
    public record UploadRequest(IFormFile File, TranslationType TranslationType, string TranslationSubject);
}
