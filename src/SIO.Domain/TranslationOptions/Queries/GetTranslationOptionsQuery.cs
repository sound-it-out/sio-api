using SIO.Infrastructure;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.TranslationOptions.Queries
{
    public class GetTranslationOptionsQuery : Query<GetTranslationOptionsQueryResult>
    {
        public GetTranslationOptionsQuery(CorrelationId? correlationId, Actor actor) : base(correlationId, actor)
        {
        }
    }
}
