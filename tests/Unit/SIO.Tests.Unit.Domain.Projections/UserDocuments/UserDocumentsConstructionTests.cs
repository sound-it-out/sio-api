using System;
using FluentAssertions;
using Xunit;

namespace SIO.Tests.Unit.Domain.Projections.UserDocuments
{
    public class UserDocumentsConstructionTests
    {
        [Fact]
        public void WhenInstantiatedWithNullProjectionWriterThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.Projections.Document.DocumentProjection(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
