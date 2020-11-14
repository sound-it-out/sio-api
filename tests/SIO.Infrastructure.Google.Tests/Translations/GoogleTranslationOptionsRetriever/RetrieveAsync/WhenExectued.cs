using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SIO.Infrastructure.Translations;
using SIO.Testing.Attributes;

namespace SIO.Infrastructure.Google.Tests.Translations.GoogleTranslationOptionsRetriever.RetrieveAsync
{
    public class WhenExectued : GoogleTranslationOptionsRetrieverSpecification
    {
        public WhenExectued(ConfigurationFixture configurationFixture, GoogleTranslationOptionsRetrieverFixture googleTranslationOptionsRetrieverFixture) : base(configurationFixture, googleTranslationOptionsRetrieverFixture)
        {
        }

        protected override async Task<IEnumerable<TranslationOption>> Given()
        {
            return await TranslationOptionsRetriever.RetrieveAsync();
        }

        protected override Task When()
        {
            return Task.CompletedTask;
        }

        [Integration]
        public void ResultShouldNotBeEmpty()
        {
            Result.Should().NotBeEmpty();
        }

        [Integration]
        public void ResultShouldOnlyContainGoogleTranslationOptions()
        {
            Result.All(to => to.Type == TranslationType.Google).Should().BeTrue();
        }
    }
}
