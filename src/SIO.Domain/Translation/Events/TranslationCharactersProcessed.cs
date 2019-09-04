﻿using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Translation.Events
{
    public class TranslationCharactersProcessed : Event
    {
        public long CharactersProcessed { get; set; }
        public TranslationCharactersProcessed(Guid aggregateId, int version, long charactersProcessed) : base(aggregateId, version)
        {
            CharactersProcessed = charactersProcessed;
        }

        public TranslationCharactersProcessed(Guid aggregateId, Guid correlationId, int version, long charactersProcessed) : base(aggregateId, version)
        {
            CharactersProcessed = charactersProcessed;
            CorrelationId = correlationId;
        }
    }
}
