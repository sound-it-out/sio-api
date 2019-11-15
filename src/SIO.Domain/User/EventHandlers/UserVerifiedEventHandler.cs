using System;
using System.Threading.Tasks;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.User.Events;

namespace SIO.Domain.User.EventHandlers
{
    internal class UserVerifiedEventHandler : IEventHandler<UserVerified>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public UserVerifiedEventHandler(IAggregateRepository aggregateRepository)
        {
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));

            _aggregateRepository = aggregateRepository;
        }

        public async Task HandleAsync(UserVerified @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var aggregate = await _aggregateRepository.GetAsync<User, UserState>(@event.AggregateId);
            aggregate.Verify(@event.AggregateId, aggregate.Version ?? 0);

            await _aggregateRepository.SaveAsync(aggregate, aggregate.Version);
        }
    }
}
