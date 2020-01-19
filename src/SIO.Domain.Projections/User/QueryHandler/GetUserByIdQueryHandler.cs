using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using OpenEventSourcing.EntityFrameworkCore.DbContexts;
using OpenEventSourcing.Queries;
using SIO.Domain.Projections.User.Queries;
using SIO.Infrastructure;

namespace SIO.Domain.Projections.User.QueryHandler
{
    internal class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserQueryResult>
    {
        private readonly IProjectionDbContextFactory _projectionDbContextFactory;

        public GetUserByIdQueryHandler(IProjectionDbContextFactory projectionDbContextFactory)
        {
            if (projectionDbContextFactory == null)
                throw new ArgumentNullException(nameof(projectionDbContextFactory));

            _projectionDbContextFactory = projectionDbContextFactory;
        }

        public async Task<UserQueryResult> RetrieveAsync(GetUserByIdQuery query)
        {
            using (var context = _projectionDbContextFactory.Create())
            {
                var user = await context.FindAsync<User>(query.AggregateId);
                return new UserQueryResult(user.Id, user.Data.Email, user.Data.FirstName, user.Data.LastName, user.Data.Deleted, user.Data.Verified, user.Data.CharacterTokens);
            }
        }
    }
}
