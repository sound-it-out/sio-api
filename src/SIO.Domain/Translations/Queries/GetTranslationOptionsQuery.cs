using System;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Translations.Queries
{
    public class GetTranslationOptionsQuery : Query<TranslationOptionQueryResult>
    {
        public GetTranslationOptionsQuery(Guid correlationId, string userId) : base(correlationId, userId)
        {
        }
    }
}
