using System.IO;
using System.Threading.Tasks;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.Document.Queries
{
    public class DownloadByIdQueryResult : IQueryResult
    {
        public ValueTask<Stream> Stream { get; }
        public string ContentType { get; }
        public string Filename { get; }

        public DownloadByIdQueryResult(ValueTask<Stream> stream, string contentType, string filename)
        {
            Stream = stream;
            ContentType = contentType;
            Filename = filename;
        }
    }
}
