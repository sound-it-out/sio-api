using System;
using System.IO;
using System.Threading.Tasks;
using Clipboard;
using SIO.Abstraction.Commands;
using SIO.Domain.Translation.Commands;
using SIO.Domain.Translation.Services;

namespace SIO.Domain.Translation.CommandHandlers
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
