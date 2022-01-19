using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIO.Domain.TranslationOptions.Projections;
using SIO.Domain.TranslationOptions.Queries;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.TranslationOptions.QueryHandlers
{
    internal class GetTranslationOptionsQueryHandler : IQueryHandler<GetTranslationOptionsQuery, GetTranslationOptionsQueryResult>
    {
        private readonly ILogger<GetTranslationOptionsQueryHandler> _logger;
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;

        public GetTranslationOptionsQueryHandler(ILogger<GetTranslationOptionsQueryHandler> logger,
            ISIOProjectionDbContextFactory projectionDbContextFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (projectionDbContextFactory == null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));

            _logger = logger;
            _projectionDbContextFactory = projectionDbContextFactory;
        }

        public async Task<GetTranslationOptionsQueryResult> RetrieveAsync(GetTranslationOptionsQuery query, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GetTranslationOptionsQueryHandler)}.{nameof(RetrieveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            using (var context = _projectionDbContextFactory.Create())
            {
                var translationOptions = await context.Set<TranslationOption>()
                    .AsNoTracking()
                    .AsQueryable()
                    .OrderBy(to => to.Subject)
                    .ToArrayAsync(cancellationToken);

                return new GetTranslationOptionsQueryResult(translationOptions);
            }
        }
    }
}
