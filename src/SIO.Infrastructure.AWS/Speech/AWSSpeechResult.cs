using System;
using System.IO;
using System.Threading.Tasks;
using SIO.Infrastructure.Speech;

namespace SIO.Infrastructure.AWS.Speech
{
    public class AWSSpeechResult : ISpeechResult
    {
        private readonly Func<ValueTask<Stream>> _stream;

        public AWSSpeechResult(Func<ValueTask<Stream>> func)
        {
            _stream = func;
        }
        public ValueTask<Stream> OpenStreamAsync()
        {
            return _stream.Invoke();
        }
    }
}
