using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.User.Events;

namespace SIO.Tests.Unit.Domain.User
{
    public class WhenUserEmailChanged : Specification<SIO.Domain.User.User, SIO.Domain.User.UserState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _email = "changed@user.com";

        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserRegistered(_aggregateId, "test@user.com", "test", "user");
            yield return new UserVerified(_aggregateId, 2);
        }

        protected override void When()
        {
            Aggregate.ChangeEmail(_aggregateId, _email, 2);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Then]
        public void ShouldContainUncommitedUserEmailChangedEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().Should().BeOfType<UserEmailChanged>();
        }

        [Then]
        public void ShouldContainUncommitedUserEmailChangedWithCorrectId()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserEmailChanged>().Single();

            @event.AggregateId.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainUncommitedUserEmailChangedWithCorrectEmail()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserEmailChanged>().Single();

            @event.Email.Should().Be(_email);
        }

        [Then]
        public void ShouldContainUncommitedUserEmailChangedEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserEmailChanged>().Single();

            @event.Version.Should().Be(3);
        }

        [Then]
        public void ShouldContainStateWithCorrectId()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<UserEmailChanged>().Single();
            Aggregate.GetState().Id.Should().Be(@event.AggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectEmail()
        {
            Aggregate.GetState().Email.Should().Be(_email);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            var @event = Aggregate.GetUncommittedEvents().OfType<UserEmailChanged>().Single();
            Aggregate.Version.Should().Be(@event.Version);
        }
    }
}
