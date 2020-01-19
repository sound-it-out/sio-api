using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenEventSourcing.Queries;
using SIO.API.V1.User.Responses;
using SIO.Domain.Projections.User.Queries;

namespace SIO.API.V1.User
{
    [Authorize]
    public class UserController : SIOController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public UserController(IQueryDispatcher queryDispatcher)
        {
            if (queryDispatcher == null)
                throw new ArgumentNullException(nameof(queryDispatcher));

            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("me")]
        public async Task<UserResponse> Me()
        {
            var userQueryResult = await _queryDispatcher.DispatchAsync(new GetUserByIdQuery(Guid.NewGuid(), CurrentUserId.ToString(), CurrentUserId.Value));
            return new UserResponse
            {
                Id = userQueryResult.Id,
                CharacterTokens = userQueryResult.CharacterTokens,
                Deleted = userQueryResult.Deleted,
                Email = userQueryResult.Email,
                FirstName = userQueryResult.FirstName,
                LastName = userQueryResult.LastName,
                Verified = userQueryResult.Verified
            };
        }
    }
}
