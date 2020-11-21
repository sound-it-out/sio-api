using System;
using System.Collections.Generic;
using System.Text;
using OpenEventSourcing.Events;

namespace SIO.Domain.Documents.Events
{
    public class DocumentDeleted : Event
    {
        public DocumentDeleted(Guid aggregateId, int version) : base(aggregateId, version)
        {
        }
    }
}
