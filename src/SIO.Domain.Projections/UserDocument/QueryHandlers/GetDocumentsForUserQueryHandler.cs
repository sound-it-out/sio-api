using System;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.Queries;
using SIO.Domain.Projections.UserDocument.Queries;

namespace SIO.Domain.Projections.UserDocument.QueryHandlers
{
    internal class GetDocumentsForUserQueryHandler : IQueryHandler<GetDocumentsForUserQuery, UserDocumentsQueryResult>
    {
        private readonly IProjectionDbContextFactory _projectionDbContextFactory;

        public GetDocumentsForUserQueryHandler(IProjectionDbContextFactory projectionDbContextFactory)
        {
            if (projectionDbContextFactory == null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));

            _projectionDbContextFactory = projectionDbContextFactory;
        }

        public Task<UserDocumentsQueryResult> RetrieveAsync(GetDocumentsForUserQuery query)
        {
            using(var context = _projectionDbContextFactory.Create())
            {
                var documents = context.Set<UserDocument>().Where(ud => ud.UserId.Equals(query.QueriedUserId))
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToArray();

                return Task.FromResult(new UserDocumentsQueryResult(documents));
            }
        }
    }
}
