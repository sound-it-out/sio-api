﻿using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.User.Events
{
    public class UserLoggedIn : Event
    {
        public UserLoggedIn(Guid aggregateId, Guid correlationId, string userId) : base(aggregateId, 0)
        {
            CorrelationId = correlationId;
            UserId = userId;
        }
    }
}
