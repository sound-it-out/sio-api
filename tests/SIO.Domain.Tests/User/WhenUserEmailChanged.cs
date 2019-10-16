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
        private readonly string _email = "test@user.com";
        private readonly string _firstName = "test";
        private readonly string _lastName = "user";
        private readonly string _newEmail = "changed@user.com";

        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserRegistered(_aggregateId, _email, _firstName, _lastName);
            yield return new UserVerified(_aggregateId, 2);
        }

        protected override void When()
        {
            Aggregate.ChangeEmail(_aggregateId, _newEmail, 2);
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

            @event.Email.Should().Be(_newEmail);
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
            Aggregate.GetState().Id.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectEmail()
        {
            Aggregate.GetState().Email.Should().Be(_newEmail);
        }

        [Then]
        public void ShouldContainStateWithCorrectFirstName()
        {
            Aggregate.GetState().FirstName.Should().Be(_firstName);
        }

        [Then]
        public void ShouldContainStateWithCorrectLastName()
        {
            Aggregate.GetState().LastName.Should().Be(_lastName);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            Aggregate.Version.Should().Be(3);
        }
    }
}
