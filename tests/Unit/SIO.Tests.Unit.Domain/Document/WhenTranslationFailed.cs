using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.Document;
using SIO.Domain.Document.Events;
using SIO.Domain.Translation.Events;

namespace SIO.Tests.Unit.Domain.Document
{
    public class WhenTranslationSucceded : Specification<SIO.Domain.Document.Document, SIO.Domain.Document.DocumentState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new DocumentUploaded(_aggregateId, SIO.Domain.TranslationType.Google, "Test Document");
            yield return new TranslationQueued(_aggregateId, 2);
            yield return new TranslationStarted(_aggregateId, 3, 4000);
            yield return new TranslationCharactersProcessed(_aggregateId, 4, 3000);
        }

        protected override void When()
        {
            Aggregate.AcceptTranslation(_aggregateId, 4);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Then]
        public void ShouldContainUncommitedTranslationStartedEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().Should().BeOfType<TranslationSucceded>();
        }

        [Then]
        public void ShouldContainUncommitedTranslationStartedEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<TranslationSucceded>().Single();

            @event.Version.Should().Be(5);
        }

        [Then]
        public void ShouldContainStateWithCorrectId()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<TranslationSucceded>().Single();
            Aggregate.GetState().Id.Should().Be(@event.AggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectDocumentCondition()
        {
            Aggregate.GetState().Condition.Should().Be(DocumentCondition.TranslationSucceded);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<TranslationSucceded>().Single();
            Aggregate.Version.Should().Be(@event.Version);
        }
    }
}
