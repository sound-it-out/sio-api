using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using SIO.Infrastructure.AWS.File;
using SIO.Infrastructure.File;
using SIO.Infrastructure.Speech;

namespace SIO.Infrastructure.AWS.Speech
{
    internal class AWSSpeechClient : ISpeechClient<AWSSpeechRequest>
    {
        public readonly IAmazonPolly _pollyClient;
        private readonly IFileClient _fileClient;

        public AWSSpeechClient()
        {
            _pollyClient = new AmazonPollyClient(new BasicAWSCredentials("", ""), RegionEndpoint.EUWest1);
            _fileClient = new S3FileClient();
        }
        public async ValueTask<ISpeechResult> TranslateTextAsync(AWSSpeechRequest request)
        {         
            var synthesizeSpeechRequest = new StartSpeechSynthesisTaskRequest
            {
                OutputFormat = request.OutputFormat,
                VoiceId = request.VoiceId,
                Text = request.Text
            };

            var response = await _pollyClient.StartSpeechSynthesisTaskAsync(synthesizeSpeechRequest);            

            using (var waitHandle = new AutoResetEvent(false))
            using (var timer = new Timer(
                    callback: async (e) => { await CheckSynthesisTask(response.SynthesisTask.TaskId, waitHandle); },
                    state: null,
                    dueTime: TimeSpan.Zero,
                    period: TimeSpan.FromSeconds(30)
                )
            )
            {
                waitHandle.WaitOne();
            }

            var task = await _pollyClient.GetSpeechSynthesisTaskAsync(new GetSpeechSynthesisTaskRequest { TaskId = response.SynthesisTask.TaskId });

            var fileResult = await _fileClient.DownloadAsync(task.SynthesisTask.OutputUri, "");
            
            return new AWSSpeechResult(fileResult.OpenStreamAsync);
        }

        private async Task CheckSynthesisTask(string id, EventWaitHandle waitHandle)
        {
            var task = await _pollyClient.GetSpeechSynthesisTaskAsync(new GetSpeechSynthesisTaskRequest { TaskId = id });

            if (task.SynthesisTask.TaskStatus == Amazon.Polly.TaskStatus.Completed)
                waitHandle.Set();
        }
    }
}
