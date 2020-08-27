using Microsoft.AspNetCore.Http;
using SIO.Infrastructure;
using SIO.Infrastructure.Translations;

namespace SIO.API.V1.Document.Requests
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public TranslationOption TranslationOption { get; set; }
    }
}
