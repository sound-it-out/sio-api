using System;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Translation.Queries
{
    public class GetTranslationOptionsQuery : Query<TranslationOptionQueryResult>
    {
        public GetTranslationOptionsQuery(Guid correlationId, string userId) : base(correlationId, userId)
        {
        }
    }
}
