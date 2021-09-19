using System.Collections.Generic;
using MessagePack;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Audits.Projections
{
    public class Audit : IProjection
    {
        public string Subject { get; set; }
        public IEnumerable<AuditEvent> Events { get; set; }
    }
}
