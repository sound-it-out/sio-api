using System.Threading.Tasks;
using OpenEventSourcing.Projections;
using SIO.Domain.Users.Events;

namespace SIO.Domain.Projections.Users
{
    public class UserProjection : Projection<User>
    {
        public UserProjection(IProjectionWriter<User> writer) : base(writer)
        {
            Handles<UserRegistered>(ApplyAsync);
            Handles<UserVerified>(ApplyAsync);
            Handles<UserEmailChanged>(ApplyAsync);
            Handles<UserPurchasedCharacterTokens>(ApplyAsync);
        }

        private async Task ApplyAsync(UserRegistered @event)
        {
            await _writer.Add(@event.AggregateId, () =>
            {
                return new User
                {
                    Id = @event.AggregateId,
                    CreatedDate = @event.Timestamp,
                    Version = 0,
                    Data = new UserData
                    {
                        Email = @event.Email,
                        FirstName = @event.FirstName,
                        LastName = @event.LastName,
                        Deleted = false,
                        Verified = false,
                        CharacterTokens = 0
                    }
                };
            });
        }

        private async Task ApplyAsync(UserVerified @event)
        {
            await _writer.Update(@event.AggregateId, document =>
            {
                document.Data.Verified = true;
                document.LastModifiedDate = @event.Timestamp;
            });
        }

        private async Task ApplyAsync(UserEmailChanged @event)
        {
            await _writer.Update(@event.AggregateId, document =>
            {
                document.Data.Email = @event.Email;
                document.LastModifiedDate = @event.Timestamp;
            });
        }

        private async Task ApplyAsync(UserPurchasedCharacterTokens @event)
        {
            await _writer.Update(@event.AggregateId, document =>
            {
                document.Data.CharacterTokens += @event.CharacterTokens;
                document.LastModifiedDate = @event.Timestamp;
            });
        }
    }
}
