using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.Files;
using SIO.Infrastructure.Local.Files;
using SIO.Infrastructure.Local.Translations;
using SIO.Infrastructure.Translations;

namespace SIO.Infrastructure.AWS
{
    public static class ServiceCollectionExtensions
    {
        public static ISIOInfrastructureBuilder AddLocalInfrastructure(this ISIOInfrastructureBuilder builder)
        {
            return builder.AddLocalFiles()
                .AddLocalTranslations();
        }

        public static ISIOInfrastructureBuilder AddLocalFiles(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IFileClient, LocalFileClient>();
            return builder;
        }

        public static ISIOInfrastructureBuilder AddLocalTranslations(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<ITranslationOptionsRetriever, LocalTranslationOptionsRetriever>();
            return builder;
        }
    }
}
