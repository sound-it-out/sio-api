using System;
using System.IO;
using System.Threading.Tasks;

namespace SIO.Infrastructure.File
{
    public class FileResult
    {
        private readonly Func<ValueTask<Stream>> _streamFunc;
        public string ContentType { get; }

        public FileResult(string contentType, Func<ValueTask<Stream>> streamFunc)
        {
            ContentType = contentType;
            _streamFunc = streamFunc;
        }

        public ValueTask<Stream> OpenStreamAsync() => _streamFunc.Invoke();
    }
}
