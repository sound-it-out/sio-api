using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using SIO.Infrastructure.Translation;

namespace SIO.Infrastructure.Google.Translation
{
    internal class GoogleSpeechResult : ISpeechResult
    {
        private readonly List<KeyValuePair<int, ByteString>> _bytes;

        public GoogleSpeechResult()
        {
            _bytes = new List<KeyValuePair<int, ByteString>>();
        }

        internal void DigestBytes(int index, ByteString bytes)
        {
            _bytes.Add(new KeyValuePair<int, ByteString>(index, bytes));
        }

        public async ValueTask<Stream> OpenStreamAsync()
        {
            var ms = new MemoryStream();
            var position = 0;

            foreach (var kvp in _bytes.OrderBy(kvp => kvp.Key))
            {
                await ms.WriteAsync(kvp.Value.ToByteArray(), position, kvp.Value.Length);
                position += kvp.Value.Length;
            }

            return ms;
        }
        
    }
}
