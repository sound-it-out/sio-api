using System;
using Newtonsoft.Json;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translation.Events
{
    public class TranslationStarted : Event
    {
        public long CharacterCount { get; set; }
        public TranslationStarted(Guid aggregateId, int version, long characterCount) : base(aggregateId, version)
        {
            CharacterCount = characterCount;
        }

        public TranslationStarted(Guid aggregateId, Guid correlationId, int version, long characterCount) : base(aggregateId, version)
        {
            CharacterCount = characterCount;
            CorrelationId = correlationId;
        }

        [JsonConstructor]
        public TranslationStarted(Guid aggregateId, Guid correlationId, Guid userId, int version, long characterCount) : base(aggregateId, version)
        {
            CharacterCount = characterCount;
            CorrelationId = correlationId;
            UserId = userId.ToString();
        }
    }
}
