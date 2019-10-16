using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace SIO.Tests.Unit.Domain.Document
{
    public class DocumentConstructionTests
    {
        [Fact]
        public void WhenConstructedWithNullStateThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.Document.Document(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
