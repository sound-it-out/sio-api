using System.IO;
using System.Threading.Tasks;

namespace SIO.Infrastructure.Speech
{
    public interface ISpeechResult
    {
        ValueTask<Stream> OpenStreamAsync();
    }
}
