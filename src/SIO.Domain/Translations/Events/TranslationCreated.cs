using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translations.Events
{
    public class TranslationCreated : Event
    {
        public string FilePath { get; set; }

        public TranslationCreated(Guid aggregateId, int version, string filePath) : base(aggregateId, version)
        {
            FilePath = filePath;
        }
    }
}
