using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translation.Events
{
    public class TranslationSucceded : Event
    {
        public TranslationSucceded(Guid aggregateId, int version) : base(aggregateId, version)
        {
        }

        public TranslationSucceded(Guid aggregateId, Guid correlationId, int version) : base(aggregateId, version)
        {
            CorrelationId = correlationId;
        }
    }
}
