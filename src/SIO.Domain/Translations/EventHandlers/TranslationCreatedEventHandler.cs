using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Translations.Commands;
using SIO.Domain.Translations.Events;
using SIO.Domain.Translations.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationCreatedEventHandler : IEventHandler<TranslationCreated>
    {
        private readonly IHubContext<TranslationHub> _hubContext;
        private readonly ICommandDispatcher _commandDispatcher;

        public TranslationCreatedEventHandler(IHubContext<TranslationHub> hubContext, ICommandDispatcher commandDispatcher)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            _hubContext = hubContext;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(TranslationCreated @event)
        {
            await _hubContext.NotifyAsync(@event);

            await _commandDispatcher.DispatchAsync(new QueueTranslationCommand(
                aggregateId: @event.AggregateId,
                correlationId: @event.CorrelationId.Value,
                version: @event.Version,
                userId: @event.UserId,
                translationType: @event.TranslationType
            ));
        }
    }
}
