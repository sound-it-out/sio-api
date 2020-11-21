﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.Documents;
using SIO.Domain.Documents.Events;
using SIO.Domain.Projections.Documents;
using SIO.Domain.Translations.Events;
using SIO.Infrastructure;
using SIO.Testing.Attributes;

namespace SIO.Tests.Unit.Domain.Projections.Document
{
    public class WhenTranslationStarted : Specification<DocumentProjection>
    {
        private readonly Guid _documentId = Guid.NewGuid().ToSequentialGuid();
        private readonly Guid _translationId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _fileName = "Test Document";
        private readonly long _characterCount = 3000;
        protected override IEnumerable<IEvent> Given()
        {
            yield return new DocumentUploaded(_documentId, TranslationType.Google, "test", _fileName);
            yield return new TranslationQueued(_translationId, _documentId, 2);
            yield return new TranslationStarted(_translationId, _documentId, 3, _characterCount);
        }

        [Then]
        public void ThenDocumentShouldNotBeNull()
        {
            var document = Context.Find<SIO.Domain.Projections.Documents.Document>(_documentId);

            document.Should().NotBeNull();
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectId()
        {
            var document = Context.Find<SIO.Domain.Projections.Documents.Document>(_documentId);

            document.Id.Should().Be(_documentId);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectFileName()
        {
            var document = Context.Find<SIO.Domain.Projections.Documents.Document>(_documentId);

            document.Data.FileName.Should().Be(_fileName);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectCondition()
        {
            var document = Context.Find<SIO.Domain.Projections.Documents.Document>(_documentId);

            document.Data.Condition.Should().Be(DocumentCondition.TranslationStarted);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectTranslationCharactersTotal()
        {
            var document = Context.Find<SIO.Domain.Projections.Documents.Document>(_documentId);

            document.Data.TranslationCharactersTotal.Should().Be(_characterCount);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectTranslationCharactersProcessed()
        {
            var document = Context.Find<SIO.Domain.Projections.Documents.Document>(_documentId);

            document.Data.TranslationCharactersProcessed.Should().Be(0);
        }

        [Then]
        public void ThenDocumentShouldHaveCorrectVersion()
        {
            var document = Context.Find<SIO.Domain.Projections.Documents.Document>(_documentId);

            document.Version.Should().Be(3);
        }
    }
}
