using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain.Translations.Services;
using SIO.Google.Translations;

namespace SIO.Google.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGoogleTranslations(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddGoogleConfiguration(configuration)
                .AddGoogleTranslations();
        }

        public static IServiceCollection AddGoogleTranslations(this IServiceCollection services)
        {
            services.AddScoped<ITranslationOptionsRetriever, GoogleTranslationOptionsRetriever>();
            return services;
        }

        public static IServiceCollection AddGoogleConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleCredentialOptions>(o =>
            {
                o.Type = configuration.GetValue<string>("Google__Credentials__type");
                o.ProjectId = configuration.GetValue<string>("Google__Credentials__project_id");
                o.PrivateKeyId = configuration.GetValue<string>("Google__Credentials__private_key_id");
                o.PrivateKey = configuration.GetValue<string>("Google__Credentials__private_key");
                o.ClientEmail = configuration.GetValue<string>("Google__Credentials__client_email");
                o.ClientId = configuration.GetValue<string>("Google__Credentials__client_id");
                o.AuthUri = configuration.GetValue<string>("Google__Credentials__auth_uri");
                o.TokenUri = configuration.GetValue<string>("Google__Credentials__token_uri");
                o.AuthProviderX509CertUrl = configuration.GetValue<string>("Google__Credentials__auth_provider_x509_cert_url");
                o.ClientX509CertUrl = configuration.GetValue<string>("Google__Credentials__client_x509_cert_url");
            });
            return services;
        }
    }
}
