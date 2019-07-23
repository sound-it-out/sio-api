using System;
using SIO.Abstraction.Commands;

namespace SIO.Domain.Translation.Commands
{
    public class QueueTranslationCommand : ICommand
    {
        public Guid AggregateId { get; }
        public Guid? CorrelationId { get; }
        public string FilePath { get; set; }

        public QueueTranslationCommand(Guid? correlationId, string filePath)
        {
            AggregateId = Guid.NewGuid();
            CorrelationId = correlationId;
            FilePath = filePath;
        }
    }
}
