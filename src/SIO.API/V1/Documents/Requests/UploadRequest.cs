using Microsoft.AspNetCore.Http;
using SIO.Domain.Documents.Events;

namespace SIO.Api.V1.Documents.Requests
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public TranslationType TranslationType { get; set; }
    }
}
