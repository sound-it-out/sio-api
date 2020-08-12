using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;
using SIO.Tests.Infrastructure;

namespace SIO.Tests.Unit.Domain.Document
{
    public class WhenTranslationQueued : Specification<SIO.Domain.Document.Document, SIO.Domain.Document.DocumentState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _fileName = "Test Document";

        protected override IEnumerable<IEvent> Given()
        {
            yield return new DocumentUploaded(_aggregateId, SIO.Domain.TranslationType.Google, _fileName);
        }

        protected override void When()
        {
            Aggregate.QueueTranslation(_aggregateId, 1);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Then]
        public void ShouldContainUncommitedTranslationQueuedEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().Should().BeOfType<TranslationQueued>();
        }

        [Then]
        public void ShouldContainUncommitedTranslationQueuedEventWithCorrectId()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<TranslationQueued>().Single();

            @event.AggregateId.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainUncommitedTranslationQueuedEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<TranslationQueued>().Single();

            @event.Version.Should().Be(2);
        }

        [Then]
        public void ShouldContainStateWithCorrectId()
        {
            Aggregate.GetState().Id.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectName()
        {
            Aggregate.GetState().FileName.Should().Be(_fileName);
        }

        [Then]
        public void ShouldContainStateWithCorrectCondition()
        {
            Aggregate.GetState().Condition.Should().Be(DocumentCondition.TranslationQueued);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            Aggregate.Version.Should().Be(2);
        }
    }
}
