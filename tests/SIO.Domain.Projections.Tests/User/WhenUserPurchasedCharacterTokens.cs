using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.Projections.User;
using SIO.Domain.User.Events;

namespace SIO.Tests.Unit.Domain.Projections.User
{
    public class WhenUserPurchasedCharacterTokens : Specification<UserProjection>
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
            yield return new UserPurchasedCharacterTokens(_aggregateId, 3, _characterTokens);
        }

        [Then]
        public void ThenUserShouldNotBeNull()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Should().NotBeNull();
        }

        [Then]
        public void ThenUserShouldHaveCorrectAggregateId()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Id.Should().Be(_aggregateId);
        }

        [Then]
        public void ThenUserShouldHaveCorrectEmail()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Data.Email.Should().Be(_email);
        }

        [Then]
        public void ThenUserShouldHaveCorrectFirstName()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Data.FirstName.Should().Be(_firstName);
        }

        [Then]
        public void ThenUserShouldHaveCorrectLastName()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Data.LastName.Should().Be(_lastName);
        }

        [Then]
        public void ThenUserShouldntBeVerified()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Data.Verified.Should().Be(true);
        }

        [Then]
        public void ThenUserShouldHaveCorrectCharacterTokens()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Data.CharacterTokens.Should().Be(_characterTokens);
        }

        [Then]
        public void ThenUserShouldHaveCorrectVersion()
        {
            var user = Context.Find<SIO.Domain.Projections.User.User>(_aggregateId);

            user.Version.Should().Be(0);
        }
    }
}
