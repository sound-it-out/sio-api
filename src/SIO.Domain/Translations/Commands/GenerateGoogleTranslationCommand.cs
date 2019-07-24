using System;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translations.Commands
{
    public class GenerateGoogleTranslationCommand : Command
    {
        public string Content { get; set; }
        public GenerateGoogleTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId, string content) : base(aggregateId, correlationId, version, userId)
        {
            Content = content;
        }
    }
}
