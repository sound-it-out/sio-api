using Microsoft.AspNetCore.Http;
using SIO.Domain.Documents.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;

namespace SIO.Domain.Documents.Commands
{
    public class UploadDocumentCommand : Command
    {
        public UploadDocumentCommand(string subject, CorrelationId? correlationId, int version, Actor actor, IFormFile file, TranslationType translationType) : base(subject, correlationId, version, actor)
        {
            File = file;
            TranslationType = translationType;
        }

        public IFormFile File { get; set; }
        public TranslationType TranslationType { get; set; }
    }
}
