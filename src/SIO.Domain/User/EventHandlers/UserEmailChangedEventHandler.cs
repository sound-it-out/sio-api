using System;
using System.Threading.Tasks;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.User.Events;

namespace SIO.Domain.User.EventHandlers
{
    internal class UserEmailChangedEventHandler : IEventHandler<UserEmailChanged>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public UserEmailChangedEventHandler(IAggregateRepository aggregateRepository)
        {
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));

            _aggregateRepository = aggregateRepository;
        }

        public async Task HandleAsync(UserEmailChanged @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var aggregate = await _aggregateRepository.GetAsync<User, UserState>(@event.AggregateId);
            aggregate.ChangeEmail(@event.AggregateId, @event.Email, aggregate.Version ?? 0);

            await _aggregateRepository.SaveAsync(aggregate, aggregate.Version);
        }
    }
}
