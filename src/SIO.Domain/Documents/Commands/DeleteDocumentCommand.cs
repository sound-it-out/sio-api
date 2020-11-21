using System;
using System.Collections.Generic;
using System.Text;
using OpenEventSourcing.Commands;

namespace SIO.Domain.Documents.Commands
{
    public class DeleteDocumentCommand : Command
    {
        public DeleteDocumentCommand(Guid aggregateId, Guid correlationId, int version, string userId) : base(aggregateId, correlationId, version, userId)
        {
        }
    }
}
