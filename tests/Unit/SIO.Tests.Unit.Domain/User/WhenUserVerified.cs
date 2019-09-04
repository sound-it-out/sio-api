using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.User.Events;

namespace SIO.Tests.Unit.Domain.User
{
    public class WhenUserVerified : Specification<SIO.Domain.User.User, SIO.Domain.User.UserState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserRegistered(_aggregateId, "test@user.com", "test", "user");
        }

        protected override void When()
        {
            Aggregate.Verify(_aggregateId, 1);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Then]
        public void ShouldContainUncommitedUserVerifiedEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().Should().BeOfType<UserVerified>();
        }

        [Then]
        public void ShouldContainUncommitedUserVerifiedWithCorrectId()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserVerified>().Single();

            @event.AggregateId.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainUncommitedUserVerifiedEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserVerified>().Single();

            @event.Version.Should().Be(2);
        }

        [Then]
        public void ShouldContainStateWithCorrectId()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<UserVerified>().Single();
            Aggregate.GetState().Id.Should().Be(@event.AggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectVerified()
        {
            Aggregate.GetState().Verified.Should().Be(true);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<UserVerified>().Single();
            Aggregate.Version.Should().Be(@event.Version);
        }
    }
}
