using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SIO.Infrastructure.Translations;

namespace SIO.Infrastructure.Google.Tests.Translations.GoogleTranslationOptionsRetriever
{
    public sealed class GoogleTranslationOptionsRetrieverFixture : ITranslationOptionsRetriever, IDisposable
    {
        private ITranslationOptionsRetriever _translationOptionsRetriever;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private Task<IEnumerable<TranslationOption>> _retrieveTask;
        private readonly object _lockObj = new object();

        public void InitSynthesizer(ITranslationOptionsRetriever translationOptionsRetriever) => _translationOptionsRetriever = translationOptionsRetriever;

        public void Dispose()
        {
            _cts.Cancel();
        }

        public async Task<IEnumerable<TranslationOption>> RetrieveAsync()
        {
            lock (_lockObj)
            {
                if (_retrieveTask == null)
                {
                    _retrieveTask = Task.Run(async () => await _translationOptionsRetriever.RetrieveAsync(), _cts.Token);
                }
            }

            return await _retrieveTask;
        }
    }
}
