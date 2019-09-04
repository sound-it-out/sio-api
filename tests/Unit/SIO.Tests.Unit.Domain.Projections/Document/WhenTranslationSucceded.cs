using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Projections.Document;
using SIO.Domain.Translation.Events;

namespace SIO.Tests.Unit.Domain.Projections.Document
{
    public class WhenTranslationSucceded : Specification<DocumentProjection>
    {
        private readonly Guid _documentId = Guid.NewGuid().ToSequentialGuid();
        private readonly Guid _translationId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _fileName = "Test Document";
        private readonly long _characterCount = 3000;
        private readonly long _characterProcessed = 3000;
        protected override IEnumerable<IEvent> Given()
        {
            yield return new DocumentUploaded(_documentId, TranslationType.Google, _fileName);
            yield return new TranslationQueued(_translationId, _documentId, 2);
            yield return new TranslationStarted(_translationId, _documentId, 3, _characterCount);
            yield return new TranslationCharactersProcessed(_translationId, _documentId, 4, _characterProcessed);
            yield return new TranslationSucceded(_translationId, _documentId, 5);
        }

        [Then]
        public void ThenDocumentShouldNotBeNull()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_documentId);

            document.Should().NotBeNull();
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectId()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_documentId);

            document.Id.Should().Be(_documentId);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectFileName()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_documentId);

            document.Data.FileName.Should().Be(_fileName);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectCondition()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_documentId);

            document.Data.Condition.Should().Be(DocumentCondition.TranslationSucceded);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectTranslationCharactersTotal()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_documentId);

            document.Data.TranslationCharactersTotal.Should().Be(_characterCount);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectTranslationCharactersProcessed()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_documentId);

            document.Data.TranslationCharactersProcessed.Should().Be(_characterProcessed);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectVersion()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_documentId);

            document.Version.Should().Be(5);
        }
    }
}
