using System;
using Newtonsoft.Json;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translations.Events
{
    public class TranslationQueued : Event
    {
        public TranslationQueued(Guid aggregateId, int version) : base(aggregateId, version)
        {
        }

        public TranslationQueued(Guid aggregateId, Guid correlationId, int version) : base(aggregateId, version)
        {
            CorrelationId = correlationId;
        }

        public TranslationQueued(Guid aggregateId, Guid correlationId, Guid userId, int version) : base(aggregateId, version)
        {
            CorrelationId = correlationId;
            UserId = userId.ToString();
        }

        [JsonConstructor]
        public TranslationQueued(Guid aggregateId, Guid correlationId, Guid causationId, Guid userId, int version) : base(aggregateId, version)
        {
            CorrelationId = correlationId;
            UserId = userId.ToString();
            CausationId = causationId;
        }
    }
}
