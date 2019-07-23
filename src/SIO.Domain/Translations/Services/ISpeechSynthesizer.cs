using System.Threading.Tasks;

namespace SIO.Domain.Translations.Services
{
    internal interface ISpeechSynthesizer
    {
        Task QueueSynthesisAsync(string text);
    }
}
