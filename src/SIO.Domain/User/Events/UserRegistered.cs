using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.User.Events
{
    public class UserRegistered : Event
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserRegistered(Guid aggregateId, int version, string email, string firstName, string lastName) : base(aggregateId, version)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
