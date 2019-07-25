using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translation.Events
{
    public class TranslationQueued : Event
    {
        public TranslationQueued(Guid aggregateId, int version) : base(aggregateId, version)
        {
        }
    }
}
