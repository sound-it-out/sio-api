using System;
using FluentAssertions;
using Xunit;

namespace SIO.Tests.Unit.Domain.Projections.UserDocuments
{
    public class UserConstructionTests
    {
        [Fact]
        public void WhenInstantiatedWithNullProjectionWriterThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.Projections.User.UserProjection(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
