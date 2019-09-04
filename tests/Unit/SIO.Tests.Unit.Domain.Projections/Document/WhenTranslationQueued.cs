using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenEventSourcing.Events;
using SIO.Domain;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Projections.Document;
using SIO.Domain.Translation.Events;

namespace SIO.Tests.Unit.Domain.Projections.Document
{
    public class WhenTranslationQueued : Specification<DocumentProjection>
    {
        private readonly Guid _aggregateId = Guid.NewGuid();
        private readonly string _fileName = "Test Document";
        protected override IEnumerable<IEvent> Given()
        {
            yield return new DocumentUploaded(_aggregateId, TranslationType.Google, _fileName);
            yield return new TranslationQueued(_aggregateId, 2);
        }

        [Then]
        public void ThenDocumentShouldNotBeNull()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_aggregateId);

            document.Should().NotBeNull();
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectId()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_aggregateId);

            document.Id.Should().Be(_aggregateId);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectFileName()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_aggregateId);

            document.Data.FileName.Should().Be(_fileName);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectCondition()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_aggregateId);

            document.Data.Condition.Should().Be(DocumentCondition.TranslationQueued);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectTranslationCharactersTotal()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_aggregateId);

            document.Data.TranslationCharactersTotal.Should().Be(0);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectTranslationCharactersProcessed()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_aggregateId);

            document.Data.TranslationCharactersProcessed.Should().Be(0);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectVersion()
        {
            var document = Context.Find<SIO.Domain.Projections.Document.Document>(_aggregateId);

            document.Version.Should().Be(2);
        }
    }
}
