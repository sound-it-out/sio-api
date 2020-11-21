using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.Projections.Users;
using SIO.Domain.Users.Events;
using SIO.Testing.Attributes;

namespace SIO.Tests.Unit.Domain.Projections.User
{
    public class WhenUserEmailChanged : Specification<UserProjection>
    {
        private readonly Guid _aggregateId = Guid.NewGuid().ToSequentialGuid();
        private readonly string _email = "test@user.com";
        private readonly string _newEmail = "changed@user.com";
        private readonly string _firstName = "test";
        private readonly string _lastName = "user";
        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserRegistered(_aggregateId, Guid.NewGuid(), _aggregateId.ToString(), _email, _firstName, _lastName, "");
            yield return new UserVerified(_aggregateId, Guid.NewGuid(), _aggregateId.ToString());
            yield return new UserEmailChanged(_aggregateId, Guid.NewGuid(), 0, _aggregateId.ToString(), _newEmail);
        }

        [Then]
        public void ThenUserShouldNotBeNull()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Should().NotBeNull();
        }

        [Then]
        public void ThenUserShouldHaveCorrectAggregateId()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Id.Should().Be(_aggregateId);
        }

        [Then]
        public void ThenUserShouldHaveCorrectEmail()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Data.Email.Should().Be(_newEmail);
        }

        [Then]
        public void ThenUserShouldHaveCorrectFirstName()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Data.FirstName.Should().Be(_firstName);
        }

        [Then]
        public void ThenUserShouldHaveCorrectLastName()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Data.LastName.Should().Be(_lastName);
        }

        [Then]
        public void ThenUserShouldntBeVerified()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Data.Verified.Should().Be(true);
        }

        [Then]
        public void ThenUserShouldHaveCorrectCharacterTokens()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Data.CharacterTokens.Should().Be(0);
        }

        [Then]
        public void ThenUserShouldHaveCorrectVersion()
        {
            var user = Context.Find<SIO.Domain.Projections.Users.User>(_aggregateId);

            user.Version.Should().Be(0);
        }
    }
}
