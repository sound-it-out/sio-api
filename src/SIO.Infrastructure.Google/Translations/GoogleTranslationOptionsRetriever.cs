using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Grpc.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SIO.Infrastructure.Translations;

namespace SIO.Infrastructure.Google.Translations
{
    internal sealed class GoogleTranslationOptionsRetriever : ITranslationOptionsRetriever
    {
        private readonly TextToSpeechClient _client;

        public GoogleTranslationOptionsRetriever(IOptions<GoogleCredentialOptions> googleCredentialOptions)
        {
            if (googleCredentialOptions == null)
                throw new ArgumentNullException(nameof(googleCredentialOptions));

            var credentials = GoogleCredential.FromJson(JsonConvert.SerializeObject(googleCredentialOptions.Value));

            var builder = new TextToSpeechClientBuilder();
            builder.ChannelCredentials = credentials.ToChannelCredentials();
            _client = builder.Build();
        }

        public async Task<IEnumerable<TranslationOption>> RetrieveAsync()
        {
            var voices = await _client.ListVoicesAsync(new ListVoicesRequest());
            return voices.Voices.Select(v => new TranslationOption(v.Name, TranslationType.Google));
        }
    }
}
