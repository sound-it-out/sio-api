using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace SIO.Infrastructure.File.Local
{
    internal class LocalFileClient : IFileClient
    {
        public static string Extract(string filename)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filename, out var contentType))
            {
                throw new Exception();

            }
            return contentType;
        }

        public Task DeleteAsync(string fileName, string userId)
        {
            var path = Path.Combine(Path.GetTempPath(), $"sio/{userId}/{fileName}");
            System.IO.File.Delete(path);
            return Task.CompletedTask;
        }

        public Task<FileResult> DownloadAsync(string fileName, string userId)
        {
            var stream = System.IO.File.OpenRead(Path.Combine(Path.GetTempPath(), $"sio/{userId}/{fileName}"));
            return Task.FromResult(new FileResult(Extract(stream.Name), () => stream));
        }

        public async Task UploadAsync(string fileName, string userId, Stream stream)
        {
            try
            {
                var path = Path.Combine(Path.GetTempPath(), $"sio/{userId}");
                Directory.CreateDirectory(path);

                using (var s = System.IO.File.OpenWrite(Path.Combine(path, fileName)))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(s);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
