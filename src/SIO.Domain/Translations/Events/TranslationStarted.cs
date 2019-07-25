using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translation.Events
{
    public class TranslationStarted : Event
    {
        public TranslationStarted(Guid aggregateId, int version) : base(aggregateId, version)
        {
        }
    }
}
