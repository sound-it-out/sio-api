using System.Threading.Tasks;
using OpenEventSourcing.Projections;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;

namespace SIO.Domain.Projections.Document
{
    public class DocumentProjection : Projection<Document>
    {
        public DocumentProjection(IProjectionWriter<Document> writer) : base(writer)
        {
            Handles<DocumentUploaded>(ApplyAsync);
            Handles<TranslationQueued>(ApplyAsync);
            Handles<TranslationStarted>(ApplyAsync);
            Handles<TranslationSucceded>(ApplyAsync);
            Handles<TranslationFailed>(ApplyAsync);
            Handles<TranslationCharactersProcessed>(ApplyAsync);
            Handles<DocumentDeleted>(ApplyAsync);
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

        private async Task ApplyAsync(DocumentDeleted @event)
        {
            await _writer.Remove(@event.AggregateId);
        }
    }
}
