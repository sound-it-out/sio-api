using System;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translations.Commands
{
    public class QueueTranslationCommand : Command
    {
        public TranslationType TranslationType { get; set; }

        public QueueTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId, TranslationType translationType) : base(aggregateId, correlationId, version, userId)
        {
            TranslationType = translationType;
        }
    }
}
