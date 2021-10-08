using SIO.Infrastructure.Events;

namespace SIO.Domain.Documents.Events
{
    public class DocumentDeleted : UserEvent
    {
        public DocumentDeleted(string subject, int version, string user) : base(subject, version, user)
        {
        }
    }
}
