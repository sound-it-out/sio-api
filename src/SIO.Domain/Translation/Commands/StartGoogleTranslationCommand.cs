using System;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translation.Commands
{
    public class StartGoogleTranslationCommand : Command
    {
        public StartGoogleTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId) : base(aggregateId, correlationId, version, userId)
        {
        }
    }
}
