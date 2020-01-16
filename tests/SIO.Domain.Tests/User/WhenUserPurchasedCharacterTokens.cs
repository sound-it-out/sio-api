using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.User.Events;

namespace SIO.Tests.Unit.Domain.User
{
    public class WhenUserPurchasedCharacterTokens : Specification<SIO.Domain.User.User, SIO.Domain.User.UserState>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _email = "test@user.com";
        private readonly string _firstName = "test";
        private readonly string _lastName = "user";
        private readonly long _characterTokens = 20000;

        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserRegistered(_aggregateId, Guid.NewGuid(), _aggregateId.ToString(), _email, _firstName, _lastName, "");
            yield return new UserVerified(_aggregateId, Guid.NewGuid(), _aggregateId.ToString());
        }

        protected override void When()
        {
            Aggregate.PurchaseTokens(_aggregateId, 0, _characterTokens);
        }

        [Then]
        public void ShouldContainOneUncommitedEvent()
        {
            Aggregate.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Then]
        public void ShouldContainUncommitedUserPurchasedCharacterTokensEvent()
        {
            var events = Aggregate.GetUncommittedEvents();

            events.Single().Should().BeOfType<UserPurchasedCharacterTokens>();
        }

        [Then]
        public void ShouldContainUncommitedUserPurchasedCharacterTokensWithCorrectId()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserPurchasedCharacterTokens>().Single();

            @event.AggregateId.Should().Be(_aggregateId);
        }

        [Then]
        public void ShouldContainUncommitedUserPurchasedCharacterTokensWithCorrectCharacterTokens()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserPurchasedCharacterTokens>().Single();

            @event.CharacterTokens.Should().Be(_characterTokens);
        }

        [Then]
        public void ShouldContainUncommitedUserPurchasedCharacterTokensEventWithCorrectVersion()
        {
            var events = Aggregate.GetUncommittedEvents();

            var @event = events.OfType<UserPurchasedCharacterTokens>().Single();

            @event.Version.Should().Be(0);
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
        public void ShouldContainStateWithCorrectCharacterTokens()
        {
            Aggregate.GetState().CharacterTokens.Should().Be(_characterTokens);
        }

        [Then]
        public void ShouldContainStateWithCorrectVersion()
        {
            Aggregate.Version.Should().Be(0);
        }
    }
}
