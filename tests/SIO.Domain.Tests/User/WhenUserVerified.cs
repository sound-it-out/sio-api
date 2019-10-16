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
        private readonly string _email = "test@user.com";
        private readonly string _firstName = "test";
        private readonly string _lastName = "user";

        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserRegistered(_aggregateId, _email, _firstName, _lastName);
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
            Aggregate.GetState().Id.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainStateWithCorrectEmail()
        {
            Aggregate.GetState().Email.Should().Be(_email);
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
        public void ShouldContainStateVerified()
        {
            Aggregate.GetState().Verified.Should().Be(true);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            Aggregate.Version.Should().Be(2);
        }
    }
}
