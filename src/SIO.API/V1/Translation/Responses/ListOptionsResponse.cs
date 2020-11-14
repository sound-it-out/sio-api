using System.Collections.Generic;
using SIO.Infrastructure.Translations;

namespace SIO.API.V1.Translation.Responses
{
    public class ListOptionsResponse
    {
        public IEnumerable<TranslationOption> Options { get; set; }

        public ListOptionsResponse(IEnumerable<TranslationOption> options)
        {
            Options = options;
        }
    }
}
