using System;
using FluentAssertions;
using SIO.Testing.Attributes;

namespace SIO.Tests.Unit.Domain.Document
{
    public class DocumentConstructionTests
    {
        [Unit]
        public void WhenConstructedWithNullStateThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.Documents.Document(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
