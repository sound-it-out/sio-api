using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEventSourcing.Events;
using OpenEventSourcing.Projections;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Projections.Document
{
    public class DocumentProjection : IProjection
    {
        private readonly IProjectionWriter<Document> _writer;
        private readonly IDictionary<Type, Func<IEvent, Task>> _handlers;

        public DocumentProjection(IProjectionWriter<Document> writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
            _handlers = new Dictionary<Type, Func<IEvent, Task>>();

            Handles<DocumentUploaded>(ApplyAsync);
            Handles<TranslationQueued>(ApplyAsync);
            Handles<TranslationStarted>(ApplyAsync);
            Handles<TranslationSucceded>(ApplyAsync);
            Handles<TranslationFailed>(ApplyAsync);
            Handles<TranslationCharactersProcessed>(ApplyAsync);
        }

        public async Task HandleAsync(IEvent @event)
        {
            if (_handlers.TryGetValue(@event.GetType(), out var handler))
                await handler(@event);
        }

        private async Task ApplyAsync(DocumentUploaded @event)
        {
            await _writer.Add(@event.AggregateId, () =>
            {
                return new Document
                {
                    Id = @event.AggregateId,
                    CreatedDate = @event.Timestamp,
                    Version = @event.Version,
                    Data = new DocumentData
                    {
                        Condition = DocumentCondition.Uploaded,
                        FileName = @event.FileName
                    }
                };
            });
        }

        private async Task ApplyAsync(TranslationQueued @event)
        {
            await _writer.Update(@event.CorrelationId.Value, document =>
            {
                document.TranslationId = @event.AggregateId;
                document.Data.Condition = DocumentCondition.TranslationQueued;
                document.LastModifiedDate = @event.Timestamp;
                document.Version = @event.Version;
            });
        }

        private async Task ApplyAsync(TranslationStarted @event)
        {
            await _writer.Update(@event.CorrelationId.Value, document =>
            {
                document.Data.Condition = DocumentCondition.TranslationStarted;
                document.Data.TranslationCharactersTotal = @event.CharacterCount;
                document.LastModifiedDate = @event.Timestamp;
                document.Version = @event.Version;
            });
        }

        private async Task ApplyAsync(TranslationSucceded @event)
        {
            await _writer.Update(@event.CorrelationId.Value, document =>
            {
                document.Data.Condition = DocumentCondition.TranslationSucceded;
                document.LastModifiedDate = @event.Timestamp;
                document.Version = @event.Version;
            });
        }

        private async Task ApplyAsync(TranslationFailed @event)
        {
            await _writer.Update(@event.CorrelationId.Value, document =>
            {
                document.Data.Condition = DocumentCondition.TranslationFailed;
                document.LastModifiedDate = @event.Timestamp;
                document.Version = @event.Version;
            });
        }

        private async Task ApplyAsync(TranslationCharactersProcessed @event)
        {
            await _writer.Update(@event.CorrelationId.Value, document =>
            {
                document.Data.TranslationCharactersProcessed += @event.CharactersProcessed;
                document.LastModifiedDate = @event.Timestamp;
                document.Version = @event.Version;
            });
        }

        private void Handles<TEvent>(Func<TEvent, Task> handler)
            where TEvent : IEvent
        {
            _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
        }
    }
}
