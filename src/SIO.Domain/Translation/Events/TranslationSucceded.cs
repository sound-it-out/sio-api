using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translation.Events
{
    public class TranslationSucceded : Event
    {
        public TranslationSucceded(Guid aggregateId, int version) : base(aggregateId, version)
        {
        }
    }
}
