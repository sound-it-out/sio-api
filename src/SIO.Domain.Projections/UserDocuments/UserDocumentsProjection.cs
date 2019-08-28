using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEventSourcing.Events;
using OpenEventSourcing.Projections;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;
using SIO.Domain.User.Events;

namespace SIO.Domain.Projections.UserDocuments
{
    public class UserDocumentsProjection : IProjection
    {
        private readonly IProjectionWriter<UserDocuments> _writer;
        private readonly IDictionary<Type, Func<IEvent, Task>> _handlers;

        public UserDocumentsProjection(IProjectionWriter<UserDocuments> writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
            _handlers = new Dictionary<Type, Func<IEvent, Task>>();

            Handles<UserVerified>(ApplyAsync);
            Handles<DocumentUploaded>(ApplyAsync);
            Handles<TranslationQueued>(ApplyAsync);
            Handles<TranslationStarted>(ApplyAsync);
            Handles<TranslationSucceded>(ApplyAsync);
            Handles<TranslationFailed>(ApplyAsync);
        }

        public async Task HandleAsync(IEvent @event)
        {
            if (_handlers.TryGetValue(@event.GetType(), out var handler))
                await handler(@event);
        }

        private async Task ApplyAsync(UserVerified @event)
        {
            await _writer.Add(new Guid(@event.UserId), () =>
            {
                return new UserDocuments
                {
                    UserId = @event.AggregateId,
                    CreatedDate = @event.Timestamp,
                    Version = @event.Version,
                    Data = new UserDocumentsData
                    {
                        Documents = new List<UserDocument>()
                    }
                };
            });
        }

        private async Task ApplyAsync(DocumentUploaded @event)
        {
            await _writer.Update(new Guid(@event.UserId), userDocuments =>
            {
                userDocuments.Version++;
                userDocuments.Data.Documents.Add(
                    new UserDocument
                    {
                        Id = @event.AggregateId,
                        FileName = @event.FileName
                    }
                );
            });
        }

        private async Task ApplyAsync(TranslationQueued @event)
        {
            await _writer.Update(new Guid(@event.UserId), userDocuments =>
            {
                userDocuments.Version++;
                userDocuments.Data.Documents.UpdateItem(
                    condition: ud => ud.Id == @event.CorrelationId, 
                    update: ud => ud.Condition = DocumentCondition.TranslationQueued
                );
            });
        }

        private async Task ApplyAsync(TranslationStarted @event)
        {
            await _writer.Update(new Guid(@event.UserId), userDocuments =>
            {
                userDocuments.Version++;
                userDocuments.Data.Documents.UpdateItem(
                    condition: ud => ud.Id == @event.CorrelationId,
                    update: ud => ud.Condition = DocumentCondition.TranslationStarted
                );
            });
        }

        private async Task ApplyAsync(TranslationSucceded @event)
        {
            await _writer.Update(new Guid(@event.UserId), userDocuments =>
            {
                userDocuments.Version++;
                userDocuments.Data.Documents.UpdateItem(
                    condition: ud => ud.Id == @event.CorrelationId,
                    update: ud => ud.Condition = DocumentCondition.TranslationSucceded
                );
            });
        }

        private async Task ApplyAsync(TranslationFailed @event)
        {
            await _writer.Update(new Guid(@event.UserId), userDocuments =>
            {
                userDocuments.Version++;
                userDocuments.Data.Documents.UpdateItem(
                    condition: ud => ud.Id == @event.CorrelationId,
                    update: ud => ud.Condition = DocumentCondition.TranslationFailed
                );
            });
        }

        private void Handles<TEvent>(Func<TEvent, Task> handler)
            where TEvent : IEvent
        {
            _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
        }
    }
}
