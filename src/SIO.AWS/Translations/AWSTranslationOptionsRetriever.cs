using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Polly;
using Microsoft.Extensions.Logging;
using SIO.Domain.Documents.Events;
using SIO.Domain.Translations.Services;

namespace SIO.AWS.Translations
{
    internal sealed class AWSTranslationOptionsRetriever : ITranslationOptionsRetriever
    {
        private readonly ILogger<AWSTranslationOptionsRetriever> _logger;

        public AWSTranslationOptionsRetriever(ILogger<AWSTranslationOptionsRetriever> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public Task<IEnumerable<TranslationOption>> GetTranslationsAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(AWSTranslationOptionsRetriever)}.{nameof(GetTranslationsAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            return Task.FromResult(RetrieveVoices().Select(v => new TranslationOption(v, TranslationType.AWS)));
        }

        private IEnumerable<string> RetrieveVoices()
        {
            yield return VoiceId.Aditi;
            yield return VoiceId.Lotte;
            yield return VoiceId.Lucia;
            yield return VoiceId.Lupe;
            yield return VoiceId.Mads;
            yield return VoiceId.Maja;
            yield return VoiceId.Marlene;
            yield return VoiceId.Mathieu;
            yield return VoiceId.Matthew;
            yield return VoiceId.Maxim;
            yield return VoiceId.Mia;
            yield return VoiceId.Miguel;
            yield return VoiceId.Mizuki;
            yield return VoiceId.Naja;
            yield return VoiceId.Nicole;
            yield return VoiceId.Penelope;
            yield return VoiceId.Raveena;
            yield return VoiceId.Ricardo;
            yield return VoiceId.Ruben;
            yield return VoiceId.Russell;
            yield return VoiceId.Salli;
            yield return VoiceId.Seoyeon;
            yield return VoiceId.Takumi;
            yield return VoiceId.Tatyana;
            yield return VoiceId.Vicki;
            yield return VoiceId.Vitoria;
            yield return VoiceId.Zeina;
            yield return VoiceId.Zhiyu;
            yield return VoiceId.Liv;
            yield return VoiceId.Kimberly;
            yield return VoiceId.Lea;
            yield return VoiceId.Enrique;
            yield return VoiceId.Astrid;
            yield return VoiceId.Bianca;
            yield return VoiceId.Brian;
            yield return VoiceId.Camila;
            yield return VoiceId.Carla;
            yield return VoiceId.Carmen;
            yield return VoiceId.Celine;
            yield return VoiceId.Chantal;
            yield return VoiceId.Conchita;
            yield return VoiceId.Cristiano;
            yield return VoiceId.Dora;
            yield return VoiceId.Emma;
            yield return VoiceId.Kendra;
            yield return VoiceId.Ewa;
            yield return VoiceId.Filiz;
            yield return VoiceId.Geraint;
            yield return VoiceId.Giorgio;
            yield return VoiceId.Gwyneth;
            yield return VoiceId.Hans;
            yield return VoiceId.Ines;
            yield return VoiceId.Ivy;
            yield return VoiceId.Jacek;
            yield return VoiceId.Jan;
            yield return VoiceId.Joanna;
            yield return VoiceId.Joey;
            yield return VoiceId.Justin;
            yield return VoiceId.Karl;
            yield return VoiceId.Amy;
        }
    }
}
