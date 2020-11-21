using System;
using Newtonsoft.Json;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translations.Events
{
    public class TranslationFailed : Event
    {
        public string Error { get; set; }
        public TranslationFailed(Guid aggregateId, int version, string error) : base(aggregateId, version)
        {
            Error = error;
        }

        public TranslationFailed(Guid aggregateId, Guid correlationId, int version, string error) : base(aggregateId, version)
        {
            Error = error;
            CorrelationId = correlationId;
        }

        [JsonConstructor]
        public TranslationFailed(Guid aggregateId, Guid correlationId, Guid userId, int version, string error) : base(aggregateId, version)
        {
            Error = error;
            CorrelationId = correlationId;
            UserId = userId.ToString();
        }
    }
}
