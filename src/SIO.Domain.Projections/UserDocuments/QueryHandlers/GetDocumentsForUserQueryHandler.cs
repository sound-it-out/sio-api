using System;
using System.Threading.Tasks;
using OpenEventSourcing.Queries;
using SIO.Domain.Projections.UserDocuments.Queries;
using SIO.Infrastructure;

namespace SIO.Domain.Projections.UserDocuments.QueryHandlers
{
    internal class GetDocumentsForUserQueryHandler : IQueryHandler<GetDocumentsForUserQuery, UserDocumentsQueryResult>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetDocumentsForUserQueryHandler(IDbConnectionFactory dbConnectionFactory)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            _dbConnectionFactory = dbConnectionFactory;
        }

        public Task<UserDocumentsQueryResult> RetrieveAsync(GetDocumentsForUserQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
