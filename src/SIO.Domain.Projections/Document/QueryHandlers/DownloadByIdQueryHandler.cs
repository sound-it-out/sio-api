using System;
using System.Threading.Tasks;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.Queries;
using SIO.Domain.Projections.Document.Queries;
using SIO.Infrastructure.Files;

namespace SIO.Domain.Projections.Document.QueryHandlers
{
    internal class DownloadByIdQueryHandler : IQueryHandler<DownloadByIdQuery, DownloadByIdQueryResult>
    {
        private readonly IProjectionDbContextFactory _projectionDbContextFactory;
        private readonly IFileClient _fileClient;

        public DownloadByIdQueryHandler(IProjectionDbContextFactory projectionDbContextFactory,
            IFileClient fileClient)
        {
            if (projectionDbContextFactory == null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));
            if (fileClient == null)
                throw new ArgumentNullException(nameof(fileClient));

            _projectionDbContextFactory = projectionDbContextFactory;
            _fileClient = fileClient;
        }
        public async Task<DownloadByIdQueryResult> RetrieveAsync(DownloadByIdQuery query)
        {
            using (var context = _projectionDbContextFactory.Create())
            {
                var document = await context.FindAsync<Document>(query.AggregateId);
                var translation = await _fileClient.DownloadAsync($"{document.TranslationId}.mp3", query.UserId);

                return new DownloadByIdQueryResult(translation.OpenStreamAsync(), translation.ContentType, document.Data.FileName);
            }
        }
    }
}
