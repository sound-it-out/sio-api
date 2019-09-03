using System;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Extensions;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Document
{
    public class Document : Aggregate<DocumentState>
    {
        public override DocumentState GetState() => new DocumentState(_state);

        public Document(DocumentState state) : base(state)
        {
            Handles<DocumentUploaded>(Handle);
            Handles<TranslationQueued>(Handle);
            Handles<TranslationStarted>(Handle);
            Handles<TranslationSucceded>(Handle);
            Handles<TranslationFailed>(Handle);
            Handles<TranslationCharactersProcessed>(Handle);
        }

        public void Upload(TranslationType translationType, string fileName)
        {
            Apply(new DocumentUploaded(
                aggregateId: Guid.NewGuid().ToSequentialGuid(),
                translationType: translationType,
                fileName: fileName
            ));
        }

        public void QueueTranslation(Guid aggregateId, int version)
        {
            version++;
            Apply(new TranslationQueued(
                aggregateId: aggregateId,
                version: version
            ));
        }

        public void StartTranslation(Guid aggregateId, long characterCount, int version)
        {
            version++;
            Apply(new TranslationStarted(
                aggregateId: aggregateId,
                version: version,
                characterCount: characterCount
            ));
        }

        public void AcceptTranslation(Guid aggregateId, int version)
        {
            version++;
            Apply(new TranslationSucceded(
                aggregateId: aggregateId,
                version: version
            ));
        }

        public void ProcessTranslationCharacters(Guid aggregateId, int version, long charactersProcessed)
        {
            version++;
            Apply(new TranslationCharactersProcessed(
                aggregateId: aggregateId,
                version: version,
                charactersProcessed: charactersProcessed
            ));
        }

        public void FailTranslation(Guid aggregateId, int version, string error)
        {
            version++;
            Apply(new TranslationFailed(
                aggregateId: aggregateId,
                version: version,
                error: error
            ));
        }

        public void Handle(DocumentUploaded @event)
        {
            _state.Id = @event.AggregateId;
            _state.Condition = DocumentCondition.Uploaded;
            _state.FileName = @event.FileName;
            Version = 1;
        }

        public void Handle(TranslationQueued @event)
        {
            _state.TranslationId = @event.AggregateId;
            _state.Condition = DocumentCondition.TranslationQueued;
            Version += 1;
        }

        public void Handle(TranslationStarted @event)
        {
            _state.Condition = DocumentCondition.TranslationStarted;
            _state.TranslationCharactersTotal = @event.CharacterCount;
            Version += 1;
        }

        public void Handle(TranslationSucceded @event)
        {
            _state.Condition = DocumentCondition.TranslationSucceded;
            Version += 1;
        }

        public void Handle(TranslationFailed @event)
        {
            _state.Condition = DocumentCondition.TranslationFailed;
            Version += 1;
        }

        public void Handle(TranslationCharactersProcessed @event)
        {
            _state.Condition = DocumentCondition.TranslationFailed;
            _state.TranslationCharactersProcessed += @event.CharactersProcessed;
            Version += 1;
        }
    }
}
