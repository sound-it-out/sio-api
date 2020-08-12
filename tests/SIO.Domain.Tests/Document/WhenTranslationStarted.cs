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
    public class WhenTranslationStarted : Specification<SIO.Domain.Document.Document, SIO.Domain.Document.DocumentState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _fileName = "Test Document";
        private readonly long _characterCount = 4000;

        protected override IEnumerable<IEvent> Given()
        {
            yield return new DocumentUploaded(_aggregateId, SIO.Domain.TranslationType.Google, _fileName);
            yield return new TranslationQueued(_aggregateId, 2);
        }

        protected override void When()
        {
            Aggregate.StartTranslation(_aggregateId, _characterCount, 2);
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

            events.Single().Should().BeOfType<TranslationStarted>();
        }

        [Then]
        public void ShouldContainUncommitedTranslationStartedEventWithCorrectId()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<TranslationStarted>().Single();

            @event.AggregateId.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainUncommitedTranslationStartedEventWithCorrectCharacterCount()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<TranslationStarted>().Single();

            @event.CharacterCount.Should().Be(_characterCount);
        }

        [Then]
        public void ShouldContainUncommitedTranslationStartedEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<TranslationStarted>().Single();

            @event.Version.Should().Be(3);
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
            Aggregate.GetState().Condition.Should().Be(DocumentCondition.TranslationStarted);
        }

        [Then]
        public void ShouldContainStateWithCorrectCharacterCount()
        {
            Aggregate.GetState().TranslationCharactersTotal.Should().Be(_characterCount);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            Aggregate.Version.Should().Be(3);
        }
    }
}
