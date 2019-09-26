using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.StaticFiles;
using SIO.Infrastructure.File;

namespace SIO.Infrastructure.AWS.File
{
    internal class S3FileClient : IFileClient
    {
        private readonly IAmazonS3 _client;

        public S3FileClient()
        {
            _client = new AmazonS3Client(new BasicAWSCredentials("", ""));
        }

        public async Task<FileResult> DownloadAsync(string fileName, string userId)
        {
            var file = await _client.GetObjectAsync(userId, fileName);
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileResult(contentType, () =>  file.ResponseStream);
        }

        public async Task UploadAsync(string fileName, string userId, Stream stream)
        {
            await _client.UploadObjectFromStreamAsync(userId, fileName, stream, new Dictionary<string, object>());
        }
    }
}
