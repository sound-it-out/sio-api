using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Google.Protobuf;
using Grpc.Auth;
using Grpc.Core;
using OpenEventSourcing.Commands;
using SIO.Domain.Extensions;

namespace SIO.Domain.Translations.Commands
{
    public class GenerateGoogleTranslationCommandHandler : ICommandHandler<GenerateGoogleTranslationCommand>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public GenerateGoogleTranslationCommandHandler(ICommandDispatcher commandDispatcher)
        {
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            _commandDispatcher = commandDispatcher;
        }

        public async Task ExecuteAsync(GenerateGoogleTranslationCommand command)
        {
            var credentials = GoogleCredential.FromServiceAccountCredential(
                new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer("")
                )
            );

            var channel = new Channel(TextToSpeechClient.DefaultEndpoint.Host, credentials.ToChannelCredentials());

            var client = TextToSpeechClient.Create(channel);

            var chunks = command.Content.ChunkWithDelimeters(5000, '.', '!', '?');

            var response = await client.SynthesizeSpeechAsync(
                input: new SynthesisInput
                {
                    Text = command.Content
                },
                voice: new VoiceSelectionParams
                {
                    LanguageCode = "en-US",
                    SsmlGender = SsmlVoiceGender.Neutral
                },
                audioConfig: new AudioConfig
                {
                    AudioEncoding = AudioEncoding.Mp3
                }
            );

            using (var ms = new MemoryStream())
            {
                response.AudioContent.WriteTo(ms);

                await _commandDispatcher.DispatchAsync(new SaveTranslationCommand(
                    aggregateId: command.AggregateId,
                    command.CorrelationId,
                    version: command.Version,
                    userId: command.UserId,
                    stream: ms
                ));
            }
        }
    }
}
