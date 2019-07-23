using System;
using System.IO;
using System.Threading.Tasks;
using Clipboard;
using OpenEventSourcing.Commands;
using SIO.Domain.Translationss.Commands;
using SIO.Domain.Translationss.Services;

namespace SIO.Domain.Translations.CommandHandlers
{
    internal class QueueTranslationCommandHandler : ICommandHandler<QueueTranslationCommand>
    {
        private readonly ISpeechSynthesizerFactory _speechSynthesizerFactory;

        public QueueTranslationCommandHandler(ISpeechSynthesizerFactory speechSynthesizerFactory)
        {
            if (speechSynthesizerFactory == null)
                throw new ArgumentNullException(nameof(speechSynthesizerFactory));

            _speechSynthesizerFactory = speechSynthesizerFactory;
        }
        public async Task ExecuteAsync(QueueTranslationCommand command)
        {
            // TODO(Matt): Get file from blob storage
            var fileStream = File.Open("", FileMode.Open);
            

            using (var extractor = TextExtractor.Open(fileStream))
            {
                var content = await extractor.ExtractAsync();

                var speechSynthesizer = _speechSynthesizerFactory.Create();
                await speechSynthesizer.QueueSynthesisAsync(content);
            }
        }
    }
}
