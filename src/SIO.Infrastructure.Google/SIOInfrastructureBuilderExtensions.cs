using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.Google.Speech;
using SIO.Infrastructure.Speech;

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
