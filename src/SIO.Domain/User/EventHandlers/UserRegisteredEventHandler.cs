using System;
using System.Threading.Tasks;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.User.Events;

namespace SIO.Domain.User.EventHandlers
{
    internal class UserRegisteredEventHandler : IEventHandler<UserRegistered>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public UserRegisteredEventHandler(IAggregateRepository aggregateRepository)
        {
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));

            _aggregateRepository = aggregateRepository;
        }

        public async Task HandleAsync(UserRegistered @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var aggregate = await _aggregateRepository.GetAsync<User, UserState>(@event.AggregateId);
            aggregate.Register(@event.AggregateId, @event.Email, @event.FirstName, @event.LastName);

            await _aggregateRepository.SaveAsync(aggregate, aggregate.Version);
        }
    }
}
