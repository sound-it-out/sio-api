using System.Threading.Tasks;

namespace SIO.Domain.Translation.Services
{
    internal interface ISpeechSynthesizer
    {
        Task QueueSynthesisAsync(string text);
    }
}
