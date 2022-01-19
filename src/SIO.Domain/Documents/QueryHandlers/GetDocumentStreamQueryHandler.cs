using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using SIO.Domain.Documents.Projections;
using SIO.Domain.Documents.Queries;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;
using SIO.Infrastructure.Files;
using SIO.Infrastructure.Queries;

namespace SIO.Domain.Documents.QueryHandlers
{
    internal sealed class GetDocumentStreamQueryHandler : IQueryHandler<GetDocumentStreamQuery, GetDocumentStreamQueryResult>
    {
        private readonly ILogger<GetDocumentStreamQueryHandler> _logger;
        private readonly IFileClient _fileClient;        
        private readonly ISIOProjectionDbContextFactory _projectionDbContextFactory;
        private readonly IContentTypeProvider _contentTypeProvider;

        public GetDocumentStreamQueryHandler(ILogger<GetDocumentStreamQueryHandler> logger,
            IFileClient fileClient,
            ISIOProjectionDbContextFactory projectionDbContextFactory,
            IContentTypeProvider contentTypeProvider)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            if (fileClient is null)
                throw new ArgumentNullException(nameof(fileClient));
            if (projectionDbContextFactory is null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));
            if (contentTypeProvider is null)
                throw new ArgumentNullException(nameof(contentTypeProvider));

            _logger = logger;
            _fileClient = fileClient;
            _projectionDbContextFactory = projectionDbContextFactory;
            _contentTypeProvider = contentTypeProvider;
        }

        public async Task<GetDocumentStreamQueryResult> RetrieveAsync(GetDocumentStreamQuery query, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(GetDocumentStreamQueryHandler)}.{nameof(RetrieveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            using (var context = _projectionDbContextFactory.Create())
            {
                var document = await context.Set<Document>().FindAsync(new object[] { query.Subject }, cancellationToken);

                if(document is null)
                    throw new ArgumentNullException(nameof(document));

                if (!_contentTypeProvider.TryGetContentType(document.FileName, out var contentType))
                {
                    contentType = "application/octet-stream";
                }


                var result = new GetDocumentStreamQueryResult(new MemoryStream(), contentType, document.FileName);

                await _fileClient.DownloadAsync($"{query.Subject}{Path.GetExtension(document.FileName)}", query.Actor, result.Stream, cancellationToken);
                result.Stream.Position = 0;
                return result;
            }
        }
    }
}
