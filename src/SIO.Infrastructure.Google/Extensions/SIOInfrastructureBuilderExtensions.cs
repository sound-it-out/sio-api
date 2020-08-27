using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.Google.Translations;
using SIO.Infrastructure.Translations;

namespace SIO.Infrastructure.Google.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ISIOInfrastructureBuilder AddGoogleInfrastructure(this ISIOInfrastructureBuilder builder, IConfiguration configuration)
        {
            return builder.AddGooglConfiguration(configuration)
                .AddGoogleTranslations();
        }

        public static ISIOInfrastructureBuilder AddGoogleTranslations(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<ITranslationOptionsRetriever, GoogleTranslationOptionsRetriever>();
            return builder;
        }

        public static ISIOInfrastructureBuilder AddGooglConfiguration(this ISIOInfrastructureBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<GoogleCredentialOptions>(configuration.GetSection("Google:Credentails"));
            return builder;
        }
    }
}
