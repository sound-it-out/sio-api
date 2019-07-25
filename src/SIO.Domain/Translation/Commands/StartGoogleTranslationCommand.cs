using System;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translation.Commands
{
    public class StartGoogleTranslationCommand : Command
    {
        public string Content { get; set; }
        public StartGoogleTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId, string content) : base(aggregateId, correlationId, version, userId)
        {
            Content = content;
        }
    }
}
