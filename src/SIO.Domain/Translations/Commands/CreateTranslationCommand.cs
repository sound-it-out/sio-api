using System;
using Microsoft.AspNetCore.Http;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translations.Commands
{
    public class CreateTranslationCommand : Command
    {
        public IFormFile File { get; set; }
        public TranslationType TranslationType { get; set; }

        public CreateTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId, IFormFile file, TranslationType translationType) : base(aggregateId, correlationId, version, userId)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            File = file;
            TranslationType = translationType;
        }
    }
}
