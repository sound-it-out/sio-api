using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEventSourcing.Projections;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Extensions;
using SIO.Domain.Translation.Events;
using SIO.Domain.User.Events;

namespace SIO.Domain.Projections.UserDocuments
{
    public class UserDocumentsProjection : Projection<UserDocuments>
    {
        public UserDocumentsProjection(IProjectionWriter<UserDocuments> writer) : base(writer)
        {
            Handles<UserVerified>(ApplyAsync);
            Handles<DocumentUploaded>(ApplyAsync);
            Handles<TranslationQueued>(ApplyAsync);
            Handles<TranslationStarted>(ApplyAsync);
            Handles<TranslationSucceded>(ApplyAsync);
            Handles<TranslationFailed>(ApplyAsync);
        }

        private async Task ApplyAsync(UserVerified @event)
        {
            await _writer.Add(@event.AggregateId, () =>
            {
                return new UserDocuments
                {
                    UserId = @event.AggregateId,
                    CreatedDate = @event.Timestamp,
                    Version = 1,
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
                        FileName = @event.FileName,
                        Version = 1
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
                    update: ud => 
                    {
                        ud.Condition = DocumentCondition.TranslationQueued;
                        ud.Version++;
                    }
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
                    update: ud =>
                    {
                        ud.Condition = DocumentCondition.TranslationStarted;
                        ud.Version++;
                    }
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
                    update: ud =>
                    {
                        ud.Condition = DocumentCondition.TranslationSucceded;
                        ud.Version++;
                    }
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
                    update: ud =>
                    {
                        ud.Condition = DocumentCondition.TranslationFailed;
                        ud.Version++;
                    }
                );
            });
        }
    }
}
