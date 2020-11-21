using System;
using OpenEventSourcing.Events;

namespace SIO.Domain.Users.Events
{
    public class UserPurchasedCharacterTokens : Event
    {
        public long CharacterTokens { get; set; }
        public UserPurchasedCharacterTokens(Guid aggregateId, int version, long characterTokens) : base(aggregateId, version)
        {
            CharacterTokens = characterTokens;
        }
    }
}
