using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.AWS.File;
using SIO.Infrastructure.File;

namespace SIO.Infrastructure.AWS
{
    public static class ServiceCollectionExtensions
    {
        public static ISIOInfrastructureBuilder AddS3FileStorage(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IFileClient, S3FileClient>();

            return builder;
        }
    }
}
