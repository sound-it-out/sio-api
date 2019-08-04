using System;
using OpenEventSourcing.Domain;

namespace SIO.Domain.Document
{
    public class DocumentState : IAggregateState
    {
        public Guid? Id { get; set; }
        public Guid? TranslationId { get; set; }
        public Guid UserId { get; set; }
        public DocumentCondition Condition { get; set; }
        public long TranslationCharactersProcessed { get; set; }
        public long TranslationCharactersTotal { get; set; }
        public int Version { get; set; }

        public DocumentState() { }
        public DocumentState(DocumentState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Id = state.Id;
            TranslationId = state.TranslationId;
            UserId = state.UserId;
            Condition = state.Condition;
            TranslationCharactersProcessed = state.TranslationCharactersProcessed;
            TranslationCharactersTotal = state.TranslationCharactersTotal;
            Version = state.Version;
        }
    }
}
