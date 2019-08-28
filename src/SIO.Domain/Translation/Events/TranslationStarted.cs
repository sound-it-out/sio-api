using System;
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
    }
}
