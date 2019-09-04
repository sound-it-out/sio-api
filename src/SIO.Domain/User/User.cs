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
        }

        public override UserState GetState() => new UserState(_state);

        public void Register(Guid aggregateId, string email, string firstName, string lastName)
        {
            Apply(new UserRegistered(
                aggregateId: aggregateId,
                email: email,
                firstName: firstName,
                lastName: lastName
            ));
        }

        public void ChangeEmail(Guid aggregateId, string email, int version)
        {
            version++;
            Apply(new UserEmailChanged(
                aggregateId: aggregateId,
                version: version,
                email: email
            ));
        }

        public void Verify(Guid aggregateId, int version)
        {
            version++;
            Apply(new UserVerified(
                aggregateId: aggregateId,
                version: version
            ));
        }

        public void Handle(UserRegistered @event)
        {
            _state.Id = @event.AggregateId;
            _state.Email = @event.Email;
            _state.FirstName = @event.FirstName;
            _state.LastName = @event.LastName;
            _state.Verified = false;
            _state.Deleted = false;
            Version = 1;
        }

        public void Handle(UserEmailChanged @event)
        {
            _state.Email = @event.Email;
            Version++;
        }

        public void Handle(UserVerified @event)
        {
            _state.Verified = true;
            Version++;
        }
    }
}
