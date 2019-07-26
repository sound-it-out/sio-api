using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.Google.Translation;
using SIO.Infrastructure.Translation;

namespace SIO.Infrastructure.Google
{
    public static class ServiceCollectionExtensions
    {
        public static ISIOInfrastructureBuilder AddGoogleSpeechToText(this ISIOInfrastructureBuilder builder)
        {
            builder.Services.AddTransient<ISpeechClient<GoogleSpeechRequest>, GoogleSpeechClient>();

            return builder;
        }
    }
}
