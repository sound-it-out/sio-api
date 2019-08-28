using System.Threading.Tasks;

namespace SIO.Infrastructure.Speech
{
    public interface ISpeechClient<TRequest>
        where TRequest : ISpeechRequest
    {
        ValueTask<ISpeechResult> TranslateTextAsync(TRequest request);
    }
}
