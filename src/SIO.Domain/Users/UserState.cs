using System;
using OpenEventSourcing.Domain;

namespace SIO.Domain.Users
{
    public class UserState : IAggregateState
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Deleted { get; set; }
        public bool Verified { get; set; }
        public long CharacterTokens { get; set; }
        public int Version { get; set; }

        public UserState()
        {

        }

        public UserState(UserState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            Id = state.Id;
            Email = state.Email;
            FirstName = state.FirstName;
            LastName = state.LastName;
            Deleted = state.Deleted;
            Verified = state.Verified;
            CharacterTokens = state.CharacterTokens;
            Version = state.Version;
        }
    }
}
