using System;
using MessagePack;

namespace SIO.Domain.Audits.Projections
{
    public class AuditEvent
    {
        public string Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
    }
}
