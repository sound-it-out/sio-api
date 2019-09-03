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
    public class WhenTranslationCharactersProcessed : Specification<SIO.Domain.Document.Document, SIO.Domain.Document.DocumentState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly long _charactersProcessed = 3000;

        protected override IEnumerable<IEvent> Given()
        {
            yield return new DocumentUploaded(_aggregateId, SIO.Domain.TranslationType.Google, "Test Document");
            yield return new TranslationQueued(_aggregateId, 2);
            yield return new TranslationStarted(_aggregateId, 3, 4000);
        }

        protected override void When()
        {
            Aggregate.ProcessTranslationCharacters(_aggregateId, 3, _charactersProcessed);
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

            events.Single().Should().BeOfType<TranslationCharactersProcessed>();
        }

        [Then]
        public void ShouldContainUncommitedTranslationStartedEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<TranslationCharactersProcessed>().Single();

            @event.Version.Should().Be(4);
        }

        [Then]
        public void ShouldContainStateWithCorrectId()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<TranslationCharactersProcessed>().Single();
            Aggregate.GetState().Id.Should().Be(@event.AggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectTranslationCharactersProcessed()
        {
            Aggregate.GetState().TranslationCharactersProcessed.Should().Be(_charactersProcessed);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<TranslationCharactersProcessed>().Single();
            Aggregate.Version.Should().Be(@event.Version);
        }
    }
}
