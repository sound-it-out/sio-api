using System;
using System.Linq;
using System.Threading.Tasks;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Domain.User.Events;

namespace SIO.Domain.User.EventHandlers
{
    internal class UserRegisteredEventHandler : IEventHandler<UserRegistered>
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IAggregateFactory _aggregateFactory;

        public UserRegisteredEventHandler(IAggregateRepository aggregateRepository, IAggregateFactory aggregateFactory)
        {
            if (aggregateRepository == null)
                throw new ArgumentNullException(nameof(aggregateRepository));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));

            _aggregateRepository = aggregateRepository;
            _aggregateFactory = aggregateFactory;
        }

        public async Task HandleAsync(UserRegistered @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var aggregate = _aggregateFactory.FromHistory<User, UserState>(Enumerable.Empty<IEvent>());
            aggregate.Register(@event.AggregateId, @event.Email, @event.FirstName, @event.LastName);

            await _aggregateRepository.SaveAsync(aggregate, aggregate.Version);
        }
    }
}
