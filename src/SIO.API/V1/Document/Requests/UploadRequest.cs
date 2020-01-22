using Microsoft.AspNetCore.Http;
using SIO.Domain;

namespace SIO.API.V1.Document.Requests
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public TranslationType TranslationType { get; set; }
    }
}
