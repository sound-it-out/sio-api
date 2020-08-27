using System;
using FluentAssertions;
using SIO.Testing.Attributes;

namespace SIO.Tests.Unit.Domain.Projections.UserDocuments
{
    public class UserConstructionTests
    {
        [Unit]
        public void WhenInstantiatedWithNullProjectionWriterThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.Projections.User.UserProjection(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
