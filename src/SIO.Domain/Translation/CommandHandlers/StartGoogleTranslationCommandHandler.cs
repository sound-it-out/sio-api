using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Clipboard;
using Google.Cloud.TextToSpeech.V1;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.Document;
using SIO.Domain.Extensions;
using SIO.Infrastructure.File;
using SIO.Infrastructure.Google.Speech;
using SIO.Infrastructure.Speech;

namespace SIO.Domain.Translation.Commands
{
    public class StartGoogleTranslationCommandHandler : ICommandHandler<StartGoogleTranslationCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IFileClient _fileClient;
        private readonly ISpeechClient<GoogleSpeechRequest> _speechClient;

        public StartGoogleTranslationCommandHandler(IEventBus eventBus, 
            ICommandDispatcher commandDispatcher, 
            IAggregateRepository aggregateRepository,
            IFileClient fileClient,
            ISpeechClient<GoogleSpeechRequest> speechClient)
        {
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (fileClient == null)
                throw new ArgumentNullException(nameof(fileClient));
            if (speechClient == null)
                throw new ArgumentNullException(nameof(speechClient));

            _eventBus = eventBus;
            _commandDispatcher = commandDispatcher;
            _aggregateRepository = aggregateRepository;
            _fileClient = fileClient;
            _speechClient = speechClient;
        }

        public async Task ExecuteAsync(StartGoogleTranslationCommand command)
        {
            var fileResult = await _fileClient.DownloadAsync(
                fileName: command.CorrelationId.ToString(),
                userId: command.UserId
            );

            var fileStream = await fileResult.OpenStreamAsync();
            var textExtractor = TextExtractor.Open(fileStream, fileResult.ContentType);

            var text = await textExtractor.ExtractAsync();

            textExtractor.Dispose();
            fileStream.Dispose();

            var textChunks = text.ChunkWithDelimeters(5000, '.', '!', '?', ')', '"', '}', ']');

            var aggregate = await _aggregateRepository.GetAsync<Document.Document, DocumentState>(command.CorrelationId);
            aggregate.StartTranslation(command.CorrelationId, command.Version, textChunks.Sum(tc => tc.Length));

            var events = aggregate.GetUncommittedEvents();

            foreach (var @event in events)
                @event.UpdateFrom(command);

            events = events.ToList();

            await _aggregateRepository.SaveAsync<DocumentState>(aggregate, command.Version);
            await _eventBus.PublishAsync(events);

            int version = command.Version;

            try
            {
                var result = await _speechClient.TranslateTextAsync(new GoogleSpeechRequest(
                    voiceSelection: new VoiceSelectionParams
                    {
                        LanguageCode = "en-US",
                        SsmlGender = SsmlVoiceGender.Neutral
                    },
                    audioConfig: new AudioConfig
                    {
                        AudioEncoding = AudioEncoding.Mp3
                    },
                    content: textChunks,
                    callback: async length =>
                    {
                        Interlocked.Increment(ref version);

                        aggregate.ProcessTranslationCharacters(command.CorrelationId, version, length);

                        var tempEvents = aggregate.GetUncommittedEvents();

                        foreach (var @event in tempEvents)
                            @event.UpdateFrom(command);

                        tempEvents = tempEvents.ToList();

                        await _aggregateRepository.SaveAsync<DocumentState>(aggregate, version);
                        await _eventBus.PublishAsync(tempEvents);
                    }
                ));


                using (var stream = await result.OpenStreamAsync())
                {
                    await _commandDispatcher.DispatchAsync(new SaveTranslationCommand(
                        aggregateId: command.AggregateId,
                        command.CorrelationId,
                        version: version + 1,
                        userId: command.UserId,
                        stream: stream
                    ));
                }
            }
            catch(Exception e)
            {
                aggregate.FailTranslation(command.CorrelationId, version, e.Message);

                var newEvents = aggregate.GetUncommittedEvents();

                foreach (var @event in newEvents)
                    @event.UpdateFrom(command);

                events = events.ToList();

                await _aggregateRepository.SaveAsync<DocumentState>(aggregate, version);
                await _eventBus.PublishAsync(events);
            }    
        }
    }
}
