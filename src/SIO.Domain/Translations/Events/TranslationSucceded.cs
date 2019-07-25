using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translation.Events
{
    public class TranslationSucceded : Event
    {
        public string TranslationPath { get; set; }

        public TranslationSucceded(Guid aggregateId, int version, string translationPath) : base(aggregateId, version)
        {
            TranslationPath = translationPath;
        }
    }
}
