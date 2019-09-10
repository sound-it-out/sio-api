using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Polly;
using SIO.Infrastructure.Speech;

namespace SIO.Infrastructure.AWS.Speech
{
    public class AWSSpeechRequest : ISpeechRequest
    {
        public OutputFormat OutputFormat { get; set; }
        public VoiceId VoiceId { get; set; }
        public string Text { get; set; }
    }
}
