using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.User.Events;

namespace SIO.Tests.Unit.Domain.User
{
    public class WhenUserRegistered : Specification<SIO.Domain.User.User, SIO.Domain.User.UserState>
    {        
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _email = "test@user.com";
        private readonly string _firstName = "test";
        private readonly string _lastName = "user";

        protected override IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected override void When()
        {
            Aggregate.Register(_aggregateId, _email, _firstName, _lastName);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Then]
        public void ShouldContainUncommitedUserRegisteredEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().Should().BeOfType<UserRegistered>();
        }
        

        [Then]
        public void ShouldContainUncommitedUserRegisteredWithCorrectId()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserRegistered>().Single();

            @event.AggregateId.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainUncommitedUserRegisteredWithCorrectEmail()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserRegistered>().Single();

            @event.Email.Should().Be(_email);
        }

        [Then]
        public void ShouldContainUncommitedUserRegisteredWithFirstName()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserRegistered>().Single();

            @event.FirstName.Should().Be(_firstName);
        }

        [Then]
        public void ShouldContainUncommitedUserRegisteredWithLastName()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserRegistered>().Single();

            @event.LastName.Should().Be(_lastName);
        }

        [Then]
        public void ShouldContainUncommitedUserRegisteredEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserRegistered>().Single();

            @event.Version.Should().Be(1);
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
        public void ShouldContainStateWithCorrectVersion()
        {
            Aggregate.Version.Should().Be(1);
        }
    }
}
