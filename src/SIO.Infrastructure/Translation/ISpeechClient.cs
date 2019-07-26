using System.Threading.Tasks;

namespace SIO.Infrastructure.Translation
{
    public interface ISpeechClient<TRequest>
        where TRequest : ISpeechRequest
    {
        ValueTask<ISpeechResult> TranslateTextAsync(TRequest request);
    }
}
