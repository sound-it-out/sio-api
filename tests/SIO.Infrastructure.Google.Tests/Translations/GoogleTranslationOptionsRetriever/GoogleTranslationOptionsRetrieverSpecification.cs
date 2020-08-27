using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Google.Extensions;
using SIO.Infrastructure.Translations;
using SIO.Testing.Abstractions;
using Xunit;

namespace SIO.Infrastructure.Google.Tests.Translations.GoogleTranslationOptionsRetriever
{
    public abstract class GoogleTranslationOptionsRetrieverSpecification : SpecificationWithConfiguration<ConfigurationFixture, IEnumerable<TranslationOption>>, IClassFixture<GoogleTranslationOptionsRetrieverFixture>
    {
        private readonly Lazy<GoogleTranslationOptionsRetrieverFixture> _googleTranslationOptionsRetrieverFixture;
        protected ITranslationOptionsRetriever TranslationOptionsRetriever => _googleTranslationOptionsRetrieverFixture.Value;

        public GoogleTranslationOptionsRetrieverSpecification(ConfigurationFixture configurationFixture, GoogleTranslationOptionsRetrieverFixture googleTranslationOptionsRetrieverFixture) : base(configurationFixture)
        {
            _googleTranslationOptionsRetrieverFixture = new Lazy<GoogleTranslationOptionsRetrieverFixture>(() =>
            {
                googleTranslationOptionsRetrieverFixture.InitSynthesizer(_serviceProvider.GetRequiredService<ITranslationOptionsRetriever>());
                return googleTranslationOptionsRetrieverFixture;
            });
        }

        protected override void BuildServices(IServiceCollection services)
        {
            base.BuildServices(services);
            services.AddSIOInfrastructure()
                .AddGoogleConfiguration(_configurationFixture.Configuration)
                .AddGoogleTranslations();
        }
    }
}
