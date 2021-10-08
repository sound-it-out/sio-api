using Microsoft.Extensions.DependencyInjection;
using SIO.AWS.Translations;
using SIO.Domain.Translations.Services;

namespace SIO.AWS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAWSTranslations(this IServiceCollection services)
        {
            services.AddSingleton<ITranslationOptionsRetriever, AWSTranslationOptionsRetriever>();
            return services;
        }
    }
}
