using System;
using Newtonsoft.Json;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translations.Events
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

        [JsonConstructor]
        public TranslationSucceded(Guid aggregateId, Guid correlationId, Guid userId, int version) : base(aggregateId, version)
        {
            CorrelationId = correlationId;
            UserId = userId.ToString();
        }
    }
}
