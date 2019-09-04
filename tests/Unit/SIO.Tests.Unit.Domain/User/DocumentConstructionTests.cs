using System;
using FluentAssertions;
using Xunit;

namespace SIO.Tests.Unit.Domain.User
{
    public class UserConstructionTests
    {
        [Fact]
        public void WhenConstructedWithNullStateThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.User.User(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
