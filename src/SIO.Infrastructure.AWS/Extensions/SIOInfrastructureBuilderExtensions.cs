using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.AWS.Files;
using SIO.Infrastructure.AWS.Translations;
using SIO.Infrastructure.Files;
using SIO.Infrastructure.Translations;

namespace SIO.Infrastructure.AWS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ISIOInfrastructureBuilder AddAWSInfrastructure(this ISIOInfrastructureBuilder builder, IConfiguration configuration)
        {
            return builder.AddAWSConfiguration(configuration)
                .AddAWSFiles()
                .AddAWSTranslations();
        }

        public static ISIOInfrastructureBuilder AddAWSFiles(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IFileClient, AWSFileClient>();
            return builder;
        }

        public static ISIOInfrastructureBuilder AddAWSConfiguration(this ISIOInfrastructureBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<AWSCredentialOptions>(configuration.GetSection("AWS:Credentails"));
            builder.Services.Configure<AWSFileOptions>(configuration.GetSection("AWS:S3"));
            return builder;
        }

        public static ISIOInfrastructureBuilder AddAWSTranslations(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<ITranslationOptionsRetriever, AWSTranslationOptionsRetriever>();
            return builder;
        }
    }
}
