using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.User.Events
{
    public class UserRegistered : Event
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserRegistered(Guid aggregateId, string email, string firstName, string lastName) : base(aggregateId, 1)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
