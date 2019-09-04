using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using SIO.Domain;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;

namespace SIO.Tests.Unit.Domain.Document
{
    public class WhenCompanyCreated : Specification<SIO.Domain.Document.Document, SIO.Domain.Document.DocumentState>
    {
        private readonly string _fileName = "Test Document";
        private readonly TranslationType _translationType = TranslationType.Google;

        protected override IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected override void When()
        {
            Aggregate.Upload(_translationType, _fileName);
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

            @event.TranslationType.Should().Be(_translationType);
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
            var @event = Aggregate.GetUncommittedEvents().OfType<DocumentUploaded>().Single();
            Aggregate.GetState().FileName.Should().Be(@event.FileName);
        }

        [Then]
        public void ShouldContainStateWithCorrectCondition()
        {
            Aggregate.GetState().Condition.Should().Be(DocumentCondition.Uploaded);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<DocumentUploaded>().Single();
            Aggregate.Version.Should().Be(@event.Version);
        }
    }
}
