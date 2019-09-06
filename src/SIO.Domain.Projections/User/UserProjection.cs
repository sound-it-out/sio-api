using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEventSourcing.Events;
using OpenEventSourcing.Projections;
using SIO.Domain.User.Events;

namespace SIO.Domain.Projections.User
{
    public class UserProjection : IProjection
    {
        private readonly IProjectionWriter<User> _writer;
        private readonly IDictionary<Type, Func<IEvent, Task>> _handlers;

        public UserProjection(IProjectionWriter<User> writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
            _handlers = new Dictionary<Type, Func<IEvent, Task>>();

            Handles<UserRegistered>(ApplyAsync);
            Handles<UserVerified>(ApplyAsync);
            Handles<UserEmailChanged>(ApplyAsync);
            Handles<UserPurchasedCharacterTokens>(ApplyAsync);
        }

        public async Task HandleAsync(IEvent @event)
        {
            if (_handlers.TryGetValue(@event.GetType(), out var handler))
                await handler(@event);
        }

        private async Task ApplyAsync(UserRegistered @event)
        {
            await _writer.Add(@event.AggregateId, () =>
            {
                return new User
                {
                    Id = @event.AggregateId,
                    CreatedDate = @event.Timestamp,
                    Version = @event.Version,
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
                document.Version = @event.Version;
            });
        }

        private async Task ApplyAsync(UserEmailChanged @event)
        {
            await _writer.Update(@event.AggregateId, document =>
            {
                document.Data.Email = @event.Email;
                document.LastModifiedDate = @event.Timestamp;
                document.Version = @event.Version;
            });
        }

        private async Task ApplyAsync(UserPurchasedCharacterTokens @event)
        {
            await _writer.Update(@event.AggregateId, document =>
            {
                document.Data.CharacterTokens += @event.CharacterTokens;
                document.LastModifiedDate = @event.Timestamp;
                document.Version = @event.Version;
            });
        }

        private void Handles<TEvent>(Func<TEvent, Task> handler)
            where TEvent : IEvent
        {
            _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
        }
    }
}
