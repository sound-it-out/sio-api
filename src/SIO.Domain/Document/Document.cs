using System;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Extensions;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation;
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
        }

        public void UploadDocument(TranslationType translationType, string filePath)
        {
            Apply(new DocumentUploaded(
                aggregateId: Guid.NewGuid().ToSequentialGuid(),
                version: _state.Version + 1,
                translationType: translationType,
                filePath: filePath
            ));
        }

        public void QueueTranslation(Guid aggregateId, int version)
        {
            Apply(new TranslationQueued(
                aggregateId: aggregateId,
                version: version
            ));
        }

        public void StartTranslation(Guid aggregateId, int version)
        {
            Apply(new TranslationStarted(
                aggregateId: aggregateId,
                version: version
            ));
        }

        public void AcceptTranslation(Guid aggregateId, int version, string translationPath)
        {
            Apply(new TranslationSucceded(
                aggregateId: aggregateId,
                version: version,
                translationPath: translationPath
            ));
        }

        public void FailTranslation(Guid aggregateId, int version, string error)
        {
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
            _state.Version = @event.Version;
        }

        public void Handle(TranslationQueued @event)
        {
            _state.Condition = DocumentCondition.TranslationQueued;
            _state.Version = @event.Version;
        }

        public void Handle(TranslationStarted @event)
        {
            _state.Condition = DocumentCondition.TranslationStarted;
            _state.Version = @event.Version;
        }

        public void Handle(TranslationSucceded @event)
        {
            _state.Condition = DocumentCondition.TranslationSucceded;
            _state.TranslationPath = @event.TranslationPath;
            _state.Version = @event.Version;
        }

        public void Handle(TranslationFailed @event)
        {
            _state.Condition = DocumentCondition.TranslationFailed;
            _state.Version = @event.Version;
        }
    }
}
