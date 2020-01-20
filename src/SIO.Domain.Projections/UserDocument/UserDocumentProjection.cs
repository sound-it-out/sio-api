using System;
using System.Threading.Tasks;
using OpenEventSourcing.Projections;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Projections.UserDocument
{
    public class UserDocumentProjection : Projection<UserDocument>
    {
        public UserDocumentProjection(IProjectionWriter<UserDocument> writer) : base(writer)
        {
            Handles<DocumentUploaded>(ApplyAsync);
            Handles<TranslationQueued>(ApplyAsync);
            Handles<TranslationStarted>(ApplyAsync);
            Handles<TranslationSucceded>(ApplyAsync);
            Handles<TranslationFailed>(ApplyAsync);
        }

        private async Task ApplyAsync(DocumentUploaded @event)
        {
            await _writer.Add(@event.AggregateId, () =>
            {
                return new UserDocument
                {
                    DocumentId = @event.AggregateId,
                    UserId = new Guid(@event.UserId),
                    CreatedDate = @event.Timestamp,
                    Version = 1,
                    Data = new UserDocumentData
                    {
                        FileName = @event.FileName,
                        Condition = DocumentCondition.Uploaded
                    }
                };
            });
        }

        private async Task ApplyAsync(TranslationQueued @event)
        {
            await _writer.Update(@event.CorrelationId.Value, userDocument =>
            {
                userDocument.Version++;
                userDocument.Data.Condition = DocumentCondition.TranslationQueued;
                userDocument.LastModifiedDate = DateTimeOffset.UtcNow;
            });
        }

        private async Task ApplyAsync(TranslationStarted @event)
        {
            await _writer.Update(@event.CorrelationId.Value, userDocument =>
            {
                userDocument.Version++;
                userDocument.Data.Condition = DocumentCondition.TranslationStarted;
                userDocument.LastModifiedDate = DateTimeOffset.UtcNow;
            });
        }

        private async Task ApplyAsync(TranslationSucceded @event)
        {
            await _writer.Update(@event.CorrelationId.Value, userDocument =>
            {
                userDocument.Version++;
                userDocument.Data.Condition = DocumentCondition.TranslationSucceded;
                userDocument.LastModifiedDate = DateTimeOffset.UtcNow;
            });
        }

        private async Task ApplyAsync(TranslationFailed @event)
        {
            await _writer.Update(@event.CorrelationId.Value, userDocument =>
            {
                userDocument.Version++;
                userDocument.Data.Condition = DocumentCondition.TranslationFailed;
                userDocument.LastModifiedDate = DateTimeOffset.UtcNow;
            });
        }
    }
}
