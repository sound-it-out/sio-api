using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Grpc.Auth;
using Grpc.Core;
using SIO.Infrastructure.Speech;

namespace SIO.Infrastructure.Google.Speech
{
    public class GoogleSpeechClient : ISpeechClient<GoogleSpeechRequest>
    {
        private readonly TextToSpeechClient _textToSpeechClient;

        public GoogleSpeechClient()
        {
            var credentials = GoogleCredential.FromServiceAccountCredential(
               new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer("")
               )
            );

            var channel = new Channel(TextToSpeechClient.DefaultEndpoint.Host, credentials.ToChannelCredentials());

            _textToSpeechClient = TextToSpeechClient.Create(channel);
        }
        public async ValueTask<ISpeechResult> TranslateTextAsync(GoogleSpeechRequest request)
        {
            var result = new GoogleSpeechResult();

            foreach (var requestChunks in request.Content.Chunk(300))
            {
                await Task.WhenAll(requestChunks.Select((c, i) =>
                    QueueText(c, i, request, result)
                ));

                // Need to wait some time due to rate limits
                Delay(61000);
            }

            return result;
        }

        private static void Delay(int delay)
        {
            int i = 0;
            using (var delayTimer = new System.Timers.Timer())
            {
                delayTimer.Interval = delay;
                delayTimer.AutoReset = false; //so that it only calls the method once
                delayTimer.Elapsed += (s, args) => i = 1;
                delayTimer.Start();
                while (i == 0) { };
            }
        }

        private async Task QueueText(string text, int index, GoogleSpeechRequest request, GoogleSpeechResult result)
        {
            var response = await _textToSpeechClient.SynthesizeSpeechAsync(
                input: new SynthesisInput
                {
                    Text = text
                },
                voice: request.VoiceSelection,
                audioConfig: request.AudioConfig
            );

            result.DigestBytes(index, response.AudioContent);
            request.CallBack(text.Length);
        }
    }
}
