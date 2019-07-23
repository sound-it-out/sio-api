using System;
using OpenEventSourcing.Domain;

namespace SIO.Domain.Translations.Aggregates
{
    public class TranslationState : IAggregateState
    {
        public Guid? Id { get; set; }
        public string FilePath { get; set; }
        public bool IsDeleted { get; set; }
        public TranslationCondition Condition { get; set; }

        public TranslationState()
        {
        }
    }
}
