using System;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Translations.Commands
{
    public class QueueTranslationCommand : Command
    {
        public string FilePath { get; set; }

        public QueueTranslationCommand(Guid aggregateId, Guid correlationId, int version, string userId, string filePath) : base(aggregateId, correlationId, version, userId)
        {
            FilePath = filePath;
        }
    }
}
