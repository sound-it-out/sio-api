using System;
using System.IO;
using System.Threading.Tasks;

namespace SIO.Infrastructure.File
{
    public interface IFileClient
    {
        Task UploadAsync(string fileName, string userId, Stream stream, string contentType);
        Task<FileResult> DownloadAsync(string fileName, string userId);
    }
}
