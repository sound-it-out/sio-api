using System;
using OpenEventSourcing.Domain;
using SIO.Domain.Translations.Events;

namespace SIO.Domain.Translations.Aggregates
{
    public class Translation : Aggregate<TranslationState>
    {
        public override Guid? Id
        {
            get => _state.Id;
            protected set => _state.Id = value;
        }

        public Translation(TranslationState state) : base(state)
        {
            Handles<TranslationCreated>(Handle);
        }

        public void Create(Guid id, string filePath)
        {
            if (Id.HasValue)
                throw new Exception("Company has already been created");

            Apply(new TranslationCreated(id, 1, filePath));
        }

        private void Handle(TranslationCreated @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            Id = @event.AggregateId;
            _state.FilePath = @event.FilePath;
            _state.Condition = TranslationCondition.Created;
            _state.IsDeleted = false;
            Version = 1;
        }

        public override TranslationState GetState()
        {
            return _state;
        }
    }
}
