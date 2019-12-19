using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OpenEventSourcing.Events;

namespace SIO.API
{
    internal class SIOEventConsumer : IHostedService
    {
        private readonly IEventBusConsumer _eventBusConsumer;

        public SIOEventConsumer(IEventBusConsumer eventBusConsumer)
        {
            if (eventBusConsumer == null)

                throw new ArgumentNullException(nameof(eventBusConsumer));

            _eventBusConsumer = eventBusConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _eventBusConsumer.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _eventBusConsumer.StopAsync(cancellationToken);
        }
    }
}
