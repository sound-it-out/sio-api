using System;
using System.Collections.Generic;
using System.Text;
using OpenEventSourcing.Queries;

namespace SIO.Domain.Projections.User.Queries
{
    public class UserQueryResult : IQueryResult
    {
        public Guid Id { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public bool Deleted { get; }
        public bool Verified { get; }
        public long CharacterTokens { get; }

        public UserQueryResult(Guid id, string email, string firstName, string lastName, bool deleted, bool verified, long characterTokens)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Deleted = deleted;
            Verified = verified;
            CharacterTokens = characterTokens;
        }
    }
}
