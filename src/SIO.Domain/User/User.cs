using System;
using OpenEventSourcing.Domain;
using SIO.Domain.User.Events;

namespace SIO.Domain.User
{
    public class User : Aggregate<UserState>
    {
        public User(UserState state) : base(state)
        {
            Handles<UserRegistered>(Handle);
            Handles<UserEmailChanged>(Handle);
            Handles<UserVerified>(Handle);
            Handles<UserPurchasedCharacterTokens>(Handle);
        }

        public override UserState GetState() => new UserState(_state);
        public override Guid? Id => _state.Id;
        public override int? Version => _state.Version;

        public void PurchaseTokens(Guid aggregateId, int version, long characterTokens)
        {
            Apply(new UserPurchasedCharacterTokens(
                aggregateId: aggregateId,
                version: version,
                characterTokens: characterTokens
            ));
        }

        public void Handle(UserRegistered @event)
        {
            _state.Id = @event.AggregateId;
            _state.Email = @event.Email;
            _state.FirstName = @event.FirstName;
            _state.LastName = @event.LastName;
            _state.CharacterTokens = 0;
            _state.Verified = false;
            _state.Deleted = false;
            _state.Version = 0;
        }

        public void Handle(UserEmailChanged @event)
        {
            _state.Email = @event.Email;
        }

        public void Handle(UserVerified @event)
        {
            _state.Verified = true;
        }

        public void Handle(UserPurchasedCharacterTokens @event)
        {
            _state.CharacterTokens += @event.CharacterTokens;
        }
    }
}
