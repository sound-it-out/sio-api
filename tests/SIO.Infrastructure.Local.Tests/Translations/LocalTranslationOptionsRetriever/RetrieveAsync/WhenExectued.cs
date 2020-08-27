using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SIO.Infrastructure.Translations;
using SIO.Testing.Attributes;

namespace SIO.Infrastructure.Local.Tests.Translations.LocalTranslationOptionsRetriever.RetrieveAsync
{
    public class WhenExectued : LocalTranslationOptionsRetrieverSpecification
    {
        protected override async Task<IEnumerable<TranslationOption>> Given()
        {
            return await TranslationOptionsRetriever.RetrieveAsync();
        }

        protected override Task When()
        {
            return Task.CompletedTask;
        }

        [Then]
        public void ResultShouldNotBeEmpty()
        {
            Result.Should().NotBeEmpty();
        }

        [Then]
        public void ResultShouldOnlyContainLocalTranslationOptions()
        {
            Result.All(to => to.Type == TranslationType.Local).Should().BeTrue();
        }
    }
}
