using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenEventSourcing.Events;
using OpenEventSourcing.Extensions;
using SIO.Domain.Projections.UserDocuments;
using SIO.Domain.User.Events;

namespace SIO.Tests.Unit.Domain.Projections.UserDocuments
{
    public class WhenUserVerified : Specification<UserDocumentsProjection>
    {
        private readonly Guid _userId = Guid.NewGuid().ToSequentialGuid();
        protected override IEnumerable<IEvent> Given()
        {
            yield return new UserVerified(_userId, Guid.NewGuid(), _userId.ToString());
        }

        [Then]
        public void ThenUserDocumentsShouldNotBeNull()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.Should().NotBeNull();
        }

        [Then]
        public void ThenUserDocumentsShouldHaveCorrectUserId()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.UserId.Should().Be(_userId);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveCorrectNumberOfDocuments()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.Data.Documents.Count.Should().Be(0);
        }

        [Then]
        public void ThenUserDocumentsShouldHaveCorrectVersion()
        {
            var userDocuments = Context.Find<SIO.Domain.Projections.UserDocuments.UserDocuments>(_userId);

            userDocuments.Version.Should().Be(1);
        }
    }
}
