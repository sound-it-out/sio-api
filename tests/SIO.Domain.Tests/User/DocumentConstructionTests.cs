using System;
using FluentAssertions;
using SIO.Testing.Attributes;

namespace SIO.Tests.Unit.Domain.User
{
    public class UserConstructionTests
    {
        [Unit]
        public void WhenConstructedWithNullStateThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.Users.User(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
