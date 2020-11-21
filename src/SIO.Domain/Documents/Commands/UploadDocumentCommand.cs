using System;
using Microsoft.AspNetCore.Http;
using OpenEventSourcing.Commands;
using SIO.Infrastructure;
using SIO.Infrastructure.Translations;

namespace SIO.Domain.Documents.Commands
{
    public class UploadDocumentCommand : Command
    {
        public IFormFile File { get; set; }
        public TranslationOption TranslationOption { get; set; }

        public UploadDocumentCommand(Guid aggregateId, Guid correlationId, int version, string userId, IFormFile file, TranslationOption translationOption) : base(aggregateId, correlationId, version, userId)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            File = file;
            TranslationOption = translationOption;
        }
    }
}
