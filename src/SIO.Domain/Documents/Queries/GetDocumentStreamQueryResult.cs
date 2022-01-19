using System.IO;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Documents.Queries
{
    public class GetDocumentStreamQueryResult : IQueryResult
    {
        public GetDocumentStreamQueryResult(Stream stream, string contentType, string fileName)
        {
            Stream = stream;
            ContentType = contentType;
            FileName = fileName;
        }

        public Stream Stream {  get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
