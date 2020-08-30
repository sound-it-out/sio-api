using System;
using Newtonsoft.Json;
using OpenEventSourcing.Events;

namespace SIO.Domain.User.Events
{
    public class UserVerified : Event
    {
        public UserVerified(Guid aggregateId, Guid correlationId, string userId) : base(aggregateId, 0)
        {
            CorrelationId = correlationId;
            UserId = userId;
        }

        [JsonConstructor]
        public UserVerified(Guid aggregateId, Guid causationId, Guid correlationId, string userId) : base(aggregateId, 0)
        {
            CorrelationId = correlationId;
            CausationId = causationId;
            UserId = userId;
        }
    }
}
