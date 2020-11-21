using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.Documents;
using SIO.Domain.Documents.Events;
using SIO.Infrastructure;
using SIO.Infrastructure.Translations;
using SIO.Testing.Attributes;

namespace SIO.Tests.Unit.Domain.Document
{
    public class WhenDocumentUploaded : Specification<SIO.Domain.Documents.Document, SIO.Domain.Documents.DocumentState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _fileName = "Test Document";
        private readonly TranslationOption _translationOption = new TranslationOption("Test", TranslationType.Google);

        protected override IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected override void When()
        {
            Aggregate.Upload(_aggregateId, Guid.NewGuid(), _translationOption, _fileName);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Then]
        public void ShouldContainUncommitedDocumentUploadedEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().Should().BeOfType<DocumentUploaded>();
        }

        [Then]
        public void ShouldContainUncommitedDocumentUploadedWithCorrectName()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<DocumentUploaded>().Single();

            @event.FileName.Should().Be(_fileName);
        }

        [Then]
        public void ShouldContainUncommitedDocumentUploadedWithCondition()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<DocumentUploaded>().Single();

            @event.TranslationType.Should().Be(_translationOption.Type);
        }

        [Then]
        public void ShouldContainUncommitedDocumentUploadedEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<DocumentUploaded>().Single();

            @event.Version.Should().Be(1);
        }

        [Then]
        public void ShouldContainStateWithCorrectId()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<DocumentUploaded>().Single();
            Aggregate.GetState().Id.Should().Be(@event.AggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectName()
        {
            Aggregate.GetState().FileName.Should().Be(_fileName);
        }

        [Then]
        public void ShouldContainStateWithCorrectCondition()
        {
            Aggregate.GetState().Condition.Should().Be(DocumentCondition.Uploaded);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            Aggregate.Version.Should().Be(1);
        }
    }
}
