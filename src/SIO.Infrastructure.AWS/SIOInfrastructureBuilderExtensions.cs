using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.AWS.S3;
using SIO.Infrastructure.File;

namespace SIO.Infrastructure.AWS
{
    public static class ServiceCollectionExtensions
    {
        public static ISIOInfrastructureBuilder AddS3FileStorage(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<IFileClient, FileClient>();

            return builder;
        }
    }
}
