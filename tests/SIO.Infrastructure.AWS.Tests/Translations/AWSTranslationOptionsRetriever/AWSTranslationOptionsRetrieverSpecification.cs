using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.AWS.Extensions;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Translations;
using SIO.Testing.Abstractions;

namespace SIO.Infrastructure.AWS.Tests.Translations.AWSTranslationOptionsRetriever
{
    public abstract class AWSTranslationOptionsRetrieverSpecification : Specification<IEnumerable<TranslationOption>>
    {
        protected ITranslationOptionsRetriever TranslationOptionsRetriever => _serviceProvider.GetRequiredService<ITranslationOptionsRetriever>();

        protected override void BuildServices(IServiceCollection services)
        {
            base.BuildServices(services);
            services.AddSIOInfrastructure()
                .AddAWSTranslations();
        }
    }
}
