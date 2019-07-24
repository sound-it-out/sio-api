using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translations.Events
{
    public class TranslationCreated : Event
    {
        public TranslationType TranslationType { get; set; }
        public TranslationCreated(Guid aggregateId, int version, TranslationType translationType) : base(aggregateId, version)
        {
            TranslationType = translationType;
        }
    }
}
