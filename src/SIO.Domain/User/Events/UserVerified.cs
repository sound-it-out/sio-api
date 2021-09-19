using MessagePack;
using SIO.Infrastructure.Events;

namespace SIO.Domain.User.Events
{
    public class UserVerified : Event
    {
        public UserVerified(string subject, int version) : base(subject, version)
        {
        }
    }
}
