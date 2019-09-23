using System;
using System.IO;
using System.Threading.Tasks;
using SIO.Infrastructure.File;
using Amazon.S3;
using Amazon;

namespace SIO.Infrastructure.AWS.File
{
    internal class S3FileClient : IFileClient
    {
        private readonly Func<S3Client> _clientFactory;

        public S3FileClient()
        {
            _clientFactory = () => new S3Client(AwsRegion.EUWest1, new AwsCredential("", ""));
        }

        public async Task<FileResult> DownloadAsync(string fileName, string userId)
        {
            var client = _clientFactory.Invoke();
            var request = new GetObjectRequest("", userId, fileName);
            var result = await client.GetObjectAsync(request);

            return new FileResult(result.ContentType, () => result.OpenAsync());
        }

        public async Task UploadAsync(string fileName, string userId, Stream stream, string contentType)
        {
            var client = _clientFactory.Invoke();
            var request = new PutObjectRequest("", userId, fileName);
            request.SetStream(stream, contentType);

            await client.PutObjectAsync(request);
        }
    }
}
