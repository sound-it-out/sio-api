using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIO.Domain.Documents.Projections;
using SIO.Domain.Documents.Queries;
using SIO.Infrastructure;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Documents.QueryHandlers
{
    internal class GetDocumentsForUserQueryHandler : IQueryHandler<GetDocumentsForUserQuery, GetDocumentsForUserQueryResult>
    {
        private readonly ILogger<GetDocumentsForUserQueryHandler> _logger;
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;

        public GetDocumentsForUserQueryHandler(ILogger<GetDocumentsForUserQueryHandler> logger,
            ISIOProjectionDbContextFactory projectionDbContextFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (projectionDbContextFactory == null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));

            _logger = logger;
            _projectionDbContextFactory = projectionDbContextFactory;
        }

        public async Task<GetDocumentsForUserQueryResult> RetrieveAsync(GetDocumentsForUserQuery query, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GetDocumentsForUserQueryHandler)}.{nameof(RetrieveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            using (var context = _projectionDbContextFactory.Create())
            {
                var documents = await context.Set<UserDocuments>().Where(ud => ud.UserId == query.Actor)
                    .AsNoTracking()
                    .AsQueryable()
                    .Include(ud => ud.Documents)
                    .SelectMany(ud => ud.Documents)
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToArrayAsync(cancellationToken);

                return new GetDocumentsForUserQueryResult(documents);
            }
        }
    }
}
