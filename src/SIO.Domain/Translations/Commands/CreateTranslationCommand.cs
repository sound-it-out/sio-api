using System;
using Microsoft.AspNetCore.Http;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translations.Commands
{
    public class CreateTranslationCommand : Command
    {
        public IFormFile File { get; set; }

        public CreateTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId, IFormFile file) : base(aggregateId, correlationId, version, userId)
        {
            File = file;
        }
    }
}
