using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Projections.UserDocuments;
using SIO.Domain.Translation.Events;
using SIO.Domain.User.Events;

namespace SIO.Tests.Unit.Domain.Projections.UserDocuments
{
    public class WhenTranslationFailed : Specification<UserDocumentsProjection>
    {
        private readonly Guid _userId = Guid.NewGuid().ToSequentialGuid();
        private readonly Guid _documentId = Guid.NewGuid().ToSequentialGuid();
        private readonly Guid _translationId = Guid.NewGuid().ToSequentialGuid();
        private readonly long _characterCount = 3000;
        private readonly string _fileName = "Test Document";
        private readonly string _error = "Test Error";

        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserVerified(_userId, 1);
            yield return new DocumentUploaded(_documentId, _userId, TranslationType.Google, _fileName);
            yield return new TranslationQueued(_translationId, _documentId, _userId, 2);
            yield return new TranslationStarted(_translationId, _documentId, _userId, 3, _characterCount);
            yield return new TranslationFailed(_translationId, _documentId, _userId, 4, _error);
        }

        [Then]
        public void ThenUserDocumentsShouldNotBeNull()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.Should().NotBeNull();
        }

        [Then]
        public void ThenUserDocumentsShouldHaveCorrectUserId()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.UserId.Should().Be(_userId);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveCorrectNumberOfDocuments()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.Data.Documents.Count.Should().Be(1);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveNonNullDocument()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);
            var document = userDocuments.Data.Documents.FirstOrDefault();

            document.Should().NotBeNull();
        }

        [Then]
        public void ThenUserDocumentsShouldHaveDocumentWithCorrectId()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);
            var document = userDocuments.Data.Documents.FirstOrDefault();

            document.Id.Should().Be(_documentId);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveDocumentWithCorrectFileName()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);
            var document = userDocuments.Data.Documents.FirstOrDefault();

            document.FileName.Should().Be(_fileName);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveDocumentWithCorrectCondition()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);
            var document = userDocuments.Data.Documents.FirstOrDefault();

            document.Condition.Should().Be(DocumentCondition.TranslationFailed);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveDocumentWithCorrectVersion()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);
            var document = userDocuments.Data.Documents.FirstOrDefault();

            document.Version.Should().Be(4);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveCorrectVersion()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.Version.Should().Be(5);
        }
    }
}
