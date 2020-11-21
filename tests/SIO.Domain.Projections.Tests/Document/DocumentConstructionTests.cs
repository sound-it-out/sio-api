using System;
using FluentAssertions;
using SIO.Testing.Attributes;

namespace SIO.Tests.Unit.Domain.Projections.Document
{
    public class DocumentConstructionTests
    {
        [Unit]
        public void WhenInstantiatedWithNullProjectionWriterThenShouldThrowArgumentNullException()
        {
            Action act = () => new SIO.Domain.Projections.Documents.DocumentProjection(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
