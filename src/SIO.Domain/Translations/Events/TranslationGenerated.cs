using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translations.Events
{
    public class TranslationGenerated : Event
    {
        public TranslationGenerated(Guid aggregateId, int version) : base(aggregateId, version)
        {
        }
    }
}
