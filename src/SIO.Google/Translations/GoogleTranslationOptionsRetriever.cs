using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Grpc.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SIO.Domain.Documents.Events;
using SIO.Domain.Translations.Services;

namespace SIO.Google.Translations
{
    internal sealed class GoogleTranslationOptionsRetriever : ITranslationOptionsRetriever
    {
        private readonly ILogger<GoogleTranslationOptionsRetriever> _logger;
        private readonly TextToSpeechClient _client;

        public GoogleTranslationOptionsRetriever(ILogger<GoogleTranslationOptionsRetriever> logger,
            IOptions<GoogleCredentialOptions> googleCredentialOptions)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (googleCredentialOptions == null)
                throw new ArgumentNullException(nameof(googleCredentialOptions));

            var credentials = GoogleCredential.FromJson(JsonConvert.SerializeObject(googleCredentialOptions.Value));

            var builder = new TextToSpeechClientBuilder();
            builder.ChannelCredentials = credentials.ToChannelCredentials();

            _logger = logger;
            _client = builder.Build();
        }

        public async Task<IEnumerable<TranslationOption>> GetTranslationsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GoogleTranslationOptionsRetriever)}.{nameof(GetTranslationsAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var voices = await _client.ListVoicesAsync(new ListVoicesRequest(), cancellationToken);
            return voices.Voices.Select(v => new TranslationOption(v.Name, TranslationType.Google));
        }
    }
}
