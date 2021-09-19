using SIO.Infrastructure.Events;

namespace SIO.Domain.User.Events
{
    public class UserRegistered : Event
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ActivationToken { get; set; }

        public UserRegistered(string subject, int version) : base(subject, version)
        {
        }
    }
}
