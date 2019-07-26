using System.IO;
using System.Threading.Tasks;

namespace SIO.Infrastructure.Translation
{
    public interface ISpeechResult
    {
        ValueTask<Stream> OpenStreamAsync();
    }
}
