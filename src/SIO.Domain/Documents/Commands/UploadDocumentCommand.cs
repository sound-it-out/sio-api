using Microsoft.AspNetCore.Http;
using SIO.Infrastructure;
using SIO.Infrastructure.Commands;
using SIO.IntegrationEvents.Documents;

namespace SIO.Domain.Documents.Commands
{
    public class UploadDocumentCommand : Command
    {
        public UploadDocumentCommand(string subject, CorrelationId? correlationId, int version, Actor actor, IFormFile file, TranslationType translationType, string translationSubject) : base(subject, correlationId, version, actor)
        {
            File = file;
            TranslationType = translationType;
            TranslationSubject = translationSubject;
        }

        public IFormFile File { get; set; }
        public TranslationType TranslationType { get; set; }
        public string TranslationSubject {  get; set; }
    }
}
