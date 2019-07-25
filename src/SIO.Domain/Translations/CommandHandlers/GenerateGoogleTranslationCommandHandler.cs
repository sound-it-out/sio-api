﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var voice = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Neutral
            };

            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            //TODO(matt): Need to further chunk to 300 as the rate limit is 300 requests per min

            var chunks = command.Content.ChunkWithDelimeters(5000, '.', '!', '?', ')', '"', '}', ']');

            var values = new List<KeyValuePair<int, ByteString>>();

            var responses = Task.WhenAll(chunks.Select((c, i) =>
                QueueText(client, c, i, voice, audioConfig, values)
            ));

            using (var ms = new MemoryStream())
            {
                var position = 0;
                foreach(var kvp in values.OrderBy(kvp => kvp.Key))
                {
                    await ms.WriteAsync(kvp.Value.ToByteArray(), position, kvp.Value.Length);
                    position += kvp.Value.Length;
                }

                await _commandDispatcher.DispatchAsync(new SaveTranslationCommand(
                    aggregateId: command.AggregateId,
                    command.CorrelationId,
                    version: command.Version,
                    userId: command.UserId,
                    stream: ms
                ));
            }            
        }

        private async Task QueueText(TextToSpeechClient client, string text, int index, VoiceSelectionParams voice, AudioConfig audioConfig, List<KeyValuePair<int, ByteString>> values)
        {
            var response = await client.SynthesizeSpeechAsync(
                input: new SynthesisInput {
                    Text = text
                },
                voice: voice,
                audioConfig: audioConfig
            );

            values.Add(new KeyValuePair<int, ByteString>(index, response.AudioContent));
        }
    }
}
