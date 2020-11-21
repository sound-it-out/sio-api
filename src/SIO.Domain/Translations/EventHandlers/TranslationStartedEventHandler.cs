using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenEventSourcing.Commands;
using OpenEventSourcing.Events;
using SIO.Domain.Extenions;
using SIO.Domain.Translations.Events;
using SIO.Infrastructure.Notifications.Hubs;

namespace SIO.Domain.Translations.EventHandlers
{
    internal class TranslationStartedEventHandler : IEventHandler<TranslationStarted>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public TranslationStartedEventHandler(IHubContext<NotificationHub> hubContext, ICommandDispatcher commandDispatcher)
        {
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));

            _hubContext = hubContext;
        }

        public async Task HandleAsync(TranslationStarted @event)
        {
            await _hubContext.NotifyAsync(@event);
        }
    }
}
